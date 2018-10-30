using System.Collections.Generic;

namespace iobloc
{
    class AnimationBoard : IBoard
    {
        BoardType _type;
        UIBorder _border;
        protected UIPanel _main;
        readonly Dictionary<string, UIPanel> _panels;
        readonly int[][,] _animation;
        readonly string _message;
        int _currentFrame;
        public UIBorder UIBorder => _border;
        public UIPanel Main => _main;
        public Dictionary<string, UIPanel> Panels => _panels;
        public string[] Help => new[] { "", "", "", _message };
        public int FrameInterval => 200;
        public int? Highscore => null;
        public int Score => 0;
        public int Level => Serializer.Level;
        public bool? Win => null;
        public bool IsRunning { get; set; }

        internal AnimationBoard(BoardType type, string message)
        {
            _type = type;
            _message = message;
            _border = new UIBorder(Animation.SIZE + 2, Animation.SIZE + 2);
            _main = new UIPanel(1, 1, Animation.SIZE, Animation.SIZE);
            _panels = new Dictionary<string, UIPanel> { { "main", _main } };
            _animation = Animation.Get((int)type);
        }

        public bool IsValidInput(string key) { return false; }
        public void HandleInput(string key) { }
        public void NextFrame()
        {
            var a = _animation[_currentFrame++];
            for (int i = 0; i < Animation.SIZE; i++)
                for (int j = 0; j < Animation.SIZE; j++)
                    _main[i, j] = a[i, j];
            if (_currentFrame >= _animation.Length)
                _currentFrame = 0;
            _main.HasChanges = true;
        }
    }
}
