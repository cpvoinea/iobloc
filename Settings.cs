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
            const int INTERVAL = 25;

            internal static int Level { get; set; } = 10;
            internal static int LevelInterval { get { return INTERVAL * (12 - Level); } }
        }

        internal static class Tetris
        {
            internal static readonly string[] HELP = { "Play:ARROW", "Exit:ESC", "Pause:ANY" };
            internal static readonly ConsoleKey[] KEYS = { ConsoleKey.LeftArrow, ConsoleKey.RightArrow, ConsoleKey.UpArrow, ConsoleKey.DownArrow };
            internal const int INTERVALS = 4;
            internal const int WIDTH = 10;
            internal const int HEIGHT = 20;
        }

        internal static class Runner
        {
            internal static readonly string[] HELP = { "Jump the fences!", "Double-jump once", "Jump:SPACE", "Exit:ESC", "Pause:ANY" };
            internal static readonly ConsoleKey[] KEYS = { ConsoleKey.Spacebar };
            internal const int INTERVALS = 1;
            internal const int WIDTH = 20;
            internal const int HEIGHT = 10;
        }

        internal static class Breakout
        {
            internal static readonly string[] HELP = { "Move:ARROWS", "Exit:ESC", "Pause:ANY" };
            internal static readonly ConsoleKey[] KEYS = { ConsoleKey.LeftArrow, ConsoleKey.RightArrow };
            internal const int INTERVALS = 2;
            internal const int WIDTH = 31;
            internal const int HEIGHT = 20;
            internal const int BLOCK_ROWS = 5;
            internal const int BLOCK_WIDTH = 3;
            internal const int BLOCK_SPACE = 1;
        }

        internal static class Invaders
        {
            internal static readonly string[] HELP = { "Move:ARROWS", "Shoot:SPACE", "Exit:ESC", "Pause:ANY" };
            internal static readonly ConsoleKey[] KEYS = { ConsoleKey.LeftArrow, ConsoleKey.RightArrow, ConsoleKey.Spacebar };
            internal const int INTERVALS = 1;
            internal const int WIDTH = 31;
            internal const int HEIGHT = 20;
            internal const int ALIEN_WIDTH = 3;
            internal const int ALIEN_SPACE = 1;
            internal const int ALIEN_ROWS = 3;
            internal const int ALIEN_COLS = 5;
            internal const int BULLET_SPEED = 2;
        }

        internal static class Snake
        {
            internal static readonly string[] HELP = { "Move:ARROWS", "Exit:ESC", "Pause:ANY" };
            internal static readonly ConsoleKey[] KEYS = { ConsoleKey.LeftArrow, ConsoleKey.RightArrow, ConsoleKey.UpArrow, ConsoleKey.DownArrow };
            internal const int INTERVALS = 2;
            internal const int WIDTH = 20;
            internal const int HEIGHT = 20;
        }
    }
}