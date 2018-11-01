using System;
using System.Collections.Generic;

namespace iobloc
{
    abstract class BaseBoard : IBoard
    {
        private BoardType Type { get; set; }
        private string[] Keys { get; set; }
        private int FrameMultiplier { get; set; }
        private int LevelThreshold { get; set; }
        private int? _highscore;
        private int _score = int.MinValue;
        private int _level = int.MinValue;

        protected int ID => (int)Type;
        protected Dictionary<string, string> Settings { get; private set; }
        protected string[] Help { get; private set; }
        protected int Width { get; private set; }
        protected int Height { get; private set; }
        protected UIPanel Main { get; private set; }
        protected int Score { get { return _score; } set { SetScore(value); } }
        protected int Level { get { return _level; } set { SetLevel(value); } }

        public UIBorder Border { get; private set; }
        public Dictionary<string, UIPanel> Panels { get; private set; }
        public int FrameInterval { get; private set; }
        public bool IsRunning { get; private set; }
        public IBoard Next { get; protected set; }

        protected BaseBoard(BoardType type)
        {
            Type = type;
            InitializeSettings();
            InitializeUI();
            InitializeStats();
            InitializeMain();
        }

        private void SetScore(int score)
        {
            if (score == _score)
                return;
            _score = score;

            Panels[Pnl.Score].Text[0] = string.Format($"{score,3}");
            Panels[Pnl.Score].HasChanges = true;

            if (score > _highscore.Value)
            {
                _highscore = score;
                if (Panels.ContainsKey(Pnl.Highscore))
                {
                    Panels[Pnl.Highscore].Text[0] = string.Format($"{score,3}");
                    Panels[Pnl.Highscore].HasChanges = true;
                }
                Serializer.UpdateHighscore(ID, score);
            }

            if (LevelThreshold > 0 && _level >= 0 && score >= LevelThreshold * (_level + 1))
                SetLevel(_level + 1);
        }

        private void SetLevel(int level)
        {
            if (level == _level)
                return;

            if (level == 16)
                Next = Serializer.GetBoard(BoardType.Fireworks);
            if (level < 16 || Type == BoardType.Sokoban && level < SokobanLevels.Count)
                _level = level;

            Panels[Pnl.Level].Text[0] = string.Format($"L{_level,2}");
            Panels[Pnl.Level].HasChanges = true;
        }

        private void InitializeSettings()
        {
            Settings = Serializer.Settings[ID];

            Width = Settings.GetInt("Width", 10);
            Height = Settings.GetInt("Height", 10);
            Help = Settings.GetList("Help");
            Keys = Settings.GetList("Keys");
            FrameMultiplier = Settings.GetInt("FrameMultiplier", 0);
            FrameInterval = Serializer.GetLevelInterval(FrameMultiplier);
            LevelThreshold = Settings.GetInt("LevelThreshold", 0);
        }

        private void InitializeUI()
        {
            Border = new UIBorder(Width + 2, Height + 2);

            Main = new UIPanel(1, 1, Height, Width);
            Main.Text = Help;
            var pnlLevel = new UIPanel(Border.Height - 1, (Border.Width + 1) / 2 - 2, Border.Height - 1, (Border.Width + 1) / 2, 1);

            Panels = new Dictionary<string, UIPanel> { { Pnl.Main, Main }, { Pnl.Level, pnlLevel } };
            if (Serializer.Highscores.ContainsKey(ID))
            {
                if (Border.Width > 8)
                    Panels.Add(Pnl.Highscore, new UIPanel(0, 1, 0, 3, 1));
                Panels.Add(Pnl.Score, new UIPanel(0, Border.Width - 4, 0, Border.Width - 2, 1));
            }
        }

        private void InitializeStats()
        {
            if (Serializer.Highscores.ContainsKey(ID))
            {
                _highscore = Serializer.Highscores[ID];
                SetScore(0);
            }
            SetLevel(Serializer.Level);
        }

        protected virtual void InitializeMain() { }
        protected virtual void Refresh()
        {
            foreach (var p in Panels.Values)
                p.HasChanges = true;
        }
        protected virtual void ChangeMain(bool set) { }
        protected virtual void Restart() { }

        private bool _isInitialized;
        public void Start()
        {
            if (!_isInitialized)
                _isInitialized = true;
            else
                Refresh();

            IsRunning = true;
        }

        public void Stop()
        {
            if (Next == null && Type != BoardType.Menu)
                Next = Serializer.GetBoard(BoardType.Menu);
            IsRunning = false;
        }

        public virtual void TogglePause()
        {
            Main.IsText = !Main.IsText;
            Main.HasChanges = true;
        }

        public virtual void NextFrame() { }

        public bool IsValidInput(string key)
        {
            foreach (string k in Keys)
                if (k == key)
                    return true;
            return false;
        }

        public abstract void HandleInput(string key);
    }
}
