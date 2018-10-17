using System;
using System.Collections.Generic;

namespace iobloc
{
    abstract class SinglePanelBoard : IBoard
    {
        protected readonly Dictionary<string, string> _settings;
        readonly string[] _help;
        readonly string[] _keys;
        protected readonly int _width;
        protected readonly int _height;
        readonly Border _border;
        protected Panel _main;
        readonly Panel[] _panels;

        public Border Border { get { return _border; } }
        public Panel MainPanel { get { return _main; } }
        public Panel[] Panels { get { return _panels; } }
        public string[] Help { get { return _help; } }
        public int FrameInterval { get; private set; }
        public bool IsRunning { get; set; }

        public virtual int Score { get; set; }
        public virtual bool Won { get { return false; } }

        protected internal SinglePanelBoard(Option option)
        {
            _settings = Config.Settings(option);
            _help = _settings.GetList("Help");
            _keys = _settings.GetList("Keys");
            _width = _settings.GetInt("Width", 10);
            _height = _settings.GetInt("Height", 10);
            _border = new Border(_width + 2, _height + 2);
            _main = new Panel(1, 1, _height, _width);
            _panels = new[] { _main };

            FrameInterval = _settings.GetInt("FrameMultiplier", 1) * Config.LevelInterval;
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
