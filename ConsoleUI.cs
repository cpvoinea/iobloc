using System;

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
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write((char)0x2554);
            WriteMultiple((char)0x2550, _board.Width);
            Console.WriteLine((char)0x2557);
            for (int i = 0; i < _board.Height; i++)
            {
                Console.Write((char)0x2551);
                WriteMultiple(' ', _board.Width);
                Console.WriteLine((char)0x2551);
            }
            Console.Write((char)0x255A);
            WriteMultiple((char)0x2550, _board.Width);
            Console.Write((char)0x255D);
            Console.SetCursorPosition(1, 1);
        }

        /// <summary>
        /// Draw Height x Width grid of blocks inside the frame
        /// </summary>
        internal void Draw()
        {
            var grid = _board.Grid;
            var color = Console.ForegroundColor;
            for (int i = 0; i < _board.Height; i++)
            {
                Console.SetCursorPosition(1, i + 1);
                for (int j = 0; j < _board.Width; j++)
                {
                    Console.ForegroundColor = (ConsoleColor)grid[i, j];
                    Console.Write((char)0x2588);
                }
            }
            Console.ForegroundColor = color;
            Console.SetCursorPosition(_board.Width / 2 - 2, 0);
            Console.Write("{0,5}", _board.Score);
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
            int row = _board.Height / 2 - _board.Help.Length / 2;
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