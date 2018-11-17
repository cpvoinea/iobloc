using System;
using System.Collections.Generic;

namespace iobloc
{
    class Table : BaseGame
    {
        internal static int BW, B, CP, CE, CN, CD, CL, CH;
        private const int SF = 3;
        private readonly Random _random = new Random();
        private bool _useFreeMove = false;
        private bool _useMarking = false;
        private bool _useNumbers = false;
        private bool _useBackground = false;
        private TableBoard _board;
        private ITableAI _player1;
        private ITableAI _player2;
        private readonly List<int> _dice = new List<int>();
        private readonly Queue<TableAction> _actions = new Queue<TableAction>();
        private readonly List<int> _allowed = new List<int>();
        private bool _isWhite;
        private int? _cursor;
        private int? _pickedFrom;
        private ITableAI CurrentPlayer => _isWhite ? _player1 : _player2;
        public bool IsCurrentPlayerAI => CurrentPlayer != null;

        public Table() : base(GameType.Table) { }

        protected override void InitializeSettings()
        {
            base.InitializeSettings();
            BW = BlockWidth;
            B = Block;
            CP = GameSettings.GetColor(Settings.PlayerColor);
            CE = GameSettings.GetColor(Settings.EnemyColor);
            CN = GameSettings.GetColor(Settings.NeutralColor);
            CD = GameSettings.GetColor("DarkColor");
            CL = GameSettings.GetColor("LightColor");
            CH = GameSettings.GetColor("HighlightColor");

            int aiCount = GameSettings.GetInt("AIs", 0);
            string assemblyPath = GameSettings.GetString(Settings.AssemblyPath);
            string className = GameSettings.GetString(Settings.ClassName);

            if (aiCount > 0)
            {
                _player2 = Serializer.InstantiateFromAssembly<ITableAI>(assemblyPath, className) ?? new TableAI();
                if (aiCount == 2)
                    _player1 = new TableAI();
            }
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
            Panels.Add(Pnl.Table.UpperTaken, new Panel(1, 6 * Block + 2, 15, 7 * Block + 1, (char)Symbol.BlockUpper));
            Panels.Add(Pnl.Table.Dice, new Panel(Height / 2 - 1, 6 * Block + 2, Height / 2 + 2, 7 * Block + 1));
            Panels.Add(Pnl.Table.LowerTaken, new Panel(Height - 14, 6 * Block + 2, Height, 7 * Block + 1, (char)Symbol.BlockLower));
            Panels.Add(Pnl.Table.UpperRight, new Panel(1, 7 * Block + 3, 17, 13 * Block + 2, (char)Symbol.BlockUpper));
            Panels.Add(Pnl.Table.MiddleRight, new Panel(18, 7 * Block + 3, Height - 17, 13 * Block + 2));
            Panels.Add(Pnl.Table.LowerRight, new Panel(Height - 16, 7 * Block + 3, Height, 13 * Block + 2, (char)Symbol.BlockLower));
            Panels.Add(Pnl.Table.UpperOut, new Panel(1, 13 * Block + 4, 16, 14 * Block + 3, (char)Symbol.BlockUpper));
            Panels.Add(Pnl.Table.LowerOut, new Panel(Height - 15, 13 * Block + 4, Height, 14 * Block + 3, (char)Symbol.BlockLower));

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
            textLeft += ",";
            textRight += ",";
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
            Level = Serializer.MasterLevel; // for frame multiplier
            _board = new TableBoard(Panels);
            _cursor = null;
            _pickedFrom = null;

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

        private void CursorMove(bool left)
        {
            if (_useFreeMove)
            {
                if (!_cursor.HasValue)
                    _cursor = left ? 23 : 0;
                else
                {
                    _board[_isWhite, _cursor.Value].SetCursor(false);
                    if (_cursor < 24)
                    {
                        if (_cursor.Value < 12)
                            _cursor += left ? 1 : -1;
                        else if (_cursor.Value < 24)
                            _cursor += left ? -1 : 1;
                        if (_cursor < 0)
                            _cursor = 26 + (_isWhite ? 0 : 1);
                        if (_cursor >= 24)
                        {
                            _cursor = 24 + (_isWhite ? 0 : 1);
                        }
                    }
                    else if (_cursor < 26)
                        _cursor = left ? 23 : 0;
                    else
                        _cursor = left ? 0 : 23;
                }

                _board[_isWhite, _cursor.Value].SetCursor(true);
            }
            else
            {
                if (_cursor.HasValue)
                    _board[_isWhite, _cursor.Value].SetCursor(false);
                if (_allowed.Count > 0)
                {
                    int i = _allowed.IndexOf(_cursor.Value);
                    if (!_cursor.HasValue || i < 0)
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

                    _board[_isWhite, _cursor.Value].SetCursor(true);
                }
            }
        }

        private void CursorAction()
        {
        }

        public override void TogglePause()
        {
            var pnl = Panels[Pnl.Table.UpperLeft];
            pnl.SwitchMode();
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
                    if (!_useFreeMove && _cursor.HasValue)
                    {
                        _board[_isWhite, _cursor.Value].SetCursor(false);
                        _cursor = null;
                    }
                    break;
                case "M":
                    _useMarking = !_useMarking;
                    if (_useMarking)
                    {
                        _board.ClearHighlight();
                        if (_cursor.HasValue)
                            _board[_isWhite, _cursor.Value].SetCursor(true);
                    }
                    break;
                case "N":
                    _useNumbers = !_useNumbers;
                    Panels[Pnl.Table.MiddleLeft].SwitchMode();
                    Panels[Pnl.Table.MiddleRight].SwitchMode();
                    break;
                case "B":
                    _useBackground = !_useBackground;
                    _board.SetBackground(_useBackground);
                    break;
                case UIKey.LeftArrow:
                case UIKey.RightArrow:
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
        }
    }
}
