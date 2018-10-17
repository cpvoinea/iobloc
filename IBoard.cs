using System;

namespace iobloc
{
    interface IBoard : IDisposable
    {
        Border Border { get; }
        Panel[] Panels { get; }
        Panel MainPanel { get; }
        string[] Help { get; }
        int FrameInterval { get; }
        bool IsRunning { get; set; }

        void NextFrame();
        bool IsValidInput(string key);
        void HandleInput(string key);
    }
}
