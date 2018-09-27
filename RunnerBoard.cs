using System;

namespace iobloc
{
    class RunnerBoard : IBoard
    {
        readonly string[] HELP = { "Play:SPACE", "Exit:ESC", "Pause:ANY" };
        readonly ConsoleKey[] KEYS = { ConsoleKey.Spacebar };
        const int INTERVAL = 100;
        const int WIDTH = 20;
        const int HEIGHT = 6;

        public string[] Help { get { return HELP; } }
        public ConsoleKey[] Keys { get { return KEYS; } }
        public int StepInterval { get { return INTERVAL; } }
        public int Width { get { return WIDTH; } }
        public int Height { get { return HEIGHT; } }
        public int[,] Grid
        {
            get
            {
                var result = _grid.Copy(HEIGHT, WIDTH);
                int h = HEIGHT - 1 - _piece;
                result[h, 1] = result[h - 1, 1] = 1;
                return result;
            }
        }

        readonly Random _random = new Random();
        readonly int[,] _grid = new int[HEIGHT, WIDTH];
        int _piece = 0;
        int _hang = 0;
        bool _upwards;
        bool _doubleJump;
        bool _skipAdvance;
        bool _kill;

        internal RunnerBoard() { }

        public bool Action(ConsoleKey key)
        {
            if (_kill)
            {
                _kill = false;
                Restart();
            }

            if (_piece == 0)
            {
                _upwards = true;
                return true;
            }

            if (_piece > 0 && !_doubleJump)
            {
                _doubleJump = true;
                _upwards = true;
                return true;
            }

            return false;
        }

        public bool Step()
        {
            if (_kill)
                return true;

            int max = _doubleJump ? 3 : 2;
            if (_upwards && _piece < max)
                _piece++;
            else
            {
                _upwards = false;
                if (_piece == max && _hang < max)
                    _hang++;
                else
                {
                    _hang = 0;

                    if (_piece > 0)
                        _piece--;
                    else
                        _doubleJump = false;
                }
            }

            _skipAdvance = !_skipAdvance;
            if (!_skipAdvance)
            {
                Advance();
                if (Collides())
                {
                    Clear(4);
                    _kill = true;
                }
            }
            return true;
        }

        void Restart()
        {
            _piece = 0;
            _hang = 0;
            _upwards = false;
            _doubleJump = false;
            Clear(0);
        }

        void Clear(int v)
        {
            for (int i = 0; i < HEIGHT; i++)
                for (int j = 0; j < WIDTH; j++)
                    _grid[i, j] = v;
        }

        bool Collides()
        {
            int fence = 0;
            int x = HEIGHT - 1;
            while (_grid[x--, 1] > 0)
                fence++;
            return _piece < fence;
        }

        void Advance()
        {
            for (int j = 1; j < WIDTH - 2; j++)
                for (int i = HEIGHT - 1; i >= HEIGHT - 3; i--)
                    _grid[i, j] = _grid[i, j + 1];
            int fence = CreateFence();
            for (int i = 0; i < 3; i++)
                _grid[HEIGHT - 1 - i, WIDTH - 2] = i < fence ? 4 : 0;
        }

        int CreateFence()
        {
            bool hasSpace = true;
            int y = WIDTH - 3;
            while (hasSpace && y >= WIDTH - 12)
                hasSpace &= _grid[HEIGHT - 1, y--] == 0;
            if (!hasSpace)
                return 0;
            return _random.Next(4);
        }
    }
}