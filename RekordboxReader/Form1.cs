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
    public partial class Form1 : Form
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
        const string D1T_KEY = "d1title";
        const string D1A_KEY = "d1artist";
        const string D2T_KEY = "d2title";
        const string D2A_KEY = "d2artist";
        const string D3T_KEY = "d3title";
        const string D3A_KEY = "d3artist";
        const string D4T_KEY = "d4title";
        const string D4A_KEY = "d4artist";
        const string MD_KEY = "master";

        int master,
            port,
            minWidth,
            normalWidth,
            normalHeight,
            settingsWidth;

        Process RekordBox;
        IntPtr handle;
        Dictionary<string, string> pointerPatterns;
        Deck[] decks;
        System.Windows.Forms.Timer t;
        TcpListener tcpListener;
        Thread httpd, rbdd;
        bool running = true;
        bool RekordboxFound = false;
        bool settingsShown = false;

        static object locker = new object();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadProperties();
            decks = new Deck[DECK_CNT];
            t = new System.Windows.Forms.Timer();

            for (var i = 0; i < decks.Length; i++)
                decks[i] = new Deck();

            minWidth = normalWidth = ClientSize.Width;
            normalHeight = ClientSize.Height;
            settingsWidth = Deck1ArtistBox.Location.X + Deck1ArtistBox.Size.Width + SIZE_PADDING;

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
            int ret = -1;

            ReadProcessMemory(handle, adr, buf, sizeof(byte) * buf.Length, out ret);
            if(adr == IntPtr.Zero)
            {
                ret = -1;
            }

            return ret;
        }

        public int read(IntPtr adr, byte[] buf, int[] ofs)
        {
            byte[] readAddr = new byte[IntPtr.Size];

            foreach (int o in ofs)
            {
                int i = read(adr, readAddr);
                if (i < readAddr.Length) return -1;
                adr = new IntPtr(BitConverter.ToInt64(readAddr, 0) + o);
            }
            return read(adr, buf);
        }

        private void HTTPdaemon() //stolen from ed like everything
        {
            byte[] buffer = new byte[256];
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
                    string metadata = "";

                    lock (locker)
                    {
                        if (master >= 0)
                        {
                            if (!string.IsNullOrWhiteSpace(decks[master].Artist))
                                metadata += decks[master].Artist + " - ";

                            metadata += decks[master].Title;
                        }
                    }

                    byte[] bytes = Encoding.UTF8.GetBytes(string.Format("HTTP/1.1 200 OK{0}Content-Type: text/plain; charset=utf-8{0}Content-Length: {1}{0}Connection: close{0}{0}{2}", "\r\n", Encoding.UTF8.GetByteCount(metadata), metadata));
                    socket.Send(bytes);
                }
                catch(ThreadAbortException)
                {
                    break;
                }
                catch (Exception)
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
                        string ret = "";
                        string lpm = "";
                        var arg = pointerPatterns[k];
                        if (string.IsNullOrWhiteSpace(arg))
                            continue;

                        byte[] raw = new byte[1024];
                        IntPtr ofs = IntPtr.Zero;
                        int[] steps = new int[0];

                        if (arg.Contains('+'))
                        {
                            string[] args = arg.Split('+');

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
                            string[] args = arg.Split('*');
                            steps = new int[args.Length - 1];

                            for (int b = 1; b < args.Length; b++)
                            {
                                if (!string.IsNullOrWhiteSpace(args[b]))
                                    steps[b - 1] = Convert.ToInt32(args[b], 16);
                            }

                            arg = args[0];
                        }

                        ofs += Convert.ToInt32(arg, 16);

                        int len = read(ofs, raw, steps);
                        if (len > 0)
                        {
                            ret = Encoding.UTF8.GetString(raw);
                            int i = ret.IndexOf('\0');

                            if (i >= 0)
                                ret = ret.Substring(0, i);
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
            foreach(var c in check)
            {
                if (c <= 0x1F) // various ASCII control codes. Means a pointer is probably fucked and reading garbage data instead of a string proper.
                    return true;
            }
            return false;
        }

        private string Serialize() // Version 2. Now shares 4 decks.
        {
            // Structure of serialization will always be [Serializer version, ...anything else]
            return "2" + "!!" + Deck1ArtistBox.Text + "!!" +
                Deck1TitleBox.Text + "!!" +
                Deck2ArtistBox.Text + "!!" +
                Deck2TitleBox.Text + "!!" +
                Deck3ArtistBox.Text + "!!" +
                Deck3TitleBox.Text + "!!" +
                Deck4ArtistBox.Text + "!!" +
                Deck4TitleBox.Text + "!!" +
                MasterDeckBox.Text;
        }

        public byte[] Compress(string key)
        {
            byte[] KeyByteArray = Encoding.UTF8.GetBytes(key);
            var OutputStream = new MemoryStream();
            byte[] OutputBytes;

            using (var Gzip = new System.IO.Compression.GZipStream(OutputStream, System.IO.Compression.CompressionMode.Compress))
            {
                Gzip.Write(KeyByteArray, 0, KeyByteArray.Length);
            }

            OutputBytes = OutputStream.ToArray();
            return OutputBytes;
        }

        public string Decompress(string key)
        {
            byte[] KeyByteArray = Convert.FromBase64String(key);
            MemoryStream OutStream = new MemoryStream();
            string OutputString;

            using (var Gzip = new System.IO.Compression.GZipStream(new MemoryStream(KeyByteArray), System.IO.Compression.CompressionMode.Decompress))
            {
                Gzip.CopyTo(OutStream);
                Gzip.Close();
            }

            OutputString = Encoding.UTF8.GetString(OutStream.ToArray());
            return OutputString;
        }

        private void SettingsApplier(string[] Settings)
        {
            switch (Settings[0]) // switch (version)
            {
                case "1": 
                    if (Settings.Length == 6) // KeyOK
                    {
                        Deck1ArtistBox.Text = Settings[1];
                        Deck1TitleBox.Text = Settings[2];
                        Deck2ArtistBox.Text = Settings[3];
                        Deck2TitleBox.Text = Settings[4];
                        MasterDeckBox.Text = Settings[5];
                    }
                    StatusMessage.Text = "Status: Loaded preset. Hope it works.";
                    break;
                case "2":
                    if (Settings.Length == 10) // KeyOK
                    {
                        Deck1ArtistBox.Text = Settings[1];
                        Deck1TitleBox.Text = Settings[2];
                        Deck2ArtistBox.Text = Settings[3];
                        Deck2TitleBox.Text = Settings[4];
                        Deck3ArtistBox.Text = Settings[5];
                        Deck3TitleBox.Text = Settings[6];
                        Deck4ArtistBox.Text = Settings[7];
                        Deck4TitleBox.Text = Settings[8];
                        MasterDeckBox.Text = Settings[9];
                    }
                    StatusMessage.Text = "Status: Loaded preset. Hope it works.";
                    break;

                default:
                    StatusMessage.Text = "Status: Bad preset, settings not loaded.";
                    break;
            }
        }

        private void DeserializeAndApply(string Key)
        {
            if(Key.Length <= 4 || Key.Substring(0,4) != "!RR!")
            {
                StatusMessage.Text = "Status: Faulty preset code";
                return;
            }
            Key = Key.Substring(4);
                
            string DGString = Decompress(Key);
            string[] SString = DGString.Split(new string[] { "!!" }, StringSplitOptions.None);
            SettingsApplier(SString);
        }

        private void PresetShare(object sender, EventArgs e)
        {
            byte[] GPreset = Compress(Serialize());
            var BPreset = "!RR!"+Convert.ToBase64String(GPreset);
            Clipboard.Clear();
            Clipboard.SetText(BPreset);
            StatusMessage.Text = "Status: Preset copied to clipboard.";
        }

        private void PresetLoad(object sender, EventArgs e)
        {
            DeserializeAndApply(Clipboard.GetText());
        }

        private void ResizeIfNeeded()
        {
            if (WindowState == FormWindowState.Minimized)
                return;

            var newWidth = Deck1ArtistLabel.Size.Width;
            var newHeight = ClientSize.Height < normalHeight ? normalHeight : ClientSize.Height; // just in case

            if (Deck1TitleLabel.Size.Width > newWidth)
                newWidth = Deck1TitleLabel.Size.Width;

            if (Deck1ArtistLabel.Size.Width > newWidth)
                newWidth = Deck1ArtistLabel.Size.Width;

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

            if (StatusMessage.Size.Width > newWidth)
                newWidth = StatusMessage.Size.Width;

            newWidth += Deck1TitleLabel.Location.X + SIZE_PADDING;
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

        private void LoadProperties()
        {
            var settings = Properties.Settings.Default;
            if (pointerPatterns == null)
                pointerPatterns = new Dictionary<string, string>();

            port = settings.port;
            pointerPatterns.Add(MD_KEY, settings.mdPtr);
            pointerPatterns.Add(D1A_KEY, settings.d1ArtistPtr);
            pointerPatterns.Add(D1T_KEY, settings.d1TitlePtr);
            pointerPatterns.Add(D2A_KEY, settings.d2ArtistPtr);
            pointerPatterns.Add(D2T_KEY, settings.d2TitlePtr);
            pointerPatterns.Add(D3A_KEY, settings.d3ArtistPtr);
            pointerPatterns.Add(D3T_KEY, settings.d3TitlePtr);
            pointerPatterns.Add(D4A_KEY, settings.d4ArtistPtr);
            pointerPatterns.Add(D4T_KEY, settings.d4TitlePtr);

            MasterDeckBox.Text = settings.mdPtr;
            Deck1ArtistBox.Text = settings.d1ArtistPtr;
            Deck1TitleBox.Text = settings.d1TitlePtr;
            Deck2ArtistBox.Text = settings.d2ArtistPtr;
            Deck2TitleBox.Text = settings.d2TitlePtr;
            Deck3ArtistBox.Text = settings.d3ArtistPtr;
            Deck3TitleBox.Text = settings.d3TitlePtr;
            Deck4ArtistBox.Text = settings.d4ArtistPtr;
            Deck4TitleBox.Text = settings.d4TitlePtr;
            PortBox.Text = port.ToString();
        }

        private void settingsBtn_Click(object sender, EventArgs e)
        {
            if (!settingsShown)
            {
                var newWidth = ClientSize.Width > settingsWidth ? ClientSize.Width : settingsWidth;
                var newHeight = PresetShareButton.Location.Y + PresetShareButton.Size.Height + SIZE_PADDING;
                var newSize = new System.Drawing.Size(newWidth, newHeight);

                minWidth = settingsWidth;
                ClientSize = newSize;
                settingsBtn.Text = "Hide Settings";
            }
            else
            {
                var newSize = new System.Drawing.Size(ClientSize.Width, normalHeight);

                minWidth = normalWidth;
                ClientSize = newSize;
                ResizeIfNeeded(); // Too lazy to calculate the width again here
                settingsBtn.Text = "Show Settings";
            }

            settingsShown = !settingsShown;
        }

        private void EnterPressed(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                PortChanged(sender, e);
            else
                running = false;
        }

        private void PortChanged(object sender, EventArgs e)
        {
            running = false;
            httpd.Interrupt(); // We have sleeps in the main loop. This wakes the thread from those sleeps, terminating the loop since we set running to false
            tcpListener.Stop();
            var settings = Properties.Settings.Default;

            try
            {
                Int32.TryParse(PortBox.Text, out port);
                settings.port = port;
                running = true;
                httpd = new Thread(HTTPdaemon);
                httpd.Start();
            }
            catch
            {
                StatusMessage.Text = "Status: Problem with the given port.";
                running = false;
            }
        }

        private void PtrBoxChanged(object sender, EventArgs e)
        {
            TextBox SenderBox = (TextBox)sender;
            var settings = Properties.Settings.Default;
            var baseStr = "Status: {0} ptr has been set to: {1}";

            switch (SenderBox.Name)
            {
                case "Deck1ArtistBox":
                    pointerPatterns[D1A_KEY] = SenderBox.Text;
                    settings.d1ArtistPtr = SenderBox.Text;
                    StatusMessage.Text = string.Format(baseStr, "Deck 1 Artist", SenderBox.Text); 
                    break;

                case "Deck1TitleBox":
                    pointerPatterns[D1T_KEY] = SenderBox.Text;
                    settings.d1TitlePtr = SenderBox.Text;
                    StatusMessage.Text = string.Format(baseStr, "Deck 1 Title", SenderBox.Text);
                    break;

                case "Deck2ArtistBox":
                    pointerPatterns[D2A_KEY] = SenderBox.Text;
                    settings.d2ArtistPtr = SenderBox.Text;
                    StatusMessage.Text = string.Format(baseStr, "Deck 2 Artist", SenderBox.Text);
                    break;

                case "Deck2TitleBox":
                    pointerPatterns[D2T_KEY] = SenderBox.Text;
                    settings.d2TitlePtr = SenderBox.Text;
                    StatusMessage.Text = string.Format(baseStr, "Deck 2 Title", SenderBox.Text);
                    break;
                case "Deck3ArtistBox":
                    pointerPatterns[D3A_KEY] = SenderBox.Text;
                    settings.d3ArtistPtr = SenderBox.Text;
                    StatusMessage.Text = string.Format(baseStr, "Deck 3 Artist", SenderBox.Text);
                    break;

                case "Deck3TitleBox":
                    pointerPatterns[D3T_KEY] = SenderBox.Text;
                    settings.d3TitlePtr = SenderBox.Text;
                    StatusMessage.Text = string.Format(baseStr, "Deck 3 Title", SenderBox.Text);
                    break;

                case "Deck4ArtistBox":
                    pointerPatterns[D4A_KEY] = SenderBox.Text;
                    settings.d4ArtistPtr = SenderBox.Text;
                    StatusMessage.Text = string.Format(baseStr, "Deck 4 Artist", SenderBox.Text);
                    break;

                case "Deck4TitleBox":
                    pointerPatterns[D4T_KEY] = SenderBox.Text;
                    settings.d4TitlePtr = SenderBox.Text;
                    StatusMessage.Text = string.Format(baseStr, "Deck 4 Title", SenderBox.Text);
                    break;

                case "MasterDeckBox":
                    pointerPatterns[MD_KEY] = SenderBox.Text;
                    settings.mdPtr = SenderBox.Text;
                    StatusMessage.Text = string.Format(baseStr, "Master Deck", SenderBox.Text);
                    break;
            }

            settings.Save();
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
