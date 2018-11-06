namespace iobloc
{
    class PaintBoard : BaseBoard
    {
        private int BW;
        private int _row;
        private int _col;
        private int _color;
        private bool _set;

        public PaintBoard() : base(BoardType.Paint) { }

        protected override void InitializeSettings()
        {
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
            _set = true;
            Change(true);
        }

        protected override void Change(bool set)
        {
            if (set)
            {
                Main[_row, _col] = 15;
                base.Change(set);
            }
            else
                Main[_row, _col] = _set ? _color : 0;
        }

        public override void HandleInput(string key)
        {
            switch (key)
            {
                case "D0":
                case "D1":
                case "D2":
                case "D3":
                case "D4":
                case "D5":
                case "D6":
                case "D7":
                case "NumPad0":
                case "NumPad1":
                case "NumPad2":
                case "NumPad3":
                case "NumPad4":
                case "NumPad5":
                case "NumPad6":
                case "NumPad7":
                    _color = int.Parse(key.Substring(key.Length - 1));
                    break;
                case "D8":
                case "NumPad8":
                    _color += _color < 8 ? 8 : -8;
                    break;
                case "D9":
                case "NumPad9":
                    _color = 15;
                    break;
                case "R":
                    Initialize();
                    break;
                case UIKey.Enter:
                    _set = !_set;
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