namespace iobloc
{
    static class SokobanLevels
    {
        private static readonly int[][,] _levels;

        public static int Count => _levels == null ? 0 : _levels.Length;

        public static int[,] Get(int lvl)
        {
            return _levels[lvl];
        }

        static SokobanLevels()
        {
            var settings = Serializer.Settings[(int)BoardType.Sokoban];
            int P = settings.GetColor("PlayerColor");
            int B = settings.GetColor("BlockColor");
            int W = settings.GetColor("WallColor");
            int T = settings.GetColor("TargetColor");
            int R = settings.GetColor("TargetBlockColor");

            _levels = new[]{
                new[,] {
                    {0, W, 0, 0},
                    {P, B, T, 0},
                    {W, W, 0, 0},
                    {0, 0, 0, 0},
                    {0, 0, 0, 0},
                    {0, 0, 0, 0}
                },
                new[,] {
                    {0, 0, 0, 0},
                    {0, 0, T, 0},
                    {0, 0, W, 0},
                    {0, 0, 0, 0},
                    {0, B, P, 0},
                    {0, 0, 0, 0}
                },
                new[,] {
                    {T, 0, 0, W},
                    {0, 0, R, 0},
                    {0, B, W, 0},
                    {0, 0, 0, 0},
                    {0, W, P, 0},
                    {0, 0, 0, 0}
                },
                new[,] {
                    {T, 0, 0, W},
                    {0, 0, 0, 0},
                    {W, B, 0, 0},
                    {0, R, 0, 0},
                    {P, 0, 0, 0},
                    {0, 0, 0, W}
                },
                new[,] {
                    {W, 0, 0, W},
                    {0, 0, B, 0},
                    {T, B, W, T},
                    {0, 0, 0, 0},
                    {P, 0, 0, 0},
                    {W, 0, 0, W}
                },
                new[,] {
                    {0, 0, 0, 0},
                    {0, P, 0, 0},
                    {0, B, W, T},
                    {0, B, 0, 0},
                    {0, 0, 0, 0},
                    {T, R, 0, W}
                },
                new[,] {
                    {W, 0, 0, 0},
                    {W, T, P, T},
                    {0, T, B, 0},
                    {0, 0, B, W},
                    {0, 0, B, 0},
                    {0, 0, 0, 0}
                },
                new[,] {
                    {0, W, 0, 0},
                    {W, 0, 0, 0},
                    {0, R, T, 0},
                    {0, B, B, 0},
                    {0, T, W, 0},
                    {P, 0, 0, 0}
                },
                new[,] {
                    {W, T, 0, P},
                    {W, R, W, B},
                    {W, 0, B, T},
                    {W, 0, 0, 0},
                    {0, 0, 0, 0},
                    {0, 0, 0, 0}
                },
                new[,] {
                    {W, 0, 0, W},
                    {0, 0, T, 0},
                    {0, 0, 0, 0},
                    {0, W, 0, 0},
                    {R, B, B, 0},
                    {P, 0, 0, T}
                },
                new[,] {
                    {W, W, W, W},
                    {0, 0, 0, T},
                    {B, B, B, P},
                    {T, 0, W, 0},
                    {0, 0, T, 0},
                    {W, W, W, W}
                },
                new[,] {
                    {W, 0, 0, T},
                    {0, R, B, T},
                    {0, W, 0, B},
                    {0, 0, 0, 0},
                    {0, 0, 0, 0},
                    {0, P, 0, 0}
                },
                new[,] {
                    {W, W, W, 0},
                    {P, 0, 0, W},
                    {0, W, B, 0},
                    {0, R, 0, T},
                    {W, 0, 0, B},
                    {W, T, 0, 0}
                },
                new[,] {
                    {0, 0, 0, W},
                    {0, W, 0, 0},
                    {T, T, 0, 0},
                    {W, B, B, T},
                    {0, B, 0, 0},
                    {0, 0, P, W}
                },
                new[,] {
                    {0, 0, 0, W},
                    {0, W, T, 0},
                    {0, 0, T, 0},
                    {W, B, 0, 0},
                    {P, B, 0, 0},
                    {T, B, 0, 0}
                },
                new[,] {
                    {0, 0, W, 0},
                    {0, 0, 0, W},
                    {0, T, R, 0},
                    {0, B, B, 0},
                    {P, W, T, 0},
                    {0, 0, 0, 0}
                },
                new[,] {
                    {0, W, W, 0},
                    {W, P, 0, W},
                    {0, B, 0, 0},
                    {W, 0, B, W},
                    {0, R, 0, 0},
                    {0, T, T, 0}
                },
                new[,] {
                    {0, T, W, 0},
                    {0, 0, W, W},
                    {R, P, 0, 0},
                    {0, 0, B, 0},
                    {0, 0, W, W},
                    {W, W, 0, 0}
                },
                new[,] {
                    {0, 0, 0, 0},
                    {0, W, P, 0},
                    {0, B, R, 0},
                    {0, T, R, 0},
                    {0, 0, 0, 0},
                    {W, W, W, W}
                },
                new[,] {
                    {W, 0, 0, 0},
                    {W, 0, T, 0},
                    {W, 0, R, 0},
                    {W, 0, R, 0},
                    {0, 0, B, 0},
                    {0, 0, P, 0}
                },
                new[,] {
                    {T, 0, 0, W},
                    {P, B, B, 0},
                    {W, 0, 0, 0},
                    {0, W, 0, 0},
                    {0, 0, W, 0},
                    {0, 0, 0, T}
                },
                new[,] {
                    {0, T, 0, 0},
                    {0, 0, W, 0},
                    {0, B, 0, 0},
                    {W, R, W, 0},
                    {W, P, 0, 0},
                    {0, W, W, W}
                },
                new[,] {
                    {0, P, 0, W},
                    {T, T, T, W},
                    {B, B, B, W},
                    {0, 0, 0, 0},
                    {0, 0, 0, 0},
                    {0, 0, 0, 0}
                },
                new[,] {
                    {W, 0, 0, 0},
                    {0, B, T, 0},
                    {0, B, 0, W},
                    {0, W, T, W},
                    {0, P, 0, W},
                    {W, W, W, 0}
                },
                new[,] {
                    {W, 0, P, 0},
                    {W, 0, 0, 0},
                    {W, W, B, 0},
                    {0, T, T, T},
                    {0, B, B, 0},
                    {W, W, 0, 0}
                },
                new[,] {
                    {0, 0, W, W},
                    {0, B, B, 0},
                    {T, T, T, 0},
                    {0, P, B, 0},
                    {0, 0, 0, W},
                    {W, W, W, 0}
                },
                new[,] {
                    {P, 0, W, W},
                    {T, B, 0, 0},
                    {T, B, 0, 0},
                    {T, B, 0, 0},
                    {0, 0, W, W},
                    {W, W, 0, 0}
                },
                new[,] {
                    {W, 0, P, 0},
                    {0, 0, B, 0},
                    {0, W, B, 0},
                    {0, T, B, 0},
                    {0, T, 0, 0},
                    {W, T, W, W}
                },
                new[,] {
                    {T, T, T, 0},
                    {0, 0, B, 0},
                    {0, W, B, W},
                    {0, 0, B, 0},
                    {0, 0, P, 0},
                    {0, W, W, 0}
                },
                new[,] {
                    {W, W, 0, P},
                    {0, 0, B, 0},
                    {0, 0, R, T},
                    {0, 0, R, T},
                    {0, 0, B, 0},
                    {W, W, 0, 0}
                },
                new[,] {
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
