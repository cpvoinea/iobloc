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
        public int FrameInterval => 250;
        public int? Highscore => null;
        public int Score => 0;
        public int Level => Config.Level;
        public bool? Win => null;
        public bool IsRunning { get; set; }

        internal AnimationBoard(int animationCode)
        {
            _code = animationCode;
            _border = new Border(Animation.SIZE + 2, Animation.SIZE + 2);
            _main = new Panel(1, 1, Animation.SIZE, Animation.SIZE);
            _panels = new[] { _main };
            _animation = Animation.Get(animationCode);
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