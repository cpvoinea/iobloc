using System;
using System.Threading;

namespace iobloc
{
    class Game : IDisposable
    {
        Config _config;
        UI _ui;
        IBoard _board;
        internal event GameExit OnExit;

        internal Game(Config config, UI ui, Option option)
        {
            _config = config;
            _ui = ui;

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
                case Option.Log: _board = null; break;
            }
        }

        internal void Start()
        {
            _ui.Clear();
            _ui.BorderDraw(_board.Border);

            bool paused = false;
            int ticks = 0;
            _board.IsRunning = true;
            while (_board.IsRunning)
            {
                for(int i = 0; i < _board.Panels.Length; i++)
                {
                    var pnl = _board.Panels[i];
                    if (pnl.HasChanges)
                    {
                        _ui.PanelDraw(pnl);
                        pnl.HasChanges = false;
                    }
                }
                
                Thread.Sleep(1);
                ticks++;
                if (ticks >= _board.FrameInterval)
                {
                    ticks = 0;
                    _board.NextFrame();
                }

                var key = _ui.Input();
                if (key.HasValue)
                {
                    if (paused)
                        paused = false;
                    else if (key == Config.INPUT_EXIT)
                        _board.IsRunning = false;
                    else if (_board.IsValidInput(key.Value))
                        _board.HandleInput(key.Value);
                    else
                        paused = true;
                }

                if (paused)
                {
                    _ui.PanelClear(_board.MainPanel);
                    _ui.PanelTextLines(_board.MainPanel, _board.Help);
                    _ui.InputWait();
                    _ui.PanelDraw(_board.MainPanel);
                }
            }

            if (OnExit != null)
                OnExit(_board);
        }

        public void Dispose()
        {
            _board.Dispose();
        }
    }
}
