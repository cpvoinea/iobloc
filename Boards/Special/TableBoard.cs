namespace iobloc
{
    class TableBoard : BaseBoard
    {
        private TableModel _model;

        public TableBoard() : base(BoardType.Table) { }

        protected override void InitializeSettings()
        {
            base.InitializeSettings();
            _model = new TableModel(BoardSettings.GetInt(Settings.Height), BoardSettings.GetInt("PieceWidth"), BoardSettings.GetColor("HighlightColor"),
                BoardSettings.GetColor(Settings.PlayerColor), BoardSettings.GetColor(Settings.EnemyColor), BoardSettings.GetColor(Settings.NeutralColor));
        }

        /// <summary>
        /// Add extra lines and panels
        /// </summary>
        protected override void InitializeUI()
        {
            base.InitializeUI();

            Border.AddLines(new[]
            {
                new UIBorderLine(0, TableModel.H + 1, 6 * TableModel.PW + 1, true, true),
                new UIBorderLine(0, TableModel.H + 1, 7 * TableModel.PW + 2, true, true),
                new UIBorderLine(0, TableModel.H + 1, 13 * TableModel.PW + 3, true, true),
                new UIBorderLine(6 * TableModel.PW + 1, 7 * TableModel.PW + 2, TableModel.H / 2 - 2, false, true),
                new UIBorderLine(6 * TableModel.PW + 1, 7 * TableModel.PW + 2, TableModel.H / 2 + 3, false, true)
            });

            Panels.Add(Pnl.Table.UpperLeft, _model.Panels[0]);
            Panels.Add(Pnl.Table.LowerLeft, _model.Panels[1]);
            Panels.Add(Pnl.Table.UpperTaken, _model.Panels[2]);
            Panels.Add(Pnl.Table.Dice, _model.Panels[3]);
            Panels.Add(Pnl.Table.LowerTaken, _model.Panels[4]);
            Panels.Add(Pnl.Table.UpperRight, _model.Panels[5]);
            Panels.Add(Pnl.Table.LowerRight, _model.Panels[6]);
            Panels.Add(Pnl.Table.UpperOut, _model.Panels[7]);
            Panels.Add(Pnl.Table.LowerOut, _model.Panels[8]);
        }

        /// <summary>
        /// Reset to starting configuration
        /// </summary>
        protected override void Initialize()
        {
            _model.Initialize();
        }

        public override void HandleInput(string key)
        {
            switch (key)
            {
                case UIKey.LeftArrow: _model.MoveLeft(); break;
                case UIKey.RightArrow: _model.MoveRight(); break;
                case UIKey.UpArrow: _model.Pick(); break;
                case UIKey.DownArrow: _model.Put(); break;
            }
            switch (_model.State)
            {
                case TableState.Win: Win(true); break;
                case TableState.Lose: Lose(); break;
            }
        }
    }
}
