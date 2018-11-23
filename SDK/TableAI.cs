using System.Collections.Generic;
using System.Linq;

namespace iobloc
{
    public class TableAI : ITableAI
    {
        public int[][] GetMoves(int[] lines, int[] dice)
        {
            List<int> remainingDice = new List<int>(dice);
            List<int[]> moves = new List<int[]>();
            while (remainingDice.Count > 0)
            {
                var m = GetNextMove(lines, dice);
                if (m == null)
                    break;
                moves.Add(m);
                lines = Move(lines, m[0], m[1]);
                remainingDice.Remove(m[2]);
            }

            return moves.ToArray();
        }

        private int[] GetNextMove(int[] lines, int[] dice)
        {
            if (dice.Length == 0)
                return null;

            var allowed = GetAllowedFrom(lines, dice);
            if (allowed.Length == 0)
                return null;
            int farthest = allowed.Max();

            allowed = GetAllowedTo(lines, dice, farthest);
            if (allowed.Length == 0)
                return null;
            int closest = allowed.Min();

            return new[] { farthest, closest, GetDice(dice, farthest, closest) };
        }

        private int[] Move(int[] lines, int from, int to)
        {
            lines[from]--;
            if (lines[to] < 0)
            {
                lines[25]++;
                lines[to] = 1;
            }
            else
                lines[to]++;

            return lines;
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

        public static int GetDice(int[] dice, int from, int to)
        {
            int val = from - to;
            if (to == 26)
            {
                if (dice.Contains(from + 1))
                    val = from + 1;
                else
                    val = dice.First(d => d > from + 1);
            }

            return val;
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
