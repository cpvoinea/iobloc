using System.Collections.Generic;

namespace iobloc
{
    // A game which can be run in the UI. The algorithm is:
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
    public interface IGame
    {
        // Get border around the Panes, to draw in UI
        Border Border { get; }
        // Rectangulars to draw in UI
        Dictionary<string, Pane> Panes { get; }
        // Duration between frames in ms
        int FrameInterval { get; }
        // List of shortcut keys which are handled by game
        string[] AllowedKeys { get; }
        // Is true while game is running, false when game needs to exit
        bool IsRunning { get; }

        // Summary:
        //      Initialize the game and start running
        void Start();
        // Summary:
        //      Stop running and cleanup
        void Stop();
        // Summary:
        //      Turns pause mode on and off
        void TogglePause();
        // Summary:
        //      Move to next frame; not all games use frames, some are static
        void NextFrame();
        // Summary:
        //      Handle allowed key
        // Parameters: key: key value as string constant
        void HandleInput(string key);
    }
}
