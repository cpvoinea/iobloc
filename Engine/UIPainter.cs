using System;
using System.Text;

namespace iobloc
{
    /// <summary>
    /// Use System.Console to paint and get input
    /// </summary>
    static class UIPainter
    {
        // initial window values to restore to
        private static int WinWidth = 48, WinHeight = 24, BuffWidth = 48, BuffHeight = 192;

        /// <summary>
        /// Resize window to fit a border - not working on all OS
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
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

        /// <summary>
        /// Use console for drawing
        /// </summary>
        public static void Initialize()
        {
            // support box drawing
            Console.OutputEncoding = Encoding.UTF8;
            // not hiding the cursor is sometimes usefull for debugging
            Console.CursorVisible = false;
            // remember initial values
            WinWidth = Console.WindowWidth;
            WinHeight = Console.WindowHeight;
            BuffWidth = Console.BufferWidth;
            BuffHeight = Console.BufferHeight;
        }

        /// <summary>
        /// Restore console to original state
        /// </summary>
        public static void Exit()
        {
            // just in case, restore color
            Console.ResetColor();
            // show cursor again
            Console.CursorVisible = true;
            // restore initial values
            try
            {
                Console.SetWindowSize(WinWidth, WinHeight);
                Console.SetBufferSize(BuffWidth, BuffHeight);
            }
            catch { }
        }

        /// <summary>
        /// Draw a border consisting of horizontal and vertical lines and clear the rest of the screen
        /// </summary>
        /// <param name="border">a collection of lines</param>
        public static void DrawBorder(UIBorder border)
        {
            Console.Clear();
            int w = border.Width;
            int h = border.Height;
            // fit window to border - don't use to maintain compatibility across OS console types
            //Resize(w, h);

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

        /// <summary>
        /// Draw a panel inside a rectangular area.
        /// The panel has either lines of text or a multi-colored matrix with a single character
        /// </summary>
        /// <param name="panel">panel to draw</param>
        public static void DrawPanel(UIPanel panel)
        {
            if (panel.IsText)
                DrawPanelText(panel, panel.Text);
            else
                DrawPanelColor(panel);
        }

        /// <summary>
        /// Put centered lines of text inside a rectangle, clear the rest
        /// </summary>
        /// <param name="panel">panel defines rectangle</param>
        /// <param name="lines">text lines to write</param>
        private static void DrawPanelText(UIPanel panel, string[] lines)
        {
            // use empty line to clear where text is missing
            string empty = new String(' ', panel.Width);
            // center vertical
            int start = (panel.Height - lines.Length) / 2;
            if (start < 0) start = 0;
            for (int row = 0; row < panel.Height; row++)
            {
                string text = empty;
                if (row >= start && row - start < lines.Length && !string.IsNullOrEmpty(lines[row - start]))
                {
                    text = lines[row - start];
                    // center horizontal
                    int left = (panel.Width - text.Length) / 2;
                    // use padding to clear if text is too short
                    text = text.PadLeft(left + text.Length).PadRight(panel.Width);
                }
                Console.SetCursorPosition(panel.FromCol, panel.FromRow + row);
                Console.Write(text);
            }
        }

        /// <summary>
        /// Draw a matrix containing a symbol of multiple colors, 0 representing background color
        /// </summary>
        /// <param name="panel">panel defines rectangle and color matrix</param>
        private static void DrawPanelColor(UIPanel panel)
        {
            for (int row = 0; row < panel.Height; row++)
            {
                Console.SetCursorPosition(panel.FromCol, panel.FromRow + row);
                int col = 0;
                while (col < panel.Width)
                {
                    // group together same color values, to draw only once
                    int from = col;
                    int last = panel[row, col];
                    do col++; while (col < panel.Width && panel[row, col] == last);
                    // fro->col section has same color: last
                    if (last == 0)
                        Console.Write(new string(' ', col - from)); // use background color
                    else
                    {
                        Console.ForegroundColor = (ConsoleColor)last; // use matrix color
                        Console.Write(new string(panel.Symbol, col - from)); // and panel-defined symbol
                    }
                }
            }
            // go back to default color
            Console.ResetColor();
        }

        /// <summary>
        /// Wait until key is pressed and return key
        /// </summary>
        /// <returns>pressed key as string constant</returns>
        public static string InputWait()
        {
            return Console.ReadKey(true).Key.ToString();
        }

        /// <summary>
        /// Check if key is pressed and return it, return null if no key is pressed
        /// </summary>
        /// <returns>key string or null</returns>
        public static string Input()
        {
            if (Console.KeyAvailable)
                return InputWait();
            return null;
        }
    }
}
