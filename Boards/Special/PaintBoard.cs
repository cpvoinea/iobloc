namespace iobloc
{
    class PaintBoard : BaseBoard
    {
        private int BW;
        private int _row;
        private int _col;
        private int _color;
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
                Main.Clear();
            _row = Height - 1;
            _col = Width / 2;
            _color = 15;
            _paint = true;
            Change(true);
        }

        protected override void Change(bool set)
        {
            if (set)
            {
                Main[_row, _col] = _color == 0 || !_paint ? 15 : _color;
                base.Change(set);
            }
            else
                Main[_row, _col] = _paint ? _color : 0;
        }

        public override void HandleInput(string key)
        {
            switch (key)
            {
                case "D0":
                case "NumPad0":
                    _color = 0;
                    break;
                case "D1":
                case "D2":
                case "D3":
                case "D4":
                case "D5":
                case "D6":
                case "D7":
                case "NumPad1":
                case "NumPad2":
                case "NumPad3":
                case "NumPad4":
                case "NumPad5":
                case "NumPad6":
                case "NumPad7":
                    _color = int.Parse(key.Substring(key.Length - 1));
                    if(_light)
                        _color += 8;
                    Change(true);
                    break;
                case "D8":
                case "NumPad8":
                    _light = !_light;
                    _color += _color < 8 ? 8 : -8;
                    Change(true);
                    break;
                case "D9":
                case "NumPad9":
                    _color = 15;
                    Change(true);
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
                        _col--;
                        Change(true);
                    }
                    break;
                case UIKey.RightArrow:
                    if (_col < Width - 1)
                    {
                        Change(false);
                        _col++;
                        Change(true);
                    }
                    break;
                case UIKey.UpArrow:
                    if (_row > 0)
                    {
                        Change(false);
                        _row--;
                        Change(true);
                    }
                    break;
                case UIKey.DownArrow:
                    if (_row < Height - 1)
                    {
                        Change(false);
                        _row++;
                        Change(true);
                    }
                    break;
            }
        }
    }
}
