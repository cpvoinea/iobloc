using System;

namespace iobloc
{
    public interface IRenderer<T> : IDisposable
    {
        void Run(IGame<T> game);
        void DrawPane(Pane<T> pane);
    }
}