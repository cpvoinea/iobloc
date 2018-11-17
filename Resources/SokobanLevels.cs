namespace iobloc
{
    // Sokoban levels ar matrix maps, using runtime colors from settings
    static class SokobanLevels
    {
        private static readonly int[][,] _levels;
        public static int Count => _levels == null ? 0 : _levels.Length;
        public static int[,] Get(int lvl) => _levels[lvl];

        static SokobanLevels()
        {
            var settings = Serializer.Settings[(int)GameType.Sokoban];
            int P = settings.GetColor(Settings.PlayerColor);
            int B = settings.GetColor("BlockColor");
            int W = settings.GetColor("WallColor");
            int T = settings.GetColor("TargetColor");
            int R = settings.GetColor("TargetBlockColor");

            _levels = new[]{
                new[,] { // 0
                    {0, W, 0, 0},
                    {P, B, T, 0},
                    {W, W, 0, 0},
                    {0, 0, 0, 0},
                    {0, 0, 0, 0},
                    {0, 0, 0, 0}
                },
                new[,] { // 1
                    {0, 0, 0, 0},
                    {0, 0, T, 0},
                    {0, 0, W, 0},
                    {0, 0, 0, 0},
                    {0, B, P, 0},
                    {0, 0, 0, 0}
                },
                new[,] { // 2
                    {T, 0, 0, W},
                    {0, 0, R, 0},
                    {0, B, W, 0},
                    {0, 0, 0, 0},
                    {0, W, P, 0},
                    {0, 0, 0, 0}
                },
                new[,] { // 3
                    {T, 0, 0, W},
                    {0, 0, 0, 0},
                    {W, B, 0, 0},
                    {0, R, 0, 0},
                    {P, 0, 0, 0},
                    {0, 0, 0, W}
                },
                new[,] { // 4
                    {W, 0, 0, W},
                    {0, 0, B, 0},
                    {T, B, W, T},
                    {0, 0, 0, 0},
                    {P, 0, 0, 0},
                    {W, 0, 0, W}
                },
                new[,] { // 5
                    {0, 0, 0, 0},
                    {0, P, 0, 0},
                    {0, B, W, T},
                    {0, B, 0, 0},
                    {0, 0, 0, 0},
                    {T, R, 0, W}
                },
                new[,] { // 6
                    {W, 0, 0, 0},
                    {W, T, P, T},
                    {0, T, B, 0},
                    {0, 0, B, W},
                    {0, 0, B, 0},
                    {0, 0, 0, 0}
                },
                new[,] { // 7
                    {0, W, 0, 0},
                    {W, 0, 0, 0},
                    {0, R, T, 0},
                    {0, B, B, 0},
                    {0, T, W, 0},
                    {P, 0, 0, 0}
                },
                new[,] { // 8
                    {W, T, 0, P},
                    {W, R, W, B},
                    {W, 0, B, T},
                    {W, 0, 0, 0},
                    {0, 0, 0, 0},
                    {0, 0, 0, 0}
                },
                new[,] { // 9
                    {W, 0, 0, W},
                    {0, 0, T, 0},
                    {0, 0, 0, 0},
                    {0, W, 0, 0},
                    {R, B, B, 0},
                    {P, 0, 0, T}
                },
                new[,] { // 10
                    {W, W, W, W},
                    {0, 0, 0, T},
                    {B, B, B, P},
                    {T, 0, W, 0},
                    {0, 0, T, 0},
                    {W, W, W, W}
                },
                new[,] { // 11
                    {W, 0, 0, T},
                    {0, R, B, T},
                    {0, W, 0, B},
                    {0, 0, 0, 0},
                    {0, 0, 0, 0},
                    {0, P, 0, 0}
                },
                new[,] { // 12
                    {W, W, W, 0},
                    {P, 0, 0, W},
                    {0, W, B, 0},
                    {0, R, 0, T},
                    {W, 0, 0, B},
                    {W, T, 0, 0}
                },
                new[,] { // 13
                    {0, 0, 0, W},
                    {0, W, 0, 0},
                    {T, T, 0, 0},
                    {W, B, B, T},
                    {0, B, 0, 0},
                    {0, 0, P, W}
                },
                new[,] { // 14
                    {0, 0, 0, W},
                    {0, W, T, 0},
                    {0, 0, T, 0},
                    {W, B, 0, 0},
                    {P, B, 0, 0},
                    {T, B, 0, 0}
                },
                new[,] { // 15
                    {0, 0, W, 0},
                    {0, 0, 0, W},
                    {0, T, R, 0},
                    {0, B, B, 0},
                    {P, W, T, 0},
                    {0, 0, 0, 0}
                },
                new[,] { // 16
                    {0, W, W, 0},
                    {W, P, 0, W},
                    {0, B, 0, 0},
                    {W, 0, B, W},
                    {0, R, 0, 0},
                    {0, T, T, 0}
                },
                new[,] { // 17
                    {0, T, W, 0},
                    {0, 0, W, W},
                    {R, P, 0, 0},
                    {0, 0, B, 0},
                    {0, 0, W, W},
                    {W, W, 0, 0}
                },
                new[,] { // 18
                    {0, 0, 0, 0},
                    {0, W, P, 0},
                    {0, B, R, 0},
                    {0, T, R, 0},
                    {0, 0, 0, 0},
                    {W, W, W, W}
                },
                new[,] { // 19
                    {W, 0, 0, 0},
                    {W, 0, T, 0},
                    {W, 0, R, 0},
                    {W, 0, R, 0},
                    {0, 0, B, 0},
                    {0, 0, P, 0}
                },
                new[,] { // 20
                    {T, 0, 0, W},
                    {P, B, B, 0},
                    {W, 0, 0, 0},
                    {0, W, 0, 0},
                    {0, 0, W, 0},
                    {0, 0, 0, T}
                },
                new[,] { // 21
                    {0, T, 0, 0},
                    {0, 0, W, 0},
                    {0, B, 0, 0},
                    {W, R, W, 0},
                    {W, P, 0, 0},
                    {0, W, W, W}
                },
                new[,] { // 22
                    {0, P, 0, W},
                    {T, T, T, W},
                    {B, B, B, W},
                    {0, 0, 0, 0},
                    {0, 0, 0, 0},
                    {0, 0, 0, 0}
                },
                new[,] { // 23
                    {W, 0, 0, 0},
                    {0, B, T, 0},
                    {0, B, 0, W},
                    {0, W, T, W},
                    {0, P, 0, W},
                    {W, W, W, 0}
                },
                new[,] { // 24
                    {W, 0, P, 0},
                    {W, 0, 0, 0},
                    {W, W, B, 0},
                    {0, T, T, T},
                    {0, B, B, 0},
                    {W, W, 0, 0}
                },
                new[,] { // 25
                    {0, 0, W, W},
                    {0, B, B, 0},
                    {T, T, T, 0},
                    {0, P, B, 0},
                    {0, 0, 0, W},
                    {W, W, W, 0}
                },
                new[,] { // 26
                    {P, 0, W, W},
                    {T, B, 0, 0},
                    {T, B, 0, 0},
                    {T, B, 0, 0},
                    {0, 0, W, W},
                    {W, W, 0, 0}
                },
                new[,] { // 27
                    {W, 0, P, 0},
                    {0, 0, B, 0},
                    {0, W, B, 0},
                    {0, T, B, 0},
                    {0, T, 0, 0},
                    {W, T, W, W}
                },
                new[,] { // 28
                    {T, T, T, 0},
                    {0, 0, B, 0},
                    {0, W, B, W},
                    {0, 0, B, 0},
                    {0, 0, P, 0},
                    {0, W, W, 0}
                },
                new[,] { // 29
                    {W, W, 0, P},
                    {0, 0, B, 0},
                    {0, 0, R, T},
                    {0, 0, R, T},
                    {0, 0, B, 0},
                    {W, W, 0, 0}
                },
                new[,] { // 30
                    {W, W, 0, 0},
                    {0, R, 0, 0},
                    {0, 0, B, 0},
                    {0, T, 0, W},
                    {P, 0, 0, W},
                    {0, W, W, 0}
                },
            };
        }
    }
}
