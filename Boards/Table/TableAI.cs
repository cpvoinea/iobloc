namespace iobloc
{
    static class TableAI
    {
        public static int[,] GetMoveMatrix(int[] lines, int[] dice)
        {
            int[,] result = new int[28, 28];

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
                    if( lines[i] > 0 )
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
                                    for (; d <= 6; d++)
                                        if (dice.Contains(d))
                                        {
                                            result[from, to] = d;
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
