using System;
using System.Threading;

namespace iobloc
{
    static class BoardRunner
    {
        internal static void Run(IBoard board)
        {
            UIPainter.DrawBorder(board.Border);

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
