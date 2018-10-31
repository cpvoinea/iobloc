using System;
using System.Collections.Generic;

namespace iobloc
{
    class SnakeBoard : BaseBoard
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

        int CP => Settings.GetColor("PlayerColor");
        int CN => Settings.GetColor("NeutralColor");

        readonly Random _random = new Random();
        readonly LinkedList<Position> _snake = new LinkedList<Position>();
        Position _point;
        int _h = 1;
        int _v = 0;
        int _nextH = 1;
        int _nextV = 0;

        internal SnakeBoard() : base(BoardType.Snake) { }

        protected override void InitializeGrid()
        {
            if (_snake.Count > 0)
            {
                Main.Clear();
                _snake.Clear();
                Score = 0;
                _h = 1;
                _v = 0;
                _nextH = 1;
                _nextV = 0;
            }

            int v = Height / 2;
            int h = Width / 2;
            for (int i = 0; i < 3; i++)
            {
                var p = new Position(v, h + i);
                _snake.AddFirst(p);
            }
            NewPoint();

            base.InitializeGrid();
        }

        protected override void ChangeGrid(bool set)
        {
            foreach (var p in _snake)
                Main[p.Row, p.Col] = set ? CP : 0;
            Main[_point.Row, _point.Col] = set ? CN : 0;

            base.ChangeGrid(set);
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
            ChangeGrid(false);
            Position next = GetNext();
            if (_snake.Contains(next))
            {
                IsWinner = false;
                IsRunning = false;
                return;
            }

            _snake.AddFirst(next);
            if (_point.Equals(next))
            {
                Score++;
                NewPoint();
            }
            else
                _snake.RemoveLast();
            ChangeGrid(true);
        }

        void NewPoint()
        {
            List<Position> candidates = new List<Position>();
            for (int r = 0; r < Height; r++)
                for (int c = 0; c < Width; c++)
                {
                    var p = new Position(r, c);
                    if (!_snake.Contains(p))
                        candidates.Add(p);
                }
            _point = candidates[_random.Next(candidates.Count)];
            candidates = null;
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
    }
}