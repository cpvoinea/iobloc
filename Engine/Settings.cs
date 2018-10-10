using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace iobloc
{
    static class Settings
    {
        #region Default Settings
        static Dictionary<int, Dictionary<string, string>> _settings = new Dictionary<int, Dictionary<string, string>>{
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
            }
        };
        #endregion

        internal const string SettingsFile = "iobloc.settings";
        internal static Dictionary<string, string> Get(GameOption gameOption)
        {
            return _settings[(int)gameOption];
        }

        internal static void FromFile(string fileName)
        {
            if (File.Exists(fileName))
                using (var sr = File.OpenText(fileName))
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
            using(var sw = File.CreateText(SettingsFile))
            {
                foreach(int code in _settings.Keys)
                {
                    sw.WriteLine("{0} {1}", code, (GameOption)code);
                    foreach(string k in _settings[code].Keys)
                        sw.WriteLine("{0} {1}", k, _settings[code][k]);
                    sw.WriteLine();
                }
            }
        }

        /// <summary>
        /// Game engine settings
        /// </summary>
        internal static class Game
        {
            internal const int LEVEL_MAX = 16;
            const int STEP_INTERVAL = 5;

            internal static int Level { get; set; } = 0;
            internal static int LevelInterval { get { return STEP_INTERVAL * (LEVEL_MAX - Level); } }
            internal static Dictionary<string, int> Highscore { get; } = new Dictionary<string, int>();
        }
    }
}