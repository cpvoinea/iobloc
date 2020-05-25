namespace iobloc
{
    // Display list of available games to link through Next
    public class Menu : BaseGame
    {
        bool _exit;

        // Summary:
        //      A text game with each line item having its own shortcut key(s)
        public Menu() : base(GameType.Menu) { }

        private void DrawLogo()
        {
            int[,] logo = Animations.Get(GameType.Logo)[0];
            var r = new System.Random();
            for (int i = 0; i <= Animations.SIZE_LOGO; i++)
                for (int j = 0; j < Animations.SIZE_LOGO; j++)
                    if (i < Height && j < Width && logo[i, j] == 1)
                        Main[i, j] = new PaneCell(r.Next(15) + 1);
        }

        // Summary:
        //      Initialize in text mode
        protected override void Initialize()
        {
            base.Initialize();
            DrawLogo();
            Main.SwitchMode();
        }

        // Summary:
        //      Overriden to display correct MasterLevel value
        public override void Start()
        {
            Level = Serializer.MasterLevel;
            base.Start();
            if (Help.Length == 1 && AllowedKeys.Length > 0)
            {
                if (_exit)
                    Stop();
                else
                {
                    _exit = true;
                    HandleInput(AllowedKeys[0]);
                }
            }
        }

        // Summary:
        //      Link to correct menu item game based on key
        public override void HandleInput(string key)
        {
            Next = Serializer.GetGame(key);
            if (Next != null)
                Stop();
        }
    }
}
