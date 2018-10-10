using System;
using System.Collections.Generic;

namespace iobloc
{
    class SnakeBoard : IBoard
    {
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

        const int W = Settings.Snake.WIDTH;
        const int H = Settings.Snake.HEIGHT;
        public string[] Help => Settings.Snake.HELP;
        public ConsoleKey[] Keys => Settings.Snake.KEYS;
        public bool Won => false;
        public int StepInterval { get; private set; } = Settings.Game.LevelInterval * Settings.Snake.INTERVALS;
        public BoardFrame Frame { get; private set; } = new BoardFrame(W + 2, H + 2);
        public int[] Clip { get; private set; } = new[] { 0, 0, W, H };
        public int Score { get; private set; }
        public int[,] Grid
        {
            get
            {
                var result = new int[H, W];
                foreach (var p in _snake)
                    result[p.Row, p.Col] = Settings.Game.COLOR_PLAYER;
                result[_point.Row, _point.Col] = Settings.Game.COLOR_NEUTRAL;
                return result;
            }
        }

        readonly Random _random = new Random();
        readonly LinkedList<Position> _snake = new LinkedList<Position>();
        Position _point;
        int _h = 1;
        int _v = 0;
        int _nextH = 1;
        int _nextV = 0;

        internal SnakeBoard()
        {
            int v = H / 2;
            int h = W / 2;
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
                Score++;
                NewPoint();
            }
            else
                _snake.RemoveLast();
            return true;
        }

        void NewPoint()
        {
            _point = new Position(_random.Next(H), _random.Next(W));
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
                nextV = H - 1;
            else if (nextV >= H)
                nextV = 0;
            if (nextH < 0)
                nextH = W - 1;
            else if (nextH >= W)
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