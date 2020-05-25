using iobloc.NativeConsole;
using System.Collections.Generic;
using System.Text;
using static iobloc.NativeConsole.Windows.Interop.Kernel32;

namespace iobloc
{
    public class Walker : NativeGame
    {
        private const int WO = 2;

        private static readonly System.Random _rand = new System.Random();
        private Area _rectBackground;
        private Area _rectCloud;
        private Area _rectActor;

        private List<Area> _clouds = new List<Area>();
        private string _groundText;
        private Area _ground;
        private Area _actor;
        private int _bkgOff = 0;
        private int _off = 0;

        public Walker() : base(UIKey.LeftArrow + "," + UIKey.RightArrow) { }

        public override void Start()
        {
            base.Start();
            Console.Title = "Gioni Walker";

            _rectBackground = new Area(1, 1, Main.Width, Main.Height, Color.BackgroundBlue | Color.BackgroundIntensity);

            StringBuilder gnd = new StringBuilder(Main.Width);
            for (int i = 0; i < Main.Width; i++)
                gnd.Append(WalkAnimation.GND[_rand.Next(WalkAnimation.GND.Length)]);
            _groundText = gnd.ToString();
            _ground = new Area(0, Main.Height - 1, Main.Width, 1, Color.BackgroundBlue | Color.BackgroundGreen | Color.ForegroundGreen | Color.ForegroundIntensity);
            _ground.SetText(_groundText);

            _rectCloud = new Area(1, 1, 12, 4, Color.ForegroundBlue | Color.ForegroundGreen | Color.ForegroundRed | Color.BackgroundBlue | Color.BackgroundIntensity);
            _rectCloud.SetText(WalkAnimation.CLD);
            int n = Main.Width / 24;
            int w = n == 0 ? 0 : Main.Width / n;
            int h = Main.Height - 12;
            for (int i = 0; i < n; i++)
            {
                int left = 1 + i * w + _rand.Next(w);
                int top = 3 + _rand.Next(h);
                _rectCloud.Move(left, top);
                _clouds.Add(_rectCloud);
            }

            _rectActor = new Area((Main.Width - 5) / 2, Main.Height - 7, 5, 6, Color.ForegroundRed | Color.ForegroundIntensity | Color.BackgroundBlue | Color.BackgroundIntensity);
            _actor = _rectActor;
            _actor.SetText(WalkAnimation.ACT[WO]);
        }

        public override void HandleInput(string key)
        {
            int off = 0;
            switch (key)
            {
                case UIKey.LeftArrow: off--; break;
                case UIKey.RightArrow: off++; break;
            }

            _off += off;
            if (_off > WO) off = WO;
            if (_off < -WO) off = -WO;
            _actor.SetText(WalkAnimation.ACT[_off + WO]);
            if (_off == -WO || _off == WO)
            {
                _off = 0;
                char ng = WalkAnimation.GND[_rand.Next(WalkAnimation.GND.Length)];
                if (off < 0)
                    _groundText = ng + _groundText.Substring(0, Main.Width - 1);
                else if (off > 0)
                    _groundText = _groundText.Substring(1) + ng;
            }
            _ground.SetText(_groundText);

            _bkgOff += off;
            if (_bkgOff == -12 || _bkgOff == 12)
            {
                _bkgOff = 0;
                List<Area> movedClouds = new List<Area>();
                foreach (var c in _clouds)
                {
                    if (c.Left - off <= 1 || c.Left - off >= Main.Width)
                    {
                        int w = Main.Width / _clouds.Count;
                        int h = Main.Height - 12;
                        int top = 1 + _rand.Next(h);
                        int left = 1 + (off > 0 ? Main.Width - w + _rand.Next(w - 12) : _rand.Next(w));
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
        }

        public override void NextFrame()
        {
            Area area = _rectBackground;
            area.Clear();
            foreach (var c in _clouds)
                area.SetArea(c);
            area.SetArea(_ground);
            area.SetArea(_actor);

            Main.Area = area;
            Main.Change();
        }
    }
}
