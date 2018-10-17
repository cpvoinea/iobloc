using System;
using System.Collections.Generic;
using System.IO;

namespace iobloc
{
    static class Config
    {
        #region Settings
        static Dictionary<int, Dictionary<string, string>> _settings = new Dictionary<int, Dictionary<string, string>>{
            {0, // Level
                new Dictionary<string, string>{
                    {"Help", "<<Easy   Hard>>"},
                    {"Keys", "LeftArrow,RightArrow,Enter"},
                    {"FrameMultiplier", "5"},
                    {"Width", "16"},
                    {"Height", "1"},
                }
            },
            {1, // Tetris
                new Dictionary<string, string>{
                    {"Help", "Play:ARROW,Exit:ESC,Pause:ANY"},
                    {"Keys", "LeftArrow,RightArrow,UpArrow,DownArrow"},
                    {"FrameMultiplier", "4"},
                    {"Width", "10"},
                    {"Height", "20"},
                }
            },
            {2, // Runner
                new Dictionary<string, string>{
                    {"Help", "Jump:SPACE,Exit:ESC,Pause:ANY"},
                    {"Keys", "Spacebar"},
                    {"FrameMultiplier", "1"},
                    {"Width", "20"},
                    {"Height", "10"},
                    {"PlayerColor", "Blue"},
                    {"EnemyColor", "Red"},
                    {"JumpSpace", "12"},
                }
            },
            {3, // Helicopter
                new Dictionary<string, string>{
                    {"Help", "Lift:SPACE,Exit:ESC,Pause:ANY"},
                    {"Keys", "Spacebar"},
                    {"FrameMultiplier", "2"},
                    {"Width", "20"},
                    {"Height", "10"},
                    {"PlayerColor", "Blue"},
                    {"EnemyColor", "Red"},
                }
            },
            {4, // Breakout
                new Dictionary<string, string>{
                    {"Help", "Move:ARROWS,Exit:ESC,Pause:ANY" },
                    {"Keys", "LeftArrow,RightArrow"},
                    {"FrameMultiplier", "2"},
                    {"Width", "31"},
                    {"Height", "20"},
                    {"PlayerColor", "Blue"},
                    {"EnemyColor", "Red"},
                    {"NeutralColor", "Gray"},
                    {"BlockWidth", "3"},
                    {"BlockSpace", "1"},
                    {"BlockRows", "5"},
                }
            },
            {5, // Invaders
                new Dictionary<string, string>{
                    {"Help", "Move:ARROWS,Shoot:SPACE,Exit:ESC,Pause:ANY"},
                    {"Keys", "LeftArrow,RightArrow,Spacebar"},
                    {"FrameMultiplier", "1"},
                    {"Width", "31"},
                    {"Height", "20"},
                    {"PlayerColor", "Blue"},
                    {"EnemyColor", "Red"},
                    {"NeutralColor", "Gray"},
                    {"AlienWidth", "3"},
                    {"AlienSpace", "1"},
                    {"AlienRows", "3"},
                    {"AlienCols", "5"},
                    {"BulletSpeed", "2"},
                }
            },
            {6, // Snake
                new Dictionary<string, string>{
                    {"Help", "Move:ARROWS,Exit:ESC,Pause:ANY"},
                    {"Keys", "LeftArrow,RightArrow,UpArrow,DownArrow"},
                    {"FrameMultiplier", "2"},
                    {"Width", "20"},
                    {"Height", "20"},
                    {"PlayerColor", "Blue"},
                    {"NeutralColor", "Gray"},
                }
            },
            {7, // Sokoban
                new Dictionary<string, string>{
                    {"Help", "R:Re,strt"}, //"Restrt:R,Move:ARW,Exit:ESC,Paus:ANY"},
                    {"Keys", "LeftArrow,RightArrow,UpArrow,DownArrow,R"},
                    {"FrameMultiplier", "5"},
                    {"Width", "4"},
                    {"Height", "6"},
                    {"BlockWidth", "1"},
                    {"WinScore", "100"},
                    {"WallColor", "DarkGray"},
                    {"PlayerColor", "Red"},
                    {"BlockColor", "Blue"},
                    {"TargetColor", "DarkYellow"},
                    {"TargetPlayerColor", "DarkRed"},
                    {"TargetBlockColor", "DarkBlue"},
                }
            },
            {9, // Log
                new Dictionary<string, string>{
                    {"Keys", "Enter"},
                    {"FrameMultiplier", "5"},
                    {"Width", "32"},
                    {"Height", "24"},
                }
            },
        };
        #endregion

        internal const int MENU_LEN_KEY = 1;
        internal const int MENU_LEN_NAME = 10;
        internal const int MENU_LEN_INFO = 3;
        internal const string MENU_INPUT_TEXT = "Option (ESC to exit)";
        internal const string INPUT_EXIT = "Escape";
        internal const char BLOCK = (char)BoxGraphics.BlockFull;
        internal const int LEVEL_MAX = 16;

        const int FRAME_INTERVAL = 5;
        const string CONFIG_FILE = "iobloc.settings";

        internal static int Level { get; set; }
        internal static int LevelInterval { get { return FRAME_INTERVAL * (LEVEL_MAX - Level); } }

        static string _configFilePath;
        readonly static List<MenuItem> _menuItems = new List<MenuItem>();

        internal static IEnumerable<MenuItem> MenuItems { get { return _menuItems; } }

        static Config()
        {
            foreach (int code in Enum.GetValues(typeof(Option)))
            {
                var o = (Option)code;
                var item = new MenuItem(o, o.ToString());
                // if (o == Option.Log)
                //     item.Visible = false;
                _menuItems.Add(item);
            }
        }

        internal static void Load(string configFilePath)
        {
            _configFilePath = configFilePath ?? CONFIG_FILE;
            if (!File.Exists(_configFilePath))
                return;

            using (var sr = File.OpenText(_configFilePath))
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

        internal static void Save()
        {
            using (var sw = File.CreateText(_configFilePath))
            {
                foreach (int code in _settings.Keys)
                {
                    sw.WriteLine("{0} {1}", code, (Option)code);
                    foreach (string k in _settings[code].Keys)
                        sw.WriteLine("{0} {1}", k, _settings[code][k]);
                    sw.WriteLine();
                }
            }
        }

        internal static Dictionary<string, string> Settings(Option option)
        {
            if (!_settings.ContainsKey((int)option))
                return new Dictionary<string, string>();
            return _settings[(int)option];
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
