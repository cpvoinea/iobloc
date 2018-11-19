using System.Collections.Generic;

namespace iobloc
{
    class TableBoard
    {
        private readonly TableLine[] _lines = new TableLine[28];
        public TableLine this[bool isWhite, int line] { get { return _lines[GetIndex(isWhite, line)]; } }

        public TableBoard(Dictionary<string, Panel> panels, int bw, int b)
        {
            var pnlUpperLeft = panels[Pnl.Table.UpperLeft];
            var pnlLowerLeft = panels[Pnl.Table.LowerLeft];
            var pnlUpperTaken = panels[Pnl.Table.UpperTaken];
            var pnlLowerTaken = panels[Pnl.Table.LowerTaken];
            var pnlUpperRight = panels[Pnl.Table.UpperRight];
            var pnlLowerRight = panels[Pnl.Table.LowerRight];
            var pnlUpperOut = panels[Pnl.Table.UpperOut];
            var pnlLowerOut = panels[Pnl.Table.LowerOut];

            for (int i = 0; i < 6; i++)
            {
                bool isDark = i % 2 == 0;
                _lines[i] = new TableLine(pnlLowerRight, bw, b, 5 - i, pnlLowerRight.Height - 1, true);
                _lines[i + 6] = new TableLine(pnlLowerLeft, bw, b, 5 - i, pnlLowerLeft.Height - 1, true);
                _lines[i + 12] = new TableLine(pnlUpperLeft, bw, b, i, 0, false);
                _lines[i + 18] = new TableLine(pnlUpperRight, bw, b, i, 0, false);
            }
            _lines[24] = new TableLine(pnlUpperTaken, bw, b, 0, 0, false);
            _lines[25] = new TableLine(pnlLowerTaken, bw, b, 0, pnlLowerTaken.Height - 1, true);
            _lines[26] = new TableLine(pnlLowerOut, bw, b, 0, pnlLowerOut.Height - 1, true);
            _lines[27] = new TableLine(pnlUpperOut, bw, b, 0, 0, false);

            _lines[0].Initialize(2, false);
            _lines[5].Initialize(5, true);
            _lines[7].Initialize(3, true);
            _lines[11].Initialize(5, false);
            _lines[12].Initialize(5, true);
            _lines[16].Initialize(3, false);
            _lines[18].Initialize(5, false);
            _lines[23].Initialize(2, true);
        }

        public int[] GetLines(bool isWhite)
        {
            int[] result = new int[28];
            for (int i = 0; i < 28; i++)
            {
                var line = this[isWhite, i];
                result[i] = line.IsWhite == isWhite ? line.Count : -line.Count;
            }
            return result;
        }

        private int GetIndex(bool isWhite, int line)
        {
            if (line < 24)
                return isWhite ? line : 23 - line;
            if (line < 26)
                return isWhite ? 24 : 25;
            return isWhite ? 26 : 27;
        }
    }
}
