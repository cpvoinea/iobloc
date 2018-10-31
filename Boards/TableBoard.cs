using System;

namespace iobloc
{
    class TableBoard : BaseBoard
    {
        int PW => (int)Settings.GetInt("PieceWidth");
        int H => (int)Settings.GetInt("Height");
        int CP => (int)Settings.GetColor("PlayerColor");
        int CE => (int)Settings.GetColor("EnemyColor");
        int CN => (int)Settings.GetColor("NeutralColor");
        UIPanel PLR => Panels[Pnl.Table.LowerRight];
        UIPanel PLL => Panels[Pnl.Table.LowerLeft];
        UIPanel PUR => Panels[Pnl.Table.UpperRight];
        UIPanel PUL => Panels[Pnl.Table.UpperLeft];

        readonly Random _random = new Random();
        int _position;
        bool _white;

        internal TableBoard() : base(BoardType.Table)
        {
            Border.AddLines(new[]
            {
                new UIBorderLine(0, H - 1, 6 * PW + 1, true, true),
                new UIBorderLine(0, H - 1, 7 * PW + 2, true, true),
                new UIBorderLine(0, H - 1, 13 * PW + 3, true, true),
                new UIBorderLine(6 * PW + 1, 7 * PW + 2, H / 2 - 3, false, true),
                new UIBorderLine(6 * PW + 1, 7 * PW + 2, H / 2 + 2, false, true)
            });
            Panels.Add(Pnl.Table.UpperLeft, new UIPanel(1, 1, H / 2 - 1, 6 * PW, (char)UISymbolType.BlockUpper));
            Panels.Add(Pnl.Table.LowerLeft, new UIPanel(H / 2, 1, H - 2, 6 * PW, (char)UISymbolType.BlockLower));
            Panels.Add(Pnl.Table.UpperTaken, new UIPanel(1, 6 * PW + 2, H / 2 - 4, 7 * PW + 1, (char)UISymbolType.BlockUpper));
            Panels.Add(Pnl.Table.Dice, new UIPanel(H / 2 - 2, 6 * PW + 2, H / 2 + 1, 7 * PW + 1, (char)0));
            Panels.Add(Pnl.Table.LowerTaken, new UIPanel(H / 2 + 3, 6 * PW + 2, H - 2, 7 * PW + 1, (char)UISymbolType.BlockLower));
            Panels.Add(Pnl.Table.UpperRight, new UIPanel(1, 7 * PW + 3, H / 2 - 1, 13 * PW + 2, (char)UISymbolType.BlockUpper));
            Panels.Add(Pnl.Table.LowerRight, new UIPanel(H / 2, 7 * PW + 3, H - 2, 13 * PW + 2, (char)UISymbolType.BlockLower));
            Panels.Add(Pnl.Table.UpperOut, new UIPanel(1, 13 * PW + 4, H / 2 - 1, 14 * PW + 3, (char)UISymbolType.BlockUpper));
            Panels.Add(Pnl.Table.LowerOut, new UIPanel(H / 2, 13 * PW + 4, H - 2, 14 * PW + 3, (char)UISymbolType.BlockLower));
        }

        protected override void InitializeGrid()
        {
            ChangeGrid(false);
            foreach (var pnl in Panels.Values)
            {
                for (int i = 0; i < pnl.Height; i++)
                    for (int j = 0; j < pnl.Width; j++)
                        pnl[i, j] = 0;
            }

            for (int j = 0; j < PW; j++)
            {
                for (int i = 0; i < 5; i++)
                {
                    PLR[PLR.Height - 2 - i, j] = CP;
                    PLL[PLL.Height - 2 - i, j] = CE;
                    PUL[i + 1, j] = CP;
                    PUR[i + 1, j] = CE;
                }
                for (int i = 0; i < 3; i++)
                {
                    PLL[PLL.Height - 2 - i, 4 * PW + j] = CP;
                    PUL[i + 1, 4 * PW + j] = CE;
                }
                for (int i = 0; i < 2; i++)
                {
                    PLR[PLR.Height - 2 - i, 5 * PW + j] = CE;
                    PUR[i + 1, 5 * PW + j] = CP;
                }
            }

            _position = 0;
            _white = true;
            base.InitializeGrid();
        }

        protected override void ChangeGrid(bool set)
        {
            UIPanel pnl = null;
            int row = 0, col = 0;
            if (_position <= 0)
            {
                pnl = _white ? Panels[Pnl.Table.LowerOut] : Panels[Pnl.Table.UpperOut];
                row = _white ? pnl.Height - 1 : 0;
            }
            else if (_position <= 6)
            {
                pnl = PLR;
                row = pnl.Height - 1;
                col = 6 - _position;
            }
            else if (_position <= 12)
            {
                pnl = PLL;
                row = pnl.Height - 1;
                col = 12 - _position;
            }
            else if (_position <= 18)
            {
                pnl = PUL;
                col = _position - 13;
            }
            else if (_position <= 24)
            {
                pnl = PUR;
                col = _position - 19;
            }
            else if (_position >= 25)
            {
                pnl = _white ? Panels[Pnl.Table.UpperTaken] : Panels[Pnl.Table.LowerTaken];
                row = _white ? 0 : pnl.Height - 1;
            }

            if (pnl != null)
            {
                for (int i = col * PW; i < (col + 1) * PW; i++)
                    pnl[row, i] = set ? CN : 0;
            }

            base.ChangeGrid(set);
        }

        public override void HandleInput(string key)
        {
            ChangeGrid(false);
            switch (key)
            {
                case "LeftArrow":
                    if (_position <= 12)
                        _position++;
                    else if (_position <= 25)
                        _position--;
                    break;
                case "RightArrow":
                    if (_position <= 12 && _position > 0)
                        _position--;
                    else if (_position > 12 && _position < 25)
                        _position++;
                    break;
                case "UpArrow": break;
                case "DownArrow": break;
            }
            ChangeGrid(true);
        }
    }
}