using System.Collections.Generic;

namespace iobloc
{
    /// <summary>
    /// A board which can be run in the UI. The algorithm is:
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
    public interface IBoard
    {
        /// <summary>
        /// Get border around the Panels, to draw in UI
        /// </summary>
        /// <value>a collection of lines</value>
        UIBorder Border { get; }
        /// <summary>
        /// Rectangulars to draw in UI
        /// </summary>
        /// <value></value>
        Dictionary<string, UIPanel> Panels { get; }
        /// <summary>
        /// Duration between frames in ms
        /// </summary>
        /// <value></value>
        int FrameInterval { get; }
        /// <summary>
        /// List of shortcut keys which are handled by board
        /// </summary>
        /// <value></value>
        string[] AllowedKeys { get; }
        /// <summary>
        /// Is true while board is running, false when board needs to exit
        /// </summary>
        /// <value></value>
        bool IsRunning { get; }

        /// <summary>
        /// Initialize the board and start running
        /// </summary>
        void Start();
        /// <summary>
        /// Stop running and cleanup
        /// </summary>
        void Stop();
        /// <summary>
        /// Turns pause mode on and off
        /// </summary>
        void TogglePause();
        /// <summary>
        /// Move to next frame; not all boards use frames, some are static
        /// </summary>
        void NextFrame();
        /// <summary>
        /// Handle allowed key
        /// </summary>
        /// <param name="key">key value as string constant</param>
        void HandleInput(string key);
    }
}
