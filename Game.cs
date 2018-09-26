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

        readonly Board _board;
        readonly ConsoleUI _ui;
        const int FRAME = 50;
        int _moveCount;
        int _frameCount;
        bool _isRunning;
        bool _isPaused;
        string _message = string.Empty;

        internal Game()
        {
            _board = new Board();
            _ui = new ConsoleUI(_board);
        }

        internal void Start()
        {
            _ui.Reset();
            _ui.Draw();
            _moveCount = 20;
            _frameCount = 0;
            _isRunning = true;
            while (_isRunning)
            {
                if (_isPaused)
                {
                    _ui.ShowHelp();
                    while (_isPaused)
                    {
                        CheckInput();
                        Thread.Sleep(FRAME);
                    }
                    _ui.Draw();
                }
                CheckInput();
                Thread.Sleep(FRAME);
                _frameCount++;
                if (_frameCount >= _moveCount)
                {
                    StepDown();
                    _frameCount = 0;
                }
            }
            if (Ended != null)
                Ended(this, new EndedArgs(_message));
        }

        internal void Close()
        {
            _ui.Restore();
        }

        void CheckInput()
        {
            if (!Console.KeyAvailable)
                return;
            var key = Console.ReadKey(true).Key;
            if (_isPaused)
            {
                _isPaused = false;
                return;
            }
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    if (_board.MoveLeft())
                        _ui.Draw();
                    break;
                case ConsoleKey.RightArrow:
                    if (_board.MoveRight())
                        _ui.Draw();
                    break;
                case ConsoleKey.UpArrow:
                    if (_board.Rotate())
                        _ui.Draw();
                    break;
                case ConsoleKey.DownArrow:
                    if (_board.MoveDown())
                        _ui.Draw();
                    break;
                case ConsoleKey.Escape:
                    _isRunning = false;
                    _message = "Bye!";
                    break;
                default:
                    _isPaused = true;
                    break;
            }
        }

        void StepDown()
        {
            if (!_isRunning || _isPaused)
                return;
            if (!_board.MoveDown())
            {
                if (!_board.Next())
                {
                    _isRunning = false;
                    _message = "Game over.";
                    return;
                }
            }
            _ui.Draw();
        }
    }
}