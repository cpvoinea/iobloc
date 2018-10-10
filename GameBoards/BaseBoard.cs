using System;

namespace iobloc
{
    abstract class BaseBoard : IBoard
    {
        protected readonly GameSettings _settings;
        protected int Width => _settings.PanelWidth;
        protected int Height => _settings.PanelHeight;
        public string Name => _settings.Name;
        public string[] Help => _settings.Help;
        public ConsoleKey[] Keys => _settings.Keys;
        public int StepInterval => _settings.StepInterval;
        public int Score { get; protected set; }
        public virtual bool Won { get; protected set; }
        public BoardFrame Frame { get; protected set; }
        public int[] Clip { get; protected set; }
        public virtual int[,] Grid { get; }

        protected BaseBoard(GameOption gameOption)
        {
            _settings = new GameSettings(gameOption);
            Frame = new BoardFrame(Width + 2, Height + 2);
            Clip = new int[] { 0, 0, Width, Height };
        }

        public abstract bool Action(ConsoleKey key);
        public abstract bool Step();
    }
}