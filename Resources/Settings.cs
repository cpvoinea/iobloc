using System.Collections.Generic;

namespace iobloc
{
    // Dictionary of settings for each game ID, a game setting is a collection of (name,value) string pairs
    sealed class Settings : Dictionary<int, Dictionary<string, string>>
    {
        public const string AssemblyPath = "AssemblyPath";
        public const string ClassName = "ClassName";
        public const string Name = "Name";
        public const string MenuKeys = "MenuKeys";
        public const string Highscore = "Highscore";
        public const string Help = "Help";
        public const string AllowedKeys = "AllowedKeys";
        public const string Width = "Width";
        public const string Height = "Height";
        public const string BlockWidth = "BlockWidth";
        public const string BlockSpace = "BlockSpace";
        public const string FrameMultiplier = "FrameMultiplier";
        public const string LevelThreshold = "LevelThreshold";
        public const string PlayerColor = "PlayerColor";
        public const string EnemyColor = "EnemyColor";
        public const string NeutralColor = "NeutralColor";

        public Settings()
        {
            // Level
            Add(0, new Dictionary<string, string>{
                {"MenuKeys", "D0,NumPad0"},
                {"Help", "<Easy-ENTR-Hard>"},
                {"AllowedKeys", "LeftArrow,RightArrow,Enter"},
                {"FrameMultiplier", "0"},
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
                {"Width", "20"},
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
                {"BlockSpace", "1"},
                {"PlayerColor", "Blue"},
                {"EnemyColor", "Red"},
                {"NeutralColor", "Gray"},
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
                {"BlockSpace", "1"},
                {"PlayerColor", "Blue"},
                {"EnemyColor", "Red"},
                {"NeutralColor", "Gray"},
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
                {"FrameMultiplier", "0"},
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
                {"Help", "Move_cursor:ARROWS,Toggle_draw:SPACE,Select_color:1-7,Toggle_shade:8,Write_color:9,Erase_color:0,Reset:R,Shape:-,Size:="},
                {"AllowedKeys", "LeftArrow,RightArrow,UpArrow,DownArrow,Spacebar,R,D0,D1,D2,D3,D4,D5,D6,D7,D8,D9,NumPad0,NumPad1,NumPad2,NumPad3,NumPad4,NumPad5,NumPad6,NumPad7,NumPad8,NumPad9,OemMinus"},
                {"FrameMultiplier", "0"},
                {"Width", "40"},
                {"Height", "20"},
            });
            // Table
            Add(9, new Dictionary<string, string>{
                {"MenuKeys", "D9,NumPad9"},
                {"Highscore", "0"},
                {"Help", "==GAMEPLAY==,|Selection:|,|LEFT-RIGHT|,|Action: UP|,|Restart: R|,+==OPTIONS=+,|Freedom: F|,|Marking: M|,|Numbers: N|,|Backgnd: B|,+----------+,| Exit: ESC|,|Pause: ANY|,============"},
                {"AllowedKeys", "LeftArrow,RightArrow,UpArrow,R,F,M,N,B"},
                {"FrameMultiplier", "0.01"},
                {"LevelThreshold", "1"},
                {"Width", "59"},
                {"Height", "40"},
                {"BlockWidth", "3"},
                {"BlockSpace", "1"},
                {"PlayerColor", "Blue"},
                {"EnemyColor", "Red"},
                {"NeutralColor", "Yellow"},
                {"MarkingColor", "White"},
                {"DarkColor", "DarkGray"},
                {"LightColor", "Gray"},
                {"AssemblyPath", "iobloc.dll"},
                {"ClassName", "iobloc.BasicAI"},
                {"AIs", "2"},
            });
            // Menu
            Add(10, new Dictionary<string, string>());
            // Fireworks
            Add(11, new Dictionary<string, string>{
                {"Help", "WINNER!"},
                {"FrameMultiplier", "1"},
                {"AllowedKeys", ""},
                {"Width", "7"},
                {"Height", "7"},
            });
            // Rain
            Add(12, new Dictionary<string, string>{
                {"Help", "GAME,OVER"},
                {"AllowedKeys", ""},
                {"FrameMultiplier", "1"},
                {"Width", "7"},
                {"Height", "7"},
            });
            // Labirint
            Add(14, new Dictionary<string, string>{
                {"MenuKeys", "L"},
                {"Help", "Move:ARROWS,Change:SPACE" },
                {"AllowedKeys", "LeftArrow,RightArrow,UpArrow,DownArrow,Spacebar"},
                {"FrameMultiplier", "0"},
                {"Width", "11"},
                {"Height", "11"},
                {"PlayerColor", "Blue"},
                {"EnemyColor", "Red"},
                {"NeutralColor", "DarkGray"},
                {"TrailColor", "Yellow"},
            });
            // Demo
            Add(20, new Dictionary<string, string>{
                 {"AssemblyPath", "iobloc.dll"},
                 {"ClassName", "iobloc.Demo"},
                 {"Name", "Demo"},
                 //{"MenuKeys", "D"},
             });
            //  Platform
            Add(21, new Dictionary<string, string>{
                 {"AssemblyPath", "iobloc.platform.dll"},
                 {"ClassName", "iobloc.platform.Platform"},
                 {"Name", "Platform"},
                 //{"MenuKeys", "P"},
             });
        }
    }
}
