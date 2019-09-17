namespace iobloc
{
    // Extend IGame with linking option where game termination transitions into another game
    interface IBaseGame<T> : IGame<T> where T : struct
    {
        // Summary:
        //      Reference to next game to run, null to terminate
        IGame<T> Next { get; }
    }
}
