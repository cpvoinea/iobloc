using System.Collections.Generic;

namespace iobloc
{
    interface IBoard
    {
        UIBorder Border { get; }
        Dictionary<string, UIPanel> Panels { get; }
        int FrameInterval { get; }
        bool IsRunning { get; }

        void Start();
        void Stop();
        void TogglePause();
        bool IsValidInput(string key);
        void HandleInput(string key);
        void NextFrame();
    }
}
