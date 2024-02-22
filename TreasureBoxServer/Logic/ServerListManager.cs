using CSharpLike;
using KissFramework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TreasureBox
{
    public class ServerListManager
    {
        public enum ServerState
        {
            Normal,
            Full,
            Maintenance,
            Close
        }
        class ServerInfo
        {
            public int id;
            public int state;
        }
        static int recommendServerId = 0;
        static Dictionary<int, ServerInfo> servers = new Dictionary<int, ServerInfo>();
        static bool change = true;
        static string mServerList = "";
        static string ServerList
        {
            get
            {
                if (change)
                {
                    change = false;
                    JSONData listServer = JSONData.NewList();
                    foreach (ServerInfo one in servers.Values)
                    {
                        JSONData json = KissJson.ToJSONData(one);
                        if (json["id"] == recommendServerId)
                            json["recommend"] = 1;
                        listServer.Add(json);
                    }
                    mServerList = listServer.ToJson();
                }
                return mServerList;
            }
        }
        [WebMethod]
        public static string GetServerList(string ip)
        {
            Logger.LogInfo($"GetServerList from {ip}");

            return ServerList;
        }
        [WebMethod]
        public static string UpdateServerState(string ip, int serverId, int state)
        {
            Logger.LogInfo($"UpdateServerState from {ip}, serverId={serverId}, state={state}");
            JSONData ret = Framework.CreateReturnJSON();
            if (Framework.IsInvalidIp(ip, ret))
                return ret;
            if (servers.TryGetValue(serverId, out ServerInfo server))
            {
                server.state = state;
                change = true;
                SaveServerState();
            }
            else
            {
                ret["code"] = 1;
                ret["error"] = $"not exist server id {serverId}";
            }

            return ret.ToJson();
        }
        [WebMethod]
        public static string ChangeServerRecommend(string ip, int serverId)
        {
            Logger.LogInfo($"ChangeServerRecommend from {ip}, serverId={serverId}");
            JSONData ret = Framework.CreateReturnJSON();
            if (Framework.IsInvalidIp(ip, ret))
                return ret;
            if (recommendServerId != serverId)
            {
                recommendServerId = serverId;
                change = true;
                SaveServerState();
            }
            else
            {
                ret["code"] = 1;
                ret["error"] = $"same serverId, no change.";
            }

            return ret.ToJson();
        }
        static void SaveServerState()
        {
            Framework.RaiseUniqueEvent(() =>
            {
                JSONData serverState = JSONData.NewDictionary();
                foreach (ServerInfo one in servers.Values)
                    serverState[one.id + ""] = one.state;
                serverState["recommend"] = recommendServerId;
                string strJSON = serverState.ToJson();
                new ThreadPoolEvent(() =>
                {
                    File.WriteAllText(Environment.CurrentDirectory + "/serverState.json", strJSON, new UTF8Encoding(false));
                }
                ,
                () =>
                {
                });
            },
            "ServerListManagerSaveJSON", 2f, 1);
        }
        public static void LoadServerState()
        {
            JSONData jsonData = null;
            new ThreadPoolEvent(() =>
            {
                string strJSON = File.ReadAllText(Environment.CurrentDirectory + "/serverState.json", new UTF8Encoding(false));
                jsonData = KissJson.ToJSONData(strJSON);
            }
            ,
            () =>
            {
                if (jsonData != null)
                {
                    servers.Clear();
                    foreach (var one in jsonData.Value as Dictionary<string,JSONData>)
                    {
                        if (one.Key == "recommend")
                            recommendServerId = one.Value;
                        else
                        {
                            ServerInfo si = new ServerInfo();
                            si.id = Convert.ToInt32(one.Key);
                            si.state = one.Value;
                            servers[si.id] = si;
                        }
                    }
                }
            });
        }
    }
}
