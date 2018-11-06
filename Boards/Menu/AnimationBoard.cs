namespace iobloc
{
    /// <summary>
    /// The simplest board, has no input, just iterates frames and exists to menu
    /// </summary>
    class AnimationBoard : BaseBoard
    {
        // all frames
        private readonly int[][,] _animation;
        // iterate animation frames
        private int _currentFrame;

        /// <summary>
        /// Initialize frames from resources
        /// </summary>
        /// <param name="type">type of animation</param>
        /// <returns></returns>
        public AnimationBoard(BoardType type) : base(type)
        {
            _animation = Animations.Get(type);
        }

        /// <summary>
        /// Overwrite Initialize to simplify, only FrameInterval needs to be set from constant level
        /// </summary>
        protected override void Initialize()
        {
            Level = 0; // for FrameInterval value, so that animation has consistent speed
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="key"></param>
        public override void HandleInput(string key) { }

        /// <summary>
        /// Copy next frame to grid, repeat from 0 when end is reached
        /// </summary>
        public override void NextFrame()
        {
            var a = _animation[_currentFrame++];
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    Main[i, j] = a[i, j];
            if (_currentFrame >= _animation.Length)
                _currentFrame = 0;
            Main.Change(true);
        }
    }
}
