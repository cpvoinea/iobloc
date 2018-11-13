using System;
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
    public abstract class BasicBoard : IBoard
    {
        /// <summary>
        /// Identifier of main panel
        /// </summary>
        /// <value></value>
        protected const string MAIN = "main";
        /// <summary>
        /// Get border around the Panels, to draw in UI
        /// </summary>
        /// <value>a collection of lines</value>
        public UIBorder Border { get; protected set; }
        /// <summary>
        /// Rectangulars to draw in UI
        /// </summary>
        /// <value></value>
        public Dictionary<string, UIPanel> Panels { get; protected set; }
        /// <summary>
        /// Duration between frames in ms
        /// </summary>
        /// <value></value>
        public int FrameInterval { get; protected set; }
        /// <summary>
        /// List of shortcut keys which are handled by board
        /// </summary>
        /// <value>values are literal strings corresponding to key description</value>
        public string[] AllowedKeys { get; protected set; }
        /// <summary>
        /// Is true while board is running, false when board needs to exit
        /// </summary>
        /// <value></value>
        public bool IsRunning { get; protected set; }
        /// <summary>
        /// The panel inside the frame
        /// </summary>
        /// <value></value>
        protected UIPanel MainPanel { get { return Panels[MAIN]; } }

        public BasicBoard(int mainPanelWidth, int mainPanelHeight, string text = null, int frameInterval = 0, string allowedKeys = "")
        {
            Border = new UIBorder(mainPanelWidth + 2, mainPanelHeight + 2);
            UIPanel main = new UIPanel(1, 1, mainPanelHeight, mainPanelWidth);
            Panels = new Dictionary<string, UIPanel> { { MAIN, main } };
            if (!string.IsNullOrEmpty(text))
                main.SetText(text.Split(','));
            FrameInterval = frameInterval;
            AllowedKeys = allowedKeys.Split(',');
        }

        /// <summary>
        /// Initialize the board and start running
        /// </summary>
        public virtual void Start()
        {
            IsRunning = true;
            MainPanel.Change();
        }

        /// <summary>
        /// Stop running and cleanup
        /// </summary>
        public virtual void Stop()
        {
            IsRunning = false;
        }

        /// <summary>
        /// Turns pause mode on and off
        /// </summary>
        public virtual void TogglePause()
        {
            if (MainPanel.Text != null)
                MainPanel.SwitchMode();
        }

        /// <summary>
        /// Move to next frame; not all boards use frames, some are static
        /// </summary>
        public virtual void NextFrame() { }

        /// <summary>
        /// Handle allowed key
        /// </summary>
        /// <param name="key">key value as string literal</param>
        public abstract void HandleInput(string key);

        /// <summary>
        /// Test the board by running it
        /// </summary>
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
                Console.CursorVisible = true;
                Console.ResetColor();
            }
        }
    }
}
