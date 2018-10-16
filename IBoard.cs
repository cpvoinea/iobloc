using System;

namespace iobloc
{
    interface IBoard : IDisposable
    {
        Border Border { get; }
        Panel[] Panels { get; }
        Panel MainPanel { get; }
        string[] Help { get; }

        bool IsRunning { get; set; }
        int FrameInterval { get; set; }

        void NextFrame();
        bool IsValidInput(int key);
        void HandleInput(int key);
    }
}
