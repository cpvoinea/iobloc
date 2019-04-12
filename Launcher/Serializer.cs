using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace iobloc
{
    // Summary:
    //      Handle persistence and caching of resources: settings, highscores, game caching, speed constants,
    //      Also include some external helpers for accessing dictionaries and arrays
    static class Serializer
    {
        // external settings file is set by run argument and settings are saved here when program ends
        public const string SettingsFileName = "settings.txt";
        // save highscores to this file for persistance
        private const string HighscoresFileName = "highscores.txt";
        // in-memory caching of games
        private readonly static Dictionary<int, IGame<int>> Games = new Dictionary<int, IGame<int>>();
        // Access to settings as a dictionary where keys are game IDs
        public readonly static Settings Settings = new Settings();
        // List of highscores, compiled from settings, not all games keep scores
        public readonly static Dictionary<int, int> Highscores = new Dictionary<int, int>();
        // Associate id shortcuts to game id
        private readonly static Dictionary<string, int> KeyToGameIdMapping = new Dictionary<string, int>();

        // Summary:
        //      A common and customizable level value across all games.
        //      During Initialization, game levels will be set to this value.
        public static int MasterLevel { get; set; }

        // Summary:
        //      Load settings, menu, highscores
        // Parameters: settingsFilePath: external settings file path
        public static void Load()
        {
            Games.Clear();
            LoadSettings();
            ReadSettings();
            LoadHighscores();
        }

        // Summary:
        //      Save settings, highscores
        public static void Save()
        {
            SaveSettings();
            SaveHighscores();
        }

        // Summary:
        //      If external settings file, overwrite default settings from file values
        // Parameters: settingsFilePath: external settings file, will be used for saving if it doesn't exist
        private static void LoadSettings()
        {
            if (File.Exists(SettingsFileName))
                Settings.Clear();
            else
                return;

            using (var sr = File.OpenText(SettingsFileName))
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (string.IsNullOrEmpty(line.Trim()))
                        break;
                    // first line contains game ID as id
                    int id = int.Parse(line.Split(' ')[0]);
                    if (!Settings.ContainsKey(id))
                        Settings.Add(id, new Dictionary<string, string>());
                    // empty line ends game settings
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
                        Settings[id][name] = val;
                    }
                }
        }

        // Summary:
        //      Iterate settings and extract menu option and highscore if exist
        private static void ReadSettings()
        {
            List<string> allowedKeys = new List<string>();
            List<string> text = new List<string>();
            KeyToGameIdMapping.Clear();
            Highscores.Clear();
            foreach (int id in Settings.Keys)
            {
                var s = Settings[id];
                if (s.ContainsKey(Settings.MenuKeys))
                {
                    var shortcuts = s[Settings.MenuKeys];
                    var shortcutList = shortcuts.Split(',');
                    foreach (var sc in shortcutList)
                        KeyToGameIdMapping.Add(sc, id);
                    allowedKeys.AddRange(shortcutList);

                    if (Enum.IsDefined(typeof(GameType), id))
                    {
                        string prefix = id < 10 ? id.ToString() : shortcuts;
                        text.Add($"{prefix}:{(GameType)id}");
                    }
                    else
                    {
                        string name = s.ContainsKey(Settings.Name) ? s[Settings.Name] : "UNKNOWN";
                        text.Add($"{shortcuts}:{name}");
                    }
                }
                if (s.ContainsKey(Settings.Highscore))
                    Highscores.Add(id, Serializer.GetInt(s, Settings.Highscore, 0));
            }

            int menuId = (int)GameType.Menu;
            if (!Settings.ContainsKey(menuId))
                Settings.Add(menuId, new Dictionary<string, string>());
            var menu = Settings[menuId];
            menu[Settings.AllowedKeys] = Join(",", allowedKeys);
            menu[Settings.Help] = Join(",", text);
            menu[Settings.Height] = text.Count.ToString();
        }

        // Summary:
        //      If external settings file was used but file does not exist, create it
        private static void SaveSettings()
        {
            using (var sw = File.CreateText(SettingsFileName))
            {
                foreach (int id in Settings.Keys)
                {
                    // first line is game ID as id of dictionary
                    sw.WriteLine($"{id} {(GameType)id}");
                    // setting values as "name value" (separated by space)
                    foreach (string k in Settings[id].Keys)
                        sw.WriteLine($"{k} {Settings[id][k]}");
                    // empty line to mark the end of settings for this game
                    sw.WriteLine();
                }
            }
        }

        // Summary:
        //      Highscores are persisted to a text file
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
                        int id = int.Parse(line[0]);
                        if (Highscores.ContainsKey(id))
                            Highscores[id] = int.Parse(line[1]);
                    }
                }
        }

        // Summary:
        //      Persist highscores to file
        private static void SaveHighscores()
        {
            // each line has to int values: ID and highscore
            using (var sw = File.CreateText(HighscoresFileName))
            {
                foreach (int id in Highscores.Keys)
                    sw.WriteLine($"{id} {Highscores[id]}");
            }
        }

        // Summary:
        //      In-memory update of highscore, if it is the case
        // Parameters: id: game ID
        // Parameters: score: score to be checked against highscore
        public static void UpdateHighscore(int id, int score)
        {
            if (Highscores.ContainsKey(id) && Highscores[id] < score)
                Highscores[id] = score;
        }

        // Summary:
        //      Calculate frame interval for certain level and game frameMultiplier setting.
        //      Currently values are between 50ms and 200ms (depending on level) multiplied by frameMultiplier
        // Parameters: frameMultiplier: game configured setting
        // Parameters: level: level to calculate for
        public static int GetLevelInterval(double frameMultiplier, int level) => (int)(frameMultiplier * (200 - 10 * level));

        // Summary:
        //      Get game from cache or create a new one and add it to cache.
        //      To save memory and keep game states on transitions.
        // Parameters: id: game ID
        public static IGame<int> GetGame(int id)
        {
            if (Games.ContainsKey(id))
                return Games[id];

            IGame<int> game = null;
            if (Enum.IsDefined(typeof(GameType), id))
                switch ((GameType)id)
                {
                    case GameType.Level: game = new LevelSelection(); break;
                    case GameType.Tetris: game = new Tetris(); break;
                    case GameType.Runner: game = new Runner(); break;
                    case GameType.Helicopt: game = new Helicopter(); break;
                    case GameType.Breakout: game = new Breakout(); break;
                    case GameType.Invaders: game = new Invaders(); break;
                    case GameType.Snake: game = new Snake(); break;
                    case GameType.Sokoban: game = new Sokoban(); break;
                    case GameType.Table: game = new Table(); break;
                    case GameType.Paint: game = new Paint(); break;
                    case GameType.Menu: game = new Menu(); break;
                    case GameType.Fireworks: game = new EndAnimation(GameType.Fireworks); break;
                    case GameType.RainingBlood: game = new EndAnimation(GameType.RainingBlood); break;
                }
            else
            {
                var s = Settings[id];
                game = InstantiateFromAssembly<IGame<int>>(s[Settings.AssemblyPath], s[Settings.ClassName]);
            }

            if (game != null)
                Games.Add(id, game);
            return game;
        }

        public static IGame<int> GetGame(string id)
        {
            if (!KeyToGameIdMapping.ContainsKey(id))
                return null;
            return GetGame(KeyToGameIdMapping[id]);
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

        public static string GetString(Dictionary<string, string> dic, string id)
        {
            if (!dic.ContainsKey(id))
                return string.Empty;
            return dic[id];
        }

        // Summary:
        //      Parse an int setting value or get a default value if not exists
        // Parameters: dic: game settings
        // Parameters: id: setting name
        // Parameters: defVal: default value
        public static int GetInt(Dictionary<string, string> dic, string id, int defVal = 0)
        {
            if (!dic.ContainsKey(id))
                return defVal;
            return int.Parse(dic[id]);
        }

        // Summary:
        //      Parse a double setting value or get a default value
        // Parameters: dic: game settings
        // Parameters: id: setting name
        // Parameters: defVal: default value
        public static double GetReal(Dictionary<string, string> dic, string id, double defVal = 0)
        {
            if (!dic.ContainsKey(id))
                return defVal;
            return double.Parse(dic[id], NumberStyles.Float, CultureInfo.InvariantCulture);
        }

        // Summary:
        //      Split comma-separated list of strings from a setting value
        // Parameters: dic: game settings
        // Parameters: id: setting name
        public static string[] GetList(Dictionary<string, string> dic, string id)
        {
            if (!dic.ContainsKey(id))
                return new string[0];
            return dic[id].Split(',');
        }

        // Summary:
        //      Parse a color value from setting color name
        // Parameters: dic: game settings
        // Parameters: id: setting name
        public static int GetColor(Dictionary<string, string> dic, string id)
        {
            if (!dic.ContainsKey(id))
                return 0;
            return (int)Enum.Parse(typeof(ConsoleColor), dic[id]);
        }

        // Summary:
        //      Check if array contains value
        // Parameters: array: 
        // Parameters: val: 
        public static bool Contains<T>(T[] array, T val)
        {
            foreach (T k in array)
                if (k.Equals(val))
                    return true;
            return false;
        }

        public static string Join<T>(string sep, List<T> array)
        {
            if (array == null || array.Count == 0)
                return string.Empty;
            string result = array[0].ToString();
            for (int i = 1; i < array.Count; i++)
                result += sep + array[i].ToString();
            return result;
        }
    }
}
