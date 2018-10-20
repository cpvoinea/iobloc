namespace iobloc
{
    static class Animation
    {
        internal const int SIZE = 7;

        internal static int[][,] Get(int code)
        {
            var animation = All[code];
            int len = animation.Length;
            int[][,] result = new int[len][,];
            for (int i = 0; i < len; i++)
            {
                result[i] = new int[SIZE, SIZE];
                for (int r = 0; r < SIZE; r++)
                    for (int c = 0; c < SIZE; c++)
                        result[i][r, c] = animation[i][r, c];
            }

            return result;
        }

        static readonly int[][][,] All = new int[][][,]{
            new int[][,]{
                new int[,]{
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 15, 0, 0, 0}
                },
                new int[,]{
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 15, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0}
                },
                new int[,]{
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 15, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0}
                },
                new int[,]{
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 15, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0}
                },
                new int[,]{
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 12, 11, 10, 0, 0},
                    {0, 0, 13, 0, 9, 0, 0},
                    {0, 0, 14, 0, 8, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0}
                },
                new int[,]{
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 12, 0, 11, 0, 10, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 13, 0, 0, 0, 9, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 14, 0, 0, 0, 8, 0},
                    {0, 0, 0, 0, 0, 0, 0}
                },
                new int[,]{
                    {0, 0, 0, 11, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {12, 0, 0, 0, 0, 0, 10},
                    {0, 0, 0, 0, 0, 0, 0},
                    {13, 0, 0, 0, 0, 0, 9},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 14, 0, 0, 0, 8, 0}
                },
            },
            new int[][,]{
                new int[,]{
                    {0, 12, 0, 0, 12, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0}
                },
                new int[,]{
                    {12, 0, 12, 0, 0, 12, 12},
                    {0, 12, 0, 0, 12, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0}
                },
                new int[,]{
                    {0, 0, 12, 12, 12, 0, 12},
                    {12, 0, 12, 0, 0, 12, 12},
                    {0, 12, 0, 0, 12, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0}
                },
                new int[,]{
                    {12, 0, 12, 0, 0, 12, 0},
                    {0, 0, 12, 12, 12, 0, 12},
                    {12, 0, 12, 0, 0, 12, 12},
                    {0, 12, 0, 0, 12, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0}
                },
                new int[,]{
                    {0, 12, 0, 0, 12, 0, 12},
                    {12, 0, 12, 0, 0, 12, 0},
                    {0, 0, 12, 12, 12, 0, 12},
                    {12, 0, 12, 0, 0, 12, 12},
                    {0, 12, 0, 0, 12, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0}
                },
                new int[,]{
                    {12, 0, 0, 12, 0, 12, 0},
                    {0, 12, 0, 0, 12, 0, 12},
                    {12, 0, 12, 0, 0, 12, 0},
                    {0, 0, 12, 12, 12, 0, 12},
                    {12, 0, 12, 0, 0, 12, 12},
                    {0, 12, 0, 0, 12, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0}
                },
                new int[,]{
                    {12, 0, 12, 0, 0, 12, 12},
                    {12, 0, 0, 12, 0, 12, 0},
                    {0, 12, 0, 0, 12, 0, 12},
                    {12, 0, 12, 0, 0, 12, 0},
                    {0, 0, 12, 12, 12, 0, 12},
                    {12, 0, 12, 0, 0, 12, 12},
                    {0, 12, 0, 0, 12, 0, 0},
                },
            },
        };
    }
}