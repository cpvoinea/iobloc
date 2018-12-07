using System;

namespace iobloc
{
    // Minimalist running of IGame. The algorithm is:
    // Draw(game.Border)
    // game.Start()
    // do
    //   Draw(game.Panes)
    //   key <= Input()
    //   if (game.AllowedKeys contains key)
    //     game.HandleInput(key)
    //   else game.TogglePause()
    //   wait for game.FrameInterval (ms)
    //   game.NextFrame()
    // while (key is not Escape)
    // game.Stop()
    public class GameRunner
    {
        private readonly IRenderer _renderer;
        private readonly IGame _game;

        public GameRunner(IRenderer renderer, IGame game)
        {
            _renderer = renderer;
            _game = game;
        }

        // Summary:
        //      Decide on key reaction: exit on Escape, handle AllowedKeys or pause on rest
        private bool HandleInput()
        {
            string key = _renderer.Input();
            if (key == null)
                return false;

            if (key == UIKey.Escape)
                _game.Stop(); // stop on Escape
            else if (_game.AllowedKeys.Contains(key))
                _game.HandleInput(key); // handle if key is allowed
            else
                return true; // pause if key is not allowed

            return false;
        }

        // Summary:
        //      Use HasChanges property to decide if draw is needed and set it to false if drawn
        // Parameters: togglePause: toggle pause before drawing (switch to text mode or back)
        private void Paint(bool togglePause = false)
        {
            if (togglePause)
                _game.TogglePause();
            foreach (var p in _game.Panes.Values)
                if (p.HasChanges)
                {
                    _renderer.DrawPane(p);
                    p.Change(false);
                }
        }

        // Summary:
        // Minimalist running of IGame. The algorithm is:
        // Draw(game.Border)
        // game.Start()
        // do
        //   Draw(game.Panes)
        //   key <= Input()
        //   if (game.AllowedKeys contains key)
        //     game.HandleInput(key)
        //   else game.TogglePause()
        //   wait for game.FrameInterval (ms)
        //   game.NextFrame()
        // while (key is not Escape)
        // game.Stop()
        public void Run()
        {
            _game.Start();
            if (!_game.IsRunning)
                return;

            _renderer.DrawBorder(_game.Border);
            _renderer.NextInLoop += NextFrame;
            _renderer.StartLoop(_game.FrameInterval);
            while (_game.IsRunning)
            {
                Paint();
                bool paused = HandleInput();
                if (!_game.IsRunning)
                    break;

                if (paused)
                {
                    Paint(true); // toggle to paused and draw
                    if (!_game.IsRunning)
                        break;
                    _renderer.InputWait(); // wait for any key press
                    Paint(true); // unpause and draw
                }
            }
            _renderer.StopLoop();
            _renderer.NextInLoop -= NextFrame;
        }

        void NextFrame()
        {
            if (_game.IsRunning)
                _game.NextFrame();
        }
    }
}
