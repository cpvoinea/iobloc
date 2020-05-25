namespace iobloc
{
    public class Platform : BasicGame
    {
        class Settings
        {
            public const int BlockWidth = 2;
            public const int PlayerHeight = 3;
            public const int PlatformWidth = 3;
            public const int PlayerSpeed = 3;
            public const int MaxJumpHeight = 3;
            public const int PlatformMaxHeight = 4;
            public const int PlatformMinHeight = 1;
            public const int PlatformPosition = 5;
            public const int Width = PlatformPosition + PlatformWidth + 2;
            public const int Height = PlatformMaxHeight + PlayerHeight + MaxJumpHeight;
            public const int ColorPlayer = 9;
            public const int ColorPlatform = 12;
            public const int ColorTarget = 14;
            public const int ColorWin = 6;
            public const int ColorLose = 4;
            public const int PlatformLiftTime = 1350;
            public const int FrameInterval = PlatformLiftTime / (PlatformMaxHeight - PlatformMinHeight) / PlayerSpeed;
            public const string AllowedKeys = UIKey.LeftArrow + "," + UIKey.RightArrow + "," + UIKey.UpArrow;
            public const string Help = "Touch the target,Use arrows to move";
            public const int TimeLimit = 10_000;
            public const string PnlTime = "time";
        }

        enum PlayerMove
        {
            None = 0,
            Left = 0x1000,
            Right = 0x0100,
            Up = 0x0010,
            Down = 0x0001
        }

        private Pane<PaneCell> TimePanel { get; set; }
        private int _playerPosition;
        private int _playerAltitude;
        private PlayerMove _playerDirection;
        private int _jumpHeight;
        private int _platformAltitude;
        private bool _isPlatformAscending;
        private bool _isPlayerOnPlatform;
        private int _skips;
        private int _timeRemaining;
        private bool _restart;

        public Platform() : base(Settings.Width * Settings.BlockWidth, Settings.Height, Settings.Help, Settings.FrameInterval, Settings.AllowedKeys)
        {
            TimePanel = new Pane<PaneCell>(0, Main.Width / 2, 0, Main.Width / 2 + 1);
            TimePanel.SetText(new[] { string.Empty });
            Panes.Add(Settings.PnlTime, TimePanel);
        }

        private void Initialize()
        {
            Main.Clear();
            _playerPosition = 0;
            _playerAltitude = 0;
            _playerDirection = PlayerMove.None;
            _jumpHeight = 0;
            _platformAltitude = Settings.PlatformMinHeight;
            _isPlatformAscending = true;
            _skips = Settings.PlayerSpeed;
            _timeRemaining = Settings.TimeLimit;
            _restart = false;
            Change(true);
        }

        private void Restart(bool win)
        {
            Change(true);
            int c = win ? Settings.ColorWin : Settings.ColorLose;
            for (int i = 0; i < Settings.Height; i++)
                for (int j = 0; j < Settings.Width; j++)
                    if (Main[Settings.Height - 1 - i, j * Settings.BlockWidth].Color == 0)
                        Set(i, j, c);
            Main.Change();
            _restart = true;
        }

        private void UpdateRemainingTime()
        {
            TimePanel.SetText($"{_timeRemaining / 1000 + 1,2}");
            TimePanel.Change();
        }

        private void Set(int height, int position, int color)
        {
            for (int j = 0; j < Settings.BlockWidth; j++)
                Main[Settings.Height - 1 - height, position * Settings.BlockWidth + j] = new PaneCell(color);
        }

        private void Change(bool set)
        {
            // player
            for (int i = 0; i < Settings.PlayerHeight; i++)
                Set(_playerAltitude + i, _playerPosition, set ? Settings.ColorPlayer : 0);
            // platform
            for (int i = 0; i < Settings.PlatformWidth; i++)
                Set(_platformAltitude, Settings.PlatformPosition + i, set ? Settings.ColorPlatform : 0);
            // target
            Set(Settings.Height - 2, Settings.Width - 1, set ? Settings.ColorTarget : 0);

            if (set)
                Main.Change();
        }

        private bool HasMove(PlayerMove move)
        {
            return (_playerDirection & move) == move;
        }

        private void SetMove(PlayerMove move)
        {
            _playerDirection |= move;
        }

        private void CancelMove(PlayerMove move)
        {
            if (HasMove(move))
                _playerDirection ^= move;
        }

        private bool IsOnTarget()
        {
            if (_playerPosition != Settings.Width - 1)
                return false;
            return _playerAltitude + Settings.PlayerHeight >= Settings.Height - 1;
        }

        private bool CheckCollission()
        {
            bool sameAltitude = _playerAltitude <= _platformAltitude && _playerAltitude > _platformAltitude - Settings.PlayerHeight;
            bool samePosition = _playerPosition >= Settings.PlatformPosition && _playerPosition < Settings.PlatformPosition + Settings.PlatformWidth;
            int playerTop = _playerAltitude + Settings.PlayerHeight;
            _isPlayerOnPlatform = samePosition && _playerAltitude == _platformAltitude + 1;

            if (_playerPosition == Settings.Width - 1 // right board edge
                || sameAltitude && _playerPosition == Settings.PlatformPosition - 1 // left platform edge
                )
                CancelMove(PlayerMove.Right);
            if (_playerPosition == 0 // left screen edge
                || sameAltitude && _playerPosition
                    == Settings.PlatformPosition + Settings.PlatformWidth // right platform edge
                )
                CancelMove(PlayerMove.Left);
            if (playerTop == Settings.Height // top edge
                || samePosition && playerTop == _platformAltitude // below platform
                )
                CancelMove(PlayerMove.Up);
            if (_playerAltitude == 0 || _isPlayerOnPlatform) // standing on something
                CancelMove(PlayerMove.Down);
            else // in the air
            {
                if (HasMove(PlayerMove.Up))
                {
                    if (_jumpHeight == Settings.MaxJumpHeight)
                    {
                        CancelMove(PlayerMove.Up);
                        SetMove(PlayerMove.Down);
                    }
                }
                else
                    SetMove(PlayerMove.Down);
            }

            return samePosition && sameAltitude;
        }

        private void MovePlayer()
        {
            if (CheckCollission())
            {
                Restart(false);
                return;
            }

            Change(false);
            if (HasMove(PlayerMove.Up))
            {
                _playerAltitude++;
                _jumpHeight++;
                CheckCollission();
            }
            else if (HasMove(PlayerMove.Down))
            {
                _playerAltitude--;
                if (_jumpHeight > 0)
                    _jumpHeight--;
                CheckCollission();
            }

            if (HasMove(PlayerMove.Left))
            {
                _playerPosition--;
                if (_playerAltitude == 0 || _isPlayerOnPlatform)
                    CancelMove(PlayerMove.Left);
            }
            else if (HasMove(PlayerMove.Right))
            {
                _playerPosition++;
                if (_playerAltitude == 0 || _isPlayerOnPlatform)
                    CancelMove(PlayerMove.Right);
            }
            Change(true);
        }

        private void MovePlatform()
        {
            if (_platformAltitude == Settings.PlatformMaxHeight)
                _isPlatformAscending = false;
            if (_platformAltitude == Settings.PlatformMinHeight)
                _isPlatformAscending = true;
            Change(false);
            CheckCollission();
            if (_isPlatformAscending)
            {
                _platformAltitude++;
                if (_isPlayerOnPlatform)
                    _playerAltitude++;
            }
            else
            {
                _platformAltitude--;
                if (_isPlayerOnPlatform)
                    _playerAltitude--;
            }
            Change(true);
        }

        public override void Start()
        {
            Initialize();
            base.Start();
        }

        public override void HandleInput(string key)
        {
            if (_restart)
            {
                Initialize();
                return;
            }

            switch (key)
            {
                case UIKey.LeftArrow:
                    CancelMove(PlayerMove.Right);
                    SetMove(PlayerMove.Left);
                    break;
                case UIKey.RightArrow:
                    CancelMove(PlayerMove.Left);
                    SetMove(PlayerMove.Right);
                    break;
                case UIKey.UpArrow:
                    if (_playerAltitude == 0 || _isPlayerOnPlatform) // is standing on something
                        SetMove(PlayerMove.Up);
                    break;
            }
        }

        public override void NextFrame()
        {
            if (_restart)
                return;

            _timeRemaining -= FrameInterval;
            UpdateRemainingTime();
            if (_timeRemaining <= 0)
            {
                Restart(false);
                return;
            }

            MovePlayer();
            if (IsOnTarget())
                Restart(true);
            else
            {
                _skips--;
                if (_skips == 0)
                {
                    _skips = Settings.PlayerSpeed;
                    MovePlatform();
                }
            }
        }
    }
}
