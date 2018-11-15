namespace iobloc
{
    class TableBoard : BaseBoard
    {
        public static int BW, B, CP, CE, CN, CH;
        private static string AssemblyPath, ClassName;
        private TableController _controller;
        private PlayMode _mode;
        private ITableAI _externalAI;
        private ITableAI _internalAI;
        private string _nextMove;

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

            AssemblyPath = BoardSettings[Settings.AssemblyPath];
            ClassName = BoardSettings[Settings.ClassName];
            _mode = (PlayMode)BoardSettings.GetInt("PlayMode", 0);
            if (_mode != PlayMode.Coop && !string.IsNullOrEmpty(AssemblyPath) && !string.IsNullOrEmpty(ClassName))
            {
                _externalAI = Serializer.InstantiateFromAssembly<ITableAI>(AssemblyPath, ClassName);
                if (_externalAI == null)
                    _externalAI = new TableAI();
            }
            if (_mode == PlayMode.AI)
                _internalAI = new TableAI();
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
            var pnl = Panels[Pnl.Table.UpperLeft];
            pnl.SwitchMode();
        }

        public override void HandleInput(string key)
        {
            if (_controller.CurrentPlayerIsAI)
                return;
            _nextMove = key;
        }

        public override void NextFrame()
        {
            if (!string.IsNullOrEmpty(_nextMove))
                switch (_nextMove)
                {
                    case UIKey.LeftArrow: _controller.Move(true); break;
                    case UIKey.RightArrow: _controller.Move(false); break;
                    case UIKey.UpArrow: _controller.Action(); break;
                    case "R": _controller.Initialize(); break;
                }
            if (_controller.State == GameState.Ended)
                Win(true);
        }
    }
}
