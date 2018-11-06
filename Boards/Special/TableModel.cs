using System;
using System.Collections.Generic;

namespace iobloc
{
    enum TableState { Running = 0, Win, Lose }

    class TableModel
    {
        public static int H, PW, HC, PC, EC, NC;
        private UIPanel pnlUpperLeft, pnlLowerLeft, pnlUpperTaken, pnlDice, pnlLowerTaken, pnlUpperRight, pnlLowerRight, pnlUpperOut, pnlLowerOut;
        private readonly TableLine[] _lines = new TableLine[28];
        private readonly Random _random = new Random();
        private readonly int[] _dice = new int[4];
        private bool _whiteTurn;
        private int _selection;
        private int _picked;

        public UIPanel[] Panels { get; private set; }

        public TableState State { get; private set; }

        public TableModel(int h, int pw, int hc, int pc, int ec, int nc)
        {
            H = h;
            PW = pw;
            HC = hc;
            PC = pc;
            EC = ec;
            NC = nc;

            var pnlUpperLeft = new UIPanel(1, 1, H / 2 - 1, 6 * PW, 0, (char)UISymbol.BlockUpper);
            var pnlLowerLeft = new UIPanel(H / 2, 1, H - 2, 6 * PW, 0, (char)UISymbol.BlockLower);
            var pnlUpperTaken = new UIPanel(1, 6 * PW + 2, H / 2 - 4, 7 * PW + 1, 0, (char)UISymbol.BlockUpper);
            var pnlDice = new UIPanel(H / 2 - 2, 6 * PW + 2, H / 2 + 1, 7 * PW + 1, 2);
            var pnlLowerTaken = new UIPanel(H / 2 + 3, 6 * PW + 2, H - 2, 7 * PW + 1, 0, (char)UISymbol.BlockLower);
            var pnlUpperRight = new UIPanel(1, 7 * PW + 3, H / 2 - 1, 13 * PW + 2, 0, (char)UISymbol.BlockUpper);
            var pnlLowerRight = new UIPanel(H / 2, 7 * PW + 3, H - 2, 13 * PW + 2, 0, (char)UISymbol.BlockLower);
            var pnlUpperOut = new UIPanel(1, 13 * PW + 4, H / 2 - 1, 14 * PW + 3, 0, (char)UISymbol.BlockUpper);
            var pnlLowerOut = new UIPanel(H / 2, 13 * PW + 4, H - 2, 14 * PW + 3, 0, (char)UISymbol.BlockLower);

            for (int i = 0; i < 6; i++)
            {
                _lines[i] = new TableLine(pnlLowerRight, 5 - i, pnlLowerRight.Height - 1, true);
                _lines[i + 6] = new TableLine(pnlLowerLeft, 5 - i, pnlLowerLeft.Height - 1, true);
                _lines[i + 12] = new TableLine(pnlUpperLeft, i);
                _lines[i + 18] = new TableLine(pnlUpperRight, i);
            }
            _lines[24] = new TableLine(pnlUpperTaken, 0);
            _lines[25] = new TableLine(pnlLowerTaken, 0, pnlLowerTaken.Height - 1, true);
            _lines[26] = new TableLine(pnlLowerOut, 0, pnlLowerOut.Height - 1, true);
            _lines[27] = new TableLine(pnlUpperOut, 0);

            // !!! don't change the order !!!
            Panels = new[] { pnlUpperLeft, pnlLowerLeft, pnlUpperTaken, pnlDice, pnlLowerTaken, pnlUpperRight, pnlLowerRight, pnlUpperOut, pnlLowerOut };
            State = TableState.Running;
        }

        private void ThrowDice()
        {
            int d1 = _random.Next(6) + 1;
            int d2 = _random.Next(6) + 1;
            _dice[0] = d1;
            _dice[1] = d2;
            _dice[2] = _dice[3] = d1 == d2 ? d1 : 0;
        }

        public void Initialize()
        {
            for (int i = 0; i < _lines.Length; i++)
                _lines[i].Clear();
            _lines[0].Set(2, false);
            _lines[5].Set(5, true);
            _lines[7].Set(3, true);
            _lines[11].Set(5, false);
            _lines[12].Set(5, true);
            _lines[16].Set(3, false);
            _lines[18].Set(5, false);
            _lines[23].Set(2, true);

            ThrowDice();
        }

        public void MoveLeft()
        {
            if (_selection < _lines.Length)
            {
                _lines[_selection].Select(false);
                _selection++;
                _lines[_selection].Select(true);
            }
        }

        public void MoveRight()
        {
            if (_selection > 0)
            {
                _lines[_selection].Select(false);
                _selection--;
                _lines[_selection].Select(true);
            }
        }

        public void Pick()
        {
            if (_lines[_selection].Count > 0)
            {
                _lines[_selection].Pick();
                _picked++;
            }
        }

        public void Put()
        {
            if (_picked > 0)
            {
                _lines[_selection].Put(_whiteTurn);
                _picked--;
            }
        }
    }
}
