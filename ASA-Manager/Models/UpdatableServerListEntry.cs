using System.Linq;

namespace ARKServerCreationTool.Models
{
    class UpdatableServerListEntry
    {
        public UpdatableServerListEntry(int serverID)
        {
            targetServerID = serverID;
        }

        private ASCTServerConfig targetServer
        {
            get
            {
                return ASCTGlobalConfig.Instance.Servers.Where(s => s.ID == targetServerID).DefaultIfEmpty(null).FirstOrDefault();
            }
        }

        public int targetServerID { get; private set; }

        public string serverName => targetServer.Name;
        public string serverCluster => targetServer.ClusterKey;
        public string serverRunning => targetServer.IsRunningToString;
    }
}

