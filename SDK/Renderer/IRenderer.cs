using System;

namespace iobloc
{
    public interface IRenderer : IDisposable
    {
        void Run(IGame game);
        void DrawPane(Pane pane);
    }
}