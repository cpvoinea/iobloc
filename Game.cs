using System;
using System.Threading;

namespace iobloc
{
    class Game : IDisposable
    {
        IBoard _board;
        internal event GameExit OnExit;

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
                case Option.Log: _board = null; break;
            }
        }

        internal void Start()
        {
            UI.BorderDraw(_board.Border);

            bool paused = false;
            int ticks = 0;
            _board.IsRunning = true;
            while (_board.IsRunning)
            {
                DrawBoard();
                paused = HandleInput(paused);
                if (paused)
                {
                    WaitScreen();
                    paused = false;
                }

                Thread.Sleep(1);
                ticks++;
                if (ticks >= _board.FrameInterval)
                {
                    ticks = 0;
                    _board.NextFrame();
                }
            }

            if (OnExit != null)
                OnExit(_board);
        }

        void DrawBoard()
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

        public void Dispose()
        {
            _board.Dispose();
        }
    }
}
