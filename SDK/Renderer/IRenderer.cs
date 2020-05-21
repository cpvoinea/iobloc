namespace iobloc
{
    public interface IRenderer<T> : System.IDisposable where T : struct
    {
        void Run(IGame<T> game);
        void DrawPane(Pane<T> pane);
    }
}