using System.Collections.Generic;

namespace iobloc
{
    public class Snake : BaseGame
    {
        int CP, CN;
        readonly System.Random _random = new System.Random();
        readonly LinkedList<Position> _snake = new LinkedList<Position>();
        Position _point;
        int _h;
        int _v;
        int _nextH;
        int _nextV;

        public Snake() : base(GameType.Snake) { }

        protected override void InitializeSettings()
        {
            base.InitializeSettings();
            CP = Serializer.GetColor(GameSettings, Settings.PlayerColor);
            CN = Serializer.GetColor(GameSettings, Settings.NeutralColor);
        }

        protected override void Initialize()
        {
            base.Initialize();
            if (IsInitialized)
            {
                Main.Clear();
                _snake.Clear();
                _v = 0;
                _nextV = 0;
            }

            int v = Height / 2;
            int h = Width / 2;
            _h = BlockWidth;
            _nextH = BlockWidth;
            for (int i = 0; i < 3; i++)
            {
                var p = new Position(v, h + i * BlockWidth);
                _snake.AddFirst(p);
            }
            NewPoint();
            Change(true);
        }

        protected override void Change(bool set)
        {
            foreach (var p in _snake)
                for (int i = 0; i < BlockWidth; i++)
                    Main[p.Row, p.Col + i] = new PaneCell(set ? CP : 0);
            for (int i = 0; i < BlockWidth; i++)
                Main[_point.Row, _point.Col + i] = new PaneCell(set ? CN : 0);
            base.Change(set);
        }

        public override void HandleInput(string key)
        {
            switch (key)
            {
                case UIKey.LeftArrow:
                    if (_h != BlockWidth)
                        SetMove(-BlockWidth, 0);
                    break;
                case UIKey.RightArrow:
                    if (_h != -BlockWidth)
                        SetMove(BlockWidth, 0);
                    break;
                case UIKey.UpArrow:
                    if (_v != 1)
                        SetMove(0, -1);
                    break;
                case UIKey.DownArrow:
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
                Lose();
            else
            {
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
        }

        void NewPoint()
        {
            List<Position> candidates = new List<Position>();
            for (int r = 0; r < Height; r++)
                for (int c = 0; c < Width; c += BlockWidth)
                {
                    var p = new Position(r, c);
                    if (!_snake.Contains(p))
                        candidates.Add(p);
                }
            _point = candidates[_random.Next(candidates.Count)];
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
                nextH = Width - BlockWidth;
            else if (nextH >= Width)
                nextH = 0;
            _h = _nextH;
            _v = _nextV;
            return new Position(nextV, nextH);
        }
    }
}
