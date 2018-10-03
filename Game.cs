using System;
using System.Threading;

namespace iobloc
{
    /// <summary>
    /// Console game engine. Runs game cycles in a loop and detects player input
    /// do {
    ///     has input?
    ///         yes -> perform action -> refresh display if action is successful
    ///     next frame
    ///     action cycle ended?
    ///         yes -> perform step -> end game if game over
    /// } until game ends
    /// </summary>
    class Game
    {
        /// <summary>
        /// Message to handle on game ended events
        /// </summary>
        internal class EndedArgs : EventArgs
        {
            /// <summary>
            /// Message to handle on game ended events
            /// </summary>
            internal string Message { get; private set; }

            internal EndedArgs(string message)
            {
                Message = message;
            }
        }

        /// <summary>
        /// Triggers when game has ended
        /// </summary>
        public event EventHandler Ended;

        /// <summary>
        /// Game rules and display grid
        /// </summary>
        /// <value></value>
        IBoard Board { get; set; }
        /// <summary>
        /// Handle display of Board
        /// </summary>
        /// <value></value>
        ConsoleUI UI { get; set; }
        /// <summary>
        /// Action cycle is started
        /// </summary>
        /// <value></value>
        bool IsRunning { get; set; }
        bool IsPaused { get; set; }
        /// <summary>
        /// Return message
        /// </summary>
        string _message = string.Empty;
        /// <summary>
        /// Count frames in the current game cycle
        /// </summary>
        int _frames;
        /// <summary>
        /// Max number of frames where game cycle ends and a step is performed
        /// </summary>
        int _stepFrames;

        /// <summary>
        /// Initialize a game
        /// </summary>
        /// <param name="board">Game rules and display grid</param>
        internal Game(IBoard board)
        {
            Board = board;
            UI = new ConsoleUI(board);
        }

        /// <summary>
        /// Start action cycle loop and perform step when game cycle duration is reached
        /// </summary>
        internal void Start()
        {
            _stepFrames = Board.StepInterval / Settings.Game.FRAME;
            _frames = 0;
            UI.Reset();
            UI.Draw();
            IsRunning = true;
            while (IsRunning) // action loop
            {
                if (IsPaused) // halt gameplay in paused mode
                {
                    UI.ShowHelp(); // display help
                    while (IsPaused) // wait to unpause
                    {
                        CheckInput();
                        Thread.Sleep(Settings.Game.FRAME);
                    }
                    UI.Draw(); // restore game display
                }

                CheckInput(); // handle any input
                Thread.Sleep(Settings.Game.FRAME);
                _frames++;
                if (_frames >= _stepFrames) // perform step when game cycle ended
                {
                    Step();
                    _frames = 0;
                }
            }
            // exit loop when game has ended
            if (Ended != null)
                Ended(this, new EndedArgs(_message));
        }

        /// <summary>
        /// Cleanup of resouces
        /// </summary>
        internal void Close()
        {
            UI.Restore();
        }

        /// <summary>
        /// When game loop is running, handle input in each frame and check if display needs refresh
        /// </summary>
        void CheckInput()
        {
            if (!Console.KeyAvailable)
                return;
            var key = Console.ReadKey(true).Key;
            if (IsPaused) // unpause on any key
            {
                IsPaused = false;
                return;
            }
            if (key == ConsoleKey.Escape) // exit on ESC
            {
                IsRunning = false;
                _message = string.Format("Exit {0} with score {1}", Board, Board.Score);
                return;
            }

            if (Board.Keys.Contains(key)) // perform action on game keys
            {
                if (Board.Action(key))
                    UI.Draw(Board.Clip); // redraw if action is performed
            }
            else // or pause on any other key
                IsPaused = true;
        }

        /// <summary>
        /// When game cycle is complete, perform step and check if game ended
        /// </summary>
        void Step()
        {
            if (!IsRunning || IsPaused)
                return;
            if (!Board.Step()) // perform step until game ended
            {
                IsRunning = false;
                _message = string.Format("Game over {0} scored {1}", Board, Board.Score);
                return;
            }
            UI.Draw(); // re-draw after each step
        }
    }
}