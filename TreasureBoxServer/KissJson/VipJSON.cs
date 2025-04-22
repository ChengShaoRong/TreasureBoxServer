/*
 * It's automatic generate by 'KissJSON', DON'T modify this file.
 * You should edit it by 'KissJSON'.
 */
using CSharpLike;
using System.Collections.Generic;
using KissFramework;

namespace TreasureBox
{
    public class VipJSON
    {
        public int vip; //Primary key
        public int exp;
        public string name;
        public string name_zh;
        public string desc;
        public string desc_zh;
        public string icon;
        public string Name => (Framework.config.Language switch {"zh" => name_zh, _ => name });
        public string Desc => (Framework.config.Language switch {"zh" => desc_zh, _ => desc });

        /// <summary>
        /// Get VipJSON object by vip.
        /// If that JSON data wasn't loaded, it will be loaded automatically.
        /// We recommend call this method directly, and no need care about other method.
        /// </summary>
        /// <returns>VipJSON object</returns>
        public static VipJSON Get(int vip) => (IsInitialized ? (KissJson.Get(_type_, vip.ToString()) as VipJSON) : null);
        /// <summary>
        /// Get all keys of the whole JSON.
        /// You can query the whole JSON by this keys.
        /// </summary>
        /// <returns>All keys of the whole JSON</returns>
        public static List<string> GetKeys() => (IsInitialized ? KissJson.GetKeys(_type_) : (new List<string>()));
        /// <summary>
        /// Get VipJSON object by key that from `GetKeys()`.
        /// </summary>
        /// <param name="key">The key from `GetKeys()`</param>
        /// <returns>VipJSON object</returns>
        public static VipJSON GetByKey(string key) => (IsInitialized ? (KissJson.Get(_type_, key) as VipJSON) : null);
        /// <summary>
        /// Get all header names (column names).
        /// </summary>
        /// <returns>All header names (column names)</returns>
        public static List<string> GetHeaders() => (IsInitialized ? KissJson.GetHeaders(_type_) : (new List<string>()));
        /// <summary>
        /// Synchronous initialize VipJSON.
        /// Normally you don't need to call this manually.
        /// </summary>
        public static bool InitializeSynchronous()
        {
            KissJson.Load((object)_type_, System.Environment.CurrentDirectory + "/KissJson/VipJSON.json");
            _IsInitialized_ = true;
            return true;
        }
        /// <summary>
        /// It's for you can reload VipJSON from web. 
        /// e.g. you modified the VipJSON file and want to reload that JSON file while the server is running..
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [WebMethod]
        public static string ReloadVipJSON(string ip)
        {
            Logger.LogInfo($"ReloadVipJSON from {ip}");
            JSONData ret = Framework.CreateReturnJSON();
            if (Framework.IsInvalidIp(ip, ret))
                return ret;
            InitializeSynchronous();

            return ret.ToJson();
        }
        static System.Type _type_ = typeof(VipJSON);
        static bool _IsInitialized_ = false;
        static bool IsInitialized => (_IsInitialized_ ? true : InitializeSynchronous());
    }
}
