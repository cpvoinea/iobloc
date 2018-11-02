namespace iobloc
{
    class AnimationBoard : BaseBoard
    {
        private int[][,] _animation;
        private int _currentFrame;

        public AnimationBoard(BoardType type) : base(type) { }

        public override void Initialize()
        {
            _animation = Animations.Get(Type);
            _currentFrame = 0;
        }

        public override void HandleInput(string key) { }

        public override void NextFrame()
        {
            var a = _animation[_currentFrame++];
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    Main[i, j] = a[i, j];
            if (_currentFrame >= _animation.Length)
                _currentFrame = 0;
            Main.HasChanges = true;
        }
    }
}
