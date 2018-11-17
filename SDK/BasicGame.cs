using System;
using System.Collections.Generic;

namespace iobloc
{
    // A game which can be run in the UI. The algorithm is:
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
    public abstract class BasicGame : IGame
    {
        // Identifier of main panel
        protected const string MAIN = "main";
        // Get border around the Panels, to draw in UI
        public Border Border { get; protected set; }
        // Rectangulars to draw in UI
        public Dictionary<string, Panel> Panels { get; protected set; }
        // Duration between frames in ms
        public int FrameInterval { get; protected set; }
        // List of shortcut keys which are handled by game
        public string[] AllowedKeys { get; protected set; }
        // Is true while game is running, false when game needs to exit
        public bool IsRunning { get; protected set; }
        // The panel inside the frame
        protected Panel MainPanel { get { return Panels[MAIN]; } }

        public BasicGame(int mainPanelWidth, int mainPanelHeight, string text = null, int frameInterval = 0, string allowedKeys = "")
        {
            Border = new Border(mainPanelWidth + 2, mainPanelHeight + 2);
            Panel main = new Panel(1, 1, mainPanelHeight, mainPanelWidth);
            Panels = new Dictionary<string, Panel> { { MAIN, main } };
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
            MainPanel.Change();
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
            if (MainPanel.Text != null)
                MainPanel.SwitchMode();
        }

        // Summary:
        //      Move to next frame; not all games use frames, some are static
        public virtual void NextFrame() { }

        // Summary:
        //      Handle allowed key
        // Parameters: key: key value as string literal
        public abstract void HandleInput(string key);

        // Summary:
        // Test the game by running it
        public void Run()
        {
            Console.Clear();
            Console.CursorVisible = false;
            try
            {
                Console.Clear();
                Console.CursorVisible = false;
                GameRunner.Run(this);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
            finally
            {
                Console.Clear();
                Console.CursorVisible = true;
                Console.ResetColor();
            }
        }
    }
}
