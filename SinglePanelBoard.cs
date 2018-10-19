using System;
using System.Collections.Generic;

namespace iobloc
{
    abstract class SinglePanelBoard : IBoard
    {
        Option _option;
        protected readonly Dictionary<string, string> _settings;
        readonly string[] _help;
        readonly string[] _keys;
        protected readonly int _width;
        protected readonly int _height;
        readonly Border _border;
        protected Panel _main;
        readonly Panel[] _panels;
        int _frameMultiplier;
        int _levelThreshold;
        int _score;
        int _level;

        public Border Border { get { return _border; } }
        public Panel MainPanel { get { return _main; } }
        public Panel[] Panels { get { return _panels; } }
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
                    Config.UpdateHighscore(_option, _score);
                }
                if(_levelThreshold > 0 && _score >= _levelThreshold * (Level + 1))
                    Level++;
            }
        }
        public int Level
        {
            get { return _level; }
            protected set
            {
                _level = value;
                FrameInterval = Config.LevelInterval(_frameMultiplier, _level);
            }
        }
        public bool? Win { get; protected set; }
        public bool IsRunning { get; set; }

        protected internal SinglePanelBoard(Option option)
        {
            _option = option;
            _settings = Config.Settings(option);
            _frameMultiplier = _settings.GetInt("FrameMultiplier", 1);
            _levelThreshold = _settings.GetInt("LevelThreshold", 0);
            _help = _settings.GetList("Help");
            _keys = _settings.GetList("Keys");
            _width = _settings.GetInt("Width", 10);
            _height = _settings.GetInt("Height", 10);
            _border = new Border(_width + 2, _height + 2);
            _main = new Panel(1, 1, _height, _width);
            _panels = new[] { _main };

            Highscore = Config.GetHighscore(option);
            Level = Config.Level;
        }

        public bool IsValidInput(string key)
        {
            return Array.Exists(_keys, x => x == key);
        }

        protected virtual void InitializeGrid()
        {
        }

        protected virtual void ChangeGrid(bool set)
        {
        }

        public abstract void HandleInput(string key);

        public abstract void NextFrame();

        public virtual void Dispose()
        {
        }
    }
}
