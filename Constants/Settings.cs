using System.Collections.Generic;

namespace iobloc
{
    sealed class Settings : Dictionary<int, Dictionary<string, string>>
    {
        public const string MenuKeys = "MenuKeys";
        public const string Highscore = "Highscore";
        public const string Help = "Help";
        public const string AllowedKeys = "AllowedKeys";
        public const string Width = "Width";
        public const string Height = "Height";
        public const string FrameMultiplier = "FrameMultiplier";
        public const string LevelThreshold = "LevelThreshold";
        public const string PlayerColor = "PlayerColor";
        public const string EnemyColor = "EnemyColor";
        public const string NeutralColor = "NeutralColor";

        public Settings()
        {
            Add(0, new Dictionary<string, string>{
                {"MenuKeys", "D0,NumPad0"},
                {"Help", "<Easy-ENTR-Hard>"},
                {"AllowedKeys", "LeftArrow,RightArrow,Enter"},
                {"Width", "16"},
                {"Height", "1"},
            });
            Add(1, new Dictionary<string, string>{
                {"MenuKeys", "D1,NumPad1"},
                {"Highscore", "0"},
                {"Help", ",,,,,,Left:LEFT,Rght:RIGHT,Rotate:UP,Speed:DOWN,Exit:ESC,Pause:ANY"},
                {"AllowedKeys", "LeftArrow,RightArrow,UpArrow,DownArrow"},
                {"FrameMultiplier", "4"},
                {"LevelThreshold", "10"},
                {"Width", "10"},
                {"Height", "20"},
            });
            Add(2, new Dictionary<string, string>{
                {"MenuKeys", "D2,NumPad2"},
                {"Highscore", "0"},
                {"Help", "Jump:UP,Dble-jump:UP,Exit:ESC,Pause:ANY"},
                {"AllowedKeys", "UpArrow"},
                {"FrameMultiplier", "1"},
                {"LevelThreshold", "5"},
                {"Width", "13"},
                {"Height", "5"},
                {"PlayerColor", "Blue"},
                {"EnemyColor", "Red"},
                {"FenceSpace", "7"},
            });
            Add(3, new Dictionary<string, string>{
                {"MenuKeys", "D3,NumPad3"},
                {"Highscore", "0"},
                {"Help", ",,Lift:UP,Exit:ESC,Pause:ANY"},
                {"AllowedKeys", "UpArrow"},
                {"FrameMultiplier", "2"},
                {"LevelThreshold", "10"},
                {"Width", "13"},
                {"Height", "7"},
                {"PlayerPosition", "1"},
                {"ObstacleSpace", "4"},
                {"PlayerColor", "Blue"},
                {"EnemyColor", "Red"},
            });
            Add(4, new Dictionary<string, string>{
                {"MenuKeys", "D4,NumPad4"},
                {"Highscore", "0"},
                {"Help", ",,,,Move:LEFT-RIGHT,Exit:ESC,Pause:ANY" },
                {"AllowedKeys", "LeftArrow,RightArrow"},
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
            });
            Add(5, new Dictionary<string, string>{
                {"MenuKeys", "D5,NumPad5"},
                {"Highscore", "0"},
                {"Help", ",,,Move:LEFT-RIGHT,Shoot:UP,Exit:ESC,Pause:ANY"},
                {"AllowedKeys", "LeftArrow,RightArrow,UpArrow"},
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
            });
            Add(6, new Dictionary<string, string>{
                {"MenuKeys", "D6,NumPad6"},
                {"Highscore", "0"},
                {"Help", ",,,Move:ARROWS,Exit:ESC,Pause:ANY"},
                {"AllowedKeys", "LeftArrow,RightArrow,UpArrow,DownArrow"},
                {"FrameMultiplier", "2"},
                {"LevelThreshold", "5"},
                {"PlayerColor", "Blue"},
                {"NeutralColor", "Gray"},
            });
            Add(7, new Dictionary<string, string>{
                {"MenuKeys", "D7,NumPad7"},
                {"Highscore", "0"},
                {"Help", "Mov:,ARRW,R:Re,strt,Ext:,ESC"},
                {"AllowedKeys", "LeftArrow,RightArrow,UpArrow,DownArrow,R"},
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
            });
            Add(8, new Dictionary<string, string>{
                {"MenuKeys", "D8,NumPad8"},
                {"Help", "Move_cursor:,LEFT-RIGHT,Take_piece:,UP_ARROW,Put_piece:,DOWN_ARROW,Exit:ESC,Pause:ANY"},
                {"AllowedKeys", "LeftArrow,RightArrow,UpArrow,DownArrow"},
                {"Height", "36"},
                {"PieceWidth", "4"},
                {"PlayerColor", "Blue"},
                {"EnemyColor", "Red"},
                {"NeutralColor", "Gray"},
            });
            Add(9, new Dictionary<string, string>{
                {"MenuKeys", "D9,NumPad9"},
                {"Help", "Move_cursor:ARROWS,Toggle_draw:ENTER,Select_color:1-7,Toggle_color:8,Write_color:9,Erase_color:0,Clear_screen:R"},
                {"AllowedKeys", "LeftArrow,RightArrow,UpArrow,DownArrow,Enter,D1,D2,D3,D4,D5,D6,D7,D8,D9,D0,R"}
            });
            Add(10, new Dictionary<string, string>{
                {"Help", ",,,Select:0-9,Exit:ESC,Help:Any"},
            });
            Add(11, new Dictionary<string, string>{
                {"MenuKeys", "F"},
                {"Help", ",,,WINNER!"},
                {"FrameMultiplier", "1"},
                {"AllowedKeys", ""},
                {"Width", "7"},
                {"Height", "7"}
            });
            Add(12, new Dictionary<string, string>{
                {"MenuKeys", "R"},
                {"Help", ",,GAME,OVER"},
                {"AllowedKeys", ""},
                {"FrameMultiplier", "1"},
                {"Width", "7"},
                {"Height", "7"}
            });
        }
    }
}
