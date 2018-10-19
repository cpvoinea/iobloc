namespace iobloc
{
    class AnimationBoard : IBoard
    {
        int _code;
        Border _border;
        protected Panel _main;
        readonly Panel[] _panels;
        int[][,] _animation;
        int _currentFrame;
        public Border Border => _border;
        public Panel MainPanel => _main;
        public Panel[] Panels => _panels;
        public string[] Help => new[] { "WINNER!" };
        public int FrameInterval => 50;
        public int? Highscore => null;
        public int Score => 0;
        public int Level => Config.Level;
        public bool? Win => true;
        public bool IsRunning { get; set; }

        internal AnimationBoard(int animationCode, Border border)
        {
            _code = animationCode;
            _border = border;
            _main = new Panel(1, 1, border.Height - 2, border.Width - 2);
            _panels = new[] { _main };
            _animation = Animation.Get(animationCode, _main.Height, _main.Width);
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