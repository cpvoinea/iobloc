using System.Collections.Generic;

namespace iobloc
{
    /// <summary>
    /// Base board handles common operations like loading settings and initialization
    /// </summary>
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

        /// <summary>
        /// Settings for this board type only
        /// </summary>
        /// <value></value>
        protected Dictionary<string, string> BoardSettings { get; private set; }
        /// <summary>
        /// Main width, excluding borders
        /// </summary>
        /// <value></value>
        protected int Width { get; private set; }
        /// <summary>
        /// Main height, excluding borders
        /// </summary>
        /// <value></value>
        protected int Height { get; private set; }
        /// <summary>
        /// Main panel inside border rectangle
        /// </summary>
        /// <value></value>
        protected UIPanel Main { get; private set; }
        /// <summary>
        /// Help text will be displayed in Main panel text mode when paused
        /// </summary>
        /// <value></value>
        protected string[] Help { get; private set; }
        /// <summary>
        /// Is set to true after first initialization, ca be used for re-initialization like postbacks
        /// </summary>
        /// <value></value>
        protected bool IsInitialized { get; private set; }

        /// <summary>
        /// Get border around the Panels, to draw in UI
        /// </summary>
        /// <value>a collection of lines</value>
        public UIBorder Border { get; private set; }
        /// <summary>
        /// Rectangulars to draw in UI
        /// </summary>
        /// <value></value>
        public Dictionary<string, UIPanel> Panels { get; private set; }
        /// <summary>
        /// Duration between frames in ms
        /// </summary>
        /// <value></value>
        public int FrameInterval { get; private set; }
        /// <summary>
        /// Is true while board is running, false when board needs to exit
        /// </summary>
        /// <value></value>
        public bool IsRunning { get; private set; }
        /// <summary>
        /// List of shortcut keys which are handled by board
        /// </summary>
        /// <value></value>
        public string[] AllowedKeys { get; protected set; }
        /// <summary>
        /// Reference to next board to run, null to terminate
        /// </summary>
        /// <value></value>
        public IBaseBoard Next { get; protected set; }
        /// <summary>
        /// Get current score. Setting the score triggers level progression, highscore update and winning conditions
        /// </summary>
        /// <returns></returns>
        protected int Score { get { return _score; } set { SetScore(value); } }
        /// <summary>
        /// Get current level. Setting the level triggers FrameInterval update and winning conditions
        /// </summary>
        /// <returns></returns>
        protected int Level { get { return _level; } set { SetLevel(value); } }

        /// <summary>
        /// Initialize with internal board type,
        /// Get settings with InitializeSettings(),
        /// Construct UI elements with InitializeUI() and
        /// Reset the board by calling Initialize()
        /// </summary>
        /// <param name="type">supported BoardType</param>
        protected BaseBoard(BoardType type)
        {
            Type = type;
            InitializeSettings();
            InitializeUI();
            Initialize();
            IsInitialized = true;
        }

        /// <summary>
        /// If new score is different than current, update highscore and progress level
        /// </summary>
        /// <param name="score">new score</param>
        private void SetScore(int score)
        {
            if (score != 0 && score == _score)
                return;
            _score = score;

            if (Panels.ContainsKey(Pnl.Score))
            {
                Panels[Pnl.Score].SetText(string.Format($"{score,3}"));
                Panels[Pnl.Score].Change(true);
            }
            SetHighscore(score);

            if (LevelThreshold > 0 && score >= LevelThreshold * (_level + 1))
                SetLevel(_level + 1);
        }

        /// <summary>
        /// If new score is better than highscore, update global highscore
        /// </summary>
        /// <param name="score">new score</param>
        private void SetHighscore(int score)
        {
            if (score < Highscore)
                return;
            Highscore = score;

            if (Panels.ContainsKey(Pnl.Highscore))
            {
                Panels[Pnl.Highscore].SetText(string.Format($"{Highscore,3}"));
                Panels[Pnl.Highscore].Change(true);
            }
            Serializer.UpdateHighscore(ID, score);
        }

        /// <summary>
        /// If new level is different than current level update FrameInterval and check winning condition
        /// </summary>
        /// <param name="level"></param>
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
                {
                    Panels[Pnl.Level].SetText(string.Format($"L{_level,2}"));
                    Panels[Pnl.Level].Change(true);
                }
            }
        }

        /// <summary>
        /// Set common setting values; can be overriden to initialize extra values
        /// </summary>
        protected virtual void InitializeSettings()
        {
            BoardSettings = Serializer.Settings[ID];
            AllowedKeys = BoardSettings.GetList(Settings.AllowedKeys);
            Help = BoardSettings.GetList(Settings.Help);
            Width = BoardSettings.GetInt(Settings.Width, 10);
            Height = BoardSettings.GetInt(Settings.Height, 10);
            FrameMultiplier = BoardSettings.GetReal(Settings.FrameMultiplier);
            LevelThreshold = BoardSettings.GetInt(Settings.LevelThreshold, 0);
        }

        /// <summary>
        /// Set basic rectangle border with main panel and (optional) highscore, score and level panels;
        /// Can be overwritten to add extra elements: lines and panels
        /// </summary>
        protected virtual void InitializeUI()
        {
            Border = new UIBorder(Width + 2, Height + 2);

            Main = new UIPanel(1, 1, Height, Width);
            Main.SetText(Help, false);
            Panels = new Dictionary<string, UIPanel> { { Pnl.Main, Main } };

            if (Type != BoardType.Fireworks && Type != BoardType.RainingBlood) // don't add level panel to animation
                Panels.Add(Pnl.Level, new UIPanel(Border.Height - 1, (Border.Width + 1) / 2 - 2, Border.Height - 1, (Border.Width + 1) / 2, 1));
            if (Serializer.Highscores.ContainsKey(ID)) // don't add score panel if board doesn't keep score
            {
                if (Border.Width > 8) // don't add highscore panel if there's no room
                    Panels.Add(Pnl.Highscore, new UIPanel(0, 1, 0, 3, 1));
                Panels.Add(Pnl.Score, new UIPanel(0, Border.Width - 4, 0, Border.Width - 2, 1));
            }
        }

        /// <summary>
        /// Initialize Level = MasterLevel, Score = 0
        /// Overwrite to initialize board or to restart (use IsInitialized value to setup the board again)
        /// </summary>
        protected virtual void Initialize()
        {
            SetLevel(Settings.MasterLevel);
            if (Serializer.Highscores.ContainsKey(ID))
            {
                SetScore(0);
                SetHighscore(Serializer.Highscores[ID]);
            }
        }

        /// <summary>
        /// Used to set/unset board panels to new values during gameplay
        /// </summary>
        /// <param name="set">when true it should also mark the panel as changed</param>
        protected virtual void Change(bool set)
        {
            if (set)
                Main.Change(true);
        }

        /// <summary>
        /// Set Next board to Fireworks animation, re-initialize on exit
        /// </summary>
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

        /// <summary>
        /// Set Next board to Rain animation and re-initialize
        /// </summary>
        /// <param name="exit">when true, the board also stops running</param>
        protected void Lose(bool exit = true)
        {
            if (Next == null)
                Next = Serializer.GetBoard((int)BoardType.RainingBlood);
            Initialize(); // if game is over, re-initialize board
            if (exit)
                Stop();
        }

        /// <summary>
        /// Runs every time a bord is opened by BoardRunner,
        /// Set Next to null, IsRunning to true, refresh all panels
        /// </summary>
        public virtual void Start()
        {
            Next = null;
            IsRunning = true;
            foreach (var p in Panels.Values) // force refresh of panels
                p.Change(true);
        }

        /// <summary>
        /// Stop running and return to menu
        /// </summary>
        public void Stop()
        {
            if (Next == null && Type != BoardType.Menu) // if no animation, return to menu, unless already in menu
                Next = Serializer.GetBoard((int)BoardType.Menu);
            IsRunning = false;
        }

        /// <summary>
        /// Turns pause mode on and off
        /// </summary>
        public virtual void TogglePause()
        {
            Main.ToggleText();
        }

        /// <summary>
        /// Move to next frame; not all boards use frames, some are static
        /// </summary>
        public virtual void NextFrame() { }

        /// <summary>
        /// Handle allowed key
        /// </summary>
        /// <param name="key">key value as string constant</param>
        public abstract void HandleInput(string key);
    }
}
