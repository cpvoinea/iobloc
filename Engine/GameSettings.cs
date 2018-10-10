using System;
using System.Collections.Generic;

namespace iobloc
{
    struct GameSettings
    {
        internal int Code { get; private set; }
        internal string Name { get; private set; }
        internal Dictionary<string, string> All { get; private set; }

        internal int PanelWidth { get; private set; }
        internal int PanelHeight { get; private set; }
        internal string[] Help { get; private set; }
        internal ConsoleKey[] Keys { get; private set; }
        internal int StepInterval { get; private set; }

        internal GameSettings(GameOption gameOption)
        {
            Code = (int)gameOption;
            Name = gameOption.ToString();
            All = Settings.Get((int)gameOption);

            PanelWidth = All.GetInt("PanelWidth", 10);
            PanelHeight = All.GetInt("PanelHeight", 10);
            Help = All.GetList("Help");
            List<ConsoleKey> keys = new List<ConsoleKey>();
            foreach (string k in All.GetList("Keys"))
                if (Enum.IsDefined(typeof(ConsoleKey), k))
                    keys.Add((ConsoleKey)Enum.Parse(typeof(ConsoleKey), k));
            Keys = keys.ToArray();
            StepInterval = All.GetInt("FrameMultiplier", 1) * Settings.Game.LevelInterval;
        }
    }
}