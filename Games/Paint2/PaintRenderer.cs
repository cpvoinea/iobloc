using System;
using System.Drawing;
using System.Windows.Forms;

namespace iobloc
{
    class PaintRenderer : ImageFormRenderer<PaintCell>
    {
        private Panel pnlBottom;

        public PaintRenderer() : base()
        {
            SupressToggle = true;
            Run(new Paint2());
        }

        protected override void InitializeControls()
        {
            pnlBottom = new Panel();
            MainPanel = new Panel();
            SuspendLayout();
            // pnlBottom
            pnlBottom.BorderStyle = BorderStyle.FixedSingle;
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new Point(0, 450);
            pnlBottom.Margin = new Padding(0);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(800, 50);
            pnlBottom.TabIndex = 0;
            pnlBottom.MouseClick += ControlMouseClick;
            pnlBottom.MouseWheel += ControlMouseWheel;
            // MainPanel
            MainPanel.BorderStyle = BorderStyle.FixedSingle;
            MainPanel.Dock = DockStyle.Fill;
            MainPanel.Location = new Point(0, 0);
            MainPanel.Margin = new Padding(0);
            MainPanel.Name = "MainPanel";
            MainPanel.Size = new Size(750, 400);
            MainPanel.TabIndex = 0;
            MainPanel.MouseClick += ControlMouseClick;
            MainPanel.MouseWheel += ControlMouseWheel;
            // Paint
            Controls.Add(MainPanel);
            Controls.Add(pnlBottom);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            ClientSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            WindowState = FormWindowState.Maximized;
            ResumeLayout(false);
        }

        public override void DrawPane(Pane<PaintCell> pane)
        {
        }
    }
}
