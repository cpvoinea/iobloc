namespace iobloc
{
    public struct Area
    {
        const int X = 1 << 16;
        public int Left { get; private set; }
        public int Top { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int[] Text { get; }

        public int this[int row, int col]
        {
            get { return Text[row * Width + col]; }
            set { Text[row * Width + col] = value; }
        }

        public Area(int left, int top, int width, int height, short? attr = null)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
            Text = new int[height * width];
            if (attr.HasValue)
                Clear(attr.Value);
        }

        public void Clear(short? attr = null)
        {
            for (int i = 0; i < Text.Length; i++)
                Text[i] = (' ' * X) + (attr ?? (Text[i] % X));
        }

        public void SetText(string text)
        {
            for (int i = 0; i < text.Length; i++)
                Text[i] = (text[i] * X) + (Text[i] % X);
        }

        public void SetArea(Area rect)
        {
            for (int ry = 0, y = rect.Top; ry < rect.Height && ry + rect.Top < Height; ry++, y++)
                if (y >= 0 && y < Height)
                    for (int rx = 0, x = rect.Left; rx < rect.Width && rx + rect.Left < Width; rx++, x++)
                        if (x >= 0 && x < Width)
                            this[y, x] = rect[ry, rx];
        }

        public void Move(int left, int top)
        {
            Left = left;
            Top = top;
        }
    }
}
