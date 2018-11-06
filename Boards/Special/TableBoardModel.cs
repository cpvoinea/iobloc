using System;
using System.Collections.Generic;

namespace iobloc
{
    enum TableState { Running = 0, Win, Lose }

    class TableBoardModel
    {
        class TablePanel
        {
            public UIPanel Panel { get; private set; }

            public TablePanel(UIPanel panel)
            {
                Panel = panel;
            }
        }

        class TableLine
        {
            public TablePanel Panel { get; private set; }
            public bool? IsWhite { get; private set; }
            public int Count { get; private set; }
            public bool IsSelected { get; private set; }

            public TableLine(TablePanel panel)
            {
                Panel = panel;
            }

            public void Set(int count, bool isWhite)
            {
                Count = count;
                IsWhite = isWhite;
                Panel.Panel.Clear();
            }
        }

        private int H, PW, HC, PC, EC, NC;
        private TablePanel pnlUpperLeft, pnlLowerLeft, pnlUpperTaken, pnlDice, pnlLowerTaken, pnlUpperRight, pnlLowerRight, pnlUpperOut, pnlLowerOut;
        private readonly TableLine[] _lines = new TableLine[28];
        private readonly Random _random = new Random();
        private readonly int[] _dice = new int[4];
        private readonly List<int> _allowedMoves = new List<int>();
        private bool _whiteTurn;
        private int _selection;

        public UIPanel[] Panels { get; private set; }

        public TableState State { get; private set; }

        public TableBoardModel(int h, int pw, int hc, int pc, int ec, int nc)
        {
            H = h;
            PW = pw;
            HC = hc;
            PC = pc;
            EC = ec;
            NC = nc;

            var pnlUpperLeft = new TablePanel(new UIPanel(1, 1, H / 2 - 1, 6 * PW, 0, (char)UISymbol.BlockUpper));
            var pnlLowerLeft = new TablePanel(new UIPanel(H / 2, 1, H - 2, 6 * PW, 0, (char)UISymbol.BlockLower));
            var pnlUpperTaken = new TablePanel(new UIPanel(1, 6 * PW + 2, H / 2 - 4, 7 * PW + 1, 0, (char)UISymbol.BlockUpper));
            var pnlDice = new TablePanel(new UIPanel(H / 2 - 2, 6 * PW + 2, H / 2 + 1, 7 * PW + 1, 2));
            var pnlLowerTaken = new TablePanel(new UIPanel(H / 2 + 3, 6 * PW + 2, H - 2, 7 * PW + 1, 0, (char)UISymbol.BlockLower));
            var pnlUpperRight = new TablePanel(new UIPanel(1, 7 * PW + 3, H / 2 - 1, 13 * PW + 2, 0, (char)UISymbol.BlockUpper));
            var pnlLowerRight = new TablePanel(new UIPanel(H / 2, 7 * PW + 3, H - 2, 13 * PW + 2, 0, (char)UISymbol.BlockLower));
            var pnlUpperOut = new TablePanel(new UIPanel(1, 13 * PW + 4, H / 2 - 1, 14 * PW + 3, 0, (char)UISymbol.BlockUpper));
            var pnlLowerOut = new TablePanel(new UIPanel(H / 2, 13 * PW + 4, H - 2, 14 * PW + 3, 0, (char)UISymbol.BlockLower));

            for(int i = 0; i < 6; i++)
            {
                _lines[i] = new TableLine(pnlLowerRight);
                _lines[i + 6] = new TableLine(pnlLowerLeft);
                _lines[i + 12] = new TableLine(pnlUpperLeft);
                _lines[i + 18] = new TableLine(pnlUpperRight);
            }
            _lines[24] = new TableLine(pnlUpperTaken);
            _lines[25] = new TableLine(pnlLowerTaken);
            _lines[26] = new TableLine(pnlLowerOut);
            _lines[27] = new TableLine(pnlUpperOut);

            // !!! don't change the order !!!
            Panels = new[] { pnlUpperLeft.Panel, pnlLowerLeft.Panel, pnlUpperTaken.Panel, pnlDice.Panel, pnlLowerTaken.Panel, pnlUpperRight.Panel, pnlLowerRight.Panel, pnlUpperOut.Panel, pnlLowerOut.Panel };
            State = TableState.Running;
        }

        public void Initialize()
        {
        }

        public void MoveLeft()
        {
        }

        public void MoveRight()
        {
        }

        public void Take()
        {
        }

        public void Put()
        {
        }
    }
}