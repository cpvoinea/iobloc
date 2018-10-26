using System;
using System.Collections.Generic;

namespace iobloc
{
    class TableBoard : IBoard
    {
        static class Pnl
        {
            internal const string Main = "upperLeft";
            internal const string UpperLeft = "upperLeft";
            internal const string LowerLeft = "lowerLeft";
            internal const string UpperRight = "upperRight";
            internal const string LowerRight = "lowerRight";
            internal const string UpperJail = "upperJail";
            internal const string LowerJail = "lowerJail";
            internal const string Dice = "dice";
            internal const string UpperOut = "upperOut";
            internal const string LowerOut = "lowerOut";
        }

        int PW => (int)_settings.GetInt("PieceWidth");
        int W => PW * 14 + 5;
        int H => (int)_settings.GetInt("Height");
        int CP => (int)_settings.GetColor("PlayerColor");
        int CE => (int)_settings.GetColor("EnemyColor");
        int CN => (int)_settings.GetColor("NeutralColor");
        Panel PLR => _panels[Pnl.LowerRight];
        Panel PLL => _panels[Pnl.LowerLeft];
        Panel PUR => _panels[Pnl.UpperRight];
        Panel PUL => _panels[Pnl.UpperLeft];

        readonly Dictionary<string, string> _settings;
        readonly string[] _help;
        readonly string[] _keys;
        readonly Border _border;
        Panel _main;
        readonly Dictionary<string, Panel> _panels;
        int _score;
        readonly Random _random;
        int _position;

        public Border Border { get { return _border; } }
        public Panel MainPanel { get { return _main; } }
        public Dictionary<string, Panel> Panels { get { return _panels; } }
        public string[] Help { get { return _help; } }
        public int FrameInterval { get { return 250; } }
        public int? Highscore { get { return null; } }
        public int Level { get { return -1; } }
        public int Score { get { return _score; } }
        public bool? Win { get; private set; }
        public bool IsRunning { get; set; }

        internal TableBoard()
        {
            _settings = Config.Settings(Option.Table);
            _help = _settings.GetList("Help");
            _keys = _settings.GetList("Keys");
            _border = new Border(new[]
            {
                new BorderLine(0, W - 1, 0, false, false),
                new BorderLine(0, W - 1, H - 1, false, false),
                new BorderLine(0, H - 1, 0, true, false),
                new BorderLine(0, H - 1, W - 1, true, false),
                new BorderLine(0, H - 1, 6 * PW + 1, true, true),
                new BorderLine(0, H - 1, 7 * PW + 2, true, true),
                new BorderLine(0, H - 1, 13 * PW + 3, true, true),
                new BorderLine(6 * PW + 1, 7 * PW + 2, H / 2 - 3, false, true),
                new BorderLine(6 * PW + 1, 7 * PW + 2, H / 2 + 2, false, true)
            });
            _panels = new Dictionary<string, Panel>
            {
                {Pnl.UpperLeft, new Panel(1, 1, H / 2 - 1, 6 * PW, (char)BoxGraphics.BlockUpper)},
                {Pnl.LowerLeft, new Panel(H / 2, 1, H - 2, 6 * PW, (char)BoxGraphics.BlockLower)},
                {Pnl.UpperJail, new Panel(1, 6 * PW + 2, H / 2 - 3, 7 * PW + 1, (char)BoxGraphics.BlockUpper)},
                {Pnl.Dice, new Panel(H / 2 - 2, 6 * PW + 2, H / 2, 7 * PW + 1, (char)0)},
                {Pnl.LowerJail, new Panel(H / 2 + 1, 6 * PW + 2, H - 2, 7 * PW + 1, (char)BoxGraphics.BlockLower)},
                {Pnl.UpperRight, new Panel(1, 7 * PW + 3, H / 2 - 1, 13 * PW + 2, (char)BoxGraphics.BlockUpper)},
                {Pnl.LowerRight, new Panel(H / 2, 7 * PW + 3, H - 2, 13 * PW + 2, (char)BoxGraphics.BlockLower)},
                {Pnl.UpperOut, new Panel(1, 13 * PW + 4, H / 2 - 1, 14 * PW + 3, (char)BoxGraphics.BlockUpper)},
                {Pnl.LowerOut, new Panel(H / 2, 13 * PW + 4, H - 2, 14 * PW + 3, (char)BoxGraphics.BlockLower)}
            };
            _main = _panels[Pnl.Main];

            _random = new Random();
            Restart();
        }

        public bool IsValidInput(string key)
        {
            return Array.Exists(_keys, x => x == key);
        }

        public void HandleInput(string key)
        {
            Set(false);
            switch (key)
            {
                case "LeftArrow":
                    if (_position <= 12)
                        _position++;
                    else if (_position <= 25)
                        _position--;
                    break;
                case "RightArrow":
                    if (_position <= 12 && _position > 0)
                        _position--;
                    else if (_position > 12 && _position < 25)
                        _position++;
                    break;
                case "Spacebar": break;
            }
            Set();
        }

        public void NextFrame()
        {
        }

        void Restart()
        {
            foreach (var pnl in _panels.Values)
            {
                for (int i = 0; i < pnl.Height; i++)
                    for (int j = 0; j < pnl.Width; j++)
                        pnl.Grid[i, j] = 0;
                pnl.HasChanges = true;
            }

            for (int j = 0; j < PW; j++)
            {
                for (int i = 0; i < 5; i++)
                {
                    PLR.Grid[PLR.Height - 1 - i, j] = CP;
                    PLL.Grid[PLL.Height - 1 - i, j] = CE;
                    PUL.Grid[i, j] = CP;
                    PUR.Grid[i, j] = CE;
                }
                for (int i = 0; i < 3; i++)
                {
                    PLL.Grid[PLL.Height - 1 - i, 4 * PW + j] = CP;
                    PUL.Grid[i, 4 * PW + j] = CE;
                }
                for (int i = 0; i < 2; i++)
                {
                    PLR.Grid[PLR.Height - 1 - i, 5 * PW + j] = CE;
                    PUR.Grid[i, 5 * PW + j] = CP;
                }
            }
        }

        void Set(bool select = true, bool white = true)
        {
            Panel pnl = null;
            int row = 0, col = 0;
            if (_position <= 0)
            {
                pnl = white ? _panels[Pnl.LowerOut] : _panels[Pnl.UpperOut];
                row = white ? pnl.Height - 1 : 0;
            }
            else if (_position <= 6)
            {
                pnl = PLR;
                row = pnl.Height - 1;
                col = 6 - _position;
            }
            else if (_position <= 12)
            {
                pnl = PLL;
                row = pnl.Height - 1;
                col = 12 - _position;
            }
            else if (_position <= 18)
            {
                pnl = PUL;
                col = _position - 13;
            }
            else if (_position <= 24)
            {
                pnl = PUR;
                col = _position - 19;
            }
            else if (_position >= 25)
            {
                pnl = white ? _panels[Pnl.UpperJail] : _panels[Pnl.LowerJail];
                row = white ? 0 : pnl.Height - 1;
            }

            if (pnl != null)
            {
                for (int i = col * PW; i < (col + 1) * PW; i++)
                    pnl.Grid[row, i] = select ? CN : 0;
                pnl.HasChanges = true;
            }
        }
    }
}