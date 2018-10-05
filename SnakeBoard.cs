using System;
using System.Collections.Generic;

namespace iobloc
{
    class SnakeBoard : IBoard
    {
        #region Settings
        public string[] Help => Settings.Snake.HELP;
        public ConsoleKey[] Keys => Settings.Snake.KEYS;
        public int Width => Settings.Snake.WIDTH;
        public int Height => Settings.Snake.HEIGHT;
        #endregion

        struct Position
        {
            internal int Row { get; set; }
            internal int Col { get; set; }

            internal Position(int row, int col)
            {
                Row = row;
                Col = col;
            }

            public override bool Equals(object obj)
            {
                Position p = (Position)obj;
                return p.Row == Row && p.Col == Col;
            }

            public override int GetHashCode()
            {
                return Col + Row * 100;
            }
        }

        readonly Random _random = new Random();
        int _score = 0;
        readonly LinkedList<Position> _snake = new LinkedList<Position>();
        Position _point;
        int _h = 1;
        int _v = 0;
        int _nextH = 1;
        int _nextV = 0;

        public int StepInterval { get { return Settings.Game.LevelInterval * Settings.Snake.INTERVALS; } }
        
        public int[,] Grid
        {
            get
            {
                var result = new int[Height, Width];
                foreach (var p in _snake)
                    result[p.Row, p.Col] = Settings.Game.ColorPlayer;
                result[_point.Row, _point.Col] = Settings.Game.ColorNeutral;
                return result;
            }
        }

        public int Score { get { return _score; } }

        public int[] Clip { get { return new[] { 0, 0, Width, Height }; } }

        internal SnakeBoard()
        {
            int v = Height / 2;
            int h = Width / 2;
            _snake.AddFirst(new Position(v, h));
            _snake.AddFirst(new Position(v, h + 1));
            _snake.AddFirst(new Position(v, h + 2));
            NewPoint();
        }

        public bool Action(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    if (_h != 1)
                        SetMove(-1, 0);
                    break;
                case ConsoleKey.RightArrow:
                    if (_h != -1)
                        SetMove(1, 0);
                    break;
                case ConsoleKey.UpArrow:
                    if (_v != 1)
                        SetMove(0, -1);
                    break;
                case ConsoleKey.DownArrow:
                    if (_v != -1)
                        SetMove(0, 1);
                    break;
            }
            return false;
        }

        public bool Step()
        {
            Position next = GetNext();
            if (_snake.Contains(next))
                return false;

            _snake.AddFirst(next);
            if (_point.Equals(next))
            {
                _score++;
                NewPoint();
            }
            else
                _snake.RemoveLast();
            return true;
        }

        void NewPoint()
        {
            _point = new Position(_random.Next(Height), _random.Next(Width));
        }

        void SetMove(int h, int v)
        {
            _nextH = h;
            _nextV = v;
        }

        Position GetNext()
        {
            Position head = _snake.First.Value;
            int nextV = head.Row + _nextV;
            int nextH = head.Col + _nextH;
            if (nextV < 0)
                nextV = Height - 1;
            else if (nextV >= Height)
                nextV = 0;
            if (nextH < 0)
                nextH = Width - 1;
            else if (nextH >= Width)
                nextH = 0;
            _h = _nextH;
            _v = _nextV;
            return new Position(nextV, nextH);
        }

        public override string ToString()
        {
            return "Snake";
        }
    }
}