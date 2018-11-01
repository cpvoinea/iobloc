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
                Draw(board);
                paused = HandleInput(board, paused);

                if (paused)
                {
                    Draw(board, true);
                    UIPainter.InputWait();
                    Draw(board, true);
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
                board.Stop();
            else if (board.IsValidInput(key))
                board.HandleInput(key);
            else
                return true;

            return paused;
        }

        static void Draw(IBoard board, bool toggle = false)
        {
            if (toggle)
                board.TogglePause();
            UIPainter.DrawPanels(board.Panels.Values);
        }
    }
}
