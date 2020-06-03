using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RekordboxReader
{
    public partial class SettingsForm : Form
    {
        private RRSettings _settings;
        private bool loading = false;

        public SettingsForm(RRSettings settings)
        {
            _settings = settings;
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            loading = true;

            PresetComboBox.Items.AddRange(_settings.Presets.Keys.ToArray());

            var index = PresetComboBox.FindString(_settings.Name);
            if (index >= 0)
            {
                PresetComboBox.SelectedIndex = index;
            }
            else
            {
                PresetComboBox.Items.Add(_settings.Name);
                PresetComboBox.SelectedItem = _settings.Name;
            }

            PortBox.Text = _settings.Port.ToString();
            Deck1ArtistBox.Text = _settings.Deck1ArtistPointer;
            Deck1TitleBox.Text = _settings.Deck1TitlePointer;
            Deck2ArtistBox.Text = _settings.Deck2ArtistPointer;
            Deck2TitleBox.Text = _settings.Deck2TitlePointer;
            Deck3ArtistBox.Text = _settings.Deck3ArtistPointer;
            Deck3TitleBox.Text = _settings.Deck3TitlePointer;
            Deck4ArtistBox.Text = _settings.Deck4ArtistPointer;
            Deck4TitleBox.Text = _settings.Deck4TitlePointer;
            MasterDeckBox.Text = _settings.MasterDeckPointer;

            loading = false;
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
                        Deck3ArtistBox.Text = "";
                        Deck3TitleBox.Text = "";
                        Deck4ArtistBox.Text = "";
                        Deck4TitleBox.Text = "";
                        StatusMessage.Text = "Preset loaded from clipboard";
                    }
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
                        StatusMessage.Text = "Preset loaded from clipboard";
                    }
                    break;
            }
        }

        private void SettingChanged(object sender, EventArgs e)
        {
            if (loading) return;

            var SenderBox = (TextBox)sender;
            typeof(RRSettings).GetProperty(SenderBox.Tag.ToString()).SetValue(_settings, SenderBox.Text);
            ((MainForm)Owner).SetPointer(SenderBox.Tag.ToString());
            StatusMessage.Text = string.Format("{0} set to {1}", SenderBox.Tag, SenderBox.Text);
        }

        private void PresetNameChanged(object sender, EventArgs e)
        {
            if (loading) return;

            _settings.Name = PresetComboBox.Text;
        }

        private void PortKeyPressed(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                ((MainForm)Owner).PortChanged();
        }

        private void PortChanged(object sender, EventArgs e)
        {
            int port;
            int.TryParse(PortBox.Text, out port);

            _settings.Port = port;
        }

        private void PresetSelected(object sender, EventArgs e)
        {
            if (_settings.LoadPreset(PresetComboBox.SelectedItem.ToString()))
            {
                loading = true;

                Deck1ArtistBox.Text = _settings.Deck1ArtistPointer;
                Deck1TitleBox.Text = _settings.Deck1TitlePointer;
                Deck2ArtistBox.Text = _settings.Deck2ArtistPointer;
                Deck2TitleBox.Text = _settings.Deck2TitlePointer;
                Deck3ArtistBox.Text = _settings.Deck3ArtistPointer;
                Deck3TitleBox.Text = _settings.Deck3TitlePointer;
                Deck4ArtistBox.Text = _settings.Deck4ArtistPointer;
                Deck4TitleBox.Text = _settings.Deck4TitlePointer;
                MasterDeckBox.Text = _settings.MasterDeckPointer;
                _settings.SaveFile();
                ((MainForm)Owner).ReloadSettings();

                StatusMessage.Text = string.Format("Preset {0} loaded", _settings.Name);
                loading = false;
            }
        }

        private void PresetSaveBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_settings.Name))
            {
                StatusMessage.Text = "Preset name cannot be empty";
                return;
            }

            _settings.SavePreset();
            _settings.SaveFile();
            StatusMessage.Text = string.Format("Preset {0} saved", _settings.Name);
        }

        public byte[] Compress(string key)
        {
            var KeyByteArray = Encoding.UTF8.GetBytes(key);
            var OutputStream = new MemoryStream();

            using (var Gzip = new System.IO.Compression.GZipStream(OutputStream, System.IO.Compression.CompressionMode.Compress))
            {
                Gzip.Write(KeyByteArray, 0, KeyByteArray.Length);
            }

            return OutputStream.ToArray();
        }

        public string Decompress(string key)
        {
            var KeyByteArray = Convert.FromBase64String(key);
            var OutStream = new MemoryStream();

            using (var Gzip = new System.IO.Compression.GZipStream(new MemoryStream(KeyByteArray), System.IO.Compression.CompressionMode.Decompress))
            {
                Gzip.CopyTo(OutStream);
                Gzip.Close();
            }

            return Encoding.UTF8.GetString(OutStream.ToArray());
        }

        private void DeserializeAndApply(string Key)
        {
            if (Key.Length <= 4 || Key.Substring(0, 4) != "!RR!")
            {
                StatusMessage.Text = "Faulty preset code";
                return;
            }
            Key = Key.Substring(4);

            var DGString = Decompress(Key);
            var SString = DGString.Split(new string[] { "!!" }, StringSplitOptions.None);
            SettingsApplier(SString);
        }

        private void PresetShare(object sender, EventArgs e)
        {
            var GPreset = Compress(Serialize());
            var BPreset = "!RR!" + Convert.ToBase64String(GPreset);
            Clipboard.Clear();
            Clipboard.SetText(BPreset);
            StatusMessage.Text = "Preset copied to clipboard";
        }

        private void PresetLoad(object sender, EventArgs e)
        {
            DeserializeAndApply(Clipboard.GetText());
        }
    }
}
