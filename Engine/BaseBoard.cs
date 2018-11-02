using System.Collections.Generic;

namespace iobloc
{
    abstract class BaseBoard : IBaseBoard
    {
        private BoardType Type { get; set; }
        private string[] Keys { get; set; }
        private int FrameMultiplier { get; set; }
        private int LevelThreshold { get; set; }
        private bool _isInitialized;
        private int? _highscore;
        private int _score;
        private int _level;

        protected int ID => (int)Type;
        protected Dictionary<string, string> Settings { get; private set; }
        protected string[] Help { get; private set; }
        protected int Width { get; private set; }
        protected int Height { get; private set; }
        protected UIPanel Main { get; private set; }

        public UIBorder Border { get; private set; }
        public Dictionary<string, UIPanel> Panels { get; private set; }
        public int FrameInterval { get; private set; }
        public bool IsRunning { get; private set; }
        public int Score { get { return _score; } set { SetScore(value); } }
        public int Level { get { return _level; } set { SetLevel(value); } }
        public IBaseBoard Next { get; protected set; }

        protected BaseBoard(BoardType type)
        {
            Type = type;
            InitializeSettings();
            InitializeUI();
            Initialize();

            _isInitialized = true;
            Reset();
        }

        private void InitializeSettings()
        {
            Settings = Serializer.Settings[ID];

            Width = Settings.GetInt("Width", 10);
            Height = Settings.GetInt("Height", 10);
            Help = Settings.GetList("Help");
            Keys = Settings.GetList("Keys");
            FrameMultiplier = Settings.GetInt("FrameMultiplier", 0);
            LevelThreshold = Settings.GetInt("LevelThreshold", 0);
        }

        private void SetScore(int score)
        {
            if (score != 0 && score == _score)
                return;
            _score = score;

            if (Panels.ContainsKey(Pnl.Score))
            {
                Panels[Pnl.Score].Text[0] = string.Format($"{score,3}");
                Panels[Pnl.Score].HasChanges = true;
            }

            if (score > _highscore.Value)
            {
                _highscore = score;
                Serializer.UpdateHighscore(ID, score);
            }

            if (Panels.ContainsKey(Pnl.Highscore))
            {
                Panels[Pnl.Highscore].Text[0] = string.Format($"{score,3}");
                Panels[Pnl.Highscore].HasChanges = true;
            }

            if (LevelThreshold > 0 && score >= LevelThreshold * (_level + 1))
                SetLevel(_level + 1);
        }

        private void SetLevel(int level)
        {
            if (level > 0 && level == _level)
                return;

            if (level == 16)
                Next = Serializer.GetBoard(BoardType.Fireworks);
            if (level < 16 || Type == BoardType.Sokoban && level < SokobanLevels.Count)
            {
                _level = level;
                if (level < 16)
                    FrameInterval = Serializer.GetLevelInterval(FrameMultiplier, _level);

                if (Panels.ContainsKey(Pnl.Level))
                {
                    Panels[Pnl.Level].Text[0] = string.Format($"L{_level,2}");
                    Panels[Pnl.Level].HasChanges = true;
                }
            }
        }

        public virtual void InitializeUI()
        {
            Border = new UIBorder(Width + 2, Height + 2);

            Main = new UIPanel(1, 1, Height, Width);
            Main.Text = Help;
            var pnlLevel = new UIPanel(Border.Height - 1, (Border.Width + 1) / 2 - 2, Border.Height - 1, (Border.Width + 1) / 2, 1);

            if (Type != BoardType.Fireworks && Type != BoardType.RainingBlood)
                Panels = new Dictionary<string, UIPanel> { { Pnl.Main, Main }, { Pnl.Level, pnlLevel } };
            if (Serializer.Highscores.ContainsKey(ID))
            {
                if (Border.Width > 8)
                    Panels.Add(Pnl.Highscore, new UIPanel(0, 1, 0, 3, 1));
                Panels.Add(Pnl.Score, new UIPanel(0, Border.Width - 4, 0, Border.Width - 2, 1));
            }
        }

        public virtual void Initialize()
        {
            SetLevel(Serializer.MasterLevel);
            if (Serializer.Highscores.ContainsKey(ID))
            {
                _highscore = Serializer.Highscores[ID];
                SetScore(0);
            }
        }

        public virtual void Reset()
        {
            Paint();
        }

        public virtual void Change(bool set)
        {
            if (set)
                Paint();
        }

        public virtual void Paint()
        {
            foreach (var p in Panels.Values)
                p.HasChanges = true;
        }

        public void Start()
        {
            if (_isInitialized)
                Paint();
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
