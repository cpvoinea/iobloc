namespace iobloc.Ascio
{
    struct Screen
    {
        public int Left { get; private set; }
        public int Top { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public CharInfo[,] Text { get; private set; }

        public Coord From => new Coord { X = (short)Left, Y = (short)Top };
        public Coord Size => new Coord { X = (short)Width, Y = (short)Height };
        public Rect Rect => new Rect { Left = (short)Left, Top = (short)Top, Right = (short)(Left + Width - 1), Bottom = (short)(Top + Height - 1) };
        public CharInfo this[int row, int col] => Text[row, col];

        public Screen(int left, int top, int width, int height, CharAttr? attr = null)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
            Text = new CharInfo[height, width];
            if (attr.HasValue)
                Clear(attr.Value);
        }

        public void Clear(CharAttr? attr = null)
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    if (attr.HasValue)
                        Text[y, x] = new CharInfo(' ', attr.Value);
                    else
                        Text[y, x].Change(' ');
        }

        public void SetText(string text, Rect? area = null)
        {
            int left = area.HasValue ? area.Value.Left : 0;
            int top = area.HasValue ? area.Value.Top : 0;
            int w = area.HasValue ? area.Value.Right - area.Value.Left + 1 : Width;
            int y = top;
            int x = left;
            int i = 0;
            while (i < text.Length)
                if (y >= 0 && y < Height)
                {
                    Text[y, x].Change(text[i]);
                    i++;
                    x++;
                    if (x >= Width || x - left >= w)
                    {
                        x = left;
                        y++;
                    }
                }
        }

        public void SetScreen(Screen rect)
        {
            for (int y = 0, sy = rect.Top; y < rect.Height && y + rect.Top < Height; y++, sy++)
                if (sy >= 0 && sy < Height)
                    for (int x = 0, sx = rect.Left; x < rect.Width && x + rect.Left < Width; x++, sx++)
                        if (sx >= 0 && sx < Width)
                        {
                            CharInfo c = rect.Text[y, x];
                            if (c.Char != ' ')
                                Text[sy, sx] = c;
                        }
        }

        public void Move(int left, int top)
        {
            Left = left;
            Top = top;
        }
    }
}
