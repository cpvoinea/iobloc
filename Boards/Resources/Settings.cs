using System.Collections.Generic;

namespace iobloc
{
    class Settings
    {
        static Dictionary<int, Dictionary<string, string>> _all = new Dictionary<int, Dictionary<string, string>>{
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
                    {"LevelThreshold", "5"},
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
                    {"LevelThreshold", "10"},
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
                    {"LevelThreshold", "15"},
                    {"Width", "25"},
                    {"Height", "15"},
                    {"PlayerColor", "Blue"},
                    {"EnemyColor", "Red"},
                    {"NeutralColor", "Gray"},
                    {"BlockWidth", "5"},
                    {"BlockSpace", "0"},
                    {"BlockRows", "3"},
                }
            },
            {5, // Invaders
                new Dictionary<string, string>{
                    {"Help", ",,,Move:LEFT-RIGHT,Shoot:UP,Exit:ESC,Pause:ANY"},
                    {"Keys", "LeftArrow,RightArrow,UpArrow"},
                    {"FrameMultiplier", "1"},
                    {"LevelThreshold", "15"},
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
                    {"Help", "Mov:,ARRW,R:Re,strt,Ext:,ESC"},
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
                    {"Help", "Move_cursor:ARROWS,Toggle_draw:ENTER,Select_color:1-7,Toggle_color:8,Write_color:9,Erase_color:0,Clear_screen:R"},
                    {"Keys", "LeftArrow,RightArrow,UpArrow,DownArrow,Enter,D1,D2,D3,D4,D5,D6,D7,D8,D9,D0,R"}
                }
            },
            {10, // Menu
                new Dictionary<string, string>{
                    {"Help", ",,,Select:0-9,Exit:ESC,Help:Any"},
                    {"Keys", "D0,D1,D2,D3,D4,D5,D6,D7,D8,D9,NumPad0,NumPad1,NumPad2,NumPad3,NumPad4,NumPad5,NumPad6,NumPad7,NumPad8,NumPad9,F,R"},
                    {"MenuItems", "0:Level,1:Tetris,2:Runner,3:Helicopt,4:Breakout,5:Invaders,6:Snake,7:Sokoban,8:Table,9:Paint"},
                }
            },
            {11, // Fireworks
                new Dictionary<string, string>{
                    {"Help", ",,,WINNER!"},
                    {"FrameMultiplier", "1"},
                    {"Keys", ""},
                    {"Width", "7"},
                    {"Height", "7"}
                }
            },
            {12, // RainingBlood
                new Dictionary<string, string>{
                    {"Help", ",,GAME,OVER"},
                    {"Keys", ""},
                    {"FrameMultiplier", "1"},
                    {"Width", "7"},
                    {"Height", "7"}
                }
            }
        };

        public Dictionary<string, string> this[int key] => _all[key];
        public IEnumerable<int> Keys { get { return _all.Keys; } }

        public bool ContainsKey(int key) { return _all.ContainsKey(key); }
    }
}
