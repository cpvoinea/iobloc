using iobloc.SDK;
using System.Windows.Forms;

namespace iobloc.Games
{
    public class Sokoban : BaseGame
    {
        int P, B, W, T, R, H;
        int Targets = int.MaxValue;
        int Row;
        int Col;
        int Level = 0;

        public Sokoban() : base() { }

        protected override void InitializeSettings()
        {
            Width = 4;
            Height = 6;
            P = 12;
            B = 9;
            W = 8;
            T = 6;
            R = 1;
            H = 4;
        }

        public override void Start()
        {
            base.Start();
            ResetLevel();
        }

        public override void HandleInput(Keys key)
        {
            if (key == Keys.R)
            {
                ResetLevel();
                return;
            }

            int h = 0;
            int v = 0;
            switch (key)
            {
                case Keys.Left: h = -1; break;
                case Keys.Right: h = 1; break;
                case Keys.Up: v = -1; break;
                case Keys.Down: v = 1; break;
            }

            if (Row + v < 0 || Row + v >= Height || Col + h < 0 || Col + h >= Width)
                return;
            int next = Main[Row + v, Col + h].Color;
            if (next == W)
                return;

            if (next == 0 || next == T)
            {
                SetBlock(Row, Col, Main[Row, Col].Color == H ? T : 0);
                Row += v;
                Col += h;
                SetBlock(Row, Col, Main[Row, Col].Color == T ? H : P);

                base.Change(true);
            }
            else if (next == B || next == R)
            {
                if (Row + 2 * v < 0 || Row + 2 * v >= Height || Col + 2 * h < 0 || Col + 2 * h >= Width)
                    return;
                int second = Main[Row + 2 * v, Col + 2 * h].Color;
                if (second == W || second == B || second == R)
                    return;

                if (second == 0 || second == T)
                {
                    SetBlock(Row, Col, Main[Row, Col].Color == H ? T : 0);
                    Row += v;
                    Col += h;
                    if (Main[Row, Col].Color == R)
                    {
                        Targets++;
                        SetBlock(Row, Col, H);
                    }
                    else
                        SetBlock(Row, Col, P);

                    if (Main[Row + v, Col + h].Color == T)
                    {
                        SetBlock(Row + v, Col + h, R);
                        Targets--;
                        if (Targets == 0)
                            NextLevel();

                    }
                    else
                        SetBlock(Row + v, Col + h, B);

                    base.Change(true);
                }
            }
        }

        void ResetLevel()
        {
            var game = SokobanLevels.Get(Level);
            Targets = 0;
            for (int i = 0; i < Height && i < 6; i++)
                for (int j = 0; j < Width && j < 4; j++)
                {
                    int v = game[i, j];
                    SetBlock(i, j, v);
                    if (v == P)
                    {
                        Row = i;
                        Col = j;
                    }
                    else if (v == T)
                        Targets++;
                }

            base.Change(true);
        }

        void NextLevel()
        {
            if (Level == SokobanLevels.Count - 1) // no more levels
                Stop(); // exit to win animation
            else
            {
                Level++;
                ResetLevel();
            }
        }

        void SetBlock(int row, int col, int val)
        {
            Main[row, col] = new PaneCell(val);
        }
    }
}
