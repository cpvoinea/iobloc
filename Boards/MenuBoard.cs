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
            IBoard board = null;
            switch (key)
            {
                case "D0": board = new LevelBoard(); break;
                case "D1": board = new TetrisBoard(); break;
                case "D2": board = new RunnerBoard(); break;
                case "D3": board = new HelicopterBoard(); break;
                case "D4": board = new BreakoutBoard(); break;
                case "D5": board = new InvadersBoard(); break;
                case "D6": board = new SnakeBoard(); break;
                case "D7": board = new SokobanBoard(); break;
                case "D8": board = new TableBoard(); break;
                case "D9": board = new PaintBoard(); break;
                case "L": board = new LogBoard(); break;
                case "F": board = new AnimationBoard(BoardType.Fireworks); break;
                case "R": board = new AnimationBoard(BoardType.RainingBlood); break;
            }

            if (board != null)
                BoardRunner.Run(board);
        }
    }
}
