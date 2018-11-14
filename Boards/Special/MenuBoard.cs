namespace iobloc
{
    /// <summary>
    /// Display list of available boards to link through Next
    /// </summary>
    class MenuBoard : BaseBoard
    {
        /// <summary>
        /// A text board with each line item having its own shortcut key(s)
        /// </summary>
        /// <returns></returns>
        public MenuBoard() : base(BoardType.Menu) { }

        /// <summary>
        /// Initialize in text mode
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            Main.SwitchMode();
        }

        /// <summary>
        /// Overriden to display correct MasterLevel value
        /// </summary>
        public override void Start()
        {
            Level = Settings.MasterLevel;
            base.Start();
        }

        /// <summary>
        /// Link to correct menu item board based on key
        /// </summary>
        /// <param name="key"></param>
        public override void HandleInput(string key)
        {
            Next = Serializer.GetBoard(key);
            if (Next != null)
                Stop();
        }
    }
}
