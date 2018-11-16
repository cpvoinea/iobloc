using System.Collections.Generic;

namespace iobloc
{
    // Base board handles common operations like loading settings and initialization
    abstract class BaseBoard : IBaseBoard
    {
        // internal code of board
        private int ID => (int)Type;
        // supported board type
        private BoardType Type { get; set; }
        // from settings, used to tweak FrameInterval value for each board
        private double FrameMultiplier { get; set; }
        // from settings, used to facilitate level progression; not used when set to 0
        private int LevelThreshold { get; set; }
        // highscore value; not used if null
        private int? Highscore { get; set; }
        // internal score, used for level progression
        private int _score;
        // internal level, in some cases decides victory condition
        private int _level;

        // Settings for this board type only
        protected Dictionary<string, string> BoardSettings { get; private set; }
        // Main width, excluding borders
        protected int Width { get; private set; }
        // Main height, excluding borders
        protected int Height { get; private set; }
        // Custom block width to compensate vertical-to-horizontal size ratio
        protected int BlockWidth { get; private set; }
        protected int BlockSpace { get; private set; }
        protected int Block { get; private set; }
        // Help text will be displayed in main panel text mode when paused
        protected string[] Help { get; private set; }
        // Summary:
        //      Is set to true after first initialization, ca be used for re-initialization like postbacks
        protected bool IsInitialized { get; private set; }

        // Summary:
        //      Get border around the Panels, to draw in UI
        public UIBorder Border { get; private set; }
        // Summary:
        //      Rectangulars to draw in UI
        public Dictionary<string, UIPanel> Panels { get; private set; }
        // Summary:
        //      Duration between frames in ms
        public int FrameInterval { get; private set; }
        // Summary:
        //      Is true while board is running, false when board needs to exit
        public bool IsRunning { get; private set; }
        // Summary:
        //      List of shortcut keys which are handled by board
        public string[] AllowedKeys { get; protected set; }
        // Summary:
        //      Reference to next board to run, null to terminate
        public IBoard Next { get; protected set; }
        // Summary:
        //      Main panel inside border rectangle
        protected UIPanel Main { get; set; }
        // Summary:
        //      Get current score. Setting the score triggers level progression, highscore update and winning conditions
        protected int Score { get { return _score; } set { SetScore(value); } }
        // Summary:
        //      Get current level. Setting the level triggers FrameInterval update and winning conditions
        protected int Level { get { return _level; } set { SetLevel(value); } }

        // Summary:
        //      Initialize with internal board type,
        //      Get settings with InitializeSettings(),
        //      Construct UI elements with InitializeUI() and
        //      Reset the board by calling Initialize()
        // Param: type: supported BoardType
        protected BaseBoard(BoardType type)
        {
            Type = type;
            InitializeSettings();
            InitializeUI();
            Initialize();
            IsInitialized = true;
        }

        // Summary:
        //      If new score is different than current, update highscore and progress level
        // Param: score: new score
        private void SetScore(int score)
        {
            if (score != 0 && score == _score)
                return;
            _score = score;

            if (Panels.ContainsKey(Pnl.Score))
                Panels[Pnl.Score].SetText($"{score,3}");
            SetHighscore(score);

            if (LevelThreshold > 0 && score >= LevelThreshold * (_level + 1))
                SetLevel(_level + 1);
        }

        // Summary:
        //      If new score is better than highscore, update global highscore

        // Param: score: new score
        private void SetHighscore(int score)
        {
            if (score < Highscore)
                return;
            Highscore = score;

            if (Panels.ContainsKey(Pnl.Highscore))
                Panels[Pnl.Highscore].SetText($"{Highscore,3}");
            Serializer.UpdateHighscore(ID, score);
        }

        // Summary:
        //      If new level is different than current level update FrameInterval and check winning condition
        protected void SetLevel(int level)
        {
            if (level > 0 && level == _level)
                return;

            if (level >= 16 && Type != BoardType.Sokoban)
                Win();
            else
            {
                _level = level;
                FrameInterval = Serializer.GetLevelInterval(FrameMultiplier, _level);

                if (Panels.ContainsKey(Pnl.Level))
                    Panels[Pnl.Level].SetText($"L{_level,2}");
            }
        }

        // Summary:
        //      Set common setting values; can be overriden to initialize extra values
        protected virtual void InitializeSettings()
        {
            BoardSettings = Serializer.Settings.ContainsKey(ID) ? Serializer.Settings[ID] : new Dictionary<string, string>();

            AllowedKeys = BoardSettings.GetList(Settings.AllowedKeys);
            Help = BoardSettings.GetList(Settings.Help);
            Width = BoardSettings.GetInt(Settings.Width, 10);
            Height = BoardSettings.GetInt(Settings.Height, 10);
            BlockWidth = BoardSettings.GetInt(Settings.BlockWidth, 1);
            BlockSpace = BoardSettings.GetInt(Settings.BlockSpace, 0);
            Block = BlockWidth + BlockSpace;
            FrameMultiplier = BoardSettings.GetReal(Settings.FrameMultiplier, 1);
            LevelThreshold = BoardSettings.GetInt(Settings.LevelThreshold, 0);
        }

        // Summary:
        //      Set basic rectangle border with main panel and (optional) highscore, score and level panels;
        //      Can be overwritten to add extra elements: lines and panels
        protected virtual void InitializeUI()
        {
            Border = new UIBorder(Width + 2, Height + 2);
            Panels = new Dictionary<string, UIPanel>();

            // don't use main for these boards
            if (Type != BoardType.Table)
            {
                Main = new UIPanel(1, 1, Height, Width);
                Main.SetText(Help, false);
                Panels.Add(Pnl.Main, Main);
            }

            // don't show level for these boards
            if (!new BoardType[] { BoardType.Fireworks, BoardType.RainingBlood, BoardType.Paint, BoardType.Table }.Contains(Type))
                Panels.Add(Pnl.Level, new UIPanel(Border.Height - 1, (Border.Width + 1) / 2 - 2, Border.Height - 1, (Border.Width + 1) / 2));
            if (Serializer.Highscores.ContainsKey(ID)) // don't add score panel if board doesn't keep score
            {
                if (Border.Width > 8) // don't add highscore panel if there's no room
                    Panels.Add(Pnl.Highscore, new UIPanel(0, 1, 0, 3));
                Panels.Add(Pnl.Score, new UIPanel(0, Border.Width - 4, 0, Border.Width - 2));
            }
        }

        // Summary:
        //      Initialize Level = MasterLevel, Score = 0
        //      Overwrite to initialize board or to restart (use IsInitialized value to setup the board again)
        protected virtual void Initialize()
        {
            SetLevel(Serializer.MasterLevel);
            if (Serializer.Highscores.ContainsKey(ID))
            {
                SetScore(0);
                SetHighscore(Serializer.Highscores[ID]);
            }
        }

        // Summary:
        //      Used to set/unset board panels to new values during gameplay
        // Param: set: when true it should also mark the panel as changed
        protected virtual void Change(bool set)
        {
            if (set)
                Main.Change();
        }

        // Summary:
        //      Set Next board to Fireworks animation, re-initialize on exit
        protected void Win(bool exit = false)
        {
            if (Next == null)
                Next = Serializer.GetBoard((int)BoardType.Fireworks);
            if (exit)
            {
                Initialize(); // if exit is called, re-initialize board
                Stop();
            }
        }

        // Summary:
        //      Set Next board to Rain animation and re-initialize
        // Param: exit: when true, the board also stops running
        protected void Lose(bool exit = true)
        {
            if (Next == null)
                Next = Serializer.GetBoard((int)BoardType.RainingBlood);
            Initialize(); // if game is over, re-initialize board
            if (exit)
                Stop();
        }

        // Summary:
        //      Runs every time a bord is opened by BoardRunner,
        //      Set Next to null, IsRunning to true, refresh all panels
        public virtual void Start()
        {
            Next = null;
            IsRunning = true;
            foreach (var p in Panels.Values) // force refresh of panels
                p.Change();
        }

        // Summary:
        //      Stop running and return to menu
        public void Stop()
        {
            if (Next == null && Type != BoardType.Menu) // if no animation, return to menu, unless already in menu
                Next = Serializer.GetBoard((int)BoardType.Menu);
            IsRunning = false;
        }

        // Summary:
        //      Turns pause mode on and off
        public virtual void TogglePause()
        {
            Main.SwitchMode();
        }

        // Summary:
        //      Move to next frame; not all boards use frames, some are static
        public virtual void NextFrame() { }

        // Summary:
        //      Handle allowed key
        // Param: key: key value as string constant
        public abstract void HandleInput(string key);
    }
}
