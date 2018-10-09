using System;
using System.Collections.Generic;

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
            internal const int COLOR_PLAYER = (int)ConsoleColor.Blue;
            internal const int COLOR_ENEMY = (int)ConsoleColor.Red;
            internal const int COLOR_NEUTRAL = (int)ConsoleColor.Gray;
            internal const int LEVEL_MAX = 16;
            const int STEP_INTERVAL = 5;

            internal static int Level { get; set; } = 0;
            internal static int LevelInterval { get { return STEP_INTERVAL * (LEVEL_MAX - Level); } }
            internal static Dictionary<string, int> Highscore { get; } = new Dictionary<string, int>();
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

        internal static class Sokoban
        {
            internal static readonly string[] HELP = { "Restrt:R", "Move:ARW", "Exit:ESC", "Paus:ANY" };
            internal static readonly ConsoleKey[] KEYS = { ConsoleKey.LeftArrow, ConsoleKey.RightArrow, ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.R };
            internal const int INTERVALS = 100;
            internal const int WIDTH = 4 * BLOCK_WIDTH;
            internal const int HEIGHT = 6;
            internal const int BLOCK_WIDTH = 2;
            internal const int LEVEL_SCORE = 20;
            internal const int MARK_PLAYER = 9;
            internal const int MARK_WALL = 12;
            internal const int MARK_BLOCK = 7;
            internal const int MARK_TARGET = 10;
            internal const int MARK_TARGET_BLOCK = 2;
            internal const int MARK_TARGET_PLAYER = 1;
        }
    }
}