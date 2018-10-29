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
                    return "Loser:(";
                return "WINNER!";
            }
        }

        Game(AnimationType animation, string message)
        {
            _board = new AnimationBoard(animation, message);
        }

        internal Game(Option option)
        {
            switch (option)
            {
                case Option.Level: _board = new LevelBoard(); break;
                case Option.Tetris: _board = new TetrisBoard(); break;
                case Option.Runner: _board = new RunnerBoard(); break;
                case Option.Helicopt: _board = new HelicopterBoard(); break;
                case Option.Breakout: _board = new BreakoutBoard(); break;
                case Option.Invaders: _board = new InvadersBoard(); break;
                case Option.Snake: _board = new SnakeBoard(); break;
                case Option.Sokoban: _board = new SokobanBoard(); break;
                case Option.Table: _board = new TableBoard(); break;
                case Option.Paint: break;
            }
        }

        internal void Start()
        {
            if (_board == null)
                return;

            int width, height;
            UI.GetSize(out width, out height);
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

            if(_board.Win.HasValue)
            {
                var animation = new Game(_board.Win.Value ? AnimationType.Fireworks : AnimationType.Rain, EndedMessage);
                UI.Clear();
                animation.Start();
            }

            UI.Resize(width, height);
        }

        void Draw()
        {
            foreach(var pnl in _board.Panels.Values)
            {
                if (pnl.HasChanges)
                {
                    UI.PanelDraw(pnl);
                    pnl.HasChanges = false;
                }
            }

            if (_board.Highscore.HasValue)
            {
                if (_board.Border.Width > 8)
                    UI.TextAt(string.Format($"{_board.Highscore.Value,3}"), 0, 1);
                UI.TextAt(string.Format($"{_board.Score,3}"), 0, _board.Border.Width - 4);
            }
            if(_board.Level >= 0)
                UI.TextAt(string.Format($"L{_board.Level,2}"),
                    _board.Border.Height - 1, (_board.Border.Width + 1) / 2 - 2,
                    15 - (_board.Level >= 15 ? 0 : _board.Level));
        }

        bool HandleInput(bool paused)
        {
            string key = UI.Input();
            if (key != null)
            {
                if (paused)
                    return false;

                if (key == "Escape")
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
