using ARKServerCreationTool.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ARKServerCreationTool.Models
{
    public class ASCTGlobalConfig
    {
        [JsonIgnore] public static ASCTGlobalConfig _instance = null;

        [JsonIgnore]
        public static ASCTGlobalConfig Instance
        {
            get
            {
                if (_instance == null)
                    _instance = LoadConfig();

                return _instance;
            }
        }

        public static void NewConfig()
        {
            if (Instance == null)
            {
                _instance = new ASCTGlobalConfig();
            }
        }

        [JsonIgnore] public const string configName = "ASCTGlobalConfig.json";

        [JsonIgnore] public const string relativeEXEPath = @"ShooterGame\Binaries\Win64\ArkAscendedServer.exe";
        [JsonIgnore] public string depotDownloaderFolder = "depotdownloader";

        [JsonIgnore] public string depotDownloaderURL =
            @"https://github.com/SteamRE/DepotDownloader/releases/latest/download/DepotDownloader-framework.zip";

        [JsonIgnore] public string depotDownloaderExe = "DepotDownloader.exe";
        [JsonIgnore] public ulong serverAppID = 2430930;

        [JsonIgnore] public static readonly string[] maps = new string[]
        {
            "TheIsland_WP", "ScorchedEarth_WP", "thecenter_wp", "Aberration_WP", "Extinction_WP", "Astraeos_WP",
            "Ragnarok_WP", "Valguero_WP", "LostColony_WP"
        };

        public string ServersInstallationPath { get; set; } =
            Path.Combine(Directory.GetCurrentDirectory(), "InstalledServers");

        public string GlobalClusterDir { get; set; } =
            Path.Combine(Directory.GetCurrentDirectory(), "InstalledServers", "ClusterData");

        public bool AutomaticallyCreateFirewallRule { get; set; } = true;

        public bool AllowAutomaticStart { get; set; } = true;
        public bool PromptStartAllServersInCluster { get; set; } = true;

        public ushort AutoStartStaggerTime { get; set; } = 30;

        public ushort StartingGamePort { get; set; } = 7777;
        public ushort StartingRCONPort { get; set; } = 27015;

        public ushort PortIncrement { get; set; } = 10;

        public bool validateUpdates = true;

        public List<ASCTServerConfig> Servers = new List<ASCTServerConfig>();

        public ushort NextAvailablePort()
        {
            return (ushort)(Servers.Select(s => s.GamePort).DefaultIfEmpty((ushort)(StartingGamePort - PortIncrement))
                .Max() + PortIncrement);
        }

        public int NextAvailableID()
        {
            return Servers.Select(s => s.ID).DefaultIfEmpty(-1).Max() + 1;
        }

        public void Save()
        {
            SaveConfig(this);
        }

        private static void SaveConfig(ASCTGlobalConfig config)
        {
            File.WriteAllText(ConfigurationStorage.GetPath(configName), JsonConvert.SerializeObject(config, Formatting.Indented));
        }

        public static ASCTGlobalConfig LoadConfig()
        {
            ASCTGlobalConfig returnConfig = null;

            string configPath = ConfigurationStorage.GetPath(configName);

            if (File.Exists(configPath))
            {
                string json = File.ReadAllText(configPath);

                returnConfig = JsonConvert.DeserializeObject<ASCTGlobalConfig>(json);
            }

            return returnConfig;
        }
    }
}
