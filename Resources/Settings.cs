using System.Collections.Generic;

namespace iobloc
{
    // Dictionary of settings for each board ID, a board setting is a collection of (name,value) string pairs
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
    }
}
