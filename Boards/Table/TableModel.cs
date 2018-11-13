namespace iobloc
{
    class TableModel
    {
        private readonly TableLine[] _lines = new TableLine[28];
        public TableLine this[PlayerSide player, LineType type] { get { return _lines[GetIndex(player, type)]; } }
        private readonly UIPanel _pnlDice;
        public UIPanel[] Panels { get; private set; }

        public TableModel(int h, int p)
        {
            var pnlUpperLeft = new UIPanel(1, 1, 17, 6 * p, 0, (char)UISymbol.BlockUpper);
            var pnlLowerLeft = new UIPanel(h - 16, 1, h, 6 * p, 0, (char)UISymbol.BlockLower);
            var pnlUpperTaken = new UIPanel(1, 6 * p + 2, 15, 7 * p + 1, 0, (char)UISymbol.BlockUpper);
            _pnlDice = new UIPanel(h / 2 - 1, 6 * p + 2, h / 2 + 2, 7 * p + 1, 2);
            var pnlLowerTaken = new UIPanel(h - 14, 6 * p + 2, h, 7 * p + 1, 0, (char)UISymbol.BlockLower);
            var pnlUpperRight = new UIPanel(1, 7 * p + 3, 17, 13 * p + 2, 0, (char)UISymbol.BlockUpper);
            var pnlLowerRight = new UIPanel(h - 16, 7 * p + 3, h, 13 * p + 2, 0, (char)UISymbol.BlockLower);
            var pnlUpperOut = new UIPanel(1, 13 * p + 4, 16, 14 * p + 3, 0, (char)UISymbol.BlockUpper);
            var pnlLowerOut = new UIPanel(h - 15, 13 * p + 4, h, 14 * p + 3, 0, (char)UISymbol.BlockLower);

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
            Panels = new[] { pnlUpperLeft, pnlLowerLeft, pnlUpperTaken, _pnlDice, pnlLowerTaken, pnlUpperRight, pnlLowerRight, pnlUpperOut, pnlLowerOut };
        }

        public void Clear()
        {
            foreach (var l in _lines)
                l.Clear();
        }

        public void ClearSelection()
        {
            foreach (var l in _lines)
                l.ClearSelection();
        }

        public void ShowDice(string[] dice)
        {
            _pnlDice.Text = dice;
            _pnlDice.HasChanges = true;
        }

        private int GetIndex(PlayerSide player, LineType type)
        {
            int index = (int)type;
            if (type == LineType.Taken || type == LineType.Out)
                return index + (int)player;
            if (player == PlayerSide.Black)
                return 23 - index;
            return index;
        }
    }
}
