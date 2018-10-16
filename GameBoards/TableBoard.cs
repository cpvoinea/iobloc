using System;

namespace iobloc
{
    class TableBoard
    {
        const int PIECE_WIDTH = 1;
        const int WIDTH = 14 * PIECE_WIDTH + 5;
        const int HEIGHT = 16;

        readonly Border _border = new Border(new[]
        {
            new BorderLine(0, WIDTH - 1, 0, false, false),
            new BorderLine(0, WIDTH - 1, HEIGHT - 1, false, false),
            new BorderLine(0, HEIGHT - 1, 0, true, false),
            new BorderLine(0, HEIGHT - 1, WIDTH - 1, true, false),
            new BorderLine(0, HEIGHT - 1, 6 * PIECE_WIDTH + 1, true, true),
            new BorderLine(0, HEIGHT - 1, 7 * PIECE_WIDTH + 2, true, true),
            new BorderLine(0, HEIGHT - 1, 13 * PIECE_WIDTH + 3, true, true),
            new BorderLine(6 * PIECE_WIDTH + 1, 7 * PIECE_WIDTH + 2, 6, false, true),
            new BorderLine(6 * PIECE_WIDTH + 1, 7 * PIECE_WIDTH + 2, 9, false, true)
        });

        internal TableBoard()
        {
        }
    }
}