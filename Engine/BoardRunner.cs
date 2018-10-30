using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace iobloc
{
    class BoardRunner
    {
        readonly StringBuilder _log;
        readonly IBoard _board;
        readonly int _winWidth, _winHeight;
        bool _paused;

        internal BoardRunner(IBoard board, StringBuilder log)
        {
            _board = board;
            _log = log;
            UIPainter.GetSize(out _winWidth, out _winHeight);
            UIPainter.DrawBorder(_board.UIBorder);
        }

        internal void Run()
        {
            DateTime start = DateTime.Now;
            _board.IsRunning = true;
            while (_board.IsRunning)
            {
                Draw();
                HandleInput();

                if (_paused)
                {
                    WaitScreen();
                    _paused = false;
                }

                if (_board.FrameInterval > 0)
                {
                    Thread.Sleep(1);
                    double ticks = DateTime.Now.Subtract(start).TotalMilliseconds;
                    if (ticks > _board.FrameInterval)
                    {
                        _board.NextFrame();
                        start = DateTime.Now;
                    }
                }
            }

            End();
        }

        void HandleInput()
        {
            string key = UIPainter.Input();
            if (key == null)
                return;

            if (_paused)
                _paused = false;
            else if (key == "Escape")
                _board.IsRunning = false;
            else if (_board.IsValidInput(key))
                _board.HandleInput(key);
            else
                _paused = true;
        }

        void WaitScreen()
        {
            UIPainter.ClearPanel(_board.Main);
            UIPainter.DrawPanelText(_board.Main, _board.Help);
            UIPainter.InputWait();
            UIPainter.DrawPanel(_board.Main);
        }

        void End()
        {
            if (_board.Win.HasValue)
            {
                var runner = _board.Win.Value ?
                    new BoardRunner(new FireworksBoard(), _log) :
                    new BoardRunner(new RainingBloodBoard(), _log);
                runner.Run();
            }

            UIPainter.Resize(_winWidth, _winHeight);
        }

        void Draw()
        {
            foreach (var pnl in _board.Panels.Values)
            {
                if (pnl.HasChanges)
                {
                    UIPainter.DrawPanel(pnl);
                    pnl.HasChanges = false;
                }
            }

            DrawScore();
            DrawLevel();
        }

        void DrawScore()
        {
            if (_board.Highscore.HasValue)
            {
                if (_board.UIBorder.Width > 8)
                    UIPainter.TextAt(0, 1, string.Format($"{_board.Highscore.Value,3}"));
                UIPainter.TextAt(0, _board.UIBorder.Width - 4, string.Format($"{_board.Score,3}"));
            }
        }

        void DrawLevel()
        {
            if (_board.Level >= 0)
                UIPainter.TextAt(
                    _board.UIBorder.Height - 1,
                    (_board.UIBorder.Width + 1) / 2 - 2,
                    string.Format($"L{_board.Level,2}"));
        }
    }
}
