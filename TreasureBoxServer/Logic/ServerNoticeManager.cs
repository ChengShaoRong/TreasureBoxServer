using CSharpLike;
using KissFramework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TreasureBox
{
    public class ServerNoticeManager
    {
        public static void LoadServerNotice()
        {
            try
            {
                mServerNotice = KissJson.ToJSONData(File.ReadAllText(Environment.CurrentDirectory + "/serverNotice.json", new UTF8Encoding(false))).ToJson();
                Logger.LogInfo($"New mServerNotice = {mServerNotice}");
            }
            catch (Exception e)
            {
                Logger.LogError($"ServerNoticeManager load file error:{e.Message}");
            }
        }
        static string mServerNotice = "";
        [WebMethod]
        public static string GetServerNotice(string ip)
        {
            Logger.LogInfo($"GetServerNotice from {ip}");

            return mServerNotice;
        }
        [WebMethod]
        public static string ChangeServerNotice(string newNotice, string ip, Action<string> delayCallback)
        {
            Logger.LogInfo($"ChangeServerNotice: {newNotice} from {ip}");
            JSONData ret = Framework.CreateReturnJSON();
            if (Framework.IsInvalidIp(ip, ret))
                return ret;
            string oldNotice = mServerNotice;
            try
            {
                JSONData newJSON = KissJson.ToJSONData(newNotice);
                mServerNotice = newJSON.ToJson();
            }
            catch(Exception e)
            {
                ret["code"] = 2;
                ret["error"] = $"Invalid JSON: {e.Message}";
                return ret.ToJson();
            }
            new ThreadPoolEvent(() =>
            {
                try
                {
                    File.WriteAllText(Environment.CurrentDirectory + "/serverNotice.json", mServerNotice, new UTF8Encoding(false));
                }
                catch (Exception e)
                {
                    ret["code"] = 3;
                    ret["error"] = $"Save json error: {e.Message}";
                    mServerNotice = oldNotice;
                    delayCallback(ret.ToJson());
                }
            }
            ,
            () =>
            {
            });

            return "";
        }
        [WebMethod]
        public static string ReloadServerNotice(string ip, Action<string> delayCallback)
        {
            Logger.LogInfo($"ReloadServerNotice from {ip}");
            JSONData ret = Framework.CreateReturnJSON();
            if (Framework.IsInvalidIp(ip, ret))
                return ret;
            new ThreadPoolEvent(() =>
            {
                try
                {
                    mServerNotice = File.ReadAllText(Environment.CurrentDirectory + "/serverNotice.json", new UTF8Encoding(false));
                    Logger.LogInfo($"New mServerNotice = {mServerNotice}");
                }
                catch(Exception e)
                {
                    ret["code"] = 2;
                    ret["error"] = $"Read file error: {e.Message}";
                }
            }
            ,
            () =>
            {
                delayCallback(ret.ToJson());
            });

            return "";
        }
    }
}
