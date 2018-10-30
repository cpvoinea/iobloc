using System;
using System.Collections.Generic;
using System.IO;

namespace iobloc
{
    static class Serializer
    {
        internal static Dictionary<int, Dictionary<string, string>> Settings = new Dictionary<int, Dictionary<string, string>>{
            {0, // Level
                new Dictionary<string, string>{
                    {"Help", "<Easy-ENTR-Hard>"},
                    {"Keys", "LeftArrow,RightArrow,Enter"},
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
                    {"PlayerColor", "Blue"},
                    {"NeutralColor", "Gray"},
                }
            },
            {7, // Sokoban
                new Dictionary<string, string>{
                    {"Help", "Mov:,ARRW,R:Re,strt,Ext:,ESC"}, //"Restrt:R,Move:ARW,Exit:ESC,Paus:ANY"},
                    {"Keys", "LeftArrow,RightArrow,UpArrow,DownArrow,R"},
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
            {9, // Paint
                new Dictionary<string, string>{
                }
            },
            {10, // Menu
                new Dictionary<string, string>{
                    {"Help", ""},
                    {"Keys", "D0,D1,D2,D3,D4,D5,D6,D7,D8,D9,L,F,R"},
                    {"MenuItems", "0:Level,1:Tetris,2:Runner,3:Helicopt,4:Breakout,5:Invaders,6:Snake,7:Sokoban,8:Table,9:Paint"},
                }
            },
            {11, // Log
                new Dictionary<string, string>{
                }
            },
            {12, // Fireworks
                new Dictionary<string, string>{
                }
            },
            {13, // RainingBlood
                new Dictionary<string, string>{
                }
            }
        };
        internal static Dictionary<int, int> Highscores = new Dictionary<int, int> { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }, { 6, 0 }, { 7, 0 }, { 8, 0 } };
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

        internal static void UpdateHighscore(int key, int score)
        {
            if (Highscores.ContainsKey(key) && Highscores[key] < score)
                Highscores[key] = score;
        }

        internal static int GetLevelInterval(int frameMultiplier, int level)
        {
            return frameMultiplier * (200 - 10 * level);
        }

        internal static string GetMessage(bool? win)
        {
            if (!win.HasValue)
                return "Quit";
            if (!win.Value)
                return "Loser:(";
            return "WINNER!";
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
