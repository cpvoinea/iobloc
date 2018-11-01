using System;
using System.Collections.Generic;

namespace iobloc
{
    interface IBoard
    {
        UIBorder Border { get; }
        Dictionary<string, UIPanel> Panels { get; }
        UIPanel Main { get; }
        string[] Help { get; }
        int FrameInterval { get; }
        bool IsRunning { get; set; }
        IBoard Next { get; set; }

        bool IsValidInput(string key);
        void HandleInput(string key);
        void NextFrame();
    }
}
