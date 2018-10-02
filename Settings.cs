using System;

namespace iobloc
{
    static class Settings
    {
        /// <summary>
        /// Game engine settings
        /// </summary>
        internal static class Game
        {
            /// <summary>
            /// Action cycle (frame) duration in miliseconds
            /// </summary>
            internal const int FRAME = 1;
            internal const int ColorPlayer = (int)ConsoleColor.Blue;
            internal const int ColorEnemy = (int)ConsoleColor.Red;
            internal const int ColorNeutral = (int)ConsoleColor.Gray;
        }

        internal static class Tetris
        {
            internal static readonly string[] HELP = { "Play:ARROW", "Exit:ESC", "Pause:ANY" };
            internal static readonly ConsoleKey[] KEYS = { ConsoleKey.LeftArrow, ConsoleKey.RightArrow, ConsoleKey.UpArrow, ConsoleKey.DownArrow };
            internal const int INTERVAL = 125;
            internal const int WIDTH = 10;
            internal const int HEIGHT = 20;
        }

        internal static class Runner
        {
            internal static readonly string[] HELP = { "Play:SPACE", "Exit:ESC", "Pause:ANY" };
            internal static readonly ConsoleKey[] KEYS = { ConsoleKey.Spacebar };
            internal const int INTERVAL = 25;
            internal const int WIDTH = 20;
            internal const int HEIGHT = 10;
        }

        internal static class Breakout
        {
            internal static readonly string[] HELP = { "Play:ARROW", "Exit:ESC", "Pause:ANY" };
            internal static readonly ConsoleKey[] KEYS = { ConsoleKey.LeftArrow, ConsoleKey.RightArrow };
            internal const int INTERVAL = 10;
            internal const int WIDTH = 31;
            internal const int HEIGHT = 20;
            internal const int BLOCK_ROWS = 5;
            internal const int BLOCK_WIDTH = 3;
            internal const int BLOCK_SPACE = 1;
        }
    }
}