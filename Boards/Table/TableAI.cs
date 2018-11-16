using System.Collections.Generic;

namespace iobloc
{
    class TableAI : ITableAI
    {
        const int L = 28;

        public int[][] GetMoves(int[] lines, int[] dice)
        {
            List<int[]> result = new List<int[]>();
            var allowed = GetMoveMatrix(lines, dice);
            for (int from = L - 1; from >= 0; from--)
                for (int to = 0; to < L; to++)
                    if (allowed[from, to] > 0)
                        result.Add(new[] { from, to, allowed[from, to] });
            return result.ToArray();
        }

        internal static int[,] GetMoveMatrix(int[] lines, int[] dice)
        {
            int[,] result = new int[L, L];

            if (lines[24] > 0)
            {
                int from = 24;
                for (int d = 1; d <= 6; d++)
                {
                    int to = 24 - d;
                    if (dice.Contains(d) && lines[to] >= -1)
                        result[from, to] = d;
                }
            }
            else
            {
                for (int from = 0; from < 24; from++)
                    if (lines[from] > 0)
                        for (int to = from - 1; to >= 0 && to >= from - 6; to--)
                        {
                            int d = from - to;
                            if (dice.Contains(d) && lines[to] >= -1)
                                result[from, to] = d;
                        }

                bool canTakeOut = true;
                for (int i = 6; i < 24 && canTakeOut; i++)
                    if (lines[i] > 0)
                        canTakeOut = false;
                if (canTakeOut)
                {
                    int to = 26;
                    for (int from = 0; from < 6; from++)
                    {
                        if (lines[from] > 0)
                        {
                            int d = from + 1;
                            if (dice.Contains(d))
                                result[from, to] = d;
                            else
                            {
                                bool clearLeft = true;
                                for (int i = from + 1; i < 6 && clearLeft; i++)
                                    if (lines[i] > 0)
                                        clearLeft = false;
                                if (clearLeft)
                                    for (int i = from + 2; i <= 6; i++)
                                        if (dice.Contains(i))
                                        {
                                            result[from, to] = i;
                                            break;
                                        }
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}