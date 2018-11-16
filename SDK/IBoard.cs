using System.Collections.Generic;

namespace iobloc
{
    // A board which can be run in the UI. The algorithm is:
    // Draw(board.Border)
    // board.Start()
    // do
    //   Draw(board.Panels)
    //   key <= Input()
    //   if (board.AllowedKeys contains key)
    //     board.HandleInput(key)
    //   else board.TogglePause()
    //   wait for board.FrameInterval (ms)
    //   board.NextFrame()
    // while (key is not Escape)
    // board.Stop()
    public interface IBoard
    {
        // Get border around the Panels, to draw in UI
        UIBorder Border { get; }
        // Rectangulars to draw in UI
        Dictionary<string, UIPanel> Panels { get; }
        // Duration between frames in ms
        int FrameInterval { get; }
        // List of shortcut keys which are handled by board
        string[] AllowedKeys { get; }
        // Is true while board is running, false when board needs to exit
        bool IsRunning { get; }

        // Summary:
        //      Initialize the board and start running
        void Start();
        // Summary:
        //      Stop running and cleanup
        void Stop();
        // Summary:
        //      Turns pause mode on and off
        void TogglePause();
        // Summary:
        //      Move to next frame; not all boards use frames, some are static
        void NextFrame();
        // Summary:
        //      Handle allowed key
        // Parameters: key: key value as string constant
        void HandleInput(string key);
    }
}
