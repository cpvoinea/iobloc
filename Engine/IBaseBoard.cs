namespace iobloc
{
    interface IBaseBoard : IBoard
    {
        IBaseBoard Next { get; }
    }
}
