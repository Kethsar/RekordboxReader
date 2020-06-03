using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace RekordboxReader
{
    public class RRSettings
    {
        private const string SETTINGS_FILE = "RekordboxReader.xml";
        private const int DEFAULT_PORT = 41379;

        public string Name { get; set; } = "<no name>";
        public string Deck1ArtistPointer { get; set; } = "";
        public string Deck1TitlePointer { get; set; } = "";
        public string Deck2ArtistPointer { get; set; } = "";
        public string Deck2TitlePointer { get; set; } = "";
        public string Deck3ArtistPointer { get; set; } = "";
        public string Deck3TitlePointer { get; set; } = "";
        public string Deck4ArtistPointer { get; set; } = "";
        public string Deck4TitlePointer { get; set; } = "";
        public string MasterDeckPointer { get; set; } = "";
        public int Port { get; set; } = DEFAULT_PORT;
        public Dictionary<string, RRPreset> Presets { get; private set; }

        public RRSettings()
        {
            Presets = new Dictionary<string, RRPreset>();
        }

        public bool LoadPreset(string presetName)
        {
            if (presetName != Name && Presets.ContainsKey(presetName))
            {
                var preset = Presets[presetName];
                Name = preset.Name;
                Deck1ArtistPointer = preset.Deck1ArtistPointer;
                Deck1TitlePointer = preset.Deck1TitlePointer;
                Deck2ArtistPointer = preset.Deck2ArtistPointer;
                Deck2TitlePointer = preset.Deck2TitlePointer;
                Deck3ArtistPointer = preset.Deck3ArtistPointer;
                Deck3TitlePointer = preset.Deck3TitlePointer;
                Deck4ArtistPointer = preset.Deck4ArtistPointer;
                Deck4TitlePointer = preset.Deck4TitlePointer;
                MasterDeckPointer = preset.MasterDeckPointer;

                return true;
            }

            return false;
        }

        public virtual XElement SerializeXml()
        {
            var rrsetRoot = new XElement("rrsettings");
            var presetsEle = new XElement("presets");

            rrsetRoot.Add(new XElement("port", Port));
            rrsetRoot.Add(new XElement("name", new XCData(Name)));
            rrsetRoot.Add(new XElement("d1aptr", Deck1ArtistPointer));
            rrsetRoot.Add(new XElement("d1tptr", Deck1TitlePointer));
            rrsetRoot.Add(new XElement("d2aptr", Deck2ArtistPointer));
            rrsetRoot.Add(new XElement("d2tptr", Deck2TitlePointer));
            rrsetRoot.Add(new XElement("d3aptr", Deck3ArtistPointer));
            rrsetRoot.Add(new XElement("d3tptr", Deck3TitlePointer));
            rrsetRoot.Add(new XElement("d4aptr", Deck4ArtistPointer));
            rrsetRoot.Add(new XElement("d4tptr", Deck4TitlePointer));
            rrsetRoot.Add(new XElement("mdptr", MasterDeckPointer));

            foreach (var pre in Presets.Values)
                presetsEle.Add(pre.SerializeXml());

            rrsetRoot.Add(presetsEle);
            return rrsetRoot;
        }

        public static RRSettings DeserializeXml(XElement settingsXml)
        {
            var settings = new RRSettings();

            if (settingsXml.HasElements)
            {
                int xmlPort;
                var preEles = settingsXml.Element("presets").Elements("rrpreset");

                if (int.TryParse(settingsXml.Element("port").Value, out xmlPort))
                    settings.Port = xmlPort;

                settings.Name = settingsXml.Element("name").Value;
                settings.Deck1ArtistPointer = settingsXml.Element("d1aptr").Value;
                settings.Deck1TitlePointer = settingsXml.Element("d1tptr").Value;
                settings.Deck2ArtistPointer = settingsXml.Element("d2aptr").Value;
                settings.Deck2TitlePointer = settingsXml.Element("d2tptr").Value;
                settings.Deck3ArtistPointer = settingsXml.Element("d3aptr").Value;
                settings.Deck3TitlePointer = settingsXml.Element("d3tptr").Value;
                settings.Deck4ArtistPointer = settingsXml.Element("d4aptr").Value;
                settings.Deck4TitlePointer = settingsXml.Element("d4tptr").Value;
                settings.MasterDeckPointer = settingsXml.Element("mdptr").Value;

                foreach (var pre in preEles)
                {
                    var preset = RRPreset.DeserializeXml(pre);
                    settings.Presets[preset.Name] = preset;
                }
            }

            return settings;
        }

        public static string GetSettingsFilename()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), SETTINGS_FILE);
        }

        public static RRSettings LoadFile()
        {
            var settings = new RRSettings();

            try
            {
                var settingsText = File.ReadAllText(GetSettingsFilename());
                var xe = XElement.Parse(settingsText);
                settings = DeserializeXml(xe);
            }
            catch
            { } // eh... settings file probably not found, just give an empty instance

            return settings;
        }

        public void SaveFile()
        {
            var settingsXml = SerializeXml();
            File.WriteAllText(GetSettingsFilename(), settingsXml.ToString());
        }

        public void SavePreset()
        {
            Presets[Name] = new RRPreset(this);
        }
    }

    public class RRPreset : RRSettings
    {
        public RRPreset() { }

        public RRPreset(RRSettings settings)
        {
            var props = typeof(RRSettings).GetProperties();
            foreach (var p in props)
            {
                p.SetValue(this, p.GetValue(settings));
            }
        }

        public override XElement SerializeXml()
        {
            var preroot = new XElement("rrpreset");

            preroot.Add(new XElement("name", new XCData(Name)));
            preroot.Add(new XElement("d1aptr", Deck1ArtistPointer));
            preroot.Add(new XElement("d1tptr", Deck1TitlePointer));
            preroot.Add(new XElement("d2aptr", Deck2ArtistPointer));
            preroot.Add(new XElement("d2tptr", Deck2TitlePointer));
            preroot.Add(new XElement("d3aptr", Deck3ArtistPointer));
            preroot.Add(new XElement("d3tptr", Deck3TitlePointer));
            preroot.Add(new XElement("d4aptr", Deck4ArtistPointer));
            preroot.Add(new XElement("d4tptr", Deck4TitlePointer));
            preroot.Add(new XElement("mdptr", MasterDeckPointer));

            return preroot;
        }

        public static new RRPreset DeserializeXml(XElement presetXml)
        {
            var preset = new RRPreset();

            if (presetXml.HasElements)
            {
                preset.Name = presetXml.Element("name").Value;
                preset.Deck1ArtistPointer = presetXml.Element("d1aptr").Value;
                preset.Deck1TitlePointer = presetXml.Element("d1tptr").Value;
                preset.Deck2ArtistPointer = presetXml.Element("d2aptr").Value;
                preset.Deck2TitlePointer = presetXml.Element("d2tptr").Value;
                preset.Deck3ArtistPointer = presetXml.Element("d3aptr").Value;
                preset.Deck3TitlePointer = presetXml.Element("d3tptr").Value;
                preset.Deck4ArtistPointer = presetXml.Element("d4aptr").Value;
                preset.Deck4TitlePointer = presetXml.Element("d4tptr").Value;
                preset.MasterDeckPointer = presetXml.Element("mdptr").Value;
            }

            return preset;
        }
    }
}
