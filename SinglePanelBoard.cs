using System;
using System.Collections.Generic;

namespace iobloc
{
    abstract class SinglePanelBoard : IBoard
    {
        protected readonly Dictionary<string, string> _config;
        readonly Border _border;
        protected Panel _mainPanel;
        readonly Panel[] _panels;
        readonly string[] _help;
        readonly int[] _keys;
        protected readonly int _width;
        protected readonly int _height;

        public Border Border { get { return _border; } }
        public Panel MainPanel { get { return _mainPanel; } }
        public Panel[] Panels { get { return _panels; } }
        public string[] Help { get { return _help; } }
        public bool IsRunning { get; set; }
        public int FrameInterval { get; set; }

        public virtual int Score { get; set; }
        public virtual bool Won { get; }

        protected internal SinglePanelBoard(Option option)
        {
            _config = Config.BoardConfig(option);
            _help = _config.GetList("Help");
            var keyNames = _config.GetList("Keys");
            List<int> keyValues = new List<int>();
            foreach (string k in keyNames)
            {
                if (Enum.IsDefined(typeof(ConsoleKey), k))
                    keyValues.Add((int)Enum.Parse(typeof(ConsoleKey), k));
            }
            _keys = keyValues.ToArray();
            FrameInterval = _config.GetInt("FrameMultiplier", 1) * Config.LevelInterval;

            _width = _config.GetInt("Width", 10);
            _height = _config.GetInt("Height", 10);
            _border = new Border(_width + 2, _height + 2);
            _mainPanel = new Panel(1, 1, _height + 1, _width + 1);
            _panels = new[] { _mainPanel };
        }

        public bool IsValidInput(int key)
        {
            return Array.Exists(_keys, x => x == key);
        }

        public abstract void HandleInput(int key);

        public abstract void NextFrame();

        public void Dispose()
        {
        }
    }
}
