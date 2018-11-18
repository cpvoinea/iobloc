using System.Collections.Generic;

namespace iobloc
{
    class TableAI : ITableAI
    {
        const int L = 28;

        public int[][] GetMoves(int[] lines, int[] dice)
        {
            var matrix = GetMoveMatrix(lines, dice);
            return new[] { GetNextMove(matrix, lines, dice[0]) };
        }

        public static int[] GetAllowedTake(int[] lines, int[] dice)
        {
            return new int[0];
        }

        public static int[] GetAllowedPut(int[] lines, int[] dice, int from)
        {
            return new int[0];
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

        internal static int[] GetNextMove(int[,] matrix, int[] lines, int dice)
        {
            bool canTakeOut = true;
            for (int i = 6; i < 24 && canTakeOut; i++)
                if (lines[i] > 0)
                    canTakeOut = false;
            if (canTakeOut)
            {
                for (int i = 0; i < 6; i++)
                    if (matrix[i, 26] == dice)
                        return new[] { i, 26, dice };
                for (int j = 0; j < 5; j++)
                    if (lines[j] == 0 || lines[j] == -1 && matrix[j + dice, j] > 0)
                        return new[] { j + dice, j, dice };
            }
            for (int from = L - 1; from >= 0; from--)
                for (int to = 0; to < L; to++)
                    if (matrix[from, to] == dice)
                        return new[] { from, to, dice };
            return null;
        }

        internal int[] DoMove(int[] lines, int from, int to)
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
    }
}
