
using CSharpLike;
using System.Collections.Generic;
using KissFramework;

namespace TreasureBox
{

    /// <summary>
    /// Define a class for the CSV table (Item).
    /// </summary>
    public class ItemCsv
    {
        public int id;

        /// <summary>
        /// Get the localization name
        /// </summary>
        public string Name
        {
            get
            {
                return Framework.config.Language == "EN" ? nameEN : nameCN;
            }
        }
        public string nameEN;
        public string nameCN;
        /// <summary>
        /// Get the localization describe
        /// </summary>
        public string Desc
        {
            get
            {
                return Framework.config.Language == "EN" ? descEN : descCN;
            }
        }
        public string descEN;
        public string descCN;
        public string icon;
        public string frame;
        public int itemType;
        public int maxStack;
        public int sellPrice;
        public static ItemCsv Get(int id)
        {
            return KissCSV.Get("Item.csv", id) as ItemCsv;
        }
        public static void Init()
        {
            KissCSV.Load(typeof(ItemCsv), "Item.csv", "id");
        }
        [WebMethod]
        public static string ReloadItemCSV(string ip)
        {
            Logger.LogInfo($"ReloadItemCSV from {ip}");
            JSONData ret = Framework.CreateReturnJSON();
            if (Framework.IsInvalidIp(ip, ret))
                return ret;
            Init();

            return ret.ToJson();
        }
    }
}
