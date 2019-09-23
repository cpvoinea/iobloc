using System;

namespace iobloc
{
    class Labirint : BaseGame
    {
        private int S, F, W, T;
        private static readonly int[][,] Mazes = Maze.All;
        private readonly int Count = Mazes.Length;
        private int _row, _col, _rowF, _colF;
        private int _next;
        private bool _finished;

        public Labirint() : base(GameType.Labirint) { }

        private void ChangeLevel()
        {
            if (Level < Count - 1)
                Level++;
            else
                Level = 0;
            Initialize();
        }

        protected override void InitializeSettings()
        {
            base.InitializeSettings();
            S = Serializer.GetColor(GameSettings, Settings.PlayerColor);
            F = Serializer.GetColor(GameSettings, Settings.EnemyColor);
            W = Serializer.GetColor(GameSettings, Settings.NeutralColor);
            T = Serializer.GetColor(GameSettings, "TrailColor");
        }

        protected override void Change(bool set)
        {
            if (!set)
                for (int k = 0; k < BlockWidth; k++)
                    Main[_row, _col + k] = new PaneCell(_next == T ? 0 : T);
            else
            {
                for (int k = 0; k < BlockWidth; k++)
                    Main[_row, _col + k] = new PaneCell(T, true);
                base.Change(true);

                if (_row == _rowF && _col == _colF)
                    _finished = true;
            }
        }

        protected override void Initialize()
        {
            if (!IsInitialized)
                base.Initialize();

            Main.Clear();
            _finished = false;

            var m = Mazes[Level];
            for (int i = 0; i < Width / BlockWidth; i++)
                for (int j = 0; j < Height; j++)
                {
                    int c = m[i, j];
                    if (c == S)
                    {
                        _row = i;
                        _col = j * BlockWidth;
                    }
                    else if (c == F)
                    {
                        _rowF = i;
                        _colF = j * BlockWidth;
                    }
                    for (int k = 0; k < BlockWidth; k++)
                        Main[i, j * BlockWidth + k] = new PaneCell(c);
                }

            Change(true);
        }

        public override void HandleInput(string key)
        {
            if (_finished)
            {
                ChangeLevel();
                return;
            }

            switch (key)
            {
                case UIKey.LeftArrow:
                    if (_col > 0 && Main[_row, _col - BlockWidth].Color != W)
                    {
                        _next = Main[_row, _col - BlockWidth].Color;
                        Change(false);
                        _col -= BlockWidth;
                        Change(true);
                    }
                    break;
                case UIKey.RightArrow:
                    if (_col < Width - BlockWidth && Main[_row, _col + BlockWidth].Color != W)
                    {
                        _next = Main[_row, _col + BlockWidth].Color;
                        Change(false);
                        _col += BlockWidth;
                        Change(true);
                    }
                    break;
                case UIKey.UpArrow:
                    if (_row > 0 && Main[_row - 1, _col].Color != W)
                    {
                        _next = Main[_row - 1, _col].Color;
                        Change(false);
                        _row--;
                        Change(true);
                    }
                    break;
                case UIKey.DownArrow:
                    if (_row < Height - 1 && Main[_row + 1, _col].Color != W)
                    {
                        _next = Main[_row + 1, _col].Color;
                        Change(false);
                        _row++;
                        Change(true);
                    }
                    break;
                case UIKey.Space:
                    ChangeLevel();
                    break;
            }
        }
    }
}
