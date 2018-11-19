using System;
using System.Collections.Generic;

namespace iobloc
{
    class Table : BaseGame
    {
        private int CP, CE, CN, CD, CL, CM;
        private readonly Random _random = new Random();
        private bool _useFreeMove = true;
        private bool _useMarking = true;
        private bool _useBackground = true;
        // private ITableAI _player1;
        // private ITableAI _player2;
        private readonly TableLine[] _lines = new TableLine[28];
        private readonly List<int> _dice = new List<int>();
        private readonly List<int> _allowed = new List<int>();
        private bool _isWhite = true;
        private int? _cursor;
        private int? _picked;
        private TableLine Current => _lines[GetIndex(_isWhite, _cursor.Value)];
        private int Color => _isWhite ? CP : CE;
        private int BackColor => _useBackground && _cursor.HasValue && _cursor.Value < 24 ? (_cursor.Value % 2 == 0 ? CD : CL) : 0;

        public Table() : base(GameType.Table) { }

        protected override void InitializeSettings()
        {
            base.InitializeSettings();
            CP = GameSettings.GetColor(Settings.PlayerColor);
            CE = GameSettings.GetColor(Settings.EnemyColor);
            CN = GameSettings.GetColor(Settings.NeutralColor);
            CD = GameSettings.GetColor("DarkColor");
            CL = GameSettings.GetColor("LightColor");
            CM = GameSettings.GetColor("MarkingColor");

            // int aiCount = GameSettings.GetInt("AIs", 0);
            // string assemblyPath = GameSettings.GetString(Settings.AssemblyPath);
            // string className = GameSettings.GetString(Settings.ClassName);
            // if (aiCount > 0)
            //     _player2 = new TableAI();
            // if (aiCount > 1)
            //     _player1 = Serializer.InstantiateFromAssembly<ITableAI>(assemblyPath, className) ?? new TableAI();
        }

        protected override void InitializeUI()
        {
            base.InitializeUI();

            Border.AddLines(new[]
            {
                new BorderLine(0, Height + 1, 6 * Block + 1, true),
                new BorderLine(0, Height + 1, 7 * Block + 2, true),
                new BorderLine(0, Height + 1, 13 * Block + 3, true),
                new BorderLine(6 * Block + 1, 7 * Block + 2, Height / 2 - 2, false),
                new BorderLine(6 * Block + 1, 7 * Block + 2, Height / 2 + 3, false)
            });

            Panels.Add(Pnl.Table.UpperLeft, new Panel(1, 1, 17, 6 * Block, (char)Symbol.BlockUpper));
            Panels.Add(Pnl.Table.MiddleLeft, new Panel(18, 1, Height - 17, 6 * Block));
            Panels.Add(Pnl.Table.LowerLeft, new Panel(Height - 16, 1, Height, 6 * Block, (char)Symbol.BlockLower));
            Panels.Add(Pnl.Table.UpperTaken, new Panel(1, 6 * Block + 2, 17, 7 * Block + 1, (char)Symbol.BlockUpper));
            Panels.Add(Pnl.Table.Dice, new Panel(Height / 2 - 1, 6 * Block + 2, Height / 2 + 2, 7 * Block + 1));
            Panels.Add(Pnl.Table.LowerTaken, new Panel(Height - 16, 6 * Block + 2, Height, 7 * Block + 1, (char)Symbol.BlockLower));
            Panels.Add(Pnl.Table.UpperRight, new Panel(1, 7 * Block + 3, 17, 13 * Block + 2, (char)Symbol.BlockUpper));
            Panels.Add(Pnl.Table.MiddleRight, new Panel(18, 7 * Block + 3, Height - 17, 13 * Block + 2));
            Panels.Add(Pnl.Table.LowerRight, new Panel(Height - 16, 7 * Block + 3, Height, 13 * Block + 2, (char)Symbol.BlockLower));
            Panels.Add(Pnl.Table.UpperOut, new Panel(1, 13 * Block + 4, 17, 14 * Block + 3, (char)Symbol.BlockUpper));
            Panels.Add(Pnl.Table.LowerOut, new Panel(Height - 16, 13 * Block + 4, Height, 14 * Block + 3, (char)Symbol.BlockLower));
            Main = Panels[Pnl.Table.UpperLeft];

            Main.SetText(Help, false);
            Panels[Pnl.Table.Dice].SwitchMode();
            int padLeft = (BlockWidth - 2) / 2;
            string textLeft = "";
            string textRight = "";
            for (int i = 0; i < 6; i++)
            {
                textLeft += $"{13 + i,2}".PadLeft(padLeft + 2).PadRight(Block);
                textRight += $"{19 + i,2}".PadLeft(padLeft + 2).PadRight(Block);
            }
            for (int i = 0; i < Panels[Pnl.Table.MiddleLeft].Height - 1; i++)
            {
                textLeft += ",";
                textRight += ",";
            }
            for (int i = 0; i < 6; i++)
            {
                textLeft += $"{12 - i,2}".PadLeft(padLeft + 2).PadRight(Block);
                textRight += $"{6 - i,2}".PadLeft(padLeft + 2).PadRight(Block);
            }
            Panels[Pnl.Table.MiddleLeft].SetText(textLeft.Split(','));
            Panels[Pnl.Table.MiddleRight].SetText(textRight.Split(','));
        }

        protected override void Initialize()
        {
            Level = Serializer.MasterLevel; // for frame multiplier
            for (int i = 0; i < 6; i++)
            {
                bool isDark = i % 2 == 0;
                _lines[i] = new TableLine(Panels[Pnl.Table.LowerRight], BlockWidth, Block, 5 - i, Main.Height - 1, true);
                _lines[i + 6] = new TableLine(Panels[Pnl.Table.LowerLeft], BlockWidth, Block, 5 - i, Main.Height - 1, true);
                _lines[i + 12] = new TableLine(Panels[Pnl.Table.UpperLeft], BlockWidth, Block, i, 0, false);
                _lines[i + 18] = new TableLine(Panels[Pnl.Table.UpperRight], BlockWidth, Block, i, 0, false);
            }
            _lines[24] = new TableLine(Panels[Pnl.Table.UpperTaken], BlockWidth, Block, 0, 0, false);
            _lines[25] = new TableLine(Panels[Pnl.Table.LowerTaken], BlockWidth, Block, 0, Main.Height - 1, true);
            _lines[26] = new TableLine(Panels[Pnl.Table.LowerOut], BlockWidth, Block, 0, Main.Height - 1, true);
            _lines[27] = new TableLine(Panels[Pnl.Table.UpperOut], BlockWidth, Block, 0, 0, false);
            _lines[0].Initialize(2, false, CE);
            _lines[5].Initialize(5, true, CP);
            _lines[7].Initialize(3, true, CP);
            _lines[11].Initialize(5, false, CE);
            _lines[12].Initialize(5, true, CP);
            _lines[16].Initialize(3, false, CE);
            _lines[18].Initialize(5, false, CE);
            _lines[23].Initialize(2, true, CP);
            if (_useBackground)
                ChangeBackground(_useBackground);

            _cursor = null;
            _picked = null;
            ThrowDice();
        }

        protected override void Change(bool set)
        {
            if (!_cursor.HasValue)
                return;
            int cc = !set && Current.IsMarked ? CM : (set ? CN : 0);
            int ch = set && _picked.HasValue ? Color : 0;
            Current.Set(0, cc);
            Current.Set(Main.Height - 1, ch);
        }

        private void EndTurn()
        {
            _isWhite = !_isWhite;
            _cursor = null;
            _picked = null;
            ThrowDice();
        }

        private void ThrowDice()
        {
            int d1 = _random.Next(6) + 1;
            int d2 = _random.Next(6) + 1;
            _dice.Clear();
            _dice.Add(d1);
            _dice.Add(d2);
            if (d1 == d2)
                _dice.AddRange(_dice);
            _dice.Sort();
            ShowDice();
        }

        private void RemoveDice(int val)
        {
            _dice.Remove(val);
            ShowDice();
        }

        private void ShowDice()
        {
            Panels[Pnl.Table.Dice].SetText(string.Join<int>(",", _dice));
        }

        private void ChangeBackground(bool set)
        {
            for (int i = 0; i < 24; i++)
            {
                TableLine line = _lines[i];
                int cb = set ? (i % 2 == 0 ? CD : CL) : 0;
                for (int j = line.Count + 1; j < Main.Height - 1; j++)
                    line.Set(j, cb);
            }
        }

        private void ChangeMarking(bool set)
        {
            int cm = set ? CM : 0;
            foreach (int i in _allowed)
            {
                TableLine line = _lines[GetIndex(_isWhite, i)];
                line.Set(0, cm);
                line.Mark(set);
            }
        }

        private void CursorMove(bool left)
        {
            Change(false);
            if (_useFreeMove)
            {
                if (!_cursor.HasValue)
                    _cursor = left ? 23 : 0;
                else
                {
                    if (_cursor < 24)
                    {
                        if (_cursor.Value < 12)
                            _cursor += left ? 1 : -1;
                        else if (_cursor.Value < 24)
                            _cursor += left ? -1 : 1;
                        if (_cursor < 0)
                            _cursor = 26;
                    }
                    else if (_cursor < 26)
                        _cursor = left ? 23 : 26;
                    else
                        _cursor = left ? 0 : 24;
                }
            }
            else
            {
                if (_allowed.Count > 0)
                {
                    if (!_cursor.HasValue)
                        _cursor = _allowed[left ? _allowed.Count - 1 : 0];
                    else
                    {
                        int i = _allowed.IndexOf(_cursor.Value);
                        if (i < 0)
                            _cursor = _allowed[left ? _allowed.Count - 1 : 0];
                        else
                        {
                            if (_cursor.Value < 12)
                                i += left ? 1 : -1;
                            else if (_cursor.Value < 24)
                                i += left ? -1 : 1;
                            else if (_cursor.Value < 26)
                                i--;
                            else
                                i++;
                            if (i < 0)
                                i = _allowed.Count - 1;
                            if (i >= _allowed.Count)
                                i = 0;
                            _cursor = _allowed[i];
                        }
                    }
                }
            }
            Change(true);
        }

        private bool CanTake()
        {
            return !_picked.HasValue && _cursor.HasValue && Current.Count > 0 && Current.IsWhite == _isWhite;
        }

        private bool CanPut()
        {
            return _picked.HasValue && _cursor.HasValue &&
                (Current.Count == 0 || Current.IsWhite == _isWhite || Current.Count == 1);
        }

        private void Take()
        {
            Current.Take(BackColor);
            _picked = _cursor.Value;
            Change(true);
        }

        private void Put()
        {
            if (Current.IsWhite != _isWhite && Current.Count > 0)
            {
                Current.Take(BackColor);
                TableLine captured = _lines[GetIndex(!_isWhite, 24)];
                captured.Put(!_isWhite, _isWhite ? CE : CP);
            }
            Current.Put(_isWhite, Color);
            _picked = null;
            Change(true);
        }

        public override void TogglePause()
        {
            Main.SwitchMode();
        }

        public override void HandleInput(string key)
        {
            switch (key)
            {
                case "R":
                    Initialize();
                    break;
                case "F":
                    Change(false);
                    _cursor = null;
                    _useFreeMove = !_useFreeMove;
                    break;
                case "M":
                    _useMarking = !_useMarking;
                    ChangeMarking(_useMarking);
                    break;
                case "N":
                    Panels[Pnl.Table.MiddleLeft].SwitchMode();
                    Panels[Pnl.Table.MiddleRight].SwitchMode();
                    break;
                case "B":
                    _useBackground = !_useBackground;
                    ChangeBackground(_useBackground);
                    break;
                case UIKey.LeftArrow:
                case UIKey.RightArrow:
                    CursorMove(key == UIKey.LeftArrow);
                    break;
                case UIKey.UpArrow:
                    if (CanTake())
                        Take();
                    else if (CanPut())
                        Put();
                    break;
            }
        }

        public override void NextFrame()
        {
        }

        private int[] GetLines()
        {
            int[] result = new int[28];
            for (int i = 0; i < 28; i++)
            {
                var line = _lines[GetIndex(_isWhite, i)];
                result[i] = line.IsWhite == _isWhite ? line.Count : -line.Count;
            }
            return result;
        }

        private static int GetIndex(bool isWhite, int line)
        {
            if (line < 24)
                return isWhite ? line : 23 - line;
            if (line < 26)
                return isWhite ? 24 : 25;
            return isWhite ? 26 : 27;
        }
    }
}
