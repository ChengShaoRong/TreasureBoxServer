using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Globalization;

namespace CSharpLike
{
    /// <summary>
    /// NetObject Utils
    /// </summary>
	public class NetObjectUtils
	{
        /// <summary>
        /// Convert string to List, that using 'CultureInfo.InvariantCulture'.
        /// e.g. '1,2,3,4' mean List '{1,2,3,4}'
        /// </summary>
        /// <typeparam name="T">Type of List</typeparam>
        /// <param name="strs">String format split by ','</param>
        /// <returns>List</returns>
        public static List<T> StringToList<T>(string strs)
        {
            List<T> ret = new List<T>();
            if (!string.IsNullOrEmpty(strs))
            {
                IList list = ret;
                Type type = typeof(T);
                foreach (string str in type.Name == "String" ? SplitString(ref strs) : strs.Split(','))
                {
                    list.Add(ConvertValue(type, str));
                }
            }
            return ret;
        }
        /// <summary>
        /// Convert List to string, that using 'CultureInfo.InvariantCulture'.
        /// e.g. List '{1,2,3,4}' mean '1,2,3,4'
        /// </summary>
        /// <param name="list">Source list</param>
        /// <returns>string</returns>
        public static string ListToString(IList list)
        {
            if (list != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (object item in list)
                {
                    AppendString(sb, item);
                }
                if (sb.Length > 0)
                    sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }
            return "";
        }
        /// <summary>
        /// Convert string to Dictionary, that using 'CultureInfo.InvariantCulture'.
        /// e.g. '1,2,3,4' mean Dictionary '{{1,2},{3,4}}'
        /// </summary>
        /// <typeparam name="T">Type of Dictionary key</typeparam>
        /// <typeparam name="K">Type of Dictionary value</typeparam>
        /// <param name="strs">String format split by ','.</param>
        /// <returns>Dictionary</returns>
        public static Dictionary<T, K> StringToDictionary<T, K>(string strs)
        {
            Dictionary<T, K> ret = new Dictionary<T, K>();
            if (!string.IsNullOrEmpty(strs))
            {
                IDictionary dic = ret;
                Type typeT = typeof(T);
                Type typeK = typeof(K);
                string[] strs2 = (typeT.Name != "String" && typeK.Name != "String") ? strs.Split(',') : SplitString(ref strs);
                int lenght = strs2.Length - 2;
                for (int i = 0; i <= lenght; i += 2)
                {
                    dic[ConvertValue(typeT, strs2[i])] = ConvertValue(typeK, strs2[i + 1]);
                }
            }
            return ret;
        }
        /// <summary>
        /// Convert Dictionary to string, that using 'CultureInfo.InvariantCulture'.
        /// e.g. Dictionary '{{1,2},{3,4}}' mean '1,2,3,4'
        /// </summary>
        /// <param name="dictionary">Source list</param>
        /// <returns>string</returns>
        public static string DictionaryToString(IDictionary dictionary)
        {
            if (dictionary != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (object key in dictionary.Keys)
                {
                    AppendString(sb, key);
                    AppendString(sb, dictionary[key]);
                }
                if (sb.Length > 0)
                    sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }
            return "";
        }
        #region internal_imp
        private static string[] SplitString(ref string strs)
        {
            List<string> ret = new List<string>();
            int lenght = strs.Length;
            if (lenght > 0)
            {
                StringBuilder sb = new StringBuilder();
                bool bQuotes = false;
                for (int i = 0; i < lenght; i++)
                {
                    char c = strs[i];
                    switch (c)
                    {
                        case '"':
                            if (bQuotes)
                            {
                                if (i + 1 < lenght && strs[i + 1] == '"')
                                {
                                    i++;
                                    sb.Append(c);
                                }
                                else
                                    bQuotes = false;
                            }
                            else
                            {
                                bQuotes = true;
                            }
                            break;
                        case ',':
                            if (bQuotes)
                            {
                                sb.Append(c);
                            }
                            else
                            {
                                ret.Add(sb.ToString());
                                sb.Clear();
                            }
                            break;
                        default:
                            sb.Append(c);
                            break;
                    }
                }
                ret.Add(sb.ToString());
            }
            return ret.ToArray();
        }
        private static object ConvertValue(Type type, string str)
        {
            return type.Name switch
            {
                "Int32" => Convert.ToInt32(str),
                "UInt32" => Convert.ToUInt32(str),
                "Int16" => Convert.ToInt16(str),
                "UInt16" => Convert.ToUInt16(str),
                "Int64" => Convert.ToInt64(str),
                "UInt64" => Convert.ToUInt64(str),
                "Double" => Convert.ToDouble(str, CultureInfo.InvariantCulture),
                "Single" => Convert.ToSingle(str, CultureInfo.InvariantCulture),
                "Byte" => Convert.ToByte(str),
                "SByte" => Convert.ToSByte(str),
                "Char" => Convert.ToChar(str),
                "Boolean" => Convert.ToBoolean(str),
                "String" => str,
                "DateTime" => Convert.ToDateTime(str, CultureInfo.InvariantCulture),
                _ => type.IsValueType ? Activator.CreateInstance(type, str) : null
            };
        }
        private static void AppendString(StringBuilder sb, object obj)
        {
            if (obj is string)
            {
                string s = obj as string;
                bool hasQuotes = s.Contains("\"");
                bool hasDot = s.Contains(",");
                if (hasQuotes || hasDot)
                {
                    if (hasQuotes)
                        s = s.Replace("\"", "\"\"");
                    sb.Append($"\"{s}\",");
                }
                else
                    sb.Append($"{s},");
            }
            else if (obj is float || obj is double || obj is DateTime)
            {
                sb.Append(Convert.ToString(obj, CultureInfo.InvariantCulture) + ",");
            }
            else
                sb.Append(obj + ",");
        }
        /// <summary>
        /// register for the internal Ahead-of-time.
        /// </summary>
        public static void ForAOT()
        {
            string str = "";
            StringToDictionary<int, int>(str);
            StringToDictionary<int, uint>(str);
            StringToDictionary<int, string>(str);
            StringToDictionary<int, bool>(str);
            StringToDictionary<int, float>(str);
            StringToDictionary<int, DateTime>(str);
            StringToDictionary<int, double>(str);
            StringToDictionary<int, byte>(str);
            StringToDictionary<int, sbyte>(str);
            StringToDictionary<int, short>(str);
            StringToDictionary<int, ushort>(str);
            StringToDictionary<int, long>(str);
            StringToDictionary<int, ulong>(str);
            StringToDictionary<string, int>(str);
            StringToDictionary<string, uint>(str);
            StringToDictionary<string, string>(str);
            StringToDictionary<string, bool>(str);
            StringToDictionary<string, float>(str);
            StringToDictionary<string, DateTime>(str);
            StringToDictionary<string, double>(str);
            StringToDictionary<string, byte>(str);
            StringToDictionary<string, sbyte>(str);
            StringToDictionary<string, short>(str);
            StringToDictionary<string, ushort>(str);
            StringToDictionary<string, long>(str);
            StringToDictionary<string, ulong>(str);
            StringToList<int>(str);
            StringToList<uint>(str);
            StringToList<string>(str);
            StringToList<bool>(str);
            StringToList<float>(str);
            StringToList<DateTime>(str);
            StringToList<double>(str);
            StringToList<byte>(str);
            StringToList<sbyte>(str);
            StringToList<short>(str);
            StringToList<ushort>(str);
            StringToList<long>(str);
            StringToList<ulong>(str);
        }
        #endregion//internal_imp
    }
}
