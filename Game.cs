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

        const int FRAME = 50;

        IBoard Board{get;set;}
        IBoardUI UI{get;set;}
        int MoveCount{get;set;}
        int FrameCount{get;set;}
        bool IsRunning{get;set;}
        bool IsPaused{get;set;}
        string _message = string.Empty;

        internal Game()
        {
            Board = new TetrisBoard();
            UI = new ConsoleUI(Board);
        }

        internal void Start()
        {
            UI.Reset();
            UI.Draw();
            MoveCount = 20;
            FrameCount = 0;
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
                FrameCount++;
                if (FrameCount >= MoveCount)
                {
                    Step();
                    FrameCount = 0;
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
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    if (Board.Move(TetrisBoard.MoveType.MoveLeft))
                        UI.Draw();
                    break;
                case ConsoleKey.RightArrow:
                    if (Board.Move(TetrisBoard.MoveType.MoveRight))
                        UI.Draw();
                    break;
                case ConsoleKey.UpArrow:
                    if (Board.Move(TetrisBoard.MoveType.Rotate))
                        UI.Draw();
                    break;
                case ConsoleKey.DownArrow:
                    if (Board.Move(TetrisBoard.MoveType.MoveDown))
                        UI.Draw();
                    break;
                case ConsoleKey.Escape:
                    IsRunning = false;
                    _message = "Bye!";
                    break;
                default:
                    IsPaused = true;
                    break;
            }
        }

        void Step()
        {
            if (!IsRunning || IsPaused)
                return;
            if (!Board.Move(TetrisBoard.MoveType.MoveDown))
            {
                if (!Board.Move(TetrisBoard.MoveType.Next))
                {
                    IsRunning = false;
                    _message = "Game over.";
                    return;
                }
            }
            UI.Draw();
        }
    }
}