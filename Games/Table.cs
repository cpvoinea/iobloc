using System;
using System.Collections.Generic;

namespace iobloc
{
    class Table : BaseGame
    {
        #region Action struct
        enum ActionType { Skip, Select, Take, Put, Throw }

        struct Action
        {
            public ActionType Type { get; private set; }
            public int? Param { get; private set; }

            public Action(ActionType type, int? param)
            {
                Type = type;
                Param = param;
            }
        }

        #endregion

        #region Settings
        private int CP, CE, CN, CD, CL, CM;
        private readonly Random _random = new Random();
        private bool _useFreeMove;
        private bool _useMarking;
        private bool _useBackground;
        private bool _isWhite;
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
        private static readonly object QueueSync = new object();
        private readonly List<int> _dice = new List<int>();
        private readonly List<int> _allowed = new List<int>();
        private readonly Queue<Action> _actions = new Queue<Action>();
        private int? _cursor;
        private int? _taken;

        #endregion

        public Table() : base(GameType.Table) { }

        #region Initialization
        protected override void InitializeSettings()
        {
            base.InitializeSettings();
            // colors
            CP = Serializer.GetColor(GameSettings, Settings.PlayerColor);
            CE = Serializer.GetColor(GameSettings, Settings.EnemyColor);
            CN = Serializer.GetColor(GameSettings, Settings.NeutralColor);
            CD = Serializer.GetColor(GameSettings, "DarkColor");
            CL = Serializer.GetColor(GameSettings, "LightColor");
            CM = Serializer.GetColor(GameSettings, "MarkingColor");
            // AI players
            int aiCount = Serializer.GetInt(GameSettings, "AIs", 0);
            string assemblyPath = Serializer.GetString(GameSettings, Settings.AssemblyPath);
            string className = Serializer.GetString(GameSettings, Settings.ClassName);
            if (aiCount > 0)
                _player2 = new BasicAI();
            if (aiCount > 1)
                _player1 = Serializer.InstantiateFromAssembly<ITableAI>(assemblyPath, className) ?? new BasicAI();
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
            // panes
            Panes.Add(Pnl.Table.UpperLeft, new Pane<int>(1, 1, 17, 6 * Block, (char)Symbol.BlockUpper));
            Panes.Add(Pnl.Table.MiddleLeft, new Pane<int>(18, 1, Height - 17, 6 * Block));
            Panes.Add(Pnl.Table.LowerLeft, new Pane<int>(Height - 16, 1, Height, 6 * Block, (char)Symbol.BlockLower));
            Panes.Add(Pnl.Table.UpperTaken, new Pane<int>(1, 6 * Block + 2, 17, 7 * Block + 1, (char)Symbol.BlockUpper));
            Panes.Add(Pnl.Table.Dice, new Pane<int>(Height / 2 - 1, 6 * Block + 2, Height / 2 + 2, 7 * Block + 1));
            Panes.Add(Pnl.Table.LowerTaken, new Pane<int>(Height - 16, 6 * Block + 2, Height, 7 * Block + 1, (char)Symbol.BlockLower));
            Panes.Add(Pnl.Table.UpperRight, new Pane<int>(1, 7 * Block + 3, 17, 13 * Block + 2, (char)Symbol.BlockUpper));
            Panes.Add(Pnl.Table.MiddleRight, new Pane<int>(18, 7 * Block + 3, Height - 17, 13 * Block + 2));
            Panes.Add(Pnl.Table.LowerRight, new Pane<int>(Height - 16, 7 * Block + 3, Height, 13 * Block + 2, (char)Symbol.BlockLower));
            Panes.Add(Pnl.Table.UpperOut, new Pane<int>(1, 13 * Block + 4, 17, 14 * Block + 3, (char)Symbol.BlockUpper));
            Panes.Add(Pnl.Table.LowerOut, new Pane<int>(Height - 16, 13 * Block + 4, Height, 14 * Block + 3, (char)Symbol.BlockLower));
            Main = Panes[Pnl.Table.UpperLeft];
            // text panes
            Main.SetText(Help, false);
            // numbers in middle panes
            int padLeft = (BlockWidth - 2) / 2;
            string textLeft = "";
            string textRight = "";
            for (int i = 0; i < 6; i++)
            {
                textLeft += $"{13 + i,2}".PadLeft(padLeft + 2).PadRight(Block);
                textRight += $"{19 + i,2}".PadLeft(padLeft + 2).PadRight(Block);
            }
            for (int i = 0; i < Panes[Pnl.Table.MiddleLeft].Height - 1; i++)
            {
                textLeft += ",";
                textRight += ",";
            }
            for (int i = 0; i < 6; i++)
            {
                textLeft += $"{12 - i,2}".PadLeft(padLeft + 2).PadRight(Block);
                textRight += $"{6 - i,2}".PadLeft(padLeft + 2).PadRight(Block);
            }
            Panes[Pnl.Table.MiddleLeft].SetText(textLeft.Split(','), false);
            Panes[Pnl.Table.MiddleRight].SetText(textRight.Split(','), false);

            for (int i = 0; i < 6; i++)
            {
                _lines[i] = new TableLine(Panes[Pnl.Table.LowerRight], BlockWidth, Block, 5 - i, Main.Height - 1, true);
                _lines[i + 6] = new TableLine(Panes[Pnl.Table.LowerLeft], BlockWidth, Block, 5 - i, Main.Height - 1, true);
                _lines[i + 12] = new TableLine(Panes[Pnl.Table.UpperLeft], BlockWidth, Block, i, 0, false);
                _lines[i + 18] = new TableLine(Panes[Pnl.Table.UpperRight], BlockWidth, Block, i, 0, false);
            }
            _lines[24] = new TableLine(Panes[Pnl.Table.UpperTaken], BlockWidth, Block, 0, 0, false);
            _lines[25] = new TableLine(Panes[Pnl.Table.LowerTaken], BlockWidth, Block, 0, Main.Height - 1, true);
            _lines[26] = new TableLine(Panes[Pnl.Table.LowerOut], BlockWidth, Block, 0, Main.Height - 1, true);
            _lines[27] = new TableLine(Panes[Pnl.Table.UpperOut], BlockWidth, Block, 0, 0, false);
        }

        protected override void Initialize()
        {
            if (IsInitialized)
            {
                for (int i = 0; i < _lines.Length; i++)
                    _lines[i].Initialize();
            }
            else
            {
                Level = Serializer.MasterLevel;
                Score = 0;
            }

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
        protected override void Change(bool set)
        {
            if (!_cursor.HasValue)
                return;
            int cc = !set && Cursor.IsMarked ? CM : (set ? CN : 0);
            int ch = set && _taken.HasValue ? Color : 0;
            Cursor.Set(0, cc);
            Cursor.Set(Main.Height - 1, ch);
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
                TableLine line = this[i];
                line.Set(0, cm);
                line.Mark(set);
            }
            Change(true);
        }

        private void ChangeNumbers()
        {
            Panes[Pnl.Table.MiddleLeft].SwitchMode();
            Panes[Pnl.Table.MiddleRight].SwitchMode();
        }

        private void ShowDice()
        {
            Panes[Pnl.Table.Dice].SetText(Serializer.Join(",", _dice));
        }

        #endregion

        #region Adding Actions
        private void AddAction(ActionType type, int? param = null)
        {
            _actions.Enqueue(new Action(type, param));
        }

        private void EndTurn()
        {
            if (_useMarking)
                ChangeMarking(false);
            if (_lines[26].Count == 15)
            {
                Score++;
                Win(true);
            }
            else if (_lines[27].Count == 15)
            {
                Score--;
                Lose();
            }
            else
                AddAction(ActionType.Throw);
        }

        private void CursorMove(bool left)
        {
            int? newValue = _cursor;
            if (_useFreeMove)
            {
                if (!newValue.HasValue)
                    newValue = left ? 23 : 0;
                else
                {
                    if (newValue < 24)
                    {
                        if (newValue < 12)
                            newValue = newValue.Value + (left ? 1 : -1);
                        else if (newValue < 24)
                            newValue = newValue.Value + (left ? -1 : 1);
                        if (newValue < 0)
                            newValue = 26;
                    }
                    else if (newValue < 26)
                        newValue = left ? 23 : 26;
                    else
                        newValue = left ? 0 : 24;
                }
            }
            else
            {
                if (_allowed.Count > 0)
                {
                    if (!newValue.HasValue)
                        newValue = _allowed[left ? _allowed.Count - 1 : 0];
                    else
                    {
                        int i = _allowed.IndexOf(newValue.Value);
                        if (i < 0)
                            newValue = _allowed[left ? _allowed.Count - 1 : 0];
                        else
                        {
                            if (newValue < 12)
                                i += left ? 1 : -1;
                            else if (newValue < 24)
                                i += left ? -1 : 1;
                            else if (newValue < 26)
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

            if (newValue.HasValue && newValue != _cursor)
                AddAction(ActionType.Select, newValue);
        }

        private void CursorAction()
        {
            if (!_cursor.HasValue)
                return;

            if (_taken.HasValue)
                AddAction(ActionType.Put);
            else
                AddAction(ActionType.Take);
        }

        private void AIAction()
        {
            if (CurrentPlayer == null)
                return;

            var moves = CurrentPlayer.GetMoves(GetLines(), _dice.ToArray());
            foreach (var m in moves)
            {
                AddAction(ActionType.Skip);
                AddAction(ActionType.Select, m[0]);
                AddAction(ActionType.Take);
                Travel(m[0], m[1]);
                AddAction(ActionType.Put);
            }
        }

        private void AutoAction()
        {
            if (CurrentPlayer != null || _actions.Count > 0)
                return;

            if (!_taken.HasValue && _allowed.Count == 1)
            {
                AddAction(ActionType.Select, _allowed[0]);
                AddAction(ActionType.Take);
            }
            else if (_taken.HasValue && _allowed.Count == 2)
            {
                int i = 0;
                while (i < _allowed.Count && _allowed[i] == _taken.Value)
                    i++;
                int to = _allowed[i];
                Travel(_taken.Value, to);
                AddAction(ActionType.Put);
            }
            else if (!_cursor.HasValue || !_allowed.Contains(_cursor.Value))
            {
                int to = 0;
                foreach (int a in _allowed)
                    if (a > to)
                        to = a;
                AddAction(ActionType.Select, to);
            }
        }

        private void Travel(int from, int to)
        {
            if (from < 24 && to < 24)
                for (int i = from - 1; i > to; i--)
                    AddAction(ActionType.Select, i);
            AddAction(ActionType.Select, to);
        }

        #endregion

        #region Doing Actions
        private void Take()
        {
            if (!_cursor.HasValue || _taken.HasValue || !_allowed.Contains(_cursor.Value))
                return;

            Cursor.Take(BackColor);
            _taken = _cursor;
            SetAllowed();

            AutoAction();
        }

        private void Put()
        {
            if (!_cursor.HasValue || !_taken.HasValue || !_allowed.Contains(_cursor.Value))
                return;

            if (Cursor.IsWhite != _isWhite && Cursor.Count > 0)
            {
                Cursor.Take(BackColor);
                var captured = _lines[GetIndex(!_isWhite, 24)];
                captured.Put(!_isWhite, _isWhite ? CE : CP);
            }

            Cursor.Put(_isWhite, Color);
            _dice.Remove(GetDice(_taken.Value, _cursor.Value));
            _taken = null;
            ShowDice();
            SetAllowed();

            if (_dice.Count == 0 || _allowed.Count == 0)
                EndTurn();
            else
                AutoAction();
        }

        private void Throw()
        {
            Change(false);
            _isWhite = !_isWhite;
            SetDice();
            SetAllowed();

            if (_allowed.Count == 0)
                EndTurn();
            else
            {
                AIAction();
                AutoAction();
            }
        }

        private void SetCursor(int c)
        {
            if (c == _cursor)
                return;

            Change(false);
            _cursor = c;
            Change(true);
        }

        private void SetAllowed()
        {
            if (_allowed.Count > 0)
            {
                if (_useMarking)
                    ChangeMarking(false);
                _allowed.Clear();
            }
            if (_dice.Count == 0)
                return;

            if (_taken.HasValue)
            {
                _allowed.AddRange(GetAllowedTo(_taken.Value));
                if (CurrentPlayer == null)
                    _allowed.Add(_taken.Value);
            }
            else
                _allowed.AddRange(GetAllowedFrom());
            _allowed.Sort();

            if (_useMarking)
                ChangeMarking(true);
            Change(true);
        }

        private void SetDice()
        {
            _dice.Clear();
            int d1 = _random.Next(6) + 1;
            int d2 = _random.Next(6) + 1;
            _dice.Add(d1);
            _dice.Add(d2);
            if (d1 == d2)
                _dice.AddRange(_dice);
            _dice.Sort();
            ShowDice();
        }

        #endregion

        #region AI
        private int[] GetAllowedFrom() => BasicAI.GetAllowedFrom(GetLines(), _dice.ToArray());
        private int[] GetAllowedTo(int from) => BasicAI.GetAllowedTo(GetLines(), _dice.ToArray(), from);
        private int GetDice(int from, int to) => BasicAI.GetDice(_dice.ToArray(), from, to);

        private static int GetIndex(bool isWhite, int line)
        {
            if (line < 24)
                return isWhite ? line : 23 - line;
            if (line < 26)
                return isWhite ? 24 : 25;
            return isWhite ? 26 : 27;
        }

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
            lock (QueueSync)
            {
                if (_actions.Count == 0)
                    return;
                var a = _actions.Dequeue();
                while (a.Type == ActionType.Select && a.Param == _cursor)
                    a = _actions.Dequeue();
                switch (a.Type)
                {
                    case ActionType.Skip: break;
                    case ActionType.Select:
                        SetCursor(a.Param.Value);
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
        }

        #endregion
    }
}
