using System;

namespace iobloc
{
    public interface IRenderer<T> : IDisposable where T : struct
    {
        void Run(IGame<T> game);
        void DrawPane(Pane<T> pane);
    }
}