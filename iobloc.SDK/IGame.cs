using System.Collections.Generic;
using System.Windows.Forms;

namespace iobloc.SDK
{
    public interface IGame
    {
        int Width { get; }
        int Height { get; }
        List<Pane> Panes { get; }
        bool IsRunning { get; }

        void Start();
        void Stop();
        void HandleInput(Keys key);
    }
}
