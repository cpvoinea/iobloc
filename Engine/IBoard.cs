using System;
using System.Collections.Generic;

namespace iobloc
{
    interface IBoard
    {
        UIBorder UIBorder { get; }
        Dictionary<string, UIPanel> Panels { get; }
        UIPanel Main { get; }
        string[] Help { get; }
        int FrameInterval { get; }
        int? Highscore { get; }
        int Score { get; }
        int Level { get; }
        bool? Win { get; }
        bool IsRunning { get; set; }

        bool IsValidInput(string key);
        void HandleInput(string key);
        void NextFrame();
    }
}
