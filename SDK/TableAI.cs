using System.Collections.Generic;

namespace iobloc
{
    public class TableAI : ITableAI
    {
        const int L = 28;

        public int[][] GetMoves(int[] lines, int[] dice)
        {
            return new int[0][];
        }

        public static int[] GetAllowedTake(int[] lines, int[] dice)
        {
            if (lines[24] > 0)
            {
                for (int d = 1; d <= 6; d++)
                    if (dice.Contains(d))
                        return new[] { 24 };
            }
            List<int> result = new List<int>();
            for (int i = 0; i < 24; i++)
                if (lines[i] > 0 && !result.Contains(i))
                    foreach (int d in dice)
                    {
                        int j = i - d;
                        if (j >= 0 && lines[j] >= -1)
                        {
                            result.Add(i);
                            break;
                        }
                    }
            return result.ToArray();
        }

        public static int[] GetAllowedPut(int[] lines, int[] dice, int from)
        {
            List<int> result = new List<int>();
            if (from == 24)
            {
                foreach (int d in dice)
                    if (lines[24 - d] >= -1)
                        result.Add(d);
                return result.ToArray();
            }
            else
            {
                foreach (int d in dice)
                    if (from - d >= 0 && lines[from - d] >= -1)
                        result.Add(from - d);


                foreach (int d in dice)
                {
                    if (lines[d - 1] > 0)
                    {
                        result.Add(26);
                        return result.ToArray();
                    }
                }
            }
            return result.ToArray();
        }

        public static int GetDice(int from, int to, int[] dice)
        {
            return from - to;
        }

        private static bool CanMove(int[] lines, int[] dice, int from, int to)
        {
            return CanTake(lines, from) && CanPut(lines, to) && (
                (from < 24 && to < 24 && dice.Contains(from - to)) ||
                (from == 24 && dice.Contains(24 - to)) ||
                (to == 26 && (dice.Contains(from + 1) || CanTakeOut(lines, dice, from)))
                );
        }

        private static bool CanTake(int[] lines, int from)
        {
            return from <= 24 && lines[from] > 0;
        }

        private static bool CanPut(int[] lines, int to)
        {
            return to < 24 && lines[to] >= -1;
        }

        private static bool CanTakeOut(int[] lines, int[] dice, int from)
        {
            bool canTakeOut = true;
            for (int i = 6; i < 24 && canTakeOut; i++)
                if (lines[i] > 0)
                    canTakeOut = false;
            if (!canTakeOut)
                return false;
            return true;
        }
    }
}
