using System;

namespace iobloc
{
    class TableBoard
    {
        const int PIECE_WIDTH = 1;
        const int WIDTH = 14 * PIECE_WIDTH + 5;
        const int HEIGHT = 16;

        readonly BoardFrame _frame = new BoardFrame(new[]
        {
            new BoardLine(0, WIDTH - 1, 0, false, false),
            new BoardLine(0, WIDTH - 1, HEIGHT - 1, false, false),
            new BoardLine(0, HEIGHT - 1, 0, true, false),
            new BoardLine(0, HEIGHT - 1, WIDTH - 1, true, false),
            new BoardLine(0, HEIGHT - 1, 6 * PIECE_WIDTH + 1, true, true),
            new BoardLine(0, HEIGHT - 1, 7 * PIECE_WIDTH + 2, true, true),
            new BoardLine(0, HEIGHT - 1, 13 * PIECE_WIDTH + 3, true, true),
            new BoardLine(6 * PIECE_WIDTH + 1, 7 * PIECE_WIDTH + 2, 6, false, true),
            new BoardLine(6 * PIECE_WIDTH + 1, 7 * PIECE_WIDTH + 2, 9, false, true)
        });

        internal TableBoard()
        {
            Console.CursorVisible = false;
            Console.Clear();
            for (int i = 0; i < _frame.Height; i++)
            {
                string line = string.Empty;
                for (int j = 0; j < _frame.Width; j++)
                {
                    int c = (int)_frame.EmptyFrame[i, j];
                    line += c == 0 ? ' ' : (char)c;
                }
                Console.WriteLine(line);
            }

            Console.ReadKey();
            Console.CursorVisible = true;
        }
    }
}