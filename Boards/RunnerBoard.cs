using System;

namespace iobloc
{
    /// <summary>
    /// Endless runner game
    /// </summary>
    class RunnerBoard : SinglePanelBoard
    {
        int CP => _settings.GetColor("PlayerColor");
        int CE => _settings.GetColor("EnemyColor");
        int JS => _settings.GetInt("JumpSpace");

        public override int Score => _highscore;

        protected readonly Random _random = new Random();
        protected int _distance;
        protected int _highscore;
        protected bool _skipAdvance;
        protected bool _kill;
        int _hang;
        bool _upwards;
        bool _doubleJump;
        protected int _score;

        protected RunnerBoard(Option option) : base(option)
        {
        }

        protected internal RunnerBoard() : this(Option.Runner)
        {
            ChangeGrid(true);
        }

        protected override void ChangeGrid(bool set)
        {
            int h = _height - 1 - _distance;
            _main.Grid[h, 1] = _main.Grid[h - 1, 1] = set ? CP : 0;
            if (set)
                _main.HasChanges = true;
        }

        public override void HandleInput(string key)
        {
            if (_kill)
            {
                _kill = false;
                Restart();
            }
            else
                Jump();
        }

        public override void NextFrame()
        {
            if (_kill)
                return;

            Move();
            _skipAdvance = !_skipAdvance;
            if (!_skipAdvance)
            {
                if (AdvanceCollides())
                {
                    Clear(CE);
                    _kill = true;
                }
            }
        }

        protected virtual bool Jump()
        {
            if (_distance == 0) // is on ground level
            {
                _upwards = true; // start moving up
                return true;
            }

            if (_distance > 0 && !_doubleJump) // is in the air and double jump is available
            {
                _doubleJump = true; // do a double jump
                _upwards = true; // start moving up again
                return true;
            }

            // if not on ground and no double jump available, no action is needed
            // air movement is perfomed in half-frame step
            return false;
        }

        protected virtual void Move()
        {
            int max = _doubleJump ? 3 : 2; // jump height limit
            if (_upwards && _distance < max) // move upwards
            {
                ChangeGrid(false);
                _distance++;
                ChangeGrid(true);
            }
            else
            {
                _upwards = false; // upward movement is done
                if (_distance == max && _hang < max) // hang in the air
                    _hang++;
                else
                {
                    _hang = 0; // hanging is done

                    if (_distance > 0) // move downwards
                    {
                        ChangeGrid(false);
                        _distance--;
                        ChangeGrid(true);
                    }
                    else
                        _doubleJump = false; // landed, double jump is available again
                }
            }
        }

        protected virtual bool AdvanceCollides()
        {
            ChangeGrid(false);
            Advance();
            int fence = 0;
            while (_main.Grid[_height - 1 - fence, 1] == CE)
                fence++;
            ChangeGrid(true);

            if (fence > 0)
            {
                if (fence > _distance)
                    return true;
                _score++;
                if (_score > _highscore)
                    _highscore = _score;
            }
            return false;
        }

        protected virtual void Restart()
        {
            _distance = 0;
            _score = 0;
            _hang = 0;
            _upwards = false;
            _doubleJump = false;
            Clear(0);
        }

        protected virtual void CreateFence()
        {
            bool hasSpace = true; // fences should not be to close together; check if there is room for new fence
            int y = _width - 4;
            while (hasSpace && y >= 0 && y >= _width - JS)
                hasSpace &= _main.Grid[_height - 1, y--] == 0;
            if (!hasSpace) // no room for new fence
                return;
            int fence = _random.Next(3); // random height, including 0
            for (int i = 0; i < 3; i++)
                _main.Grid[_height - 1 - i, _width - 2] = i < fence ? CE : 0; // set fence to grid
        }

        protected void Advance()
        {
            for (int j = 1; j < _width - 1; j++)
                for (int i = 0; i < _height; i++)
                    _main.Grid[i, j] = _main.Grid[i, j + 1];

            for (int i = 0; i < _height; i++)
                _main.Grid[i, _width - 1] = 0;
            CreateFence();
        }

        protected void Clear(int v)
        {
            for (int i = 0; i < _height; i++)
                for (int j = 0; j < _width; j++)
                    _main.Grid[i, j] = v;
            ChangeGrid(true);
        }
    }
}
