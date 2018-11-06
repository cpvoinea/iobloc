namespace iobloc
{
    class PaintBoard : BaseBoard
    {
        private int BW;
        private int _row;
        private int _col;
        private int _color;
        private int _prev;
        private bool _paint;
        private bool _light;

        public PaintBoard() : base(BoardType.Paint) { }

        protected override void InitializeSettings()
        {
            base.InitializeSettings();
            BW = BoardSettings.GetInt("BlockWidth");
        }

        protected override void Initialize()
        {
            base.Initialize();
            if (IsInitialized)
            {
                Main.Clear();
                _color = 0;
                _prev = 0;
                _paint = false;
            }
            _row = Height / 2;
            _col = Width / 2;
            _color = 9;
            _light = true;
            Change(true);
        }

        protected override void Change(bool set)
        {
            if (set)
                _prev = Main[_row, _col];
            for (int i = 0; i < BW; i++)
                Main[_row, _col + i] = _paint ? _color : (set ? 15 : _prev);
            base.Change(set);
        }

        public override void HandleInput(string key)
        {
            switch (key)
            {
                case "D1": case "D2": case "D3": case "D4": case "D5": case "D6": case "D7":
                case "NumPad1": case "NumPad2": case "NumPad3": case "NumPad4": case "NumPad5": case "NumPad6": case "NumPad7":
                    if (_paint)
                    {
                        _color = int.Parse(key.Substring(key.Length - 1));
                        if (_light)
                            _color += 8;
                    }
                    break;
                case "D8": case "NumPad8":
                    if (_paint)
                    {
                        _light = !_light;
                        _color += _color < 8 ? 8 : -8;
                    }
                    break;
                case "D9": case "NumPad9":
                    if (_paint)
                        _color = 15;
                    break;
                case "D0": case "NumPad0":
                    if (_paint)
                        _color = 0;
                    break;
                case "R":
                    Initialize();
                    break;
                case UIKey.Enter:
                    _paint = !_paint;
                    break;
                case UIKey.LeftArrow:
                    if (_col > 0)
                    {
                        Change(false);
                        _col -= BW;
                    }
                    break;
                case UIKey.RightArrow:
                    if (_col < Width - BW)
                    {
                        Change(false);
                        _col += BW;
                    }
                    break;
                case UIKey.UpArrow:
                    if (_row > 0)
                    {
                        Change(false);
                        _row--;
                    }
                    break;
                case UIKey.DownArrow:
                    if (_row < Height - 1)
                    {
                        Change(false);
                        _row++;
                    }
                    break;
            }
            Change(true);
        }
    }
}
