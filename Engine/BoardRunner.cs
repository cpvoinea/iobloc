using System;
using System.Threading;

namespace iobloc
{
    /// <summary>
    /// Minimalist running of IBoard. The algorithm is:
    /// Draw(board.Border)
    /// board.Start()
    /// do
    ///   Draw(board.Panels)
    ///   key <= Input()
    ///   if (board.AllowedKeys contains key)
    ///     board.HandleInput(key)
    ///   else board.TogglePause()
    ///   wait for board.FrameInterval (ms)
    ///   board.NextFrame()
    /// while (key is not Escape)
    /// board.Stop()
    /// </summary>
    static class BoardRunner
    {
        /// <summary>
        /// Minimalist running of IBoard. The algorithm is:
        /// Draw(board.Border)
        /// board.Start()
        /// do
        ///   Draw(board.Panels)
        ///   key <= Input()
        ///   if (board.AllowedKeys contains key)
        ///     board.HandleInput(key)
        ///   else board.TogglePause()
        ///   wait for board.FrameInterval (ms)
        ///   board.NextFrame()
        /// while (key is not Escape)
        /// board.Stop()
        /// </summary>
        public static void Run(IBoard board)
        {
            UIPainter.DrawBorder(board.Border); // initial setup

            DateTime start = DateTime.Now; // frame start time
            int ticks = 0; // elapsed time in ms
            board.Start();
            while (board.IsRunning)
            {
                Paint(board);
                bool paused = HandleInput(board);
                if (!board.IsRunning)
                    break;

                if (paused)
                {
                    Paint(board, true); // toggle to paused and draw
                    UIPainter.InputWait(); // wait for any key press
                    Paint(board, true); // unpause and draw
                }

                if (board.FrameInterval > 0)
                {
                    Thread.Sleep(20);
                    ticks = (int)DateTime.Now.Subtract(start).TotalMilliseconds;
                    if (ticks > board.FrameInterval) // move to next frame
                    {
                        board.NextFrame();
                        start = DateTime.Now;
                        ticks -= board.FrameInterval;
                    }
                }
            }
        }

        /// <summary>
        /// Decide on key reaction: exit on Escape, handle AllowedKeys or pause on rest
        /// </summary>
        /// <param name="board"></param>
        /// <returns>true if paused</returns>
        private static bool HandleInput(IBoard board)
        {
            string key = UIPainter.Input();
            if (key == null)
                return false;

            if (key == UIKeys.Escape)
                board.Stop(); // stop on Escape
            else if (board.AllowedKeys.Contains(key))
                board.HandleInput(key); // handle if key is allowed
            else
                return true; // pause if key is not allowed

            return false;
        }

        /// <summary>
        /// Use HasChanges property to decide if draw is needed and set it to false if drawn
        /// </summary>
        /// <param name="board"></param>
        /// <param name="togglePause">toggle pause before drawing (switch to text mode or back)</param>
        private static void Paint(IBoard board, bool togglePause = false)
        {
            if (togglePause)
                board.TogglePause();
            foreach (var p in board.Panels.Values)
                if (p.HasChanges)
                {
                    UIPainter.DrawPanel(p);
                    p.Change(false);
                }
        }
    }
}
