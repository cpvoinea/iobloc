using System.Collections.Generic;

namespace iobloc
{
    class AnimationBoard : IBoard
    {
        AnimationType _type;
        Border _border;
        protected Panel _main;
        readonly Dictionary<string, Panel> _panels;
        readonly int[][,] _animation;
        readonly string _message;
        int _currentFrame;
        public Border Border => _border;
        public Panel MainPanel => _main;
        public Dictionary<string, Panel> Panels => _panels;
        public string[] Help => new[] { "", "", "", _message };
        public int FrameInterval => 200;
        public int? Highscore => null;
        public int Score => 0;
        public int Level => Config.Level;
        public bool? Win => null;
        public bool IsRunning { get; set; }

        internal AnimationBoard(AnimationType type, string message)
        {
            _type = type;
            _message = message;
            _border = new Border(Animation.SIZE + 2, Animation.SIZE + 2);
            _main = new Panel(1, 1, Animation.SIZE, Animation.SIZE);
            _panels = new Dictionary<string, Panel> { { "main", _main } };
            _animation = Animation.Get((int)type);
        }

        public bool IsValidInput(string key) { return false; }
        public void HandleInput(string key) { }
        public void NextFrame()
        {
            _main.Grid = _animation[_currentFrame++];
            if (_currentFrame >= _animation.Length)
                _currentFrame = 0;
            _main.HasChanges = true;
        }
    }
}