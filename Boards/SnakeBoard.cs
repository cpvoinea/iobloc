using System;
using System.Collections.Generic;

namespace iobloc
{
    class SnakeBoard : SinglePanelBoard
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

        int CP => _settings.GetColor("PlayerColor");
        int CE => _settings.GetColor("EnemyColor");
        int CN => _settings.GetColor("NeutralColor");

        readonly Random _random = new Random();
        readonly LinkedList<Position> _snake = new LinkedList<Position>();
        Position _point;
        int _h = 1;
        int _v = 0;
        int _nextH = 1;
        int _nextV = 0;

        internal SnakeBoard() : base(Option.Snake)
        {
            int v = _height / 2;
            int h = _width / 2;
            for (int i = 0; i < 3; i++)
            {
                var p = new Position(v, h);
                _snake.AddFirst(p);
                _main.Grid[p.Row, p.Col] = CP;
            }

            NewPoint();
        }

        public override void HandleInput(string key)
        {
            switch (key)
            {
                case "LeftArrow":
                    if (_h != 1)
                        SetMove(-1, 0);
                    break;
                case "RightArrow":
                    if (_h != -1)
                        SetMove(1, 0);
                    break;
                case "UpArrow":
                    if (_v != 1)
                        SetMove(0, -1);
                    break;
                case "DownArrow":
                    if (_v != -1)
                        SetMove(0, 1);
                    break;
            }
        }

        public override void NextFrame()
        {
            Position next = GetNext();
            if (_snake.Contains(next))
            {
                IsRunning = false;
                return;
            }

            _snake.AddFirst(next);
            _main.Grid[next.Row, next.Col] = CP;
            _main.HasChanges = true;
            if (_point.Equals(next))
            {
                Score++;
                NewPoint();
            }
            else
            {
                var last = _snake.Last.Value;
                if (last.Equals(_point))
                    _main.Grid[last.Row, last.Col] = CN;
                else
                    _main.Grid[last.Row, last.Col] = 0;
                _snake.RemoveLast();
            }
        }

        void NewPoint()
        {
            _point = new Position(_random.Next(_height), _random.Next(_width));
            _main.Grid[_point.Row, _point.Col] = CN;
            _main.HasChanges = true;
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
                nextV = _height - 1;
            else if (nextV >= _height)
                nextV = 0;
            if (nextH < 0)
                nextH = _width - 1;
            else if (nextH >= _width)
                nextH = 0;
            _h = _nextH;
            _v = _nextV;
            return new Position(nextV, nextH);
        }
    }
}