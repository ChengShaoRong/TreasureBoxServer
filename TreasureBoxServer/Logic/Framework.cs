
using CSharpLike;
using KissFramework;
using System;
using System.Collections.Generic;
using System.IO;
using KissServerFramework;
using System.Text;

namespace TreasureBox
{
    /// <summary>
    /// All our logic running in main thread, 
    /// You don't need to use 'lock' syntax!
    /// </summary>
    public class Framework : FrameworkBase
    {
        static Framework instance;
        /// <summary>
        /// Singleton instance
        /// </summary>
        public static Framework Instance
        {
            get
            {
                if (instance == null)
                    instance = new Framework();
                return instance;
            }
        }
        public static string NewJSONMsg(string msg)
        {
            JSONData jsonData = JSONData.NewDictionary();
            jsonData["msg"] = msg;
            return jsonData.ToJson();
        }
        /// <summary>
        /// Initialize CSV file, that run before OnStart.
        /// You can call this function by yourself to reload them if your CSV file was moidfied and need to be reload.
        /// </summary>
        public override void InitializeCSV()
        {
            Logger.LogInfo("Framework:InitializeCSV");
            ItemCsv.Init();
            SignInCsv.Init();
            ExpCsv.Init();
            VipCsv.Init();
            CharacterCsv.Init();

            ServerListManager.LoadServerState();
            ServerNoticeManager.LoadServerNotice();
        }
        public static long GetCurrentTimestamp()
        {
            TimeSpan ts = (DateTime.Now - new DateTime(1970, 1, 1));
            return (long)ts.TotalSeconds;
        }

        /// <summary>
        /// Sign JSONData
        /// </summary>
        public static void Sign(JSONData json)
        {
            SortedDictionary<string, JSONData> sortedDics = new SortedDictionary<string, JSONData>();
            json["_time_"] = GetCurrentTimestamp();
            foreach (var one in json.Value as Dictionary<string, JSONData>)
                sortedDics.Add(one.Key, one.Value);
            string str = "";
            foreach (var one in sortedDics)
            {
                if (str.Length == 0)
                {
                    str = one.Key + "=" + one.Value;
                }
                else
                {
                    str += "&" + one.Key + "=" + one.Value;
                }
            }
            str += config.md5VerifyHTTPSign;
            json["_sign_"] = GetMD5(str);
        }
        /// <summary>
        /// Create a return JSON with ["code":0] and ["error":""]
        /// </summary>
        public static JSONData CreateReturnJSON()
        {
            JSONData json = JSONData.NewDictionary();
            json["code"] = 0;
            json["error"] = "";
            return json;
        }
        /// <summary>
        /// Check the ip whether is GM IP
        /// </summary>
        public static bool IsInvalidIp(string ip, JSONData json)
        {
            if (ip != config.gmIP)
            {
                json["code"] = 1;
                json["error"] = "LT_Auth_InvalidIP";
                return true;
            }
            return false;
        }

        /// <summary>
        /// When the KISS framework initialized will call this function, you can call your initialize here.
        /// </summary>
        protected override void OnStart()
        {
            Logger.LogInfo("Framework:OnStart");

            //RaiseEvent(ForceCheckCacheFile, 1f);
        }
        [CommandMethod]
        public static void Reload()
        {
            // Register command 'reload' for reload JSON config file in console
            // Reload the config value from your config JSON file.
            // You can consider as you had reload the value in Config, but exclude the value in ConfigBase (The value in ConfigBase changed but not take effect.).
            FrameworkBase.config = Framework.Instance.CreateConfig(Environment.CurrentDirectory
                + "/" + System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".json");

        }

        /// <summary>
        /// When you input 'exit' or 'quit' in console will call this function.
        /// But if you use 'Ctrl+C' or click 'X' close button won't reach here,
        /// it's not a good way to exit which cause Update/Insert database failed.
        /// Make sure your work done before exit.
        /// When all your work done, call FrameworkBase.MarkAllDoneForSafeExit() 
        /// to notify the framework exit.
        /// </summary>
        protected override void OnNormalExit()
        {
            Logger.LogInfo("Framework:OnNormalExit");
            //Do your work in main thread here.

            //And then do your work in thread, e.g. save data into database.
            //And call FrameworkBase.MarkAllDoneForSafeExit() in that thread.

            FrameworkBase.MarkAllDoneForSafeExit();//You should call this while your work all done.
        }
        /// <summary>
        /// return client ip to client
        /// </summary>
        [WebMethod]
        static string GetSelfIP(string ip)
        {
            return ip;
        }
        #region don't modify it
        /// <summary>
        /// The config instance.
        /// Normally you don't need to modify it.
        /// </summary>
        public static new Config config
        {
            get
            {
                return (Config)FrameworkBase.config;
            }
        }
        /// <summary>
        /// Custom config instance, this function call by FrameworkBase only.
        /// Generally, you don't need to modify it.
        /// </summary>
        /// <param name="strJSON">The config JSON string load from the file
        /// (Environment.CurrentDirectory + "/" + System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".json").
        /// If that file not exist will receive a empty string</param>
        /// <returns>The Config instance</returns>
        protected override ConfigBase CreateConfig(string strJSON)
        {
            return string.IsNullOrEmpty(strJSON) ? new Config() : (Config)KissJson.ToObject(typeof(Config), strJSON);
        }
        /// <summary>
        /// Custom Player instance, this function call by FrameworkBase only.
        /// Normally you don't need to modify it.
        /// </summary>
        protected override PlayerBase CreatePlayer()
        {
            return new Player();
        }
        #endregion
    }
}
