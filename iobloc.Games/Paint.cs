using iobloc.SDK;
using System.Collections.Generic;
using System.Windows.Forms;

namespace iobloc.Games
{
    public class Paint : BaseGame
    {
        Pane Info;
        int Row;
        int Col;
        int Color;
        int Prev;
        bool IsPaintMode;
        bool Light;

        public Paint() : base()
        {
            IsPaintMode = true;
            ShowInfo();
            Change(true);
        }

        protected override void InitializeSettings()
        {
            Width = 20;
            Height = 20;
        }

        protected override void InitializeUI()
        {
            Main = new Pane(0, 0, Height - 1, Width);
            Info = new Pane(Height, 1, Height, Width);
            Panes = new List<Pane> { Main, Info };
        }

        void ShowInfo()
        {
            Info[0, 0] = new PaneCell(IsPaintMode ? 1 + (Light ? 8 : 0) : 0);
            Info[0, 1] = new PaneCell(IsPaintMode ? 2 + (Light ? 8 : 0) : 0);
            Info[0, 2] = new PaneCell(IsPaintMode ? 3 + (Light ? 8 : 0) : 0);
            Info[0, 3] = new PaneCell(IsPaintMode ? 4 + (Light ? 8 : 0) : 0);
            Info[0, 4] = new PaneCell(IsPaintMode ? 5 + (Light ? 8 : 0) : 0);
            Info[0, 5] = new PaneCell(IsPaintMode ? 6 + (Light ? 8 : 0) : 0);
            Info[0, 6] = new PaneCell(IsPaintMode ? 7 + (Light ? 8 : 0) : 0);
            Info[0, 7] = new PaneCell(IsPaintMode ? Color + 8 * (Color >= 8 ? -1 : 1) : 0);
            Info[0, 8] = new PaneCell(IsPaintMode ? 15 : 0);
            Info[0, 9] = new PaneCell(0);
            Info[0, 10] = new PaneCell(IsPaintMode ? 0 : Color);
            Info.Change(true);
        }

        protected override void Initialize()
        {
            base.Initialize();

            if (IsInitialized)
            {
                Main.Clear();
                Color = 0;
                Prev = 0;
                IsPaintMode = false;
            }
            Row = Height / 2;
            Col = Width / 2;
            Color = 9;
            Light = true;
            ShowInfo();
            Change(true);
        }

        protected override void Change(bool set)
        {
            if (!set)
                    Main[Row, Col] = new PaneCell(IsPaintMode ? Color : Prev, set);
            else
            {
                Prev = Main[Row, Col].Color;
                    Main[Row, Col] = new PaneCell((!IsPaintMode || Color == 0 && Prev == 0) ? 15 : Color, set);
                base.Change(set);
            }
        }

        public override void HandleInput(Keys key)
        {
            switch (key)
            {
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.NumPad1:
                case Keys.NumPad2:
                case Keys.NumPad3:
                case Keys.NumPad4:
                case Keys.NumPad5:
                case Keys.NumPad6:
                case Keys.NumPad7:
                    if (IsPaintMode)
                    {
                        var s = key.ToString();
                        Color = int.Parse(s.Substring(s.Length - 1));
                        if (Light && Color < 15)
                            Color += 8;
                        ShowInfo();
                        Change(true);
                    }
                    break;
                case Keys.D8:
                case Keys.NumPad8:
                    if (IsPaintMode)
                    {
                        Light = !Light;
                        Color += Color < 8 ? 8 : -8;
                        ShowInfo();
                        Change(true);
                    }
                    break;
                case Keys.D9:
                case Keys.NumPad9:
                    if (IsPaintMode)
                    {
                        Color = 15;
                        ShowInfo();
                        Change(true);
                    }
                    break;
                case Keys.D0:
                case Keys.NumPad0:
                    if (IsPaintMode)
                    {
                        Color = 0;
                        Change(true);
                    }
                    break;
                case Keys.Space:
                    IsPaintMode = !IsPaintMode;
                    Change(true);
                    break;
                case Keys.Left:
                    if (Col > 0)
                    {
                        Change(false);
                        Col--;
                        Change(true);
                    }
                    break;
                case Keys.Right:
                    if (Col < Width - 1)
                    {
                        Change(false);
                        Col++;
                        Change(true);
                    }
                    break;
                case Keys.Up:
                    if (Row > 0)
                    {
                        Change(false);
                        Row--;
                        Change(true);
                    }
                    break;
                case Keys.Down:
                    if (Row < Height - 2)
                    {
                        Change(false);
                        Row++;
                        Change(true);
                    }
                    break;
            }
        }
    }
}
