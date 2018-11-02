using System.Collections.Generic;

namespace iobloc
{
    abstract class BaseBoard : IBaseBoard
    {
        private int ID => (int)Type;
        private BoardType Type { get; set; }
        private double FrameMultiplier { get; set; }
        private int LevelThreshold { get; set; }
        private int? Highscore { get; set; }
        private int _score;
        private int _level;

        protected Dictionary<string, string> BoardSettings { get; private set; }
        protected int Width { get; private set; }
        protected int Height { get; private set; }
        protected UIPanel Main { get; private set; }
        protected string[] Help { get; private set; }
        protected bool IsInitialized { get; private set; }

        public UIBorder Border { get; private set; }
        public Dictionary<string, UIPanel> Panels { get; private set; }
        public int FrameInterval { get; private set; }
        public bool IsRunning { get; private set; }
        public string[] AllowedKeys { get; protected set; }
        public IBaseBoard Next { get; protected set; }
        protected int Score { get { return _score; } set { SetScore(value); } }
        protected int Level { get { return _level; } set { SetLevel(value); } }

        protected BaseBoard(BoardType type)
        {
            Type = type;
            InitializeSettings();
            InitializeUI();
            Initialize();
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
            SetHighscore(score);

            if (LevelThreshold > 0 && score >= LevelThreshold * (_level + 1))
                SetLevel(_level + 1);
        }

        private void SetHighscore(int score)
        {
            if (score < Highscore)
                return;
            Highscore = score;

            if (Panels.ContainsKey(Pnl.Highscore))
            {
                Panels[Pnl.Highscore].Text[0] = string.Format($"{Highscore,3}");
                Panels[Pnl.Highscore].HasChanges = true;
            }
            Serializer.UpdateHighscore(ID, score);
        }

        protected void SetLevel(int level)
        {
            if (level > 0 && level == _level)
                return;

            if (level == 16)
                Win();
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

        protected virtual void InitializeSettings()
        {
            BoardSettings = Serializer.Settings[ID];
            AllowedKeys = BoardSettings.GetList(Settings.AllowedKeys);
            Help = BoardSettings.GetList(Settings.Help);
            Width = BoardSettings.GetInt(Settings.Width, 10);
            Height = BoardSettings.GetInt(Settings.Height, 10);
            FrameMultiplier = BoardSettings.GetReal(Settings.FrameMultiplier);
            LevelThreshold = BoardSettings.GetInt(Settings.LevelThreshold, 0);
        }

        protected virtual void InitializeUI()
        {
            Border = new UIBorder(Width + 2, Height + 2);

            Main = new UIPanel(1, 1, Height, Width);
            Main.Text = Help;
            Panels = new Dictionary<string, UIPanel> { { Pnl.Main, Main } };

            if (Type != BoardType.Fireworks && Type != BoardType.RainingBlood)
                Panels.Add(Pnl.Level, new UIPanel(Border.Height - 1, (Border.Width + 1) / 2 - 2, Border.Height - 1, (Border.Width + 1) / 2, 1));
            if (Serializer.Highscores.ContainsKey(ID))
            {
                if (Border.Width > 8)
                    Panels.Add(Pnl.Highscore, new UIPanel(0, 1, 0, 3, 1));
                Panels.Add(Pnl.Score, new UIPanel(0, Border.Width - 4, 0, Border.Width - 2, 1));
            }
        }

        protected virtual void Initialize()
        {
            IsInitialized = true;
            SetLevel(Settings.MasterLevel);
            if (Serializer.Highscores.ContainsKey(ID))
            {
                SetScore(0);
                SetHighscore(Serializer.Highscores[ID]);
            }
        }

        protected virtual void Change(bool set)
        {
            if (set)
                Main.HasChanges = true;
        }

        protected void Win()
        {
            Next = Serializer.GetBoard((int)BoardType.Fireworks);
        }

        protected void Lose(bool exit = true)
        {
            Next = Serializer.GetBoard((int)BoardType.RainingBlood);
            Initialize();
            if (exit)
                Stop();
        }

        public virtual void Start()
        {
            Next = null;
            IsRunning = true;
            foreach (var p in Panels.Values)
                p.HasChanges = true;
        }

        public void Stop()
        {
            if (Next == null && Type != BoardType.Menu)
                Next = Serializer.GetBoard((int)BoardType.Menu);
            IsRunning = false;
        }

        public virtual void TogglePause()
        {
            Main.IsText = !Main.IsText;
            Main.HasChanges = true;
        }

        public virtual void NextFrame() { }

        public abstract void HandleInput(string key);
    }
}
