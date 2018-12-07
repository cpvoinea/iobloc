using System;
using System.Text;
using System.Timers;

namespace iobloc
{
    // Use System.Console to paint and get input
    public class ConsoleRenderer : IRenderer
    {
        private static bool SAFE_MODE = true; // made it static instead of const to avoid warnings
        private const int MinWidth = 103;
        private const int MinHeight = 44;
        private readonly int _winWidth = MinWidth;
        private readonly int _winHeight = MinHeight;
        private static int _currentBorderHeight;
        private readonly Timer _timer = new Timer();

        public event LoopHandler NextInLoop;

        // Summary:
        //      Use console for drawing
        public ConsoleRenderer()
        {
            // support box drawing
            Console.OutputEncoding = Encoding.UTF8;
            // not hiding the cursor is sometimes usefull for debugging
            Console.CursorVisible = false;
            // remember initial values
            _winWidth = Console.WindowWidth;
            _winHeight = Console.WindowHeight;
            if (SAFE_MODE || (_winWidth < MinWidth && _winWidth <= Console.LargestWindowWidth || _winHeight < MinHeight && _winHeight <= Console.LargestWindowHeight))
                Resize(MinWidth, MinHeight);
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

        private void OnNextInLoop(object sender, ElapsedEventArgs args)
        {
            if (NextInLoop != null)
                NextInLoop();
        }

        public void StartLoop(int interval)
        {
            StopLoop();
            if (interval == 0)
                return;

            _timer.Interval = interval;
            _timer.Elapsed += OnNextInLoop;
            _timer.Start();
        }

        public void StopLoop()
        {
            _timer.Stop();
            _timer.Elapsed -= OnNextInLoop;
        }

        // Summary:
        //      Draw a border consisting of horizontal and vertical lines and clear the rest of the screen
        // Parameters: border: a collection of lines
        public void DrawBorder(Border border)
        {
            Console.Clear();
            int w = border.Width;
            int h = border.Height;
            // fit window to border - don't use to maintain compatibility across OS console types
            if (!SAFE_MODE)
                Resize(w, h);
            else
                _currentBorderHeight = h + 1;

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

        // Summary:
        //      Wait until key is pressed and return key
        public string InputWait()
        {
            // on Mac the interception of key doesn't always work
            // moving the cursor outside the border
            if (SAFE_MODE && _currentBorderHeight < Console.WindowHeight)
                Console.SetCursorPosition(0, _currentBorderHeight);
            var k = Console.ReadKey(true).Key.ToString();
            // hiding pressed key
            if (SAFE_MODE && _currentBorderHeight < Console.WindowHeight)
            {
                Console.SetCursorPosition(0, _currentBorderHeight);
                Console.Write("    ");
            }

            return k;
        }

        // Summary:
        //      Check if key is pressed and return it, return null if no key is pressed
        public string Input()
        {
            if (Console.KeyAvailable)
                return InputWait();
            return null;
        }

        public void Dispose()
        {
            StopLoop();
            _timer.Dispose();
            // just in case, restore color
            Console.ResetColor();
            // show cursor again
            Console.CursorVisible = true;
            // restore initial values
            Resize(_winWidth, _winHeight);
        }
    }
}
