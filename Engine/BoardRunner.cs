using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace iobloc
{
    static class BoardRunner
    {
        internal static void Run(IBoard board)
        {
            UIPainter.DrawBorder(board.UIBorder);

            bool paused = false;
            DateTime start = DateTime.Now;
            board.IsRunning = true;
            while (board.IsRunning)
            {
                Draw(board);
                paused = HandleInput(board, paused);

                if (paused)
                {
                    WaitScreen(board);
                    paused = false;
                }

                if (board.FrameInterval > 0)
                {
                    Thread.Sleep(1);
                    double ticks = DateTime.Now.Subtract(start).TotalMilliseconds;
                    if (ticks > board.FrameInterval)
                    {
                        board.NextFrame();
                        start = DateTime.Now;
                    }
                }
            }

            if (board.Next != null)
                Run(board.Next);
        }

        static bool HandleInput(IBoard board, bool paused)
        {
            string key = UIPainter.Input();
            if (key == null)
                return paused;

            if (paused)
                return false;
            if (key == "Escape")
                board.IsRunning = false;
            else if (board.IsValidInput(key))
                board.HandleInput(key);
            else
                return true;

            return paused;
        }

        static void Draw(IBoard board)
        {
            foreach (var pnl in board.Panels.Values)
            {
                if (pnl.HasChanges)
                {
                    UIPainter.DrawPanel(pnl);
                    pnl.HasChanges = false;
                }
            }

            if (board.Highscore.HasValue)
            {
                if (board.UIBorder.Width > 8)
                    UIPainter.TextAt(0, 1, string.Format($"{board.Highscore.Value,3}"));
                UIPainter.TextAt(0, board.UIBorder.Width - 4, string.Format($"{board.Score,3}"));
            }

            if (board.Level >= 0)
                UIPainter.TextAt(board.UIBorder.Height - 1, (board.UIBorder.Width + 1) / 2 - 2, string.Format($"L{board.Level,2}"));
        }

        static void WaitScreen(IBoard board)
        {
            UIPainter.ClearPanel(board.Main);
            UIPainter.DrawPanelText(board.Main, board.Help);
            UIPainter.InputWait();
            UIPainter.DrawPanel(board.Main);
        }
    }
}
