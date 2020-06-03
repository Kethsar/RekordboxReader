using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RekordboxReader
{
    public partial class MainForm : Form
    {
        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001f0fff,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            VMRead = 0x00000010,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize = 0x00100000,
        };

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesWritten);
        [DllImport("kernel32.dll")]
        private static extern IntPtr CloseHandle(IntPtr hProcess);

        const int SIZE_PADDING = 12;
        const int DECK_CNT = 4;
        const string D1T_KEY = "Deck1TitlePointer";
        const string D1A_KEY = "Deck1ArtistPointer";
        const string D2T_KEY = "Deck2TitlePointer";
        const string D2A_KEY = "Deck2ArtistPointer";
        const string D3T_KEY = "Deck3TitlePointer";
        const string D3A_KEY = "Deck3ArtistPointer";
        const string D4T_KEY = "Deck4TitlePointer";
        const string D4A_KEY = "Deck4ArtistPointer";
        const string MD_KEY = "MasterDeckPointer";

        const string HTTP_RESPONSE = "HTTP/1.1 200 OK{0}" +
            "Content-Type: text/plain; charset=utf-8{0}" +
            "Content-Length: {1}{0}" +
            "Connection: close{0}{0}{2}";

        int master,
            port,
            minWidth,
            normalWidth,
            normalHeight;

        Process RekordBox;
        IntPtr handle;
        Dictionary<string, string> pointerPatterns;
        Deck[] decks;
        System.Windows.Forms.Timer t;
        TcpListener tcpListener;
        Thread httpd, rbdd;
        RRSettings settings;
        Type settingsType;
        bool running = true,
             RekordboxFound = false,
             settingsShown = false;

        static object locker = new object();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            settingsType = typeof(RRSettings);
            LoadSettings();
            decks = new Deck[DECK_CNT];
            t = new System.Windows.Forms.Timer();

            for (var i = 0; i < decks.Length; i++)
                decks[i] = new Deck();

            minWidth = normalWidth = ClientSize.Width;
            normalHeight = ClientSize.Height;

            t.Interval = 500;
            t.Tick += CheckValues;
            t.Start();

            Text = "RekordboxReader v.'the version number is too long and can't be seen without opening the settings. How retarded'.1.0.1";
            FormClosing += AppExiting;

            httpd = new Thread(HTTPdaemon);
            rbdd = new Thread(RekordboxDoko);

            rbdd.Start();
            httpd.Start();
        }

        public int read(IntPtr adr, byte[] buf)
        {
            int ret = -1,
                readsize = buf.Length;
            var warked = false;

            for (var i = 0; i < 4; i++)
            {
                warked = ReadProcessMemory(handle, adr, buf, sizeof(byte) * readsize, out ret);
                if (warked) break;
                readsize /= 2;
            }

            return ret;
        }

        public int read(IntPtr adr, byte[] buf, int[] ofs)
        {
            var readAddr = new byte[IntPtr.Size];

            foreach (int o in ofs)
            {
                var i = read(adr, readAddr);
                if (i < readAddr.Length) return -1;
                adr = new IntPtr(BitConverter.ToInt64(readAddr, 0) + o);
            }
            return read(adr, buf);
        }

        private void HTTPdaemon() //stolen from ed like everything
        {
            tcpListener = new TcpListener(IPAddress.Loopback, port);
            Socket socket = null;

            try
            {
                tcpListener.Start();
            }
            catch (Exception)
            {
                MessageBox.Show(string.Format("Another copy of Loopstr- RekordboxReader seems to be running (port {1} busy){0}{0}the copy you just started will now stop serving http.", "\r\n", port), "Already Running");
                running = false;
            }

            while (running)
            {
                try
                {
                    if (!tcpListener.Pending())
                    {
                        Thread.Sleep(200);
                        continue;
                    }

                    socket = tcpListener.AcceptSocket();
                    var metadata = "";

                    lock (locker)
                    {
                        if (master >= 0)
                        {
                            if (!string.IsNullOrWhiteSpace(decks[master].Artist))
                                metadata += decks[master].Artist + " - ";

                            metadata += decks[master].Title;
                        }
                    }

                    var bytes = Encoding.UTF8.GetBytes(string.Format(HTTP_RESPONSE, "\r\n", Encoding.UTF8.GetByteCount(metadata), metadata));
                    socket.Send(bytes);
                }
                catch (ThreadAbortException)
                {
                    break;
                }
                catch 
                {
                    Thread.Sleep(500);
                }
                finally
                {
                    if (socket != null)
                    {
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                        socket = null;
                    }
                }
            }
        }

        private void AppExiting(object sender, FormClosingEventArgs e)
        {
            t.Stop();
            running = false;
            RekordboxFound = false;
            CloseHandle(handle);
            httpd.Abort();

            if (rbdd.IsAlive)
                rbdd.Abort();
        }

        private void CheckValues(object sender, EventArgs e)
        {
            if (!RekordboxFound)
                return;

            if (RekordBox.HasExited)
            {
                RekordboxFound = false;
                rbdd = new Thread(RekordboxDoko);
                rbdd.Start();
            }

            try
            {
                ProcessModule pm = null;

                lock (locker)
                {
                    foreach (var k in pointerPatterns.Keys)
                    {
                        var ret = "";
                        var lpm = "";
                        var arg = pointerPatterns[k];
                        if (string.IsNullOrWhiteSpace(arg))
                            continue;

                        var raw = new byte[1024];
                        var ofs = IntPtr.Zero;
                        var steps = new int[0];

                        if (arg.Contains('+'))
                        {
                            var args = arg.Split('+');

                            if (args[0] != lpm)
                            {
                                pm = null;
                                lpm = null;

                                foreach (ProcessModule mod in RekordBox.Modules)
                                {
                                    if (mod.ModuleName == args[0])
                                    {
                                        pm = mod;
                                        lpm = pm.ModuleName;
                                        ofs = pm.BaseAddress;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                ofs = pm.BaseAddress;
                            }

                            arg = args[1];
                        }

                        if (arg.Contains('*'))
                        {
                            var args = arg.Split('*');
                            steps = new int[args.Length - 1];

                            for (var b = 1; b < args.Length; b++)
                            {
                                if (!string.IsNullOrWhiteSpace(args[b]))
                                    steps[b - 1] = Convert.ToInt32(args[b], 16);
                            }

                            arg = args[0];
                        }

                        ofs += Convert.ToInt32(arg, 16);

                        var len = read(ofs, raw, steps);
                        if (len > 0)
                        {
                            ret = Encoding.UTF8.GetString(raw);
                            var i = ret.IndexOf('\0');

                            if (i >= 0)
                                ret = ret.Substring(0, i);
                            else
                                ret = "(read error)"; // While I guess technically possible, I doubt we'll encounter a proper string that is 1024 bytes long
                        }

                        if (ShitsGarbage(ret))
                            ret = "(read error) ASCII control codes found";

                        switch (k)
                        {
                            case MD_KEY:
                                master = BitConverter.ToInt32(raw, 0);
                                MasterDeckInfo.Text = master.ToString();
                                break;
                            case D1A_KEY:
                                if (ret.IsNormalized())
                                {
                                    decks[0].Artist = ret;
                                    Deck1ArtistLabel.Text = ret;
                                }
                                break;
                            case D1T_KEY:
                                if (ret.IsNormalized())
                                {
                                    decks[0].Title = ret;
                                    Deck1TitleLabel.Text = ret;
                                }
                                break;
                            case D2A_KEY:
                                if (ret.IsNormalized())
                                {
                                    decks[1].Artist = ret;
                                    Deck2ArtistLabel.Text = ret;
                                }
                                break;
                            case D2T_KEY:
                                if (ret.IsNormalized())
                                {
                                    decks[1].Title = ret;
                                    Deck2TitleLabel.Text = ret;
                                }
                                break;
                            case D3A_KEY:
                                if (ret.IsNormalized())
                                {
                                    decks[2].Artist = ret;
                                    Deck3ArtistLabel.Text = ret;
                                }
                                break;
                            case D3T_KEY:
                                if (ret.IsNormalized())
                                {
                                    decks[2].Title = ret;
                                    Deck3TitleLabel.Text = ret;
                                }
                                break;
                            case D4A_KEY:
                                if (ret.IsNormalized())
                                {
                                    decks[3].Artist = ret;
                                    Deck4ArtistLabel.Text = ret;
                                }
                                break;
                            case D4T_KEY:
                                if (ret.IsNormalized())
                                {
                                    decks[3].Title = ret;
                                    Deck4TitleLabel.Text = ret;
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessage.Text = string.Format("Somebody Fucked Up: {0}", ex.Message);
            }
        }

        private bool ShitsGarbage(string check)
        {
            foreach (var c in check)
            {
                if (c <= 0x1F) // various ASCII control codes. Means a pointer is probably fucked and reading garbage data instead of a string proper.
                    return true;
            }
            return false;
        }

        private void ResizeIfNeeded()
        {
            if (WindowState == FormWindowState.Minimized)
                return;

            var newWidth = Deck1ArtistLabel.Size.Width;
            var newHeight = ClientSize.Height < normalHeight ? normalHeight : ClientSize.Height; // just in case

            if (Deck1TitleLabel.Size.Width > newWidth)
                newWidth = Deck1TitleLabel.Size.Width;

            if (Deck2ArtistLabel.Size.Width > newWidth)
                newWidth = Deck2ArtistLabel.Size.Width;

            if (Deck2TitleLabel.Size.Width > newWidth)
                newWidth = Deck2TitleLabel.Size.Width;

            if (Deck3TitleLabel.Size.Width > newWidth)
                newWidth = Deck3TitleLabel.Size.Width;

            if (Deck3ArtistLabel.Size.Width > newWidth)
                newWidth = Deck3ArtistLabel.Size.Width;

            if (Deck4TitleLabel.Size.Width > newWidth)
                newWidth = Deck4TitleLabel.Size.Width;

            if (Deck4ArtistLabel.Size.Width > newWidth)
                newWidth = Deck4ArtistLabel.Size.Width;

            newWidth += Deck1TitleLabel.Location.X;

            if (StatusMessage.Size.Width > newWidth)
                newWidth = StatusMessage.Size.Width + StatusMessage.Location.X;

            newWidth += SIZE_PADDING;
            if (newWidth < minWidth)
                newWidth = minWidth;

            var newSize = new System.Drawing.Size(newWidth, newHeight);
            ClientSize = newSize;
        }

        private void Label_Resize(object sender, EventArgs e)
        {
            ResizeIfNeeded();
        }

        private void RekordboxDoko()
        {
            while (running)
            {

                try
                {
                    RekordBox = Process.GetProcessesByName("rekordbox").FirstOrDefault();
                    if (RekordBox != null)
                    {
                        handle = OpenProcess(ProcessAccessFlags.VMRead, false, RekordBox.Id);
                        RekordboxFound = true;

                        // Invoke needed as this is being run in a separate thread.
                        StatusMessage.Invoke((Action)(() =>
                        {
                            StatusMessage.Text = "Status: Rekordbox process found. Reading.";
                        }));

                        break;
                    }

                    StatusMessage.Invoke((Action)(() =>
                    {
                        StatusMessage.Text = "Status: Rekordbox process was not found.";
                    }));

                    Thread.Sleep(1000);
                }
                catch (ThreadAbortException)
                {
                    break;
                }
            }
        }

        public void LoadSettings()
        {
            if (!File.Exists(RRSettings.GetSettingsFilename()))
            {
                using (Stream presetStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("RekordboxReader.Resources.presets.xml"),
                              settFileStream = new FileStream(RRSettings.GetSettingsFilename(), FileMode.CreateNew))
                {
                    presetStream.CopyTo(settFileStream);
                }
            }

            settings = RRSettings.LoadFile();
            if (pointerPatterns == null)
                pointerPatterns = new Dictionary<string, string>();

            ReloadSettings();
        }

        public void ReloadSettings()
        {
            port = settings.Port;
            SetPointer(MD_KEY);
            SetPointer(D1A_KEY);
            SetPointer(D1T_KEY);
            SetPointer(D2A_KEY);
            SetPointer(D2T_KEY);
            SetPointer(D3A_KEY);
            SetPointer(D3T_KEY);
            SetPointer(D4A_KEY);
            SetPointer(D4T_KEY);
        }

        public void SetPointer(string ptrType)
        {
            lock (locker)
            {
                pointerPatterns[ptrType] = settingsType.GetProperty(ptrType).GetValue(settings).ToString();
            }
        }

        private void SettingsBtn_Click(object sender, EventArgs e)
        {
            if (!settingsShown)
            {
                var settForm = new SettingsForm(settings);
                settForm.FormClosed += SettingsFormClosed;
                settForm.Show(this);
                settingsShown = true;
            }
        }

        public void SettingsFormClosed(object sender, EventArgs e)
        {
            settingsShown = false;
            PortChanged();
        }

        public void PortChanged()
        {
            if (port != settings.Port)
            {
                running = false;
                httpd.Interrupt();
                tcpListener.Stop();
                port = settings.Port;

                try
                {
                    running = true;
                    httpd = new Thread(HTTPdaemon);
                    httpd.Start();
                }
                catch (Exception ex)
                {
                    running = false;
                    StatusMessage.Text = string.Format("Status: Error restarting HTTP Daemon: {0}", ex.Message);
                }
            }
        }
    }

    public class Deck
    {
        public string Artist { get; set; }
        public string Title { get; set; }

        public Deck()
        {
            Artist = "";
            Title = "";
        }

        public Deck(string artist, string title)
        {
            Artist = artist;
            Title = title;
        }
    }

}
