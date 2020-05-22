using System;
using System.Collections.Generic;
using System.Text;

namespace iobloc.Ascio
{
    class Walker : Game
    {
        private readonly Random _rand = new Random();
        private Screen _rectBackground;
        private Screen _rectCloud;
        private Screen _rectActor;

        private List<Screen> _clouds = new List<Screen>();
        private string _groundText;
        private Screen _ground;
        private Screen _actor;
        private int _bkgOff = 0;
        private int _off = 0;

        protected override void Init()
        {
            Console.Title = "Gioni Walker";

            _rectBackground = new Screen(0, 0, Width, Height, CharAttr.BACKGROUND_BLUE | CharAttr.BACKGROUND_INTENSITY);

            StringBuilder gnd = new StringBuilder(Width);
            for (int i = 0; i < Width; i++)
                gnd.Append(WalkAnimation.GND[_rand.Next(WalkAnimation.GND.Length)]);
            _groundText = gnd.ToString();
            _ground = new Screen(0, Height - 1, Width, 1, CharAttr.BACKGROUND_BLUE | CharAttr.BACKGROUND_GREEN | CharAttr.FOREGROUND_GREEN | CharAttr.FOREGROUND_INTENSITY);
            _ground.SetText(_groundText);

            _rectCloud = new Screen(0, 0, 12, 4, CharAttr.FOREGROUND_WHITE | CharAttr.BACKGROUND_BLUE | CharAttr.BACKGROUND_INTENSITY);
            _rectCloud.SetText(WalkAnimation.CLD);
            int n = Width / 24;
            int w = Width / n;
            int h = Height - 12;
            for (int i = 0; i < n; i++)
            {
                int left = i * w + _rand.Next(w);
                int top = _rand.Next(h) + 2;
                _rectCloud.Move(left, top);
                _clouds.Add(_rectCloud);
            }

            _rectActor = new Screen((Width - 5) / 2, Height - 7, 5, 6, CharAttr.FOREGROUND_RED | CharAttr.FOREGROUND_INTENSITY | CharAttr.BACKGROUND_BLUE | CharAttr.BACKGROUND_INTENSITY);
            _actor = _rectActor;
            _actor.SetText(WalkAnimation.ACT[2]);
        }

        protected override Screen GetScreen()
        {
            Screen screen = _rectBackground;
            screen.Clear();
            foreach (var c in _clouds)
                screen.SetScreen(c);
            screen.SetScreen(_ground);
            screen.SetScreen(_actor);

            return screen;
        }

        protected override bool DoAction()
        {
            if (!base.DoAction() || !LastAction.HasValue)
                return false;

            int off = 0;
            switch (LastAction.Value)
            {
                case ConsoleKey.LeftArrow: off--; break;
                case ConsoleKey.RightArrow: off++; break;
            }

            _off += off;
            if (_off > 2) off = 2;
            if (_off < -2) off = -2;
            _actor.SetText(WalkAnimation.ACT[_off + 2]);
            if (_off == -2 || _off == 2)
            {
                _off = 0;
                char ng = WalkAnimation.GND[_rand.Next(WalkAnimation.GND.Length)];
                if (off < 0)
                    _groundText = ng + _groundText.Substring(0, Width - 1);
                else if (off > 0)
                    _groundText = _groundText.Substring(1) + ng;
            }
            _ground.SetText(_groundText);

            _bkgOff += off;
            if (_bkgOff == -12 || _bkgOff == 12)
            {
                _bkgOff = 0;
                List<Screen> movedClouds = new List<Screen>();
                foreach (var c in _clouds)
                {
                    if (c.Left - off <= 0 || c.Left - off >= Width - 1)
                    {
                        int w = Width / _clouds.Count;
                        int h = Height - 12;
                        int top = _rand.Next(h);
                        int left = off > 0 ? Width - w + _rand.Next(w - 12) : _rand.Next(w);
                        _rectCloud.Move(left, top);
                        movedClouds.Add(_rectCloud);
                    }
                    else
                    {
                        c.Move(c.Left - off, c.Top);
                        movedClouds.Add(c);
                    }
                }

                _clouds = movedClouds;
            }

            return true;
        }
    }
}
