using System;
using System.Collections.Generic;
using System.IO;

namespace iobloc
{
    static class Serializer
    {
        internal static Settings Settings = new Settings();
        internal static Dictionary<int, int> Highscores = new Dictionary<int, int> { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }, { 6, 0 }, { 7, 0 }, { 8, 0 } };
        static Dictionary<int, IBoard> Boards = new Dictionary<int, IBoard>();
        static string SettingsFileName;
        const string HighscoresFileName = "highscores.txt";
        internal static int Level { get; set; }

        internal static void LoadSettings(string settingsFilePath = null)
        {
            SettingsFileName = settingsFilePath;
            if (string.IsNullOrEmpty(settingsFilePath) || !File.Exists(settingsFilePath))
                return;

            using (var sr = File.OpenText(settingsFilePath))
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

        internal static void SaveSettings()
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

        internal static void LoadHighscores()
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

        internal static void SaveHighscores()
        {
            using (var sw = File.CreateText(HighscoresFileName))
            {
                foreach (int key in Highscores.Keys)
                    sw.WriteLine($"{key} {Highscores[key]}");
            }
        }

        internal static void UpdateHighscore(BoardType type, int score)
        {
            int key = (int)type;
            if (Highscores.ContainsKey(key) && Highscores[key] < score)
                Highscores[key] = score;
        }

        internal static int GetLevelInterval(int frameMultiplier, int level)
        {
            return frameMultiplier * (200 - 10 * level);
        }

        internal static IBoard GetBoard(BoardType type)
        {
            int key = (int)type;
            if (Boards.ContainsKey(key))
                return Boards[key];
            IBoard board = null;
            switch (type)
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

        internal static int GetInt(this Dictionary<string, string> dic, string key, int defVal = 1)
        {
            if (!dic.ContainsKey(key))
                return defVal;
            return int.Parse(dic[key]);
        }

        internal static string[] GetList(this Dictionary<string, string> dic, string key)
        {
            if (!dic.ContainsKey(key))
                return new string[0];
            return dic[key].Split(',');
        }

        internal static int GetColor(this Dictionary<string, string> dic, string key)
        {
            if (!dic.ContainsKey(key))
                return 0;
            return (int)Enum.Parse(typeof(ConsoleColor), dic[key]);
        }
    }
}
