using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ARKServerCreationTool.Services;

namespace ARKServerCreationTool.Models
{
    public class ASCTServerConfig
    {
        public ASCTServerConfig(int ID, ushort GamePort)
        {
            this.ID = ID;
            this.Name = $"ASA Server {ID}";
            this.GameDirectory = $"ASA Server {ID}";
            this.GamePort = GamePort;
        }

        [JsonIgnore] internal GameProcessManager ProcessManager => GameProcessManager.GetGameProcessManager(this.ID);
        [JsonIgnore] public bool IsRunning => ProcessManager.IsRunning;
        [JsonIgnore] public string IsRunningToString => IsRunning ? "Running" : "Stopped";

        public bool StartAutomatically { get; set; } = false;

        public int ID { get; private set; } //to be used as a primary key
        public string Name { get; set; } //friendly name for identifying the server to the user

        public string ClusterKey { get; set; } = string.Empty; //The cluster targetServerID 

        public string GameDirectory { get; set; } //Path to where the server is located

        [JsonIgnore]
        public string EXEPath
        {
            get { return Path.Combine(GameDirectory, ASCTGlobalConfig.relativeEXEPath); }
        }

        public bool UseMultihome { get; set; } =
            false; //Whether the server should be launched with the MultihomeArgs argument

        public string IPAddress { get; set; } = string.Empty; //The IP address to pass to multihome
        public ushort GamePort { get; set; } //main gameport

        public string Map { get; set; } = "TheIsland_WP"; //Map that will be loaded

        public bool useCustomLaunchArgs { get; set; } = false; //Whether to use the user provided launch arguments
        public string customLaunchArgs { get; set; } = string.Empty; //user provided launch arguments

        public bool NoBattleye { get; set; } = false;

        public uint Slots { get; set; } = 70;

        public bool AllowCrossplay { get; set; } = false;

        public string ActiveEvent { get; set; } = string.Empty;

        public HashSet<ulong> modIDs = new HashSet<ulong>();
        public HashSet<string> userLaunchArguments = new HashSet<string>();

        [JsonIgnore]
        public string LaunchArguments
        {
            get
            {
                if (useCustomLaunchArgs)
                {
                    return customLaunchArgs;
                }
                else
                {
                    return
                        $"\"{Map}{MultihomeArgs}\" \"-port={GamePort}\" -WinLiveMaxPlayers={Slots}{ModArgs}{ClusterArgs}{CrossplayArgs}{NoBattleyeArgs}{ActiveEventArgs}{userlaunchargs} -log -servergamelog"
                            .Trim();
                }
            }
        }

        [JsonIgnore]
        public string NoBattleyeArgs
        {
            get { return NoBattleye ? " -NoBattlEye" : string.Empty; }
        }
        [JsonIgnore]
        public string userlaunchargs
        {
            get
            {
                if (userLaunchArguments.Count <= 0)
                {
                    return string.Empty;
                }

                return $" {string.Join(" ", userLaunchArguments)}";
        }
        }

        [JsonIgnore]
        public string ModArgs
        {
            get
            {
                if (modIDs.Count <= 0)
                {
                    return string.Empty;
                }

                return $" \"-mods={string.Join(",", modIDs)}\"";
            }
        }

        [JsonIgnore]
        public string ClusterArgs
        {
            get
            {
                if (ClusterKey == string.Empty)
                {
                    return string.Empty;
                }

                return
                    $" \"-clusterid={ClusterKey}\" \"-ClusterDirOverride={ASCTGlobalConfig.Instance.GlobalClusterDir}\"";
            }
        }

        [JsonIgnore]
        public string MultihomeArgs
        {
            get
            {
                if (UseMultihome && IPAddress != string.Empty)
                {
                    return $"?MultiHome={this.IPAddress}";
                }

                else
                {
                    return string.Empty;
                }
            }
        }

        [JsonIgnore]
        public string CrossplayArgs
        {
            get { return AllowCrossplay ? " -ServerPlatform=ALL" : string.Empty; }
        }

        [JsonIgnore]
        public string ActiveEventArgs
        {
            get { return !string.IsNullOrEmpty(ActiveEvent) ? $" -ActiveEvent={ActiveEvent}" : string.Empty; }
        }
    }
}