using System;
using System.Collections.Generic;

namespace iobloc
{
    abstract class BaseBoard : IBoard
    {
        protected readonly BoardType Type;
        protected readonly Dictionary<string, string> Settings;
        protected readonly int Width;
        protected readonly int Height;

        readonly UIBorder _border;
        readonly Dictionary<string, UIPanel> _panels;
        UIPanel _main;
        readonly string[] _help;
        readonly string[] _keys;
        readonly int _frameMultiplier;
        readonly int _levelThreshold;

        int _score = int.MinValue;
        int? _highscore = int.MinValue;
        int _level = int.MinValue;
        bool _isRunning;

        public UIBorder Border { get { return _border; } }
        public Dictionary<string, UIPanel> Panels { get { return _panels; } }
        public UIPanel Main { get { return _main; } }
        public string[] Help { get { return _help; } }
        public int FrameInterval { get; private set; }
        public IBoard Next { get; set; }

        public int Level
        {
            get { return _level; }
            protected set
            {
                FrameInterval = Serializer.GetLevelInterval(_frameMultiplier, value);
                if (value == _level)
                    return;

                _level = value;
                if (_panels.ContainsKey(Pnl.Level))
                    _panels[Pnl.Level].SetText(string.Format($"L{_level,2}"));
            }
        }

        public int? Highscore
        {
            get { return _highscore; }
            private set
            {
                if (value == _highscore)
                    return;

                _highscore = value;
                if (_highscore.HasValue && _panels.ContainsKey(Pnl.Highscore))
                    _panels[Pnl.Highscore].SetText(string.Format($"{_highscore.Value,3}"));
            }
        }

        public int Score
        {
            get { return _score; }
            set
            {
                if (value == _score)
                    return;

                _score = value;
                if (_panels.ContainsKey(Pnl.Score))
                    _panels[Pnl.Score].SetText(string.Format($"{_score,3}"));

                if (_score == 0)
                    return;

                if (Highscore.HasValue && _score > Highscore.Value)
                {
                    Highscore = _score;
                    Serializer.UpdateHighscore(Type, _score);
                }

                if (_levelThreshold > 0 && _score >= _levelThreshold * (_level + 1))
                    NextLevel();
            }
        }

        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                if (value == _isRunning)
                    return;

                _isRunning = value;
                if (_isRunning)
                    Refresh();
                else if (Next == null && Type != BoardType.Menu)
                    Next = Serializer.GetBoard(BoardType.Menu);
            }
        }

        protected internal BaseBoard(BoardType type)
        {
            Type = type;
            int key = (int)type;
            Settings = Serializer.Settings[key];
            Width = Settings.GetInt("Width", 10);
            Height = Settings.GetInt("Height", 10);

            _frameMultiplier = Settings.GetInt("FrameMultiplier", 0);
            _levelThreshold = Settings.GetInt("LevelThreshold", 0);
            _help = Settings.GetList("Help");
            _keys = Settings.GetList("Keys");

            _border = new UIBorder(Width + 2, Height + 2);
            _main = new UIPanel(1, 1, Height, Width);
            _panels = new Dictionary<string, UIPanel> { { Pnl.Main, _main } };
            _panels.Add(Pnl.Level, new UIPanel(_border.Height - 1, (_border.Width + 1) / 2 - 2, _border.Height - 1, (_border.Width + 1) / 2));
            if (Serializer.Highscores.ContainsKey(key))
            {
                if (_border.Width > 8)
                    _panels.Add(Pnl.Highscore, new UIPanel(0, 1, 0, 3));
                _panels.Add(Pnl.Score, new UIPanel(0, _border.Width - 4, 0, _border.Width - 2));
            }

            Initialize();
        }

        protected virtual void Initialize()
        {
            int key = (int)Type;
            if (Serializer.Highscores.ContainsKey(key))
            {
                Highscore = Serializer.Highscores[key];
                Score = 0;
            }
            Level = Serializer.Level;
        }

        protected virtual void Refresh()
        {
            foreach (var p in _panels.Values)
                p.HasChanges = true;
        }

        protected virtual void NextLevel()
        {
            if (Level == 15)
                Next = Serializer.GetBoard(BoardType.Fireworks);
            if (Level < 15 || Type == BoardType.Sokoban && Level < SokobanLevels.Count - 1)
                Level++;
        }


        protected virtual void Lose()
        {
            Next = Serializer.GetBoard(BoardType.RainingBlood);
            Clear();
            Initialize();
        }

        protected void Clear()
        {
            foreach (var p in _panels.Values)
                if (!p.IsText)
                    p.Clear();
        }

        protected virtual void Restart() { }
        protected virtual void Change(bool set) { }

        public virtual void NextFrame() { }

        public bool IsValidInput(string key)
        {
            foreach (string k in _keys)
                if (k == key)
                    return true;
            return false;
        }

        public abstract void HandleInput(string key);
    }
}
