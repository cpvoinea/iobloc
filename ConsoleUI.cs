using System;

namespace iobloc
{
    class ConsoleUI : IBoardUI
    {
        readonly IBoard _board;

        internal ConsoleUI(IBoard board)
        {
            _board = board;
        }

        public void Reset()
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

        public void Draw()
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
            Console.SetCursorPosition(1, 1);
        }

        public void Restore()
        {
            Console.CursorVisible = true;
            Console.ResetColor();
        }

        public void ShowHelp()
        {
            Reset();
            int row = _board.Height / 2 - 1;
            Console.SetCursorPosition(1, row);
            Console.WriteLine("Play:ARROW");
            Console.SetCursorPosition(1, row + 1);
            Console.WriteLine("Exit:ESC");
            Console.SetCursorPosition(1, row + 2);
            Console.WriteLine("Pause:ANY");
        }

        void WriteMultiple(char c, int times)
        {
            for (int i = 0; i < times; i++)
                Console.Write(c);
        }
    }
}