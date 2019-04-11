using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace iobloc
{
    public abstract class FormRenderer : Form, IRenderer
    {
        protected const int SCALE_FONT = 14;

        private readonly Timer _timer = new Timer();
        protected IGame Game = null;
        protected bool IsPaused = false;

        public FormRenderer()
        {
            SuspendLayout();
            //
            // _timer
            //
            _timer.Tick += FrameTimer_Tick;
            // 
            // FormRenderer
            // 
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font(Font.FontFamily, SCALE_FONT);
            DoubleBuffered = true;
            Name = "FormRenderer";
            Text = "";
            ResumeLayout(false);
        }

        protected abstract void InitializeControls();
        public abstract void DrawPane(Pane pane);
        protected virtual string GetMenuKey(Control sender, MouseEventArgs e) { return null; }
        protected virtual void SetSize() { }

        public void Run(IGame game)
        {
            Game = game;
            SuspendLayout();
            if (Game.FrameInterval > 0)
                _timer.Interval = Game.FrameInterval;
            InitializeControls();
            //if (!(Game is Menu))
            //{
            //    ControlBox = false;
            //    ShowIcon = false;
            //    TopMost = true;
            //    WindowState = FormWindowState.Maximized;
            //}
            ResumeLayout(false);

            Game.Start();
            if (!Game.IsRunning)
                return;

            if (Game.FrameInterval > 0)
                _timer.Start();
        }

        protected void DrawAll(bool force = false)
        {
            SuspendLayout();
            foreach (var p in Game.Panes.Values)
                if (p.HasChanges || force)
                {
                    DrawPane(p);
                    p.Change(false);
                }
            ResumeLayout(true);
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
            else
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

        protected override void Dispose(bool disposing)
        {
            _timer.Stop();
            _timer.Tick -= FrameTimer_Tick;
            _timer.Dispose();
            base.Dispose(disposing);
        }
    }
}
