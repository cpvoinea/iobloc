namespace iobloc
{
    public class Paint : BaseGame
    {
        private int _row;
        private int _col;
        private int _color;
        private int _prev;
        private bool _paint;
        private bool _light;
        CellShape _shape;

        public Paint() : base(GameType.Paint)
        {
            _paint = true;
            ShowInfo();
            Change(true);
        }

        protected override void InitializeUI()
        {
            base.InitializeUI();
            Panes.Add("Canvas", new Pane<PaneCell>(1, 1, Height - 1, Width));
            Panes.Add("Info", new Pane<PaneCell>(Height, 1, Height, Width));
            Main = Panes["Canvas"];
            Main.SetText(Help, false);
        }

        void ShowInfo()
        {
            var info = Panes["Info"];
            info[0, 0] = new PaneCell(_paint ? 1 + (_light ? 8 : 0) : 0, shape: _shape, ch: '1');
            info[0, 1] = new PaneCell(_paint ? 2 + (_light ? 8 : 0) : 0, shape: _shape, ch: '2');
            info[0, 2] = new PaneCell(_paint ? 3 + (_light ? 8 : 0) : 0, shape: _shape, ch: '3');
            info[0, 3] = new PaneCell(_paint ? 4 + (_light ? 8 : 0) : 0, shape: _shape, ch: '4');
            info[0, 4] = new PaneCell(_paint ? 5 + (_light ? 8 : 0) : 0, shape: _shape, ch: '5');
            info[0, 5] = new PaneCell(_paint ? 6 + (_light ? 8 : 0) : 0, shape: _shape, ch: '6');
            info[0, 6] = new PaneCell(_paint ? 7 + (_light ? 8 : 0) : 0, shape: _shape, ch: '7');
            info[0, 7] = new PaneCell(_paint ? _color + 8 * (_color >= 8 ? -1 : 1) : 0, shape: _shape, ch: '8');
            info[0, 8] = new PaneCell(_paint ? 15 : 0, shape: _shape, ch: '9');
            info[0, 9] = new PaneCell(0, shape: _shape, ch: '0');
            info[0, 10] = new PaneCell(_paint ? 0 : _color, shape: _shape, ch: 'S');
            info[0, 11] = new PaneCell(_color, shape: 1 - _shape, ch: '-');
            info.Change(true);
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
                _shape = CellShape.Block;
            }
            _row = Height / 2;
            _col = Width / 2;
            _color = 9;
            _light = true;
            ShowInfo();
            Change(true);
        }

        protected override void Change(bool set)
        {
            if (!set)
                for (int i = 0; i < BlockWidth; i++)
                    Main[_row, _col + i] = new PaneCell(_paint ? _color : _prev, set, _shape);
            else
            {
                _prev = Main[_row, _col].Color;
                for (int i = 0; i < BlockWidth; i++)
                    Main[_row, _col + i] = new PaneCell((!_paint || _color == 0 && _prev == 0) ? 15 : _color, set, _shape);
                base.Change(set);
            }
        }

        public override void HandleInput(string key)
        {
            switch (key)
            {
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
                    if (_paint)
                    {
                        _color = int.Parse(key.Substring(key.Length - 1));
                        if (_light && _color < 15)
                            _color += 8;
                        ShowInfo();
                        Change(true);
                    }
                    break;
                case "D8":
                case "NumPad8":
                    if (_paint)
                    {
                        _light = !_light;
                        _color += _color < 8 ? 8 : -8;
                        ShowInfo();
                        Change(true);
                    }
                    break;
                case "9":
                case "D9":
                case "NumPad9":
                    if (_paint)
                    {
                        _color = 15;
                        ShowInfo();
                        Change(true);
                    }
                    break;
                case "D0":
                case "NumPad0":
                    if (_paint)
                    {
                        _color = 0;
                        Change(true);
                    }
                    break;
                case "R":
                    Initialize();
                    break;
                case UIKey.Space:
                    _paint = !_paint;
                    Change(true);
                    break;
                case UIKey.LeftArrow:
                    if (_col > 0)
                    {
                        Change(false);
                        _col -= BlockWidth;
                        Change(true);
                    }
                    break;
                case UIKey.RightArrow:
                    if (_col < Width - BlockWidth)
                    {
                        Change(false);
                        _col += BlockWidth;
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
                    if (_row < Height - 2)
                    {
                        Change(false);
                        _row++;
                        Change(true);
                    }
                    break;
                case "OemMinus":
                    _shape = 1 - _shape;
                    ShowInfo();
                    Change(true);
                    break;
            }
        }
    }
}
