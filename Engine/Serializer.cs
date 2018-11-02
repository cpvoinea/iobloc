using System;
using System.Collections.Generic;
using System.IO;

namespace iobloc
{
    static class Serializer
    {
        private const string HighscoresFileName = "highscores.txt";
        private static string SettingsFileName;
        private static Dictionary<int, IBaseBoard> Boards = new Dictionary<int, IBaseBoard>();
        public static Settings Settings = new Settings();
        public static Dictionary<int, string[]> MenuKeys = new Dictionary<int, string[]>();
        public static Dictionary<int, int> Highscores = new Dictionary<int, int>();
        public static int MasterLevel { get; set; }

        public static void Load(string settingsFilePath = null)
        {
            LoadSettings(settingsFilePath);
            ReadSettings();
            LoadHighscores();
        }

        public static void Save()
        {
            SaveSettings();
            SaveHighscores();
        }

        private static void LoadSettings(string settingsFilePath)
        {
            SettingsFileName = settingsFilePath;
            if (string.IsNullOrEmpty(SettingsFileName) || !File.Exists(SettingsFileName))
                return;

            using (var sr = File.OpenText(SettingsFileName))
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    int key = int.Parse(line.Split(' ')[0]);
                    while (!string.IsNullOrEmpty(line) && !sr.EndOfStream)
                    {
                        line = sr.ReadLine();
                        var attr = line.Split(' ');
                        if (attr.Length >= 2 && Settings.ContainsKey(key) && Settings[key].ContainsKey(attr[0]))
                            Settings[key][attr[0]] = attr[1];
                    }
                }
        }

        private static void ReadSettings()
        {
            foreach (int key in Settings.Keys)
            {
                var s = Settings[key];
                if (s.ContainsKey(Settings.MenuKeys))
                    MenuKeys.Add(key, s.GetList(Settings.MenuKeys));
                if (s.ContainsKey(Settings.Highscore))
                    Highscores.Add(key, s.GetInt(Settings.Highscore, 0));
            }
        }

        private static void SaveSettings()
        {
            if (string.IsNullOrEmpty(SettingsFileName) || File.Exists(SettingsFileName))
                return;

            using (var sw = File.CreateText(SettingsFileName))
            {
                foreach (int key in Settings.Keys)
                {
                    sw.WriteLine($"{key} {(BoardType)key}");
                    foreach (string k in Settings[key].Keys)
                        sw.WriteLine($"{k} {Settings[key][k]}");
                    sw.WriteLine();
                }
            }
        }

        private static void LoadHighscores()
        {
            if (!File.Exists(HighscoresFileName))
                return;

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

        private static void SaveHighscores()
        {
            using (var sw = File.CreateText(HighscoresFileName))
            {
                foreach (int key in Highscores.Keys)
                    sw.WriteLine($"{key} {Highscores[key]}");
            }
        }

        public static void UpdateHighscore(int key, int score)
        {
            if (Highscores.ContainsKey(key) && Highscores[key] < score)
                Highscores[key] = score;
        }

        public static int GetLevelInterval(int frameMultiplier, int level)
        {
            return frameMultiplier * (200 - 10 * level);
        }

        public static IBaseBoard GetBoard(int key)
        {
            if (Boards.ContainsKey(key))
                return Boards[key];
            IBaseBoard board = null;
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
            if (board == null)
                return null;
            Boards.Add(key, board);
            return board;
        }

        public static int GetInt(this Dictionary<string, string> dic, string key, int defVal = 1)
        {
            if (!dic.ContainsKey(key))
                return defVal;
            return int.Parse(dic[key]);
        }

        public static string[] GetList(this Dictionary<string, string> dic, string key)
        {
            if (!dic.ContainsKey(key))
                return new string[0];
            return dic[key].Split(',');
        }

        public static int GetColor(this Dictionary<string, string> dic, string key)
        {
            if (!dic.ContainsKey(key))
                return 0;
            return (int)Enum.Parse(typeof(ConsoleColor), dic[key]);
        }

        public static bool Contains(this string[] array, string val)
        {
            foreach (string k in array)
                if (k == val)
                    return true;
            return false;
        }
    }
}
