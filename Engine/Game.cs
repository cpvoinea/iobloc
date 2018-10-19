using System;
using System.Diagnostics;
using System.Threading;

namespace iobloc
{
    class Game
    {
        IBoard _board;
        internal string EndedMessage
        {
            get
            {
                if (_board == null)
                    return string.Empty;
                if (!_board.Win.HasValue)
                    return "Quit";
                if (!_board.Win.Value)
                    return "Game Over";
                return "WINNER";
            }
        }

        internal Game(Option option)
        {
            switch (option)
            {
                case Option.Level: _board = new LevelBoard(); break;
                case Option.Tetris: _board = new TetrisBoard(); break;
                case Option.Runner: _board = new RunnerBoard(); break;
                case Option.Helicopter: _board = new HelicopterBoard(); break;
                case Option.Breakout: _board = new BreakoutBoard(); break;
                case Option.Invaders: _board = new InvadersBoard(); break;
                case Option.Snake: _board = new SnakeBoard(); break;
                case Option.Sokoban: _board = new SokobanBoard(); break;
                case Option.Animation: _board = new AnimationBoard(0); break;
            }
        }

        internal void Start()
        {
            if (_board == null)
                return;

            UI.BorderDraw(_board.Border);
            bool paused = false;
            _board.IsRunning = true;
            DateTime start = DateTime.Now;
            while (_board.IsRunning)
            {
                Draw();
                paused = HandleInput(paused);
                if (paused)
                {
                    WaitScreen();
                    paused = false;
                }

                Thread.Sleep(1);
                double ticks = DateTime.Now.Subtract(start).TotalMilliseconds;
                if (ticks > _board.FrameInterval)
                {
                    _board.NextFrame();
                    start = DateTime.Now;
                }
            }

            if (_board.Win == true)
            {
                var congrats = new Game(Option.Animation);
                UI.Clear();
                congrats.Start();
            }
        }

        void Draw()
        {
            for (int i = 0; i < _board.Panels.Length; i++)
            {
                var pnl = _board.Panels[i];
                if (pnl.HasChanges)
                {
                    UI.PanelDraw(pnl);
                    pnl.HasChanges = false;
                }
            }

            if (_board.Highscore.HasValue)
            {
                if (2 * Config.LEN_INFO < _board.Border.Width - 2)
                    UI.TextAt(string.Format($"{_board.Highscore.Value,Config.LEN_INFO}"), 0, 1);
                UI.TextAt(string.Format($"{_board.Score,Config.LEN_INFO}"), 0, _board.Border.Width - 1 - Config.LEN_INFO);
            }
            UI.TextAt(string.Format($"L{_board.Level,2}"), _board.Border.Height - 1, _board.Border.Width / 2 - 2);
        }

        bool HandleInput(bool paused)
        {
            string key = UI.Input();
            if (key != null)
            {
                if (paused)
                    return false;

                if (key == Config.INPUT_EXIT)
                    _board.IsRunning = false;
                else if (_board.IsValidInput(key))
                    _board.HandleInput(key);
                else
                    return true;
            }

            return paused;
        }

        void WaitScreen()
        {
            UI.PanelClear(_board.MainPanel);
            UI.PanelTextLines(_board.MainPanel, _board.Help);
            UI.InputWait();
            UI.PanelDraw(_board.MainPanel);
        }
    }
}
