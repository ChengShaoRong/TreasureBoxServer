/*
 * It's automatic generate by 'KissJSON', DON'T modify this file.
 * You should edit it by 'KissJSON'.
 */
using CSharpLike;
using System.Collections.Generic;
using KissFramework;

namespace TreasureBox
{
    public class LocalizationJSON
    {
        public string key; //Primary key
        public string content;
        public string content_zh;
        public string Content => (Framework.config.Language switch {"zh" => content_zh, _ => content });

        /// <summary>
        /// Get LocalizationJSON object by key.
        /// If that JSON data wasn't loaded, it will be loaded automatically.
        /// We recommend call this method directly, and no need care about other method.
        /// </summary>
        /// <returns>LocalizationJSON object</returns>
        public static LocalizationJSON Get(string key) => (IsInitialized ? (KissJson.Get(_type_, key.ToString()) as LocalizationJSON) : null);
        /// <summary>
        /// Get all keys of the whole JSON.
        /// You can query the whole JSON by this keys.
        /// </summary>
        /// <returns>All keys of the whole JSON</returns>
        public static List<string> GetKeys() => (IsInitialized ? KissJson.GetKeys(_type_) : (new List<string>()));
        /// <summary>
        /// Get LocalizationJSON object by key that from `GetKeys()`.
        /// </summary>
        /// <param name="key">The key from `GetKeys()`</param>
        /// <returns>LocalizationJSON object</returns>
        public static LocalizationJSON GetByKey(string key) => (IsInitialized ? (KissJson.Get(_type_, key) as LocalizationJSON) : null);
        /// <summary>
        /// Get all header names (column names).
        /// </summary>
        /// <returns>All header names (column names)</returns>
        public static List<string> GetHeaders() => (IsInitialized ? KissJson.GetHeaders(_type_) : (new List<string>()));
        /// <summary>
        /// Synchronous initialize LocalizationJSON.
        /// Normally you don't need to call this manually.
        /// </summary>
        public static bool InitializeSynchronous()
        {
            KissJson.Load((object)_type_, System.Environment.CurrentDirectory + "/KissJson/LocalizationJSON.json");
            _IsInitialized_ = true;
            return true;
        }
        /// <summary>
        /// It's for you can reload LocalizationJSON from web. 
        /// e.g. you modified the LocalizationJSON file and want to reload that JSON file while the server is running..
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [WebMethod]
        public static string ReloadLocalizationJSON(string ip)
        {
            Logger.LogInfo($"ReloadLocalizationJSON from {ip}");
            JSONData ret = Framework.CreateReturnJSON();
            if (Framework.IsInvalidIp(ip, ret))
                return ret;
            InitializeSynchronous();

            return ret.ToJson();
        }
        static System.Type _type_ = typeof(LocalizationJSON);
        static bool _IsInitialized_ = false;
        static bool IsInitialized => (_IsInitialized_ ? true : InitializeSynchronous());
    }
}
