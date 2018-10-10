using System;
using System.Text;

namespace iobloc
{
    /// <summary>
    /// Display a framed grid of blocks, each grid value representing the console color
    /// </summary>
    class ConsoleUI
    {
        readonly IBoard _board;

        /// <summary>
        /// Set the board to display
        /// </summary>
        /// <param name="board">provides Height, Width and Grid[Height,Width] containing ConsoleColor to display</param>
        internal ConsoleUI(IBoard board)
        {
            _board = board;
        }

        /// <summary>
        /// Draw an empty frame
        /// </summary>
        internal void Reset()
        {
            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.UTF8;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            DrawFrame();
            Console.SetCursorPosition(1, 1);
        }

        internal void DrawFrame()
        {
            for (int i = 0; i < _board.Frame.Height; i++)
            {
                string line = string.Empty;
                for (int j = 0; j < _board.Frame.Width; j++)
                {
                    int c = (int)_board.Frame.EmptyFrame[i, j];
                    line += c == 0 ? ' ' : (char)c;
                }
                Console.WriteLine(line);
            }
        }

        /// <summary>
        /// Draw Height x Width grid of blocks inside the frame
        /// </summary>
        internal void DrawPanel(int[] clip)
        {
            var grid = _board.Grid;
            var color = Console.ForegroundColor;
            for (int i = clip[1]; i < clip[3]; i++)
            {
                Console.SetCursorPosition(clip[0] + 1, i + 1);
                for (int j = clip[0]; j < clip[2]; j++)
                {
                    int c = grid[i,j];
                    Console.ForegroundColor = c; // == 0 ? Console.BackgroundColor : (ConsoleColor)c;
                    Console.Write((char)BoxGraphics.BlockFull);
                }
            }
            Console.ForegroundColor = color;
            Console.SetCursorPosition(_board.Frame.Width / 2 - 1, 0);
            Console.Write("{0,3}", _board.Score);
            Console.SetCursorPosition(_board.Frame.Width / 2 - 1, _board.Frame.Height - 1);
            Console.Write("L{0,2}", Settings.Game.Level);
            Console.SetCursorPosition(1, 1);
        }

        /// <summary>
        /// Restores console values
        /// </summary>
        internal void Restore()
        {
            Console.CursorVisible = true;
            Console.ResetColor();
        }

        /// <summary>
        /// Hide grid and display help text inside frame
        /// </summary>
        internal void ShowHelp()
        {
            Reset();
            int row = _board.Frame.Height / 2 - _board.Help.Length / 2;
            if (row <= 0)
                row = 1;
            string[] help = _board.Help;
            foreach (string s in help)
            {
                Console.SetCursorPosition(1, row++);
                Console.WriteLine(s);
            }
        }

        /// <summary>
        /// Repeat same block
        /// </summary>
        /// <param name="c">value to write</param>
        /// <param name="times">number of times to repeat</param>
        void WriteMultiple(char c, int times)
        {
            for (int i = 0; i < times; i++)
                Console.Write(c);
        }
    }
}