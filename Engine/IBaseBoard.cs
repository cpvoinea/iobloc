namespace iobloc
{
    /// <summary>
    /// Extend IBoard with linking option where board termination transitions into another board
    /// </summary>
    interface IBaseBoard : IBoard
    {
        /// <summary>
        /// Reference to next board to run, null to terminate
        /// </summary>
        /// <value></value>
        IBaseBoard Next { get; }
    }
}
