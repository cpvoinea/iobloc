namespace iobloc
{
    // Display list of available boards to link through Next
    class MenuBoard : BaseBoard
    {
        bool _exit;

        // Summary:
        //      A text board with each line item having its own shortcut key(s)
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

        // Summary:
        //      Initialize in text mode
        protected override void Initialize()
        {
            base.Initialize();
            DrawLogo();
            Main.SwitchMode();
        }

        // Summary:
        //      Overriden to display correct MasterLevel value
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

        // Summary:
        //      Link to correct menu item board based on key
        public override void HandleInput(string key)
        {
            Next = Serializer.GetBoard(key);
            if (Next != null)
                Stop();
        }
    }
}
