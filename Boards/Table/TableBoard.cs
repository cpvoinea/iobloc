namespace iobloc
{
    class TableBoard : BaseBoard
    {
        private TableModel _model;

        public TableBoard() : base(BoardType.Table) { }

        protected override void InitializeSettings()
        {
            base.InitializeSettings();
            _model = new TableModel(Height, BlockWidth, BoardSettings.GetColor("HighlightColor"),
                BoardSettings.GetColor(Settings.PlayerColor), BoardSettings.GetColor(Settings.EnemyColor), BoardSettings.GetColor(Settings.NeutralColor));
        }

        protected override void InitializeUI()
        {
            base.InitializeUI();

            Border.AddLines(new[]
            {
                new UIBorderLine(0, Height+ 1, 6 * BlockWidth + 1, true),
                new UIBorderLine(0, Height + 1, 7 * BlockWidth + 2, true),
                new UIBorderLine(0, Height + 1, 13 * BlockWidth + 3, true),
                new UIBorderLine(6 * BlockWidth + 1, 7 * BlockWidth + 2, Height / 2 - 2, false),
                new UIBorderLine(6 * BlockWidth + 1, 7 * BlockWidth + 2, Height / 2 + 3, false)
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

        protected override void Initialize()
        {
            Panels[Pnl.Table.UpperLeft].SetText(Help, false);
            _model.Initialize();
        }

        public override void TogglePause()
        {
            Panels[Pnl.Table.UpperLeft].ToggleText();
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
