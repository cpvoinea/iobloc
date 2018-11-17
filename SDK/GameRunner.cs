using System;
using System.Threading;

namespace iobloc
{
    // Minimalist running of IGame. The algorithm is:
    // Draw(game.Border)
    // game.Start()
    // do
    //   Draw(game.Panels)
    //   key <= Input()
    //   if (game.AllowedKeys contains key)
    //     game.HandleInput(key)
    //   else game.TogglePause()
    //   wait for game.FrameInterval (ms)
    //   game.NextFrame()
    // while (key is not Escape)
    // game.Stop()
    public static class GameRunner
    {
        // Summary:
        // Minimalist running of IGame. The algorithm is:
        // Draw(game.Border)
        // game.Start()
        // do
        //   Draw(game.Panels)
        //   key <= Input()
        //   if (game.AllowedKeys contains key)
        //     game.HandleInput(key)
        //   else game.TogglePause()
        //   wait for game.FrameInterval (ms)
        //   game.NextFrame()
        // while (key is not Escape)
        // game.Stop()
        public static void Run(IGame game)
        {
            game.Start();
            if (!game.IsRunning)
                return;

            Renderer.DrawBorder(game.Border); // initial setup
            DateTime start = DateTime.Now; // frame start time
            int ticks = 0; // elapsed time in ms
            while (game.IsRunning)
            {
                Paint(game);
                bool paused = HandleInput(game);
                if (!game.IsRunning)
                    break;

                if (paused)
                {
                    Paint(game, true); // toggle to paused and draw
                    if (!game.IsRunning)
                        break;
                    Renderer.InputWait(); // wait for any key press
                    Paint(game, true); // unpause and draw
                }

                if (game.IsRunning && game.FrameInterval > 0)
                {
                    Thread.Sleep(20);
                    ticks = (int)DateTime.Now.Subtract(start).TotalMilliseconds;
                    if (ticks > game.FrameInterval) // move to next frame
                    {
                        game.NextFrame();
                        start = DateTime.Now;
                        ticks -= game.FrameInterval;
                    }
                }
            }
        }

        // Summary:
        //      Decide on key reaction: exit on Escape, handle AllowedKeys or pause on rest
        private static bool HandleInput(IGame game)
        {
            string key = Renderer.Input();
            if (key == null)
                return false;

            if (key == UIKey.Escape)
                game.Stop(); // stop on Escape
            else if (game.AllowedKeys.Contains(key))
                game.HandleInput(key); // handle if key is allowed
            else
                return true; // pause if key is not allowed

            return false;
        }

        // Summary:
        //      Use HasChanges property to decide if draw is needed and set it to false if drawn
        // Parameters: togglePause: toggle pause before drawing (switch to text mode or back)
        private static void Paint(IGame game, bool togglePause = false)
        {
            if (togglePause)
                game.TogglePause();
            foreach (var p in game.Panels.Values)
                if (p.HasChanges)
                {
                    Renderer.DrawPanel(p);
                    p.Change(false);
                }
        }
    }
}
