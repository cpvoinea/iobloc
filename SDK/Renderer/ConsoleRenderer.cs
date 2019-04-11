using System;
using System.Text;
using System.Threading;

namespace iobloc
{
    // Use System.Console to paint and get input
    public class ConsoleRenderer : IRenderer
    {
        private const bool SAFE_MODE = true; // made it static instead of const to avoid warnings
        private const int MinWidth = 103;
        private const int MinHeight = 44;
        private static int WinWidth = MinWidth;
        private static int WinHeight = MinHeight;
        private static int CurrentBorderHeight;

        private IGame _game = null;

        // Summary:
        //      Use console for drawing
        public ConsoleRenderer()
        {
            AllocConsole();
            // support box drawing
            Console.OutputEncoding = Encoding.UTF8;
            // not hiding the cursor is sometimes usefull for debugging
            Console.CursorVisible = false;
            // remember initial values
            WinWidth = Console.WindowWidth;
            WinHeight = Console.WindowHeight;
            if (SAFE_MODE || (WinWidth < MinWidth && WinWidth <= Console.LargestWindowWidth || WinHeight < MinHeight && WinHeight <= Console.LargestWindowHeight))
                Resize(MinWidth, MinHeight);
            Console.Clear();
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
        public void Run(IGame game)
        {
            _game = game;

            _game.Start();
            if (!_game.IsRunning)
                return;

            DrawBorder(_game.Border);
            DateTime start = DateTime.Now; // frame start time
            int ticks = 0; // elapsed time in ms
            while (_game.IsRunning)
            {
                DrawAll();
                bool paused = HandleInput();
                if (!_game.IsRunning)
                    break;

                if (paused)
                {
                    DrawAll(true); // toggle to paused and draw
                    if (!_game.IsRunning)
                        break;
                    InputWait(); // wait for any key press
                    DrawAll(true); // unpause and draw
                }

                if (_game.IsRunning && _game.FrameInterval > 0)
                {
                    Thread.Sleep(20);
                    ticks = (int)DateTime.Now.Subtract(start).TotalMilliseconds;
                    if (ticks > _game.FrameInterval) // move to next frame
                    {
                        _game.NextFrame();
                        start = DateTime.Now;
                        ticks -= _game.FrameInterval;
                    }
                }
            }
        }

        // Summary:
        //      Draw a pane inside a rectangular area.
        //      The pane has either lines of text or a multi-colored matrix with a single character
        // Parameters: pane: pane to draw
        public void DrawPane(Pane pane)
        {
            if (pane.IsTextMode)
                DrawPaneText(pane, pane.Text);
            else
                DrawPaneColor(pane);
        }

        public void Dispose()
        {
            // just in case, restore color
            Console.ResetColor();
            // show cursor again
            Console.CursorVisible = true;
            // restore initial values
            Resize(WinWidth, WinHeight);
            FreeConsole();
        }

        // Summary:
        //      Wait until key is pressed and return key
        private static string InputWait()
        {
            // on Mac the interception of key doesn't always work
            // moving the cursor outside the border
            if (SAFE_MODE && CurrentBorderHeight < Console.WindowHeight)
                Console.SetCursorPosition(0, CurrentBorderHeight);
            var k = Console.ReadKey(true).Key.ToString();
            // hiding pressed key
            if (SAFE_MODE && CurrentBorderHeight < Console.WindowHeight)
            {
                Console.SetCursorPosition(0, CurrentBorderHeight);
                Console.Write("    ");
            }

            return k;
        }

        // Summary:
        //      Check if key is pressed and return it, return null if no key is pressed
        private static string Input()
        {
            if (Console.KeyAvailable)
                return InputWait();
            return null;
        }

        // Summary:
        //      Decide on key reaction: exit on Escape, handle AllowedKeys or pause on rest
        private bool HandleInput()
        {
            string key = Input();
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
        private void DrawAll(bool togglePause = false)
        {
            if (togglePause)
                _game.TogglePause();
            foreach (var p in _game.Panes.Values)
                if (p.HasChanges)
                {
                    DrawPane(p);
                    p.Change(false);
                }
        }

        // Summary:
        //      Draw a border consisting of horizontal and vertical lines and clear the rest of the screen
        // Parameters: border: a collection of lines
        private static void DrawBorder(Border border)
        {
            Console.Clear();
            int w = border.Width;
            int h = border.Height;
            // fit window to border - don't use to maintain compatibility across OS console types
            if (!SAFE_MODE)
                Resize(w, h);
            else
                CurrentBorderHeight = h + 1;

            for (int row = 0; row < h; row++)
            {
                // initialize to empty line
                StringBuilder line = new StringBuilder(new string(' ', w));
                // put drawing symbols in line
                for (int j = 0; j < w; j++)
                {
                    if (border[row, j] > 0)
                        line[j] = (char)border[row, j];
                }
                // draw whole line at once
                Console.SetCursorPosition(0, row);
                Console.Write(line);
            }
        }

        // Summary:
        //      Put centered lines of text inside a rectangle, clear the rest
        // Parameters: pane: pane defines rectangle
        // Parameters: lines: text lines to write
        private static void DrawPaneText(Pane pane, string[] lines)
        {
            // use empty line to clear where text is missing
            string empty = new String(' ', pane.Width);
            // center vertical
            int start = (pane.Height - lines.Length) / 2;
            if (start < 0) start = 0;
            for (int row = 0; row < pane.Height; row++)
            {
                string text = empty;
                if (row >= start && row - start < lines.Length && !string.IsNullOrEmpty(lines[row - start]))
                {
                    text = lines[row - start];
                    // center horizontal
                    int left = (pane.Width - text.Length) / 2;
                    // use padding to clear if text is too short
                    text = text.PadLeft(left + text.Length).PadRight(pane.Width);
                }
                Console.SetCursorPosition(pane.FromCol, pane.FromRow + row);
                Console.Write(text);
            }
        }

        // Summary:
        //      Draw a matrix containing a symbol of multiple colors, 0 representing background color
        // Parameters: pane: pane defines rectangle and color matrix
        private static void DrawPaneColor(Pane pane)
        {
            for (int row = 0; row < pane.Height; row++)
            {
                Console.SetCursorPosition(pane.FromCol, pane.FromRow + row);
                int col = 0;
                while (col < pane.Width)
                {
                    // group together same color values, to draw only once
                    int from = col;
                    int last = pane[row, col];
                    do col++; while (col < pane.Width && pane[row, col] == last);
                    // fro->col section has same color: last
                    if (last == 0)
                        Console.Write(new string(' ', col - from)); // use background color
                    else
                    {
                        Console.ForegroundColor = (ConsoleColor)last; // use matrix color
                        Console.Write(new string(pane.BlockChar, col - from)); // and pane-defined symbol
                    }
                }
            }
            // go back to default color
            Console.ResetColor();
        }

        // Summary:
        //      Resize window to fit a border - not working on all OS
        // Parameters: width: 
        // Parameters: height: 
        private static void Resize(int width, int height)
        {
            // add extra column for cursor (?)
            width = width < 12 ? 12 : width + 1;
            // sometimes setting the buffer size throws exception when width is too small, depending on platform
            bool success = false;
            do try
                {
                    Console.SetWindowSize(width, height);
                    Console.SetBufferSize(width, height);
                    Console.SetWindowSize(width, height);
                    success = true;
                }
                catch { width++; } // on some operating systems the window resize is not supported
            while (!success && width < 16);
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool FreeConsole();
    }
}
