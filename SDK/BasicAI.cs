using System.Collections.Generic;

namespace iobloc
{
    public class BasicAI : ITableAI
    {
        public virtual int[][] GetMoves(int[] lines, int[] dice)
        {
            List<int> remainingDice = new List<int>(dice);
            List<int[]> moves = new List<int[]>();
            while (remainingDice.Count > 0)
            {
                var m = GetNextMove(lines, remainingDice.ToArray());
                if (m == null)
                    break;
                moves.Add(m);
                lines = Move(lines, m[0], m[1]);
                remainingDice.Remove(m[2]);
            }

            return moves.ToArray();
        }

        protected virtual int[] GetNextMove(int[] lines, int[] dice)
        {
            if (dice.Length == 0)
                return null;

            var allowed = GetAllowedFrom(lines, dice);
            if (allowed.Length == 0)
                return null;
            int farthest = 0;
            foreach (int a in allowed)
                if (a > farthest)
                    farthest = a;

            allowed = GetAllowedTo(lines, dice, farthest);
            if (allowed.Length == 0)
                return null;
            int min = int.MaxValue;
            bool has26 = false;
            foreach (int a in allowed)
            {
                if (a < min)
                    min = a;
                if (a == 26)
                    has26 = true;
            }
            int closest = has26 ? 26 : min;

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
                if (CanPutFrom(lines, dice, 24))
                    return new[] { 24 };

                return new int[0];
            }
            else
            {
                List<int> result = new List<int>();
                bool canTakeOut = CanTakeOut(lines);
                for (int i = 0; i < 24; i++)
                    if (CanTake(lines, i) && (CanPutFrom(lines, dice, i) || i < 6 && canTakeOut && CanTakeOutFrom(lines, dice, i)))
                        result.Add(i);

                return result.ToArray();
            }
        }

        public static int[] GetAllowedTo(int[] lines, int[] dice, int from)
        {
            List<int> result = new List<int>();
            List<int> distinct = new List<int>();
            foreach (int d in dice)
                if (!distinct.Contains(d))
                    distinct.Add(d);
            foreach (int d in distinct)
            {
                int to = from - d;
                if (to >= 0 && CanPut(lines, to))
                    result.Add(to);
            }
            if (CanTakeOut(lines) && CanTakeOutFrom(lines, dice, from))
                result.Add(26);

            return result.ToArray();
        }

        private static bool CanPutFrom(int[] lines, int[] dice, int from)
        {
            List<int> distinct = new List<int>();
            foreach (int d in dice)
                if (!distinct.Contains(d))
                    distinct.Add(d);
            foreach (int d in distinct)
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
            if (Serializer.Contains(dice, from + 1))
                return true;
            for (int i = from + 1; i < 6; i++)
                if (CanTake(lines, i))
                    return false;
            foreach (int d in dice)
                if (d > from + 1)
                    return true;
            return false;
        }

        private static bool CanTake(int[] lines, int from) => lines[from] > 0;
        private static bool CanPut(int[] lines, int to) => lines[to] >= -1;

        public static int GetDice(int[] dice, int from, int to)
        {
            if (from == to)
                return 0;

            int val = from - to;
            if (to == 26)
            {
                if (Serializer.Contains(dice, from + 1))
                    val = from + 1;
                else
                    foreach (int d in dice)
                        if (d > from + 1)
                        {
                            val = d;
                            break;
                        }
            }

            return val;
        }
    }
}
