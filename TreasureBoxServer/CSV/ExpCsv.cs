
using CSharpLike;
using KissFramework;
using System.Collections.Generic;

namespace TreasureBox
{

    /// <summary>
    /// Define a class for the CSV table (Exp).
    /// </summary>
    public class ExpCsv
    {
        public int lv;
        public int type;
        public int exp;

        public static int GetExp(int lv, int type)
        {
            ExpCsv csv = Get(lv, type);
            if (csv != null)
                return csv.exp;
            return 99999999;
        }
        public static ExpCsv Get(int lv, int type)
        {
            return KissCSV.Get("Exp.csv", lv.ToString(), type.ToString()) as ExpCsv;
        }
        public static void Init()
        {
            KissCSV.Load(typeof(ExpCsv), "Exp.csv", "lv", null, "type");
        }

        [WebMethod]
        public static string ReloadExpCSV(string ip)
        {
            Logger.LogInfo($"ReloadExpCSV from {ip}");
            JSONData ret = Framework.CreateReturnJSON();
            if (Framework.IsInvalidIp(ip, ret))
                return ret;
            Init();

            return ret.ToJson();
        }
    }
}
