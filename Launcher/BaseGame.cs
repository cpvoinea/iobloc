using System.Collections.Generic;

namespace iobloc
{
    // Base game handles common operations like loading settings and initialization
    abstract class BaseGame : IBaseGame
    {
        // internal code of game
        private int ID => (int)Type;
        // supported game type
        private GameType Type { get; set; }
        // from settings, used to tweak FrameInterval value for each game
        private double FrameMultiplier { get; set; }
        // from settings, used to facilitate level progression; not used when set to 0
        private int LevelThreshold { get; set; }
        // highscore value; not used if null
        private int? Highscore { get; set; }
        // internal score, used for level progression
        private int _score;
        // internal level, in some cases decides victory condition
        private int _level;

        // Settings for this game type only
        protected Dictionary<string, string> GameSettings { get; private set; }
        // Main width, excluding borders
        protected int Width { get; private set; }
        // Main height, excluding borders
        protected int Height { get; private set; }
        // Custom block width to compensate vertical-to-horizontal size ratio
        protected int BlockWidth { get; private set; }
        protected int BlockSpace { get; private set; }
        protected int Block { get; private set; }
        // Help text will be displayed in main pane text mode when paused
        protected string[] Help { get; private set; }
        // Summary:
        //      Is set to true after first initialization, ca be used for re-initialization like postbacks
        protected bool IsInitialized { get; private set; }

        // Summary:
        //      Get border around the Panes, to draw in UI
        public Border Border { get; private set; }
        // Summary:
        //      Rectangulars to draw in UI
        public Dictionary<string, Pane> Panes { get; private set; }
        // Summary:
        //      Duration between frames in ms
        public int FrameInterval { get; private set; }
        // Summary:
        //      Is true while game is running, false when game needs to exit
        public bool IsRunning { get; private set; }
        // Summary:
        //      List of shortcut keys which are handled by game
        public string[] AllowedKeys { get; protected set; }
        // Summary:
        //      Reference to next game to run, null to terminate
        public IGame Next { get; protected set; }
        // Summary:
        //      Main pane inside border rectangle
        protected Pane Main { get; set; }
        // Summary:
        //      Get current score. Setting the score triggers level progression, highscore update and winning conditions
        protected int Score { get { return _score; } set { SetScore(value); } }
        // Summary:
        //      Get current level. Setting the level triggers FrameInterval update and winning conditions
        protected int Level { get { return _level; } set { SetLevel(value); } }

        // Summary:
        //      Initialize with internal game type,
        //      Get settings with InitializeSettings(),
        //      Construct UI elements with InitializeUI() and
        //      Reset the game by calling Initialize()
        // Parameters: type: supported GameType
        protected BaseGame(GameType type)
        {
            Type = type;
            InitializeSettings();
            InitializeUI();
            Initialize();
            IsInitialized = true;
        }

        // Summary:
        //      If new score is different than current, update highscore and progress level
        // Parameters: score: new score
        private void SetScore(int score)
        {
            if (score != 0 && score == _score)
                return;
            _score = score;

            if (Panes.ContainsKey(Pnl.Score))
                Panes[Pnl.Score].SetText($"{score,3}");
            SetHighscore(score);

            if (LevelThreshold > 0 && score >= LevelThreshold * (_level + 1))
                SetLevel(_level + 1);
        }

        // Summary:
        //      If new score is better than highscore, update global highscore

        // Parameters: score: new score
        private void SetHighscore(int score)
        {
            if (score < Highscore)
                return;
            Highscore = score;

            if (Panes.ContainsKey(Pnl.Highscore))
                Panes[Pnl.Highscore].SetText($"{Highscore,3}");
            Serializer.UpdateHighscore(ID, score);
        }

        // Summary:
        //      If new level is different than current level update FrameInterval and check winning condition
        protected void SetLevel(int level)
        {
            if (level > 0 && level == _level)
                return;

            if (level >= 16 && Type != GameType.Sokoban)
                Win();
            else
            {
                _level = level;
                FrameInterval = Serializer.GetLevelInterval(FrameMultiplier, _level);

                if (Panes.ContainsKey(Pnl.Level))
                    Panes[Pnl.Level].SetText($"L{_level,2}");
            }
        }

        // Summary:
        //      Set common setting values; can be overriden to initialize extra values
        protected virtual void InitializeSettings()
        {
            GameSettings = Serializer.Settings.ContainsKey(ID) ? Serializer.Settings[ID] : new Dictionary<string, string>();

            AllowedKeys = Serializer.GetList(GameSettings, Settings.AllowedKeys);
            Help = Serializer.GetList(GameSettings, Settings.Help);
            Width = Serializer.GetInt(GameSettings, Settings.Width, 10);
            Height = Serializer.GetInt(GameSettings, Settings.Height, 10);
            BlockWidth = Serializer.GetInt(GameSettings, Settings.BlockWidth, 1);
            BlockSpace = Serializer.GetInt(GameSettings, Settings.BlockSpace);
            Block = BlockWidth + BlockSpace;
            FrameMultiplier = Serializer.GetReal(GameSettings, Settings.FrameMultiplier, 1);
            LevelThreshold = Serializer.GetInt(GameSettings, Settings.LevelThreshold);
        }

        // Summary:
        //      Set basic rectangle border with main pane and (optional) highscore, score and level panes;
        //      Can be overwritten to add extra elements: lines and panes
        protected virtual void InitializeUI()
        {
            Border = new Border(Width + 2, Height + 2);
            Panes = new Dictionary<string, Pane>();

            // don't use main for these games
            if (Type != GameType.Table)
            {
                Main = new Pane(1, 1, Height, Width);
                Main.SetText(Help, false);
                Panes.Add(Pnl.Main, Main);
            }

            // don't show level for these games
            if (!Serializer.Contains(new GameType[] { GameType.Fireworks, GameType.RainingBlood, GameType.Paint }, Type))
                Panes.Add(Pnl.Level, new Pane(Border.Height - 1, (Border.Width + 1) / 2 - 2, Border.Height - 1, (Border.Width + 1) / 2));
            if (Serializer.Highscores.ContainsKey(ID)) // don't add score pane if game doesn't keep score
            {
                if (Border.Width > 8) // don't add highscore pane if there's no room
                    Panes.Add(Pnl.Highscore, new Pane(0, 1, 0, 3));
                Panes.Add(Pnl.Score, new Pane(0, Border.Width - 4, 0, Border.Width - 2));
            }
        }

        // Summary:
        //      Initialize Level = MasterLevel, Score = 0
        //      Overwrite to initialize game or to restart (use IsInitialized value to setup the game again)
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
        //      Used to set/unset game panes to new values during gameplay
        // Parameters: set: when true it should also mark the pane as changed
        protected virtual void Change(bool set)
        {
            if (set)
                Main.Change();
        }

        // Summary:
        //      Set Next game to Fireworks animation, re-initialize on exit
        protected void Win(bool exit = false)
        {
            if (Next == null)
                Next = Serializer.GetGame((int)GameType.Fireworks);
            if (exit)
            {
                Initialize(); // if exit is called, re-initialize game
                Stop();
            }
        }

        // Summary:
        //      Set Next game to Rain animation and re-initialize
        // Parameters: exit: when true, the game also stops running
        protected void Lose(bool exit = true)
        {
            if (Next == null)
                Next = Serializer.GetGame((int)GameType.RainingBlood);
            Initialize(); // if game is over, re-initialize game
            if (exit)
                Stop();
        }

        // Summary:
        //      Runs every time a bord is opened by GameRunner,
        //      Set Next to null, IsRunning to true, refresh all panes
        public virtual void Start()
        {
            Next = null;
            IsRunning = true;
            foreach (var p in Panes.Values) // force refresh of panes
                p.Change();
        }

        // Summary:
        //      Stop running and return to menu
        public void Stop()
        {
            if (Next == null && Type != GameType.Menu) // if no animation, return to menu, unless already in menu
                Next = Serializer.GetGame((int)GameType.Menu);
            IsRunning = false;
        }

        // Summary:
        //      Turns pause mode on and off
        public virtual void TogglePause()
        {
            Main.SwitchMode();
        }

        // Summary:
        //      Move to next frame; not all games use frames, some are static
        public virtual void NextFrame() { }

        // Summary:
        //      Handle allowed key
        // Parameters: key: key value as string constant
        public abstract void HandleInput(string key);
    }
}
