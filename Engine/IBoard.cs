using System;
using System.Collections.Generic;

namespace iobloc
{
    interface IBoard
    {
        Border Border { get; }
        Dictionary<string, Panel> Panels { get; }
        Panel MainPanel { get; }
        string[] Help { get; }
        int FrameInterval { get; }
        int? Highscore { get; }
        int Score { get; }
        int Level { get; }
        bool? Win { get; }
        bool IsRunning { get; set; }

        void NextFrame();
        bool IsValidInput(string key);
        void HandleInput(string key);
    }
}
