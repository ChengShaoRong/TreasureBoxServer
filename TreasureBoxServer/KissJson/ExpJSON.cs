/*
 * It's automatic generate by 'KissJSON', DON'T modify this file.
 * You should edit it by 'KissJSON'.
 */
using CSharpLike;
using System.Collections.Generic;
using KissFramework;

namespace TreasureBox
{
    public class ExpJSON
    {
        public int lv; //Primary key
        public int type; //Primary key
        public int exp;

        /// <summary>
        /// Get ExpJSON object by lv and type.
        /// If that JSON data wasn't loaded, it will be loaded automatically.
        /// We recommend call this method directly, and no need care about other method.
        /// </summary>
        /// <returns>ExpJSON object</returns>
        public static ExpJSON Get(int lv, int type) => (IsInitialized ? (KissJson.Get(_type_, lv.ToString() + "_" + type.ToString()) as ExpJSON) : null);
        /// <summary>
        /// Get all keys of the whole JSON.
        /// You can query the whole JSON by this keys.
        /// </summary>
        /// <returns>All keys of the whole JSON</returns>
        public static List<string> GetKeys() => (IsInitialized ? KissJson.GetKeys(_type_) : (new List<string>()));
        /// <summary>
        /// Get ExpJSON object by key that from `GetKeys()`.
        /// </summary>
        /// <param name="key">The key from `GetKeys()`</param>
        /// <returns>ExpJSON object</returns>
        public static ExpJSON GetByKey(string key) => (IsInitialized ? (KissJson.Get(_type_, key) as ExpJSON) : null);
        /// <summary>
        /// Get all header names (column names).
        /// </summary>
        /// <returns>All header names (column names)</returns>
        public static List<string> GetHeaders() => (IsInitialized ? KissJson.GetHeaders(_type_) : (new List<string>()));
        /// <summary>
        /// Synchronous initialize ExpJSON.
        /// Normally you don't need to call this manually.
        /// </summary>
        public static bool InitializeSynchronous()
        {
            KissJson.Load((object)_type_, System.Environment.CurrentDirectory + "/KissJson/ExpJSON.json");
            _IsInitialized_ = true;
            return true;
        }
        /// <summary>
        /// It's for you can reload ExpJSON from web. 
        /// e.g. you modified the ExpJSON file and want to reload that JSON file while the server is running..
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [WebMethod]
        public static string ReloadExpJSON(string ip)
        {
            Logger.LogInfo($"ReloadExpJSON from {ip}");
            JSONData ret = Framework.CreateReturnJSON();
            if (Framework.IsInvalidIp(ip, ret))
                return ret;
            InitializeSynchronous();

            return ret.ToJson();
        }
        static System.Type _type_ = typeof(ExpJSON);
        static bool _IsInitialized_ = false;
        static bool IsInitialized => (_IsInitialized_ ? true : InitializeSynchronous());
    }
}
