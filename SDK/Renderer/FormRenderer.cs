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
            ShowIcon = false;
            ShowInTaskbar = false;
            Name = "FormRenderer";
            Text = "iObloc";
            ResumeLayout(false);
        }

        protected abstract void InitializeControls();
        public abstract void DrawPane(Pane pane);

        public void Run(IGame game)
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

        protected void DrawAll()
        {
            SuspendLayout();
            foreach (var p in Game.Panes.Values)
                if (p.HasChanges)
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
            else if (Game.AllowedKeys.Contains(key))
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

        protected override void Dispose(bool disposing)
        {
            _timer.Stop();
            _timer.Tick -= FrameTimer_Tick;
            _timer.Dispose();
            base.Dispose(disposing);
        }
    }
}
