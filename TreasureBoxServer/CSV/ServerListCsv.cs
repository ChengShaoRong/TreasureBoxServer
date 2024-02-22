
using CSharpLike;
using KissFramework;
using System.Collections.Generic;

namespace TreasureBox
{

    /// <summary>
    /// Define a class for the CSV table (Server List).
    /// </summary>
    public class ServerListCsv
    {
        public int id;
        public string name;
        public string webSocket;
        public string socketIp;
        public int socketPort;
        [WebMethod]
        public static string ReloadServerListCSV(string ip)
        {
            Logger.LogInfo($"ReloadServerListCSV from {ip}");
            JSONData ret = Framework.CreateReturnJSON();
            if (Framework.IsInvalidIp(ip, ret))
                return ret;
            KissCSV.Load(typeof(ServerListCsv), "ServerList.csv", "id");

            return ret.ToJson();
        }
    }
}
