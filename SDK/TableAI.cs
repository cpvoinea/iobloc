using System.Collections.Generic;
using System.Linq;

namespace iobloc
{
    public class TableAI : ITableAI
    {
        public int[][] GetMoves(int[] lines, int[] dice)
        {
            return new int[][];
        }

        public static int[] GetAllowedFrom(int[] lines, int[] dice)
        {
            if (CanTake(lines, 24))
            {
                if (CanTakeFrom(lines, dice, 24))
                    return new[] { 24 };
                return new int[0];
            }
            else
            {
                List<int> result = new List<int>();
                for (int i = 0; i < 24; i++)
                    if (CanTake(lines, i) && CanTakeFrom(lines, dice, i))
                        result.Add(i);
                if (CanTakeOut(lines))
                    for (int i = 0; i < 6; i++)
                        if (CanTakeOutFrom(lines, dice, i))
                            result.Add(i);
                return result.ToArray();
            }
        }

        public static int[] GetAllowedTo(int[] lines, int[] dice, int from)
        {
            List<int> result = new List<int> { from };
            foreach (int d in dice.Distinct())
            {
                int to = from - d;
                if (to >= 0 && CanPut(lines, to))
                    result.Add(to);
            }
            if (CanTakeOut(lines) && CanTakeOutFrom(lines, dice, from))
                result.Add(26);

            return result.ToArray();
        }

        private static bool CanTakeFrom(int[] lines, int[] dice, int from)
        {
            foreach (int d in dice.Distinct())
                if (from - d >= 0 && CanPut(lines, from - d))
                    return true;
            return false;
        }

        private static bool CanTakeOut(int[] lines)
        {
            for (int i = 6; i <= 24; i++)
                if (CanTake(lines, i))
                    return false;
            return true;
        }

        private static bool CanTakeOutFrom(int[] lines, int[] dice, int from)
        {
            if (from >= 6)
                return false;
            if (dice.Contains(from + 1))
                return true;
            for (int i = from + 1; i < 6; i++)
                if (CanTake(lines, i))
                    return false;
            return dice.Any(d => d > from + 1);
        }

        private static bool CanTake(int[] lines, int from) => lines[from] > 0;
        private static bool CanPut(int[] lines, int to) => lines[to] >= -1;
    }
}
