namespace iobloc
{
    class MenuBoard : BaseBoard
    {
        private string[] _menuItems;

        public MenuBoard() : base(BoardType.Menu) { }

        public override void Reset()
        {
            Main.Text = Settings.GetList("MenuItems");
            Main.IsText = true;
            base.Reset();
        }

        public override void Paint()
        {
            Level = Serializer.MasterLevel;
            Next = null;
            Main.HasChanges = true;
        }

        public override void TogglePause()
        {
            if (_menuItems == null)
            {
                _menuItems = Main.Text;
                Main.Text = Help;
            }
            else
            {
                Main.Text = _menuItems;
                _menuItems = null;
            }
            Main.HasChanges = true;
        }

        public override void HandleInput(string key)
        {
            BoardType type = BoardType.Menu;
            switch (key)
            {
                case "D0": case "NumPad0": type = BoardType.Level; break;
                case "D1": case "NumPad1": type = BoardType.Tetris; break;
                case "D2": case "NumPad2": type = BoardType.Runner; break;
                case "D3": case "NumPad3": type = BoardType.Helicopt; break;
                case "D4": case "NumPad4": type = BoardType.Breakout; break;
                case "D5": case "NumPad5": type = BoardType.Invaders; break;
                case "D6": case "NumPad6": type = BoardType.Snake; break;
                case "D7": case "NumPad7": type = BoardType.Sokoban; break;
                case "D8": case "NumPad8": type = BoardType.Table; break;
                case "D9": case "NumPad9": type = BoardType.Paint; break;
                case "F": type = BoardType.Fireworks; break;
                case "R": type = BoardType.RainingBlood; break;
            }

            if (type != BoardType.Menu)
            {
                Next = Serializer.GetBoard(type);
                Stop();
            }
        }
    }
}
