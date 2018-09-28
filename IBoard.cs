using System;

namespace iobloc
{
    interface IBoard
    {
        string[] Help { get; }
        int Score { get; }
        ConsoleKey[] Keys { get; }
        int StepInterval { get; }
        int Width { get; }
        int Height { get; }
        int[,] Grid { get; }
        bool Action(ConsoleKey key);
        bool Step();
    }
}