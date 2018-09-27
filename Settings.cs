using System;

namespace iobloc
{
    static class Settings
    {
        internal static class Game
        {
            internal const int FRAME = 20;
        }

        internal static class Tetris
        {
            internal static readonly string[] HELP = { "Play:ARROW", "Exit:ESC", "Pause:ANY" };
            internal static readonly ConsoleKey[] KEYS = { ConsoleKey.LeftArrow, ConsoleKey.RightArrow, ConsoleKey.UpArrow, ConsoleKey.DownArrow };
            internal const int INTERVAL = 1000;
            internal const int WIDTH = 10;
            internal const int HEIGHT = 20;
        }

        internal static class Runner
        {
            internal static readonly string[] HELP = { "Play:SPACE", "Exit:ESC", "Pause:ANY" };
            internal static readonly ConsoleKey[] KEYS = { ConsoleKey.Spacebar };
            internal const int INTERVAL = 100;
            internal const int WIDTH = 20;
            internal const int HEIGHT = 6;
        }

        internal static class Helicopter
        {
            internal static readonly string[] HELP = { "Play:SPACE", "Exit:ESC", "Pause:ANY" };
            internal static readonly ConsoleKey[] KEYS = { ConsoleKey.Spacebar };
            internal const int INTERVAL = 100;
            internal const int WIDTH = 30;
            internal const int HEIGHT = 10;
        }
    }
}