using System;
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
    public abstract class BasicBoard : IBoard
    {
        // Identifier of main panel
        protected const string MAIN = "main";
        // Get border around the Panels, to draw in UI
        public UIBorder Border { get; protected set; }
        // Rectangulars to draw in UI
        public Dictionary<string, UIPanel> Panels { get; protected set; }
        // Duration between frames in ms
        public int FrameInterval { get; protected set; }
        // List of shortcut keys which are handled by board
        public string[] AllowedKeys { get; protected set; }
        // Is true while board is running, false when board needs to exit
        public bool IsRunning { get; protected set; }
        // The panel inside the frame
        protected UIPanel MainPanel { get { return Panels[MAIN]; } }

        public BasicBoard(int mainPanelWidth, int mainPanelHeight, string text = null, int frameInterval = 0, string allowedKeys = "")
        {
            Border = new UIBorder(mainPanelWidth + 2, mainPanelHeight + 2);
            UIPanel main = new UIPanel(1, 1, mainPanelHeight, mainPanelWidth);
            Panels = new Dictionary<string, UIPanel> { { MAIN, main } };
            if (!string.IsNullOrEmpty(text))
                main.SetText(text.Split(','), false);
            FrameInterval = frameInterval;
            AllowedKeys = allowedKeys.Split(',');
        }

        // Summary:
        //      Initialize the board and start running
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
        //      Move to next frame; not all boards use frames, some are static
        public virtual void NextFrame() { }

        // Summary:
        //      Handle allowed key
        // Param: key: key value as string literal
        public abstract void HandleInput(string key);

        // Summary:
        // Test the board by running it
        public void Run()
        {
            Console.Clear();
            Console.CursorVisible = false;
            try
            {
                Console.Clear();
                Console.CursorVisible = false;
                BoardRunner.Run(this);
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
