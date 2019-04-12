using System.Collections.Generic;

namespace iobloc
{
    class Paint2 : IGame<PaintCell>
    {
        const int W = 100;
        const int H = 75;

        PaintPane _pnControl = new PaintPane(1, 10);
        PaintPane _pnCanvas = new PaintPane(H, W);

        public Border Border => default;
        public int FrameInterval => default;
        public bool IsRunning { get; set; }
        public string[] AllowedKeys => new string[]
        {
            UIKey.LeftArrow, UIKey.RightArrow, UIKey.UpArrow, UIKey.DownArrow, UIKey.Space,
            "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "D0", "Plus", "Add", "Subtract",
        };
        public Dictionary<string, Pane<PaintCell>> Panes => new Dictionary<string, Pane<PaintCell>>
        {
            { Pnl.Main, _pnCanvas },
            { Pnl.Level, _pnControl },
        };

        private int _row;
        private int _col;
        private PaintCell _cell;
        private PaintCell _prev;
        private bool _paint;

        private void Change(bool set)
        {
            if (set)
            {
                _pnCanvas[_row, _col] = _cell;
                _pnControl[1, _cell.Color - 9].Select();
                switch (_cell.Shape)
                {
                    case PaintShape.Square: _pnControl[1, 7].Select(); break;
                    case PaintShape.Triangle: _pnControl[1, 8].Select(); break;
                    case PaintShape.Circle: _pnControl[1, 9].Select(); break;
                }
            }
            else
            {
            }
        }

        public void HandleInput(string key)
        {
            switch (key)
            {
                case UIKey.LeftArrow:
                    if (_col > 0)
                    {
                        Change(false);
                        _col--;
                        Change(true);
                    }
                    break;
                case UIKey.RightArrow:
                    if (_col < W)
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
                    if (_row < H)
                    {
                        Change(false);
                        _row++;
                        Change(true);
                    }
                    break;
                case UIKey.Space:
                    {
                        Change(false);
                        _paint = !_paint;
                        Change(true);
                    }
                    break;
                case "D1":
                case "D2":
                case "D3":
                case "D4":
                case "D5":
                case "D6":
                case "D7":
                    _cell.Color = int.Parse(key.Substring(key.Length - 1)) + 8;
                    Change(true);
                    break;
                case "D8":
                    _cell.Shape = PaintShape.Square;
                    Change(true);
                    break;
                case "D9":
                    _cell.Shape = PaintShape.Triangle;
                    Change(true);
                    break;
                case "D0":
                    _cell.Shape = PaintShape.Circle;
                    Change(true);
                    break;
                case "Add":
                    break;
                case "Subtract":
                    break;
            }
        }

        public void Start() { IsRunning = true; }
        public void Stop() { IsRunning = false; }
        public void NextFrame() { }
        public void TogglePause() { }
    }
}