using System;
using System.Threading;

namespace iobloc
{
    static class BoardRunner
    {
        public static void Run(IBoard board)
        {
            UIPainter.DrawBorder(board.Border);

            bool paused = false;
            DateTime start = DateTime.Now;
            board.Start();
            while (board.IsRunning)
            {
                Paint(board);
                paused = HandleInput(board, paused);

                if (paused)
                {
                    Paint(board, true);
                    UIPainter.InputWait();
                    Paint(board, true);
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

        private static bool HandleInput(IBoard board, bool paused)
        {
            string key = UIPainter.Input();
            if (key == null)
                return paused;

            if (paused)
                return false;
            if (key == "Escape")
                board.Stop();
            else if (board.IsValidInput(key))
                board.HandleInput(key);
            else
                return true;

            return paused;
        }

        private static void Paint(IBoard board, bool toggle = false)
        {
            if (toggle)
                board.TogglePause();
            foreach (var p in board.Panels.Values)
                if (p.HasChanges)
                {
                    UIPainter.DrawPanel(p);
                    p.HasChanges = false;
                }
        }
    }
}
