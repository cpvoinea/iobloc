using System;

namespace iobloc
{
    /// <summary>
    /// Game board template
    /// </summary>
    interface IBoard
    {
        /// <summary>
        /// Help text rows to display when game is paused
        /// </summary>
        string[] Help { get; }
        /// <summary>
        /// Score to display
        /// </summary>
        int Score { get; }
        /// <summary>
        /// Allowed keys which trigger game action; rest of keys pause the game
        /// </summary>
        bool Won { get; }
        ConsoleKey[] Keys { get; }
        /// <summary>
        /// Game cycle (step) duration in miliseconds
        /// </summary>
        int StepInterval { get; }
        /// <summary>
        /// Grid width
        /// </summary>
        int Width { get; }
        /// <summary>
        /// Grid height
        /// </summary>
        int Height { get; }
        /// <summary>
        /// Grid to display, including static board and player actions
        /// </summary>
        int[,] Grid { get; }
        int[] Clip { get; }
        /// <summary>
        /// Try performing an action when a valid input is provided
        /// </summary>
        /// <param name="key">input option</param>
        /// <returns>true if display needs to be refreshed, false otherwise</returns>
        bool Action(ConsoleKey key);
        /// <summary>
        /// Try performing a step when game cycle has ended
        /// </summary>
        /// <returns>true if game can continue, false if game is over</returns>
        bool Step();
    }
}