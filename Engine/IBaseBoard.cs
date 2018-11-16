namespace iobloc
{
    // Extend IBoard with linking option where board termination transitions into another board
    interface IBaseBoard : IBoard
    {
        // Summary:
        //      Reference to next board to run, null to terminate
        IBoard Next { get; }
    }
}
