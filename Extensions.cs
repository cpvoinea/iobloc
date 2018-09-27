using System;

namespace iobloc
{
    static class Extensions
    {
        internal static int[,] Copy(this int[,] array, int x, int y)
        {
            int[,] result = new int[x, y];
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    result[i, j] = array[i, j];
            return result;
        }

        internal static bool Contains<T>(this T[] array, T val)
        {
            return Array.IndexOf(array, val) >= 0;
        }
    }
}