namespace iobloc
{
    interface IBoard
    {
        int Width { get; }
        int Height { get; }
        int[,] Grid { get; }
        bool Move(object action);
    }
}