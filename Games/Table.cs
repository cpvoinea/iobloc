using System;
using System.Collections.Generic;
using System.Linq;

namespace iobloc
{
    class Table : BaseGame
    {
        #region Action struct
        enum ActionType { Skip, Select, Take, Put, Throw }

        struct Action
        {
            public ActionType Type { get; private set; }
            public int? Line { get; private set; }

            public Action(ActionType type, int? line)
            {
                Type = type;
                Line = line;
            }
        }

        #endregion

        #region Settings
        private int CP, CE, CN, CD, CL, CM;
        private readonly Random _random = new Random();
        private bool _useFreeMove = false;
        private bool _useMarking = true;
        private bool _useBackground = false;
        private bool _isWhite = true;
        private ITableAI _player1;
        private ITableAI _player2;
        private readonly TableLine[] _lines = new TableLine[28];

        #endregion

        #region Accessors
        private TableLine this[int i] => _lines[GetIndex(_isWhite, i)];
        private ITableAI CurrentPlayer => _isWhite ? _player1 : _player2;
        private TableLine Cursor => this[_cursor.Value];
        private int Color => _isWhite ? CP : CE;
        private int BackColor => _useBackground && _cursor.HasValue && _cursor.Value < 24 ? (_cursor.Value % 2 == (_isWhite ? 0 : 1) ? CD : CL) : 0;

        #endregion

        #region Variables - need initialization
        private readonly List<int> _dice = new List<int>();
        private readonly List<int> _allowed = new List<int>();
        private readonly Queue<Action> _actions = new Queue<Action>();
        private int? _cursor;
        private int? _picked;

        #endregion

        public Table() : base(GameType.Table) { }

        #region Initialization
        protected override void InitializeSettings()
        {
            base.InitializeSettings();
            // colors
            CP = GameSettings.GetColor(Settings.PlayerColor);
            CE = GameSettings.GetColor(Settings.EnemyColor);
            CN = GameSettings.GetColor(Settings.NeutralColor);
            CD = GameSettings.GetColor("DarkColor");
            CL = GameSettings.GetColor("LightColor");
            CM = GameSettings.GetColor("MarkingColor");
            // AI players
            int aiCount = GameSettings.GetInt("AIs", 0);
            string assemblyPath = GameSettings.GetString(Settings.AssemblyPath);
            string className = GameSettings.GetString(Settings.ClassName);
            if (aiCount > 0)
                _player2 = new TableAI();
            if (aiCount > 1)
                _player1 = Serializer.InstantiateFromAssembly<ITableAI>(assemblyPath, className) ?? new TableAI();
        }

        protected override void InitializeUI()
        {
            base.InitializeUI();
            // border lines
            Border.AddLines(new[]
            {
                new BorderLine(0, Height + 1, 6 * Block + 1, true),
                new BorderLine(0, Height + 1, 7 * Block + 2, true),
                new BorderLine(0, Height + 1, 13 * Block + 3, true),
                new BorderLine(6 * Block + 1, 7 * Block + 2, Height / 2 - 2, false),
                new BorderLine(6 * Block + 1, 7 * Block + 2, Height / 2 + 3, false)
            });
            // panels
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
            // text panels
            Main.SetText(Help, false);
            // numbers in middle panels
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
            Panels[Pnl.Table.MiddleLeft].SetText(textLeft.Split(','), false);
            Panels[Pnl.Table.MiddleRight].SetText(textRight.Split(','), false);
        }

        protected override void Initialize()
        {
            _actions.Clear();
            _allowed.Clear();
            _dice.Clear();
            _cursor = null;
            _picked = null;

            Level = Serializer.MasterLevel; // for frame multiplier
            // re-create lines
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

            AddAction(ActionType.Throw);
        }

        #endregion

        #region UI
        // show cursor
        protected override void Change(bool set)
        {
            if (!_cursor.HasValue)
                return;
            int cc = !set && Cursor.IsMarked ? CM : (set ? CN : 0);
            int ch = set && _picked.HasValue ? Color : 0;
            Cursor.Set(0, cc);
            Cursor.Set(Main.Height - 1, ch);
        }

        // toggle background
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

        // toggle highlight
        private void ChangeMarking(bool set)
        {
            int cm = set ? CM : 0;
            foreach (int i in _allowed)
            {
                TableLine line = this[i];
                line.Set(0, cm);
                line.Mark(set);
            }
        }

        // toggle numbers
        private void ChangeNumbers()
        {
            Panels[Pnl.Table.MiddleLeft].SwitchMode();
            Panels[Pnl.Table.MiddleRight].SwitchMode();
        }

        private void ShowDice()
        {
            Panels[Pnl.Table.Dice].SetText(string.Join<int>(",", _dice));
        }

        #endregion

        #region Adding Actions
        private void AddAction(ActionType type, int? line = null)
        {
            _actions.Enqueue(new Action(type, line));
        }

        private void EndTurn()
        {
            if (_lines[25].Count == 15 || _lines[26].Count == 15)
            {
                Initialize();
                return;
            }

            _isWhite = !_isWhite;
            _cursor = null;
            _picked = null;
            AddAction(ActionType.Throw);
        }

        private void CursorMove(bool left)
        {
            int? newValue = _cursor;
            if (_useFreeMove)
            {
                if (!_cursor.HasValue)
                    newValue = left ? 23 : 0;
                else
                {
                    if (_cursor < 24)
                    {
                        if (_cursor < 12)
                            newValue = _cursor.Value + (left ? 1 : -1);
                        else if (_cursor < 24)
                            newValue = _cursor.Value + (left ? -1 : 1);
                        if (newValue < 0)
                            newValue = 26;
                    }
                    else if (_cursor < 26)
                        newValue = left ? 23 : 26;
                    else
                        newValue = left ? 0 : 24;
                }
            }
            else
            {
                if (_allowed.Count > 0)
                {
                    if (!_cursor.HasValue)
                        newValue = _allowed[left ? _allowed.Count - 1 : 0];
                    else
                    {
                        int i = _allowed.IndexOf(_cursor.Value);
                        if (i < 0)
                            newValue = _allowed[left ? _allowed.Count - 1 : 0];
                        else
                        {
                            if (_cursor < 12)
                                i += left ? 1 : -1;
                            else if (_cursor < 24)
                                i += left ? -1 : 1;
                            else if (_cursor < 26)
                                i--;
                            else
                                i++;
                            if (i < 0)
                                i = _allowed.Count - 1;
                            if (i >= _allowed.Count)
                                i = 0;
                            newValue = _allowed[i];
                        }
                    }
                }
            }

            AddAction(ActionType.Select, newValue);
        }

        private void CursorAction()
        {
            if (_picked.HasValue)
                AddAction(ActionType.Put);
            else
                AddAction(ActionType.Take);
        }

        private void SetAllowed()
        {
            Change(false);
            if (_useMarking)
                ChangeMarking(false);
            _allowed.Clear();

            if (_dice.Count == 0)
            {
                EndTurn();
                return;
            }
            if (_picked.HasValue)
            {
                SetAllowedTo();
                if (_allowed.Count == 1)
                {
                    Travel(_picked.Value, _allowed[0]);
                    AddAction(ActionType.Put);
                }
            }
            else
            {
                SetAllowedFrom();
                if (_allowed.Count == 1)
                {
                    AddAction(ActionType.Select, _allowed[0]);
                    AddAction(ActionType.Take);
                }
            }
            if (_allowed.Count == 0)
            {
                EndTurn();
                return;
            }
            _allowed.Sort();

            if (_useMarking)
                ChangeMarking(true);
            if (!_cursor.HasValue)
                _cursor = _allowed[_allowed.Count - 1];
            Change(true);
        }

        private void Travel(int from, int to)
        {
            if (from < 24)
                for (int i = from - 1; i > to; i--)
                    AddAction(ActionType.Select, i);
            AddAction(ActionType.Select, to);
        }

        #endregion

        #region Doing Actions
        private void DoAction(Action action)
        {
            switch (action.Type)
            {
                case ActionType.Skip: break;
                case ActionType.Select:
                    Change(false);
                    _cursor = action.Line;
                    Change(true);
                    break;
                case ActionType.Take:
                    Take();
                    break;
                case ActionType.Put:
                    Put();
                    break;
                case ActionType.Throw:
                    Throw();
                    break;
            }
        }

        private void Throw()
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

            SetAllowed();
        }

        private void Take()
        {
            if (!_cursor.HasValue || _picked.HasValue || !_allowed.Contains(_cursor.Value))
                return;

            Cursor.Take(BackColor);
            _picked = _cursor;

            SetAllowed();
        }

        private void Put()
        {
            if (!_cursor.HasValue || !_picked.HasValue || !_allowed.Contains(_cursor.Value))
                return;

            if (Cursor.IsWhite != _isWhite && Cursor.Count > 0)
            {
                Cursor.Take(BackColor);
                TableLine captured = _lines[GetIndex(!_isWhite, 24)];
                captured.Put(!_isWhite, _isWhite ? CE : CP);
            }

            Cursor.Put(_isWhite, Color);
            RemoveDice(_picked.Value, _cursor.Value);
            _picked = null;

            SetAllowed();
        }

        #endregion

        #region Helpers
        private void RemoveDice(int from, int to)
        {
            int val = from - to;
            if (to == 26)
            {
                if (_dice.Contains(from + 1))
                    val = from + 1;
                else
                    val = _dice.First(d => d > from + 1);
            }
            _dice.Remove(val);
            ShowDice();
        }

        private void SetAllowedFrom()
        {
            if (CanTake(this[24]))
            {
                if (CanTakeFrom(24))
                    _allowed.Add(24);
            }
            else
            {
                for (int i = 0; i < 24; i++)
                    if (CanTake(this[i]) && CanTakeFrom(i))
                        _allowed.Add(i);
                if (CanTakeOut())
                    for (int i = 0; i < 6; i++)
                        if (CanTakeOutFrom(i))
                            _allowed.Add(i);
            }
        }

        private void SetAllowedTo()
        {
            int from = _picked.Value;
            _allowed.Add(from);
            foreach (int d in _dice.Distinct())
            {
                int to = from - d;
                if (to >= 0 && CanPut(this[to]))
                    _allowed.Add(to);
            }
            if (CanTakeOut() && CanTakeOutFrom(from))
                _allowed.Add(26);
        }

        private bool CanTakeFrom(int from)
        {
            foreach (int d in _dice.Distinct())
                if (from - d >= 0 && CanPut(this[from - d]))
                    return true;
            return false;
        }

        private bool CanTakeOut()
        {
            for (int i = 6; i <= 24; i++)
                if (CanTake(this[i]))
                    return false;
            return true;
        }

        private bool CanTakeOutFrom(int from)
        {
            if (from >= 6)
                return false;
            if (_dice.Contains(from + 1))
                return true;
            for (int i = from + 1; i < 6; i++)
                if (CanTake(this[i]))
                    return false;
            return _dice.Any(d => d > from + 1);
        }

        private bool CanTake(TableLine line)
        {
            return line.Count > 0 && line.IsWhite == _isWhite;
        }

        private bool CanPut(TableLine line)
        {
            return line.Count <= 1 || line.IsWhite == _isWhite;
        }

        private static int GetIndex(bool isWhite, int line)
        {
            if (line < 24)
                return isWhite ? line : 23 - line;
            if (line < 26)
                return isWhite ? 24 : 25;
            return isWhite ? 26 : 27;
        }

        #endregion

        #region AI
        private int[] GetLines()
        {
            int[] result = new int[28];
            for (int i = 0; i < 28; i++)
            {
                var line = this[i];
                result[i] = line.IsWhite == _isWhite ? line.Count : -line.Count;
            }
            return result;
        }

        #endregion

        #region Implementation
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
                    ChangeNumbers();
                    break;
                case "B":
                    _useBackground = !_useBackground;
                    ChangeBackground(_useBackground);
                    break;
                case UIKey.LeftArrow:
                case UIKey.RightArrow:
                    if (_actions.Count == 0)
                        CursorMove(key == UIKey.LeftArrow);
                    break;
                case UIKey.UpArrow:
                    if (_actions.Count == 0)
                        CursorAction();
                    break;
            }
        }

        public override void NextFrame()
        {
            if (_actions.Count == 0)
                return;
            DoAction(_actions.Dequeue());
        }

        #endregion
    }
}
