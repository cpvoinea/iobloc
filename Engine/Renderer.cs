using System;
using System.Text;

namespace iobloc
{
    // Use System.Console to paint and get input
    static class Renderer
    {
        private static bool SAFE_MODE = true; // made it static instead of const to avoid warnings
        private static int WinWidth = 48, WinHeight = 24, BuffWidth = 48, BuffHeight = 192;
        private static int CurrentBorderHeight;

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

        // Summary:
        //      Use console for drawing
        public static void Initialize()
        {
            // support box drawing
            Console.OutputEncoding = Encoding.UTF8;
            // not hiding the cursor is sometimes usefull for debugging
            Console.CursorVisible = false;
            // remember initial values
            if (!SAFE_MODE)
            {
                WinWidth = Console.WindowWidth;
                WinHeight = Console.WindowHeight;
                BuffWidth = Console.BufferWidth;
                BuffHeight = Console.BufferHeight;
            }
        }

        // Summary:
        //      Restore console to original state
        public static void Exit()
        {
            // just in case, restore color
            Console.ResetColor();
            // show cursor again
            Console.CursorVisible = true;
            // restore initial values
            if (!SAFE_MODE)
                try
                {
                    Console.SetWindowSize(WinWidth, WinHeight);
                    Console.SetBufferSize(BuffWidth, BuffHeight);
                }
                catch { }
        }

        // Summary:
        //      Draw a border consisting of horizontal and vertical lines and clear the rest of the screen
        // Parameters: border: a collection of lines
        public static void DrawBorder(Border border)
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
        //      Draw a panel inside a rectangular area.
        //      The panel has either lines of text or a multi-colored matrix with a single character
        // Parameters: panel: panel to draw
        public static void DrawPanel(Panel panel)
        {
            if (panel.IsTextMode)
                DrawPanelText(panel, panel.Text);
            else
                DrawPanelColor(panel);
        }

        // Summary:
        //      Put centered lines of text inside a rectangle, clear the rest
        // Parameters: panel: panel defines rectangle
        // Parameters: lines: text lines to write
        private static void DrawPanelText(Panel panel, string[] lines)
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

        // Summary:
        //      Draw a matrix containing a symbol of multiple colors, 0 representing background color
        // Parameters: panel: panel defines rectangle and color matrix
        private static void DrawPanelColor(Panel panel)
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
                        Console.Write(new string(panel.BlockChar, col - from)); // and panel-defined symbol
                    }
                }
            }
            // go back to default color
            Console.ResetColor();
        }

        // Summary:
        //      Wait until key is pressed and return key
        public static string InputWait()
        {
            // on Mac the interception of key doesn't always work
            // moving the cursor outside the border
            if (SAFE_MODE)
                Console.SetCursorPosition(0, CurrentBorderHeight);
            var k = Console.ReadKey(true).Key.ToString();
            // hiding pressed key
            if (SAFE_MODE)
            {
                Console.SetCursorPosition(0, CurrentBorderHeight);
                Console.Write("    ");
            }

            return k;
        }

        // Summary:
        //      Check if key is pressed and return it, return null if no key is pressed
        public static string Input()
        {
            if (Console.KeyAvailable)
                return InputWait();
            return null;
        }
    }
}
