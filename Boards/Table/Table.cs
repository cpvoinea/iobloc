using System;
using System.Collections.Generic;

namespace iobloc
{
    class Table : BaseGame
    {
        internal static int BW, B, CP, CE, CN, CD, CL, CH;
        private const int SF = 3;
        private readonly Random _random = new Random();
        private bool _skipFrame;
        private bool _useHighlight;
        private bool _useCursor;
        private TableBoard _board;
        private readonly List<int> _dice = new List<int>();
        private bool _isWhite;
        private int? _cursor;
        private int? _pickedFrom;
        private ITableAI _player1;
        private ITableAI _player2;
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
            Panels[Pnl.Table.MiddleLeft].SetText(textLeft);
            Panels[Pnl.Table.MiddleRight].SetText(textRight);
        }

        protected override void Initialize()
        {
            Level = Serializer.MasterLevel; // for frame multiplier
            _board = new TableBoard(Panels);
            _cursor = null;
            _pickedFrom = null;
            _skipFrame = true;
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

        public override void TogglePause()
        {
            var pnl = Panels[Pnl.Table.UpperLeft];
            pnl.SwitchMode();
        }

        public override void HandleInput(string key)
        {
            if (key == "R")
                Initialize();
            else if (!IsCurrentPlayerAI)
                switch (key)
                {
                    // case UIKey.LeftArrow: CursorMove(true); break;
                    // case UIKey.RightArrow: CursorMove(false); break;
                    // case UIKey.UpArrow: CursorAction(); break;
                }
        }

        public override void NextFrame()
        {
            // if (State == GameState.Ended)
            //     Win(true);
            // else if (CurrentPlayerIsAI)
            // {
            //     if (_skipFrame)
            //         _skipFrame = false;
            //     else
            //     {
            //         PlayerAction();
            //         _skipFrame = true;
            //     }
            // }
        }
    }
}
