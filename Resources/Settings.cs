using System.Collections.Generic;

namespace iobloc
{
    /// <summary>
    /// Dictionary of settings for each board ID, a board setting is a collection of (name,value) string pairs
    /// </summary>
    sealed class Settings : Dictionary<int, Dictionary<string, string>>
    {
        public const string MenuKeys = "MenuKeys";
        public const string Highscore = "Highscore";
        public const string Help = "Help";
        public const string AllowedKeys = "AllowedKeys";
        public const string Width = "Width";
        public const string Height = "Height";
        public const string BlockWidth = "BlockWidth";
        public const string FrameMultiplier = "FrameMultiplier";
        public const string LevelThreshold = "LevelThreshold";
        public const string PlayerColor = "PlayerColor";
        public const string EnemyColor = "EnemyColor";
        public const string NeutralColor = "NeutralColor";

        /// <summary>
        /// A common and customizable level value across all boards.
        /// During Initialization, board levels will be set to this value.
        /// </summary>
        /// <value>0-15</value>
        public static int MasterLevel { get; set; }

        /// <summary>
        /// Default settings for built-in boards
        /// </summary>
        public Settings()
        {
            // Level
            Add(0, new Dictionary<string, string>{
                {"MenuKeys", "D0,NumPad0"},
                {"Help", "<Easy-ENTR-Hard>"},
                {"AllowedKeys", "LeftArrow,RightArrow,Enter"},
                {"Width", "16"},
                {"Height", "1"},
            });
            // Tetris
            Add(1, new Dictionary<string, string>{
                {"MenuKeys", "D1,NumPad1"},
                {"Highscore", "0"},
                {"Help", "Left:LEFT,Rght:RIGHT,Rotate:UP,Speed:DOWN,Exit:ESC,Pause:ANY"},
                {"AllowedKeys", "LeftArrow,RightArrow,UpArrow,DownArrow"},
                {"FrameMultiplier", "4"},
                {"LevelThreshold", "10"},
                {"Width", "10"},
                {"Height", "20"},
                {"BlockWidth", "2"},
            });
            // Runner
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
            // Helicopter
            Add(3, new Dictionary<string, string>{
                {"MenuKeys", "D3,NumPad3"},
                {"Highscore", "0"},
                {"Help", "Lift:UP,Exit:ESC,Pause:ANY"},
                {"AllowedKeys", "UpArrow"},
                {"FrameMultiplier", "2"},
                {"LevelThreshold", "10"},
                {"Width", "13"},
                {"Height", "7"},
                {"BlockWidth", "2"},
                {"ObstacleSpace", "4"},
                {"PlayerColor", "Blue"},
                {"EnemyColor", "Red"},
            });
            // Breakout
            Add(4, new Dictionary<string, string>{
                {"MenuKeys", "D4,NumPad4"},
                {"Highscore", "0"},
                {"Help", "Move:LEFT-RIGHT,Exit:ESC,Pause:ANY" },
                {"AllowedKeys", "LeftArrow,RightArrow"},
                {"FrameMultiplier", "1.5"},
                {"Width", "29"},
                {"Height", "15"},
                {"BlockWidth", "5"},
                {"PlayerColor", "Blue"},
                {"EnemyColor", "Red"},
                {"NeutralColor", "Gray"},
                {"BlockSpace", "1"},
                {"BlockRows", "3"},
            });
            // Invaders
            Add(5, new Dictionary<string, string>{
                {"MenuKeys", "D5,NumPad5"},
                {"Highscore", "0"},
                {"Help", "Move:LEFT-RIGHT,Shoot:UP,Exit:ESC,Pause:ANY"},
                {"AllowedKeys", "LeftArrow,RightArrow,UpArrow"},
                {"FrameMultiplier", "0.75"},
                {"Width", "19"},
                {"Height", "10"},
                {"BlockWidth", "3"},
                {"PlayerColor", "Blue"},
                {"EnemyColor", "Red"},
                {"NeutralColor", "Gray"},
                {"AlienSpace", "1"},
                {"AlienRows", "3"},
                {"AlienCols", "3"},
            });
            // Snake
            Add(6, new Dictionary<string, string>{
                {"MenuKeys", "D6,NumPad6"},
                {"Highscore", "0"},
                {"Help", "Move:ARRWS,Exit:ESC,Pause:ANY"},
                {"AllowedKeys", "LeftArrow,RightArrow,UpArrow,DownArrow"},
                {"FrameMultiplier", "2"},
                {"Width", "20"},
                {"Height", "10"},
                {"BlockWidth", "2"},
                {"LevelThreshold", "4"},
                {"PlayerColor", "Blue"},
                {"NeutralColor", "Gray"},
            });
            // Sokoban
            Add(7, new Dictionary<string, string>{
                {"MenuKeys", "D7,NumPad7"},
                {"Highscore", "0"},
                {"Help", "Mov:,ARRW,R:Re,strt,Ext:,ESC"},
                {"AllowedKeys", "LeftArrow,RightArrow,UpArrow,DownArrow,R"},
                {"Width", "8"},
                {"Height", "6"},
                {"BlockWidth", "2"},
                {"WinScore", "50"},
                {"WallColor", "DarkGray"},
                {"PlayerColor", "Red"},
                {"BlockColor", "Blue"},
                {"TargetColor", "DarkYellow"},
                {"TargetPlayerColor", "DarkRed"},
                {"TargetBlockColor", "DarkBlue"},
            });
            // Paint
            Add(8, new Dictionary<string, string>{
                {"MenuKeys", "D8,NumPad8"},
                {"Help", "Move_cursor:ARROWS,Toggle_draw:ENTER,Select_color:1-7,Toggle_shade:8,Write_color:9,Erase_color:0,Reset:R"},
                {"AllowedKeys", "LeftArrow,RightArrow,UpArrow,DownArrow,Enter,R,D0,D1,D2,D3,D4,D5,D6,D7,D8,D9,NumPad0,NumPad1,NumPad2,NumPad3,NumPad4,NumPad5,NumPad6,NumPad7,NumPad8,NumPad9"},
                {"Width", "80"},
                {"Height", "40"},
                {"BlockWidth", "2"},
            });
            // Table
            Add(9, new Dictionary<string, string>{
                {"MenuKeys", "D9,NumPad9"},
                {"Help", "Move:,LEFT-RIGHT,Take/Put:,UP_ARROW,Exit:ESC,Pause:ANY"},
                {"AllowedKeys", "LeftArrow,RightArrow,UpArrow"},
                {"FrameMultiplier", "10"},
                {"Width", "59"},
                {"Height", "36"},
                {"BlockWidth", "4"},
                {"PlayerColor", "Blue"},
                {"EnemyColor", "Red"},
                {"NeutralColor", "Yellow"},
                {"HighlightColor", "DarkGray"},
            });
            // Menu
            Add(10, new Dictionary<string, string>{
                {"Help", "Select:0-9,Exit:ESC,Help:Any"},
            });
            // Fireworks
            Add(11, new Dictionary<string, string>{
                {"Help", "WINNER!"},
                {"FrameMultiplier", "1"},
                {"AllowedKeys", ""},
                {"Width", "7"},
                {"Height", "7"}
            });
            // Rain
            Add(12, new Dictionary<string, string>{
                {"Help", "GAME,OVER"},
                {"AllowedKeys", ""},
                {"FrameMultiplier", "1"},
                {"Width", "7"},
                {"Height", "7"}
            });
        }
    }
}
