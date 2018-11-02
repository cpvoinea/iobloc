using System;
using System.Collections.Generic;

namespace iobloc
{
    class SnakeBoard : BaseBoard
    {
        int CP => BoardSettings.GetColor(Settings.PlayerColor);
        int CN => BoardSettings.GetColor(Settings.NeutralColor);

        readonly Random _random = new Random();
        readonly LinkedList<Position> _snake = new LinkedList<Position>();
        Position _point;
        int _h = 1;
        int _v = 0;
        int _nextH = 1;
        int _nextV = 0;

        public SnakeBoard() : base(BoardType.Snake) { }

        protected override void Initialize()
        {
            base.Initialize();

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
        }

        protected override void Change(bool set)
        {
            foreach (var p in _snake)
                Main[p.Row, p.Col] = set ? CP : 0;
            Main[_point.Row, _point.Col] = set ? CN : 0;
            if(set)
                Main.HasChanges = true;
        }

        public override void HandleInput(string key)
        {
            switch (key)
            {
                case UIKeys.LeftArrow:
                    if (_h != 1)
                        SetMove(-1, 0);
                    break;
                case UIKeys.RightArrow:
                    if (_h != -1)
                        SetMove(1, 0);
                    break;
                case UIKeys.UpArrow:
                    if (_v != 1)
                        SetMove(0, -1);
                    break;
                case UIKeys.DownArrow:
                    if (_v != -1)
                        SetMove(0, 1);
                    break;
            }
        }

        public override void NextFrame()
        {
            Change(false);
            Position next = GetNext();
            if (_snake.Contains(next))
            {
                Stop();
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
            Change(true);
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
