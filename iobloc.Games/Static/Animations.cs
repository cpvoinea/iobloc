namespace iobloc
{
    // EndAnimation frames
    static class Animations
    {
        public const int SIZE_ENDING = 7;
        public const int SIZE_LOGO = 10;

        private static readonly int[][][,] All = new int[][][,]{
            new[]{
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
            new[]{
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
            new[]{
                new[,]{
                    {0,0,1,1,0,0,1,1,0,0},
                    {0,0,1,1,0,1,0,0,1,0},
                    {0,0,1,1,0,0,1,1,0,0},
                    {0,0,0,0,0,0,0,0,0,0},
                    {0,1,1,0,0,0,1,1,0,0},
                    {0,1,1,1,1,0,1,1,0,0},
                    {0,1,1,1,1,0,1,1,1,0},
                    {0,0,0,0,0,0,0,0,0,0},
                    {0,0,1,1,0,0,1,1,1,0},
                    {0,1,0,0,1,0,1,0,0,0},
                    {0,0,1,1,0,0,1,1,1,0},
                },
            },
        };

        public static int[][,] Get(GameType type) => All[type - GameType.Fireworks];
    }
}
