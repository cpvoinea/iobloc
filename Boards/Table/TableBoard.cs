namespace iobloc
{
    class TableBoard : BaseBoard
    {
        public static int BW, B, CP, CE, CN, CH;
        private TableController _controller;

        public TableBoard() : base(BoardType.Table) { }

        protected override void InitializeSettings()
        {
            base.InitializeSettings();
            BW = BlockWidth;
            B = Block;
            CP = BoardSettings.GetColor(Settings.PlayerColor);
            CE = BoardSettings.GetColor(Settings.EnemyColor);
            CN = BoardSettings.GetColor(Settings.NeutralColor);
            CH = BoardSettings.GetColor("HighlightColor");
        }

        protected override void InitializeUI()
        {
            base.InitializeUI();

            Border.AddLines(new[]
            {
                new UIBorderLine(0, Height + 1, 6 * Block + 1, true),
                new UIBorderLine(0, Height + 1, 7 * Block + 2, true),
                new UIBorderLine(0, Height + 1, 13 * Block + 3, true),
                new UIBorderLine(6 * Block + 1, 7 * Block + 2, Height / 2 - 2, false),
                new UIBorderLine(6 * Block + 1, 7 * Block + 2, Height / 2 + 3, false)
            });

            var model = new TableModel(Height, Block);
            Panels.Add(Pnl.Table.UpperLeft, model.Panels[0]);
            Panels.Add(Pnl.Table.LowerLeft, model.Panels[1]);
            Panels.Add(Pnl.Table.UpperTaken, model.Panels[2]);
            Panels.Add(Pnl.Table.Dice, model.Panels[3]);
            Panels.Add(Pnl.Table.LowerTaken, model.Panels[4]);
            Panels.Add(Pnl.Table.UpperRight, model.Panels[5]);
            Panels.Add(Pnl.Table.LowerRight, model.Panels[6]);
            Panels.Add(Pnl.Table.UpperOut, model.Panels[7]);
            Panels.Add(Pnl.Table.LowerOut, model.Panels[8]);

            _controller = new TableController(model);
        }

        protected override void Initialize()
        {
            Level = 0; // for frame multiplier
            Panels[Pnl.Table.UpperLeft].SetText(Help, false);
            _controller.Initialize();
        }

        public override void TogglePause()
        {
            Panels[Pnl.Table.UpperLeft].ToggleText();
        }

        public override void HandleInput(string key)
        {
            switch (key)
            {
                case UIKey.LeftArrow: _controller.Move(true); break;
                case UIKey.RightArrow: _controller.Move(false); break;
                case UIKey.UpArrow: _controller.Action(true); break;
                case UIKey.DownArrow: _controller.Action(false); break;
                case "R": _controller.Initialize(); break;
            }
            if (_controller.State == State.Ended)
                Win(true);
        }
    }
}
