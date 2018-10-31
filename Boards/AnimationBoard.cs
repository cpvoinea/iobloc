namespace iobloc
{
    class AnimationBoard : BaseBoard
    {
        int[][,] _animation;
        int _currentFrame;

        internal AnimationBoard(BoardType type) : base(type) { }

        protected override void InitializeGrid()
        {
            _animation = Animations.Get(Type);
            _currentFrame = 0;

            base.InitializeGrid();
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
