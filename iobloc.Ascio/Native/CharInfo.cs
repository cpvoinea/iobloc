namespace iobloc.Ascio
{
    struct Coord
    {
        public short X { get; set; }
        public short Y { get; set; }
    }

    struct Rect
    {
        public short Left { get; set; }
        public short Top { get; set; }
        public short Right { get; set; }
        public short Bottom { get; set; }
    }

    enum CharAttr : ushort
    {
        None = 0x0000,
        FOREGROUND_BLUE = 0x0001,
        FOREGROUND_GREEN = 0x0002,
        FOREGROUND_RED = 0x0004,
        FOREGROUND_WHITE = FOREGROUND_BLUE | FOREGROUND_GREEN | FOREGROUND_RED,
        FOREGROUND_INTENSITY = 0x0008,
        BACKGROUND_BLUE = 0x0010,
        BACKGROUND_GREEN = 0x0020,
        BACKGROUND_RED = 0x0040,
        BACKGROUND_INTENSITY = 0x0080,
        COMMON_LVB_LEADING_BYTE = 0x0100,
        COMMON_LVB_TRAILING_BYTE = 0x0200,
        COMMON_LVB_GRID_HORIZONTAL = 0x0400,
        COMMON_LVB_GRID_LVERTICAL = 0x0800,
        COMMON_LVB_GRID_RVERTICAL = 0x1000,
        COMMON_LVB_REVERSE_VIDEO = 0x4000,
        COMMON_LVB_UNDERSCORE = 0x8000,
    }

    struct CharInfo
    {
        public char Char { get; private set; }
        public CharAttr Attr { get; private set; }

        public CharInfo(char c, CharAttr a)
        {
            Char = c;
            Attr = a;
        }

        public void Change(char c)
        {
            Char = c;
        }
    }
}
