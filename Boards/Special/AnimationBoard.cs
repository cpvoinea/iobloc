namespace iobloc
{
    // The simplest board, has no input, just iterates frames and exists to menu
    class AnimationBoard : BaseBoard
    {
        // all frames
        private readonly int[][,] _animation;
        // iterate animation frames
        private int _currentFrame;

        // Summary:
        //      Initialize frames from resources
        // Param: type: type of animation
        public AnimationBoard(BoardType type) : base(type)
        {
            _animation = Animations.Get(type);
        }

        // Summary:
        //      Overwrite Initialize to simplify, only FrameInterval needs to be set from constant level
        protected override void Initialize()
        {
            Level = 0; // for FrameInterval value, so that animation has consistent speed
        }

        // Summary:
        //      Not used
        public override void HandleInput(string key) { }

        // Summary:
        //      Copy next frame to grid, repeat from 0 when end is reached
        public override void NextFrame()
        {
            var a = _animation[_currentFrame++];
            for (int i = 0; i < Height && i < Animations.SIZE; i++)
                for (int j = 0; j < Width && j < Animations.SIZE; j++)
                    Main[i, j] = a[i, j];
            if (_currentFrame >= _animation.Length)
                _currentFrame = 0;
            base.Change(true);
        }
    }
}
