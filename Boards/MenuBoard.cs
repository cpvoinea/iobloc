namespace iobloc
{
    class MenuBoard : BaseBoard
    {
        internal MenuBoard() : base(BoardType.Menu) { }

        protected override void InitializeGrid()
        {
            Main.SetText(Settings.GetList("MenuItems"));
            
            base.InitializeGrid();
        }

        protected override void ChangeGrid(bool set)
        {
            Level = Serializer.Level;

            base.ChangeGrid(set);
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
                IsRunning = false;
                Next = Serializer.GetBoard(type);
            }
        }
    }
}
