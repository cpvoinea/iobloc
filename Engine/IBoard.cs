using System.Collections.Generic;

namespace iobloc
{
    interface IBoard
    {
        UIBorder Border { get; }
        Dictionary<string, UIPanel> Panels { get; }
        int FrameInterval { get; }
        bool IsRunning { get; }
        string[] AllowedKeys { get; }

        void Start();
        void Stop();
        void TogglePause();
        void HandleInput(string key);
        void NextFrame();
    }
}
