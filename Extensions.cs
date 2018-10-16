using System;
using System.Collections.Generic;

namespace iobloc
{
    static class Extensions
    {
        internal static int GetInt(this Dictionary<string, string> dic, string key, int defVal = 1)
        {
            if (!dic.ContainsKey(key))
                return defVal;
            return int.Parse(dic[key]);
        }

        internal static string[] GetList(this Dictionary<string, string> dic, string key)
        {
            if (!dic.ContainsKey(key))
                return new string[0];
            return dic[key].Split(',');
        }

        internal static ConsoleColor GetColor(this Dictionary<string, string> dic, string key)
        {
            if (!dic.ContainsKey(key))
                return ConsoleColor.Black;
            return (ConsoleColor)Enum.Parse(typeof(ConsoleColor), dic[key]);
        }
    }
}