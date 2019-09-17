using System;
using System.Windows.Forms;
using System.Drawing;

namespace iobloc
{
    public abstract class FormRenderer : Form, IRenderer<PaneCell>
    {
        protected const int SCALE_FONT = 14;

        private readonly Timer _timer = new Timer();
        protected IGame<PaneCell> Game;
        protected bool IsPaused;
        protected bool IsInitialized;
        protected bool SupressToggle;

        public FormRenderer()
        {
            // _timer
            _timer.Tick += FrameTimer_Tick;
            // FormRenderer
            Font = new Font(Font.FontFamily, SCALE_FONT);
            Name = "FormRenderer";
        }

        protected abstract void InitializeControls();
        public abstract void DrawPane(Pane<PaneCell> pane);
        protected virtual string GetMenuKey(Control sender, MouseEventArgs e) { return null; }

        public void Run(IGame<PaneCell> game)
        {
            Game = game;
            SuspendLayout();
            if (Game.FrameInterval > 0)
                _timer.Interval = Game.FrameInterval;
            InitializeControls();
            ResumeLayout(false);

            Game.Start();
            DrawAll();
            if (!Game.IsRunning)
                return;

            if (Game.FrameInterval > 0)
                _timer.Start();
        }

        protected void DrawAll(bool force = false)
        {
            if (!IsInitialized)
                return;

            foreach (var p in Game.Panes.Values)
                if (p.HasChanges || force)
                {
                    DrawPane(p);
                    p.Change(false);
                }
        }

        protected void Pause(bool pause)
        {
            Game.TogglePause();
            DrawAll();
            IsPaused = pause;
        }

        protected void HandleInput(string key)
        {
            Game.HandleInput(key); // handle if key is allowed
            DrawAll();
            if (!Game.IsRunning)
                Close();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (IsPaused)
            {
                Pause(false);
                return;
            }

            string key = RenderMapping.FormKey.ContainsKey(e.KeyCode) ? RenderMapping.FormKey[e.KeyCode] : e.KeyCode.ToString();

            if (key == UIKey.Escape)
            {
                Game.Stop(); // stop on Escape
                Close();
            }
            else if (Serializer.Contains(Game.AllowedKeys, key))
                HandleInput(key);
            else if (!SupressToggle)
                Pause(true);
        }

        private void FrameTimer_Tick(object sender, EventArgs e)
        {
            if (!Game.IsRunning)
            {
                _timer.Stop();
                Close();
                return;
            }

            if (IsPaused)
                return;

            Game.NextFrame();
            DrawAll();
        }

        protected virtual void ControlMouseClick(object sender, MouseEventArgs e)
        {
            if (IsPaused)
            {
                Pause(false);
                return;
            }

            string key = null;
            if (Game is Menu) // in menu, get clicked item
            {
                key = GetMenuKey(sender as Control, e);
            }
            else
                switch (e.Button)
                {
                    case MouseButtons.Left: key = UIKey.UpArrow; break;
                    case MouseButtons.Right: key = UIKey.DownArrow; break;
                    case MouseButtons.Middle: key = UIKey.Enter; break;
                }

            if (key != null && Serializer.Contains(Game.AllowedKeys, key))
                HandleInput(key);
        }

        protected virtual void ControlMouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta == 0)
                return;

            if (IsPaused)
            {
                Pause(false);
                return;
            }

            string key = e.Delta < 0 ? UIKey.LeftArrow : UIKey.RightArrow;
            if (Serializer.Contains(Game.AllowedKeys, key))
                HandleInput(key);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            IsInitialized = true;
            OnSizeChanged(e);
        }

        protected override void Dispose(bool disposing)
        {
            _timer.Stop();
            _timer.Tick -= FrameTimer_Tick;
            _timer.Dispose();
            base.Dispose(disposing);
        }
    }
}
