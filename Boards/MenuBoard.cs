using System;

namespace iobloc
{
    class MenuBoard : BaseBoard
    {
        internal MenuBoard() : base(BoardType.Menu)
        {
            _main.SetText(_settings.GetList("MenuItems"));
        }

        public override void HandleInput(string key)
        {
            var option = GetSelection(key);
            IBoard board = null;
            switch(option)
            {
                case BoardType.Level: board = new LevelBoard(); break;
            }
        }

        public override void NextFrame(){}

        BoardType? GetSelection(string input)
        {
            ConsoleKey key = (ConsoleKey)Enum.Parse(typeof(ConsoleKey), input);

            if (key == ConsoleKey.L)
                return BoardType.Log;
            if (key == ConsoleKey.F)
                return BoardType.Fireworks;
            if (key == ConsoleKey.R)
                return BoardType.RainingBlood;

            int? val = null;
            if (key >= ConsoleKey.D0 && key <= ConsoleKey.D9)
                val = key - ConsoleKey.D0;
            else if (key >= ConsoleKey.NumPad0 && key <= ConsoleKey.NumPad9)
                val = key - ConsoleKey.NumPad0;
            if (val.HasValue)
                return (BoardType)val.Value;

            return null;
        }
    }
}
