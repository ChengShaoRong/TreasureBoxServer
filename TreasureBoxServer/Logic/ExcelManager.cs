using CSharpLike;
using KissFramework;
using System;
using System.Collections.Generic;
using System.IO;

namespace TreasureBox
{
    public class ExcelManager
    {
        /// <summary>
        /// Clear all loaded Excel.
        /// Normally you should call this manually after you initializ your hot update script..
        /// </summary>
        public static void ClearAll()
        {
            object o = null;
            KissJson.Clear(o);
            mLoadStates.Clear();
        }
        /// <summary>
        /// Get a row data from Excel by key.
        /// You must make sure that data was initialized.
        /// </summary>
        /// <param name="type">Type of your data. e.g. typeof(ItemJSON)</param>
        /// <param name="key">The unique key of your data. That is string type.</param>
        /// <returns>Data ojbect</returns>
        public static object Get(object type, string key)
        {
            return KissJson.Get(type, key);
        }
        /// <summary>
        /// Get JSONData object from Excel data.
        /// If that Excel data wasn't loaded, it will load it automatically.
        /// </summary>
        /// <param name="fileName">File name of the Excel file name</param>
        /// <param name="key">Row key</param>
        /// <param name="column">Column name</param>
        /// <returns>JSONData object</returns>
        public static JSONData GetJSON(string fileName, string key, string column)
        {
            return KissJson.GetJSON(fileName, key, column);
        }
        /// <summary>
        /// Synchronizing load all Excel data by Type.
        /// </summary>
        /// <param name="type">Type of your data. e.g. typeof(ItemJSON)</param>
        /// <param name="fileName">File name in AssetBundle</param>
        public static void Load(object type, string fileName)
        {
            mLoadStates[type] = false;
            DateTime startTime = DateTime.Now;
            byte[] buff = File.ReadAllBytes(fileName);
            if (buff != null)
            {
                Logger.LogInfo($"Load `{fileName}` from AssetBundle use time {DateTime.Now.Subtract(startTime).TotalMilliseconds} ms");
                startTime = DateTime.Now;
                if (type is string)
                    KissJson.Load(type as string, KissJson.ToJSONData(buff));
                else
                    KissJson.Load(type, KissJson.ToJSONData(buff));
                Logger.LogInfo($"Convert `{fileName}` use time {DateTime.Now.Subtract(startTime).TotalMilliseconds} ms");
                mLoadStates[type] = true;
            }
            else
                Logger.LogError($"Load `{fileName}` from AssetBundle error");
        }
        static Dictionary<object, bool> mLoadStates = new Dictionary<object, bool>();
    }
}