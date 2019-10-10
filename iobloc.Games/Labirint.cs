using iobloc.SDK;
using System.Windows.Forms;

namespace iobloc.Games
{
    public class Labirint : BaseGame
    {
        int S, F, W, T;
        static readonly int[][,] Mazes = LabirintMazes.All;
        readonly int Count = Mazes.Length;
        int Level = 0;
        int Row, Col, RowF, ColF;
        int Next;
        bool Finished;

        public Labirint() : base() { }

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
            Width = 11;
            Height = 11;
            S = 9;
            F = 12;
            W = 8;
            T = 14;
        }

        protected override void Change(bool set)
        {
            if (!set)
                Main[Row, Col] = new PaneCell(Next == T ? 0 : T);
            else
            {
                Main[Row, Col] = new PaneCell(T, true);
                base.Change(true);

                if (Row == RowF && Col == ColF)
                    Finished = true;
            }
        }

        protected override void Initialize()
        {
            if (!IsInitialized)
                base.Initialize();

            Main.Clear();
            Finished = false;

            var m = Mazes[Level];
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {
                    int c = m[i, j];
                    if (c == S)
                    {
                        Row = i;
                        Col = j;
                    }
                    else if (c == F)
                    {
                        RowF = i;
                        ColF = j;
                    }
                    Main[i, j] = new PaneCell(c);
                }

            Change(true);
        }

        public override void HandleInput(Keys key)
        {
            if (Finished)
            {
                ChangeLevel();
                return;
            }

            switch (key)
            {
                case Keys.Left:
                    if (Col > 0 && Main[Row, Col - 1].Color != W)
                    {
                        Next = Main[Row, Col - 1].Color;
                        Change(false);
                        Col--;
                        Change(true);
                    }
                    break;
                case Keys.Right:
                    if (Col < Width - 1 && Main[Row, Col + 1].Color != W)
                    {
                        Next = Main[Row, Col + 1].Color;
                        Change(false);
                        Col++;
                        Change(true);
                    }
                    break;
                case Keys.Up:
                    if (Row > 0 && Main[Row - 1, Col].Color != W)
                    {
                        Next = Main[Row - 1, Col].Color;
                        Change(false);
                        Row--;
                        Change(true);
                    }
                    break;
                case Keys.Down:
                    if (Row < Height - 1 && Main[Row + 1, Col].Color != W)
                    {
                        Next = Main[Row + 1, Col].Color;
                        Change(false);
                        Row++;
                        Change(true);
                    }
                    break;
                case Keys.Space:
                    ChangeLevel();
                    break;
            }
        }
    }
}
