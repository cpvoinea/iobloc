using System;

namespace iobloc
{
    class ConsoleUI
    {
        readonly Board _board;

        internal ConsoleUI(Board board)
        {
            _board = board;
        }

        internal void Reset()
        {
            Console.CursorVisible = false;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write((char)0x2554);
            WriteMultiple((char)0x2550, 10);
            Console.WriteLine((char)0x2557);
            for (int i = 0; i < 20; i++)
            {
                Console.Write((char)0x2551);
                WriteMultiple(' ', 10);
                Console.WriteLine((char)0x2551);
            }
            Console.Write((char)0x255A);
            WriteMultiple((char)0x2550, 10);
            Console.Write((char)0x255D);
            Console.SetCursorPosition(1, 1);
        }

        internal void Draw()
        {
            var grid = _board.GetGridWithPiece();
            var color = Console.ForegroundColor;
            for (int i = 0; i < 20; i++)
            {
                Console.SetCursorPosition(1, i + 1);
                for (int j = 0; j < 10; j++)
                {
                    Console.ForegroundColor = (ConsoleColor)grid[i, j];
                    Console.Write((char)0x2588);
                }
            }
            Console.ForegroundColor = color;
            Console.SetCursorPosition(1,1);
        }

        internal void Restore()
        {
            Console.CursorVisible = true;
            Console.ResetColor();
        }

        internal void ShowHelp()
        {
            Reset();
            Console.SetCursorPosition(1, 9);
            Console.WriteLine("Play:ARROW");
            Console.SetCursorPosition(1, 10);
            Console.WriteLine("Exit:ESC");
            Console.SetCursorPosition(1, 11);
            Console.WriteLine("Pause:ANY");
        }

        void WriteMultiple(char c, int times)
        {
            for (int i = 0; i < times; i++)
                Console.Write(c);
        }
    }
}