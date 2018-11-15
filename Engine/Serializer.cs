using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace iobloc
{
    /// <summary>
    /// Handle persistence and caching of resources: settings, highscores, board caching, speed constants,
    /// Also include some external helpers for accessing dictionaries and arrays
    /// </summary>
    static class Serializer
    {
        // save highscores to this file for persistance
        private const string HighscoresFileName = "highscores.txt";
        // external settings file is set by run argument and settings are saved here when program ends
        private static string SettingsFileName;
        // in-memory caching of boards
        private readonly static Dictionary<int, IBoard> Boards = new Dictionary<int, IBoard>();
        /// <summary>
        /// Access to settings as a dictionary where keys are board IDs
        /// </summary>
        /// <returns></returns>
        public readonly static Settings Settings = new Settings();
        /// <summary>
        /// List of highscores, compiled from settings, not all boards keep scores
        /// </summary>
        public readonly static Dictionary<int, int> Highscores = new Dictionary<int, int>();
        /// <summary>
        /// Associate key shortcuts to board id
        /// </summary>
        private readonly static Dictionary<string, int> KeyToBoardIdMapping = new Dictionary<string, int>();

        /// <summary>
        /// Load settings, menu, highscores
        /// </summary>
        /// <param name="settingsFilePath">external settings file path</param>
        public static void Load(string settingsFilePath = null)
        {
            LoadSettings(settingsFilePath);
            ReadSettings();
            LoadHighscores();
        }

        /// <summary>
        /// Save settings, highscores
        /// </summary>
        public static void Save()
        {
            SaveSettings();
            SaveHighscores();
        }

        /// <summary>
        /// If external settings file, overwrite default settings from file values
        /// </summary>
        /// <param name="settingsFilePath">external settings file, will be used for saving if it doesn't exist</param>
        private static void LoadSettings(string settingsFilePath)
        {
            SettingsFileName = settingsFilePath;
            if (string.IsNullOrEmpty(SettingsFileName) || !File.Exists(SettingsFileName))
                return;

            using (var sr = File.OpenText(SettingsFileName))
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    // first line contains board ID as key
                    int key = int.Parse(line.Split(' ')[0]);
                    if (!Settings.ContainsKey(key))
                        Settings.Add(key, new Dictionary<string, string>());
                    // empty line ends board settings
                    while (!string.IsNullOrEmpty(line) && !sr.EndOfStream)
                    {
                        line = sr.ReadLine();
                        // 2 words on each line (name = value) separated by space, extra words will be ignored as comments
                        int i = line.IndexOf(' ');
                        if (i <= 0)
                            continue;
                        string name = line.Substring(0, i);
                        string val = line.Substring(i + 1);
                        // if setting name exists, it will be overwritten with file value as a string
                        Settings[key][name] = val;
                    }
                }
        }

        /// <summary>
        /// Iterate settings and extract menu option and highscore if exist
        /// </summary>
        private static void ReadSettings()
        {
            List<string> allowedKeys = new List<string>();
            List<string> text = new List<string>();
            foreach (int key in Settings.Keys)
            {
                var s = Settings[key];
                if (s.ContainsKey(Settings.MenuKeys))
                {
                    var shortcuts = s[Settings.MenuKeys];
                    var shortcutList = shortcuts.Split(',');
                    foreach (var sc in shortcutList)
                        KeyToBoardIdMapping.Add(sc, key);
                    allowedKeys.AddRange(shortcutList);

                    if (Enum.IsDefined(typeof(BoardType), key))
                        text.Add($"{key}:{(BoardType)key}");
                    else
                    {
                        string name = s.ContainsKey(Settings.Name) ? s[Settings.Name] : "UNKNOWN";
                        text.Add($"{shortcuts}:{name}");
                    }
                }
                if (s.ContainsKey(Settings.Highscore))
                    Highscores.Add(key, s.GetInt(Settings.Highscore, 0));
            }
            var menu = Settings[(int)BoardType.Menu];
            menu[Settings.AllowedKeys] = string.Join(',', allowedKeys);
            menu[Settings.Help] = string.Join(',', text);
            menu[Settings.Height] = text.Count.ToString();
        }

        /// <summary>
        /// If external settings file was used but file does not exist, create it
        /// </summary>
        private static void SaveSettings()
        {
            if (string.IsNullOrEmpty(SettingsFileName) || File.Exists(SettingsFileName))
                return;

            using (var sw = File.CreateText(SettingsFileName))
            {
                foreach (int key in Settings.Keys)
                {
                    // first line is board ID as key of dictionary
                    sw.WriteLine($"{key} {(BoardType)key}");
                    // setting values as "name value" (separated by space)
                    foreach (string k in Settings[key].Keys)
                        sw.WriteLine($"{k} {Settings[key][k]}");
                    // empty line to mark the end of settings for this board
                    sw.WriteLine();
                }
            }
        }

        /// <summary>
        /// Highscores are persisted to a text file
        /// </summary>
        private static void LoadHighscores()
        {
            // leave default value at first run
            if (!File.Exists(HighscoresFileName))
                return;

            // each line has to int values: ID and highscore
            using (var sr = File.OpenText(HighscoresFileName))
                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split(' ');
                    if (line.Length >= 2)
                    {
                        int key = int.Parse(line[0]);
                        if (Highscores.ContainsKey(key))
                            Highscores[key] = int.Parse(line[1]);
                    }
                }
        }

        /// <summary>
        /// Persist highscores to file
        /// </summary>
        private static void SaveHighscores()
        {
            // each line has to int values: ID and highscore
            using (var sw = File.CreateText(HighscoresFileName))
            {
                foreach (int key in Highscores.Keys)
                    sw.WriteLine($"{key} {Highscores[key]}");
            }
        }

        /// <summary>
        /// In-memory update of highscore, if it is the case
        /// </summary>
        /// <param name="key">board ID</param>
        /// <param name="score">score to be checked against highscore</param>
        public static void UpdateHighscore(int key, int score)
        {
            if (Highscores.ContainsKey(key) && Highscores[key] < score)
                Highscores[key] = score;
        }

        /// <summary>
        /// Calculate frame interval for certain level and board frameMultiplier setting.
        /// Currently values are between 50ms and 200ms (depending on level) multiplied by frameMultiplier
        /// </summary>
        /// <param name="frameMultiplier">board configured setting</param>
        /// <param name="level">level to calculate for</param>
        /// <returns></returns>
        public static int GetLevelInterval(double frameMultiplier, int level) => (int)(frameMultiplier * (200 - 10 * level));

        /// <summary>
        /// Get board from cache or create a new one and add it to cache.
        /// To save memory and keep board states on transitions.
        /// </summary>
        /// <param name="key">board ID</param>
        /// <returns>cached board</returns>
        public static IBoard GetBoard(int key)
        {
            if (Boards.ContainsKey(key))
                return Boards[key];

            IBoard board = null;
            if (Enum.IsDefined(typeof(BoardType), key))
                switch ((BoardType)key)
                {
                    case BoardType.Level: board = new LevelBoard(); break;
                    case BoardType.Tetris: board = new TetrisBoard(); break;
                    case BoardType.Runner: board = new RunnerBoard(); break;
                    case BoardType.Helicopt: board = new HelicopterBoard(); break;
                    case BoardType.Breakout: board = new BreakoutBoard(); break;
                    case BoardType.Invaders: board = new InvadersBoard(); break;
                    case BoardType.Snake: board = new SnakeBoard(); break;
                    case BoardType.Sokoban: board = new SokobanBoard(); break;
                    case BoardType.Table: board = new TableBoard(); break;
                    case BoardType.Paint: board = new PaintBoard(); break;
                    case BoardType.Menu: board = new MenuBoard(); break;
                    case BoardType.Fireworks: board = new AnimationBoard(BoardType.Fireworks); break;
                    case BoardType.RainingBlood: board = new AnimationBoard(BoardType.RainingBlood); break;
                }
            else
            {
                var s = Settings[key];
                board = InstantiateFromAssembly<IBoard>(s[Settings.AssemblyPath], s[Settings.ClassName]);
            }

            if (board != null)
                Boards.Add(key, board);
            return board;
        }

        public static IBoard GetBoard(string key)
        {
            if (!KeyToBoardIdMapping.ContainsKey(key))
                return null;
            return GetBoard(KeyToBoardIdMapping[key]);
        }

        public static T InstantiateFromAssembly<T>(string assemblyPath, string className) where T : class
        {
            try
            {
                var asm = Assembly.LoadFrom(assemblyPath);
                if (asm != null)
                {
                    var t = asm.GetType(className);
                    if (t != null)
                        return asm.CreateInstance(t.FullName) as T;
                }
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Parse an int setting value or get a default value if not exists
        /// </summary>
        /// <param name="dic">board settings</param>
        /// <param name="key">setting name</param>
        /// <param name="defVal">default value</param>
        /// <returns>int value of setting</returns>
        public static int GetInt(this Dictionary<string, string> dic, string key, int defVal = 1)
        {
            if (!dic.ContainsKey(key))
                return defVal;
            return int.Parse(dic[key]);
        }

        /// <summary>
        /// Parse a double setting value or get a default value
        /// </summary>
        /// <param name="dic">board settings</param>
        /// <param name="key">setting name</param>
        /// <param name="defVal">default value</param>
        /// <returns>double value of setting</returns>
        public static double GetReal(this Dictionary<string, string> dic, string key, double defVal = 0)
        {
            if (!dic.ContainsKey(key))
                return defVal;
            return double.Parse(dic[key], NumberStyles.Float, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Split comma-separated list of strings from a setting value
        /// </summary>
        /// <param name="dic">board settings</param>
        /// <param name="key">setting name</param>
        /// <returns>string list</returns>
        public static string[] GetList(this Dictionary<string, string> dic, string key)
        {
            if (!dic.ContainsKey(key))
                return new string[0];
            return dic[key].Split(',');
        }

        /// <summary>
        /// Parse a color value from setting color name
        /// </summary>
        /// <param name="dic">board settings</param>
        /// <param name="key">setting name</param>
        /// <returns>int value of color</returns>
        public static int GetColor(this Dictionary<string, string> dic, string key)
        {
            if (!dic.ContainsKey(key))
                return 0;
            return (int)Enum.Parse(typeof(ConsoleColor), dic[key]);
        }

        /// <summary>
        /// Check if array contains value
        /// </summary>
        /// <param name="array"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool Contains<T>(this T[] array, T val)
        {
            foreach (T k in array)
                if (k.Equals(val))
                    return true;
            return false;
        }
    }
}
