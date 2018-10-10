using System;
using System.Collections.Generic;

namespace iobloc
{
    class GameSelection
    {
        protected internal int Code { get; private set; }
        protected internal string Name { get; private set; }
        protected internal Dictionary<string, string> GameSettings { get; private set; }
        protected internal string[] Help => (GameSettings["Help"] ?? string.Empty).Split(',');
        protected internal int FrameMultiplier => int.Parse(GameSettings["FrameMultiplier"] ?? "1");
        protected internal int PanelWidth => int.Parse(GameSettings["PanelWidth"] ?? "10");
        protected internal int PanelHeight => int.Parse(GameSettings["PanelHeight"] ?? "10");

        internal GameSelection(int code)
        {
            Code = code;
            Name = ((GameOption)code).ToString();
            GameSettings = Settings.Get(code);
        }
    }
}