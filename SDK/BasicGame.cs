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
    public abstract class BasicGame : IGame
    {
        // Identifier of main pane
        protected const string MAIN = "main";
        // Get border around the Panes, to draw in UI
        public Border Border { get; protected set; }
        // Rectangulars to draw in UI
        public Dictionary<string, Pane> Panes { get; protected set; }
        // Duration between frames in ms
        public int FrameInterval { get; protected set; }
        // List of shortcut keys which are handled by game
        public string[] AllowedKeys { get; protected set; }
        // Is true while game is running, false when game needs to exit
        public bool IsRunning { get; protected set; }
        // The pane inside the frame
        protected Pane Main { get { return Panes[MAIN]; } }

        public BasicGame(int mainWidth, int mainHeight, string text = null, int frameInterval = 0, string allowedKeys = "")
        {
            Border = new Border(mainWidth + 2, mainHeight + 2);
            Pane main = new Pane(1, 1, mainHeight, mainWidth);
            Panes = new Dictionary<string, Pane> { { MAIN, main } };
            if (!string.IsNullOrEmpty(text))
                main.SetText(text.Split(','), false);
            FrameInterval = frameInterval;
            AllowedKeys = allowedKeys.Split(',');
        }

        // Summary:
        //      Initialize the game and start running
        public virtual void Start()
        {
            IsRunning = true;
            Main.Change();
        }

        // Summary:
        //      Stop running and cleanup
        public virtual void Stop()
        {
            IsRunning = false;
        }

        // Summary:
        //      Turns pause mode on and off
        public virtual void TogglePause()
        {
            if (Main.Text != null)
                Main.SwitchMode();
        }

        // Summary:
        //      Move to next frame; not all games use frames, some are static
        public virtual void NextFrame() { }

        // Summary:
        //      Handle allowed key
        // Parameters: key: key value as string literal
        public abstract void HandleInput(string key);
    }
}
