using System.Collections.Generic;

namespace iobloc
{
    class Paint2 : IGame<PaneCell>
    {
        public Border Border => new Border(10, 13);
        public Dictionary<string, Pane<PaneCell>> Panes => new Dictionary<string, Pane<PaneCell>> { { Pnl.Main, new Pane<PaneCell>(0, 0, 1, 1) } };
        public int FrameInterval => 0;
        public string[] AllowedKeys => new string[] { };
        public bool IsRunning => true;

        public void HandleInput(string key)
        {
        }

        public void Start() { }
        public void Stop() { }
        public void NextFrame() { }
        public void TogglePause() { }
    }
}