using System;
using System.Collections.Generic;
using System.IO;

namespace iobloc
{
    static class Config
    {
        static Dictionary<int, Dictionary<string, string>> _settings = new Dictionary<int, Dictionary<string, string>>{
            {0, // Level
                new Dictionary<string, string>{
                    {"Help", "<Easy-ENTR-Hard>"},
                    {"Keys", "LeftArrow,RightArrow,Enter"},
                    {"FrameMultiplier", "5"},
                    {"LevelThreshold", "0"},
                    {"Width", "16"},
                    {"Height", "1"},
                }
            },
            {1, // Tetris
                new Dictionary<string, string>{
                    {"Help", ",,,,,,Left:LEFT,Rght:RIGHT,Rotate:UP,Speed:DOWN,Exit:ESC,Pause:ANY"},
                    {"Keys", "LeftArrow,RightArrow,UpArrow,DownArrow"},
                    {"FrameMultiplier", "4"},
                    {"LevelThreshold", "10"},
                    {"Width", "10"},
                    {"Height", "20"},
                }
            },
            {2, // Runner
                new Dictionary<string, string>{
                    {"Help", "Jump:UP,Dble-jump:UP,Exit:ESC,Pause:ANY"},
                    {"Keys", "UpArrow"},
                    {"FrameMultiplier", "1"},
                    {"LevelThreshold", "10"},
                    {"Width", "13"},
                    {"Height", "5"},
                    {"PlayerColor", "Blue"},
                    {"EnemyColor", "Red"},
                    {"FenceSpace", "7"},
                }
            },
            {3, // Helicopter
                new Dictionary<string, string>{
                    {"Help", ",,Lift:UP,Exit:ESC,Pause:ANY"},
                    {"Keys", "UpArrow"},
                    {"FrameMultiplier", "2"},
                    {"LevelThreshold", "20"},
                    {"Width", "13"},
                    {"Height", "7"},
                    {"PlayerPosition", "1"},
                    {"ObstacleSpace", "4"},
                    {"PlayerColor", "Blue"},
                    {"EnemyColor", "Red"},
                }
            },
            {4, // Breakout
                new Dictionary<string, string>{
                    {"Help", ",,,,Move:LEFT-RIGHT,Exit:ESC,Pause:ANY" },
                    {"Keys", "LeftArrow,RightArrow"},
                    {"FrameMultiplier", "2"},
                    {"LevelThreshold", "0"},
                    {"Width", "25"},
                    {"Height", "15"},
                    {"PlayerColor", "Blue"},
                    {"EnemyColor", "Red"},
                    {"NeutralColor", "Gray"},
                    {"BlockWidth", "5"},
                    {"BlockSpace", "0"},
                    {"BlockRows", "5"},
                }
            },
            {5, // Invaders
                new Dictionary<string, string>{
                    {"Help", ",,,Move:LEFT-RIGHT,Shoot:UP,Exit:ESC,Pause:ANY"},
                    {"Keys", "LeftArrow,RightArrow,UpArrow"},
                    {"FrameMultiplier", "1"},
                    {"LevelThreshold", "0"},
                    {"Width", "19"},
                    {"Height", "11"},
                    {"PlayerColor", "Blue"},
                    {"EnemyColor", "Red"},
                    {"NeutralColor", "Gray"},
                    {"AlienWidth", "3"},
                    {"AlienSpace", "1"},
                    {"AlienRows", "3"},
                    {"AlienCols", "3"},
                    {"BulletSpeed", "2"},
                }
            },
            {6, // Snake
                new Dictionary<string, string>{
                    {"Help", ",,,Move:ARROWS,Exit:ESC,Pause:ANY"},
                    {"Keys", "LeftArrow,RightArrow,UpArrow,DownArrow"},
                    {"FrameMultiplier", "2"},
                    {"LevelThreshold", "5"},
                    {"Width", "10"},
                    {"Height", "10"},
                    {"PlayerColor", "Blue"},
                    {"NeutralColor", "Gray"},
                }
            },
            {7, // Sokoban
                new Dictionary<string, string>{
                    {"Help", "Mov:,ARRW,R:Re,strt,Ext:,ESC"}, //"Restrt:R,Move:ARW,Exit:ESC,Paus:ANY"},
                    {"Keys", "LeftArrow,RightArrow,UpArrow,DownArrow,R"},
                    {"FrameMultiplier", "5"},
                    {"LevelThreshold", "0"},
                    {"Width", "4"},
                    {"Height", "6"},
                    {"BlockWidth", "1"},
                    {"WinScore", "50"},
                    {"WallColor", "DarkGray"},
                    {"PlayerColor", "Red"},
                    {"BlockColor", "Blue"},
                    {"TargetColor", "DarkYellow"},
                    {"TargetPlayerColor", "DarkRed"},
                    {"TargetBlockColor", "DarkBlue"},
                }
            },
            {8, // Table
                new Dictionary<string, string>{
                    {"Help", "Move_cursor:,LEFT-RIGHT,Take_piece:,UP_ARROW,Put_piece:,DOWN_ARROW,Exit:ESC,Pause:ANY"},
                    {"Keys", "LeftArrow,RightArrow,UpArrow,DownArrow"},
                    {"Height", "36"},
                    {"PieceWidth", "4"},
                    {"PlayerColor", "Blue"},
                    {"EnemyColor", "Red"},
                    {"NeutralColor", "Gray"},
                }
            },
            {9, //Paint
                new Dictionary<string, string>{
                    {"Help", ""},
                    {"Keys", ""},
                    {"Width", ""},
                    {"Height", ""},
                }
            }
        };

        static Dictionary<int, int> _highscores = new Dictionary<int, int>{
            {1, 0},
            {2, 0},
            {3, 0},
            {4, 0},
            {5, 0},
            {6, 0},
            {7, 0},
            {8, 0}
        };

        const string FILE_HIGHSCORES = "highscores.txt";
        const int INTERVAL_MIN = 50;
        const int INTERVAL_MAX = 200;
        internal const int LEVEL_MAX = 16;
        static int INTERVAL_LEVEL = (INTERVAL_MAX - INTERVAL_MIN) / (LEVEL_MAX - 1);
        internal static int Level { get; set; }
        internal static bool SokobanComplete { get; set; }
        static string _settingsFilePath;
        readonly static List<MenuItem> _menuItems = new List<MenuItem>();
        internal static IEnumerable<MenuItem> MenuItems { get { return _menuItems; } }

        static Config()
        {
            foreach (int code in _settings.Keys)
                _menuItems.Add(new MenuItem((Option)code));
        }

        internal static void Load(string settingsFilePath = null)
        {
            _settingsFilePath = settingsFilePath;
            LoadSettings();
            LoadHighscores();
        }

        internal static void Save(bool settings)
        {
            if (settings)
                SaveSettings();
            SaveHighscores();
        }

        internal static int LevelInterval(int frameMultiplier, int level)
        {
            return frameMultiplier * (INTERVAL_MAX - level * INTERVAL_LEVEL);
        }

        internal static Dictionary<string, string> Settings(Option option)
        {
            int key = (int)option;
            if (!_settings.ContainsKey(key))
                return new Dictionary<string, string>();
            return _settings[key];
        }

        internal static int? GetHighscore(Option option)
        {
            int key = (int)option;
            if (_highscores.ContainsKey(key))
                return _highscores[key];
            return null;
        }

        internal static void UpdateHighscore(Option option, int score)
        {
            int key = (int)option;
            if (_highscores.ContainsKey(key) && _highscores[key] < score)
                _highscores[key] = score;
        }

        static void LoadSettings()
        {
            if (string.IsNullOrEmpty(_settingsFilePath) || !File.Exists(_settingsFilePath))
                return;

            using (var sr = File.OpenText(_settingsFilePath))
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    int code = int.Parse(line.Split(' ')[0]);
                    while (!string.IsNullOrEmpty(line) && !sr.EndOfStream)
                    {
                        line = sr.ReadLine();
                        var attr = line.Split(' ');
                        if (attr.Length >= 2 && _settings.ContainsKey(code) && _settings[code].ContainsKey(attr[0]))
                            _settings[code][attr[0]] = attr[1];
                    }
                }
        }

        static void LoadHighscores()
        {
            if (!File.Exists(FILE_HIGHSCORES))
                return;

            using (var sr = File.OpenText(FILE_HIGHSCORES))
                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split(' ');
                    if (line.Length >= 2)
                    {
                        int key = int.Parse(line[0]);
                        if (_highscores.ContainsKey(key))
                            _highscores[key] = int.Parse(line[1]);
                    }
                }
        }

        static void SaveSettings()
        {
            if (File.Exists(_settingsFilePath))
                return;

            using (var sw = File.CreateText(_settingsFilePath))
            {
                foreach (int code in _settings.Keys)
                {
                    sw.WriteLine($"{code} {(Option)code}");
                    foreach (string k in _settings[code].Keys)
                        sw.WriteLine($"{k} {_settings[code][k]}");
                    sw.WriteLine();
                }
            }
        }

        static void SaveHighscores()
        {
            using (var sw = File.CreateText(FILE_HIGHSCORES))
            {
                foreach (int code in _highscores.Keys)
                    sw.WriteLine($"{code} {_highscores[code]}");
            }
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
