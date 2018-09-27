using System;
using System.Threading;

namespace iobloc
{
    class Game
    {
        internal class EndedArgs : EventArgs
        {
            internal string Message { get; private set; }

            internal EndedArgs(string message)
            {
                Message = message;
            }
        }

        public event EventHandler Ended;

        const int FRAME = 20;

        IBoard Board { get; set; }
        IBoardUI UI { get; set; }
        bool IsRunning { get; set; }
        bool IsPaused { get; set; }
        string _message = string.Empty;
        int _frames;
        int _stepFrames;

        internal Game(IBoard board)
        {
            Board = board;
            UI = new ConsoleUI(board);
        }

        internal void Start()
        {
            _stepFrames = Board.StepInterval / FRAME;
            _frames = 0;
            UI.Reset();
            UI.Draw();
            IsRunning = true;
            while (IsRunning)
            {
                if (IsPaused)
                {
                    UI.ShowHelp();
                    while (IsPaused)
                    {
                        CheckInput();
                        Thread.Sleep(FRAME);
                    }
                    UI.Draw();
                }

                CheckInput();
                Thread.Sleep(FRAME);
                _frames++;
                if (_frames >= _stepFrames)
                {
                    Step();
                    _frames = 0;
                }
            }
            if (Ended != null)
                Ended(this, new EndedArgs(_message));
        }

        internal void Close()
        {
            UI.Restore();
        }

        void CheckInput()
        {
            if (!Console.KeyAvailable)
                return;
            var key = Console.ReadKey(true).Key;
            if (IsPaused)
            {
                IsPaused = false;
                return;
            }
            if (key == ConsoleKey.Escape)
            {
                IsRunning = false;
                _message = "Bye!";
                return;
            }

            if (Board.Keys.Contains(key))
            {
                if (Board.Action(key))
                    UI.Draw();
            }
            else
                IsPaused = true;
        }

        void Step()
        {
            if (!IsRunning || IsPaused)
                return;
            if (!Board.Step())
            {
                IsRunning = false;
                _message = "Game over.";
                return;
            }
            UI.Draw();
        }
    }
}