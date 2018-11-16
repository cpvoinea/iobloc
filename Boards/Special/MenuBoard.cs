namespace iobloc
{
    /// <summary>
    /// Display list of available boards to link through Next
    /// </summary>
    class MenuBoard : BaseBoard
    {
        bool _exit;

        /// <summary>
        /// A text board with each line item having its own shortcut key(s)
        /// </summary>
        /// <returns></returns>
        public MenuBoard() : base(BoardType.Menu) { }

        private void DrawLogo()
        {
            int[,] logo = new[,]{
                {0,0,1,1,0,0,1,1,0,0},
                {0,0,1,1,0,1,0,0,1,0},
                {0,0,1,1,0,0,1,1,0,0},
                {0,0,0,0,0,0,0,0,0,0},
                {0,1,1,0,0,0,1,1,0,0},
                {0,1,1,1,1,0,1,1,0,0},
                {0,1,1,1,1,0,1,1,1,0},
                {0,0,0,0,0,0,0,0,0,0},
                {0,0,1,1,0,0,1,1,1,0},
                {0,1,0,0,1,0,1,0,0,0},
                {0,0,1,1,0,0,1,1,1,0},
            };
            var r = new System.Random();
            for (int i = 0; i < 11; i++)
                for (int j = 0; j < 10; j++)
                    if (i < Height && j < Width && logo[i, j] == 1)
                        Main[i, j] = r.Next(15) + 1;
        }

        /// <summary>
        /// Initialize in text mode
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            DrawLogo();
            Main.SwitchMode();
        }

        /// <summary>
        /// Overriden to display correct MasterLevel value
        /// </summary>
        public override void Start()
        {
            Level = Settings.MasterLevel;
            base.Start();
            if (Help.Length == 1 && AllowedKeys.Length > 0)
            {
                if (_exit)
                    Stop();
                else
                {
                    _exit = true;
                    HandleInput(AllowedKeys[0]);
                }
            }
        }

        /// <summary>
        /// Link to correct menu item board based on key
        /// </summary>
        /// <param name="key"></param>
        public override void HandleInput(string key)
        {
            Next = Serializer.GetBoard(key);
            if (Next != null)
                Stop();
        }
    }
}
