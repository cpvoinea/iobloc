using System;

namespace iobloc
{
    static class BoxGraphics
    {
        internal const int None = 0;
        internal const int HorizontalLine = 0x2550;
        internal const int VerticalLine = 0x2551;
        internal const int UpperLeftCorner = 0x2554;
        internal const int UpperRightCorner = 0x2557;
        internal const int LowerLeftCorner = 0x255A;
        internal const int LowerRightCorner = 0x255D;
        internal const int LeftSplit = 0x255F;
        internal const int RightSplit = 0x2562;
        internal const int UpperSplit = 0x2564;
        internal const int LowerSplit = 0x2567;
        internal const int SingleHorizontalLine = 0x2500;
        internal const int SingleVerticalLine = 0x2502;
        internal const int SingleUpperLeftCorner = 0x250C;
        internal const int SingleUpperRightCorner = 0x2510;
        internal const int SingleLowerLeftCorner = 0x2514;
        internal const int SingleLowerRightCorner = 0x2518;
        internal const int SingleLeftSplit = 0x251C;
        internal const int SingleRightSplit = 0x2524;
        internal const int SingleUppperSplit = 0x252C;
        internal const int SingleLowerSplit = 0x2534;
        internal const int SingleIntersection = 0x253C;
        internal const int BlockFull = 0x2588;
        internal const int BlockUpper = 0x2580;
        internal const int BlockLower = 0x2584;
        internal const int BlockLeft = 0x258C;
        internal const int BlockRight = 0x2590;
        internal const int BlockLight = 0x2591;
        internal const int BlockMedium = 0x2592;
        internal const int BlockDark = 0x2593;

        internal static void List()
        {
            Console.WriteLine("BOX DRAWING");
            for (int i = 0x2500; i < 0x2580; i++)
                Console.WriteLine("{0:X}: {1}", i, (char)i);
            Console.WriteLine();
            Console.WriteLine("BLOCK ELEMENTS");
            for (int i = 0x2580; i < 0x25A0; i++)
                Console.WriteLine("{0:X}: {1}", i, (char)i);
            // Console.WriteLine();
            // Console.WriteLine("GEOMETRIC SHAPES");
            // for (int i = 0x25A0; i < 0x2600; i++)
            //     Console.WriteLine("{0:X}: {1}", i, (char)i);
            // Console.WriteLine();
            // Console.WriteLine("MISC SYMBOLS");
            // for (int i = 0x2600; i < 0x2700; i++)
            //     Console.WriteLine("{0:X}: {1}", i, (char)i);
        }
    }
}