using System;
using System.Collections.Generic;

namespace iobloc
{
    static class Extensions
    {
        /// <summary>
        /// Copy values to an [x,y] array
        /// </summary>
        /// <returns>new array</returns>
        internal static int[,] Copy(this int[,] array, int x, int y)
        {
            int[,] result = new int[x, y];
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    result[i, j] = array[i, j];
            return result;
        }

        /// <summary>
        /// Check if array contains generic value
        /// </summary>
        /// <returns>true if contains</returns>
        internal static bool Contains<T>(this T[] array, T val)
        {
            return Array.IndexOf(array, val) >= 0;
        }

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
            if(!dic.ContainsKey(key))
                return ConsoleColor.Black;
            return (ConsoleColor)Enum.Parse(typeof(ConsoleColor), dic[key]);
        }
    }
}