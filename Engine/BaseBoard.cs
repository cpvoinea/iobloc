using System;
using System.Collections.Generic;

namespace iobloc
{
    abstract class BaseBoard : IBoard
    {
        BoardType _type;
        protected readonly Dictionary<string, string> _settings;
        readonly UIBorder _border;
        readonly Dictionary<string, UIPanel> _panels;
        protected UIPanel _main;
        readonly string[] _help;
        readonly string[] _keys;
        protected readonly int _width;
        protected readonly int _height;
        int _frameMultiplier;
        int _levelThreshold;
        int _score;
        int _level;
        bool _isRunning;

        public UIBorder UIBorder { get { return _border; } }
        public Dictionary<string, UIPanel> Panels { get { return _panels; } }
        public UIPanel Main { get { return _main; } }
        public string[] Help { get { return _help; } }
        public int FrameInterval { get; private set; }
        public int? Highscore { get; private set; }
        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                if (Highscore.HasValue && _score > Highscore.Value)
                {
                    Highscore = _score;
                    Serializer.UpdateHighscore((int)_type, _score);
                }
                if (_levelThreshold > 0 && _score >= _levelThreshold * (Level + 1))
                {
                    Level++;
                    if (Level > 15)
                    {
                        Win = true;
                        if (_type != BoardType.Sokoban || Level >= SokobanLevels.Count)
                            IsRunning = false;
                    }
                }
            }
        }
        public int Level
        {
            get { return _level; }
            protected set
            {
                _level = value;
                FrameInterval = Serializer.GetLevelInterval(_frameMultiplier, _level);
            }
        }
        public bool? Win { get; protected set; }
        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                _isRunning = value;
                if (!_isRunning)
                {
                    if (Win == true)
                        Next = new AnimationBoard(BoardType.Fireworks);
                    else if (Win == false)
                        Next = new AnimationBoard(BoardType.RainingBlood);
                    else
                        Next = new MenuBoard();
                }
            }
        }
        public IBoard Next { get; set; }

        protected internal BaseBoard(BoardType type)
        {
            _type = type;
            int key = (int)type;
            _settings = Serializer.Settings[key];

            _frameMultiplier = _settings.GetInt("FrameMultiplier", 0);
            _levelThreshold = _settings.GetInt("LevelThreshold", 0);
            _help = _settings.GetList("Help");
            _keys = _settings.GetList("Keys");
            _width = _settings.GetInt("Width", 10);
            _height = _settings.GetInt("Height", 10);

            _border = new UIBorder(_width + 2, _height + 2);
            _main = new UIPanel(1, 1, _height, _width);
            _panels = new Dictionary<string, UIPanel> { { "main", _main } };

            if (Serializer.Highscores.ContainsKey(key))
                Highscore = Serializer.Highscores[key];
            Level = Serializer.Level;
        }

        public bool IsValidInput(string key) { return Array.Exists(_keys, x => x == key); }
        protected virtual void InitializeGrid() { }
        protected virtual void ChangeGrid(bool set) { }
        public virtual void NextFrame() { }
        public abstract void HandleInput(string key);
    }
}
