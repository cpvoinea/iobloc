using System;
using System.Drawing;
using System.Windows.Forms;

namespace iobloc
{
    class PaintRenderer : ImageFormRenderer<PaneCell>
    {
        private Panel pnlTop;
        private Panel pnlBottom;
        private Panel pnlLeft;

        public PaintRenderer() : base()
        {
            Run(new Paint2());
        }

        protected override void InitializeControls()
        {
            pnlTop = new Panel();
            pnlBottom = new Panel();
            pnlLeft = new Panel();
            MainPanel = new Panel();
            SuspendLayout();
            // pnlTop
            pnlTop.BorderStyle = BorderStyle.FixedSingle;
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Margin = new Padding(0);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(800, 50);
            pnlTop.TabIndex = 0;
            pnlTop.MouseClick += ControlMouseClick;
            pnlTop.MouseWheel += ControlMouseWheel;
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
            // pnlLeft
            pnlLeft.BorderStyle = BorderStyle.FixedSingle;
            pnlLeft.Dock = DockStyle.Left;
            pnlLeft.Location = new Point(0, 0);
            pnlLeft.Margin = new Padding(0);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Size = new Size(50, 450);
            pnlLeft.TabIndex = 0;
            pnlLeft.MouseClick += ControlMouseClick;
            pnlLeft.MouseWheel += ControlMouseWheel;
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
            Controls.Add(pnlLeft);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTop);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            ClientSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            WindowState = FormWindowState.Maximized;
            ResumeLayout(false);
        }

        public override void DrawPane(Pane<PaneCell> pane)
        {
            var r = new Random();
            var logo = Animations.Get(GameType.Logo)[0];
            using (var g = MainPanel.CreateGraphics())
                for (int i = 0; i <= Animations.SIZE_LOGO; i++)
                    for (int j = 0; j < Animations.SIZE_LOGO; j++)
                        if (logo[i, j] == 1)
                            g.FillEllipse(RenderMapping.FormBrush[r.Next(15) + 1], j * CellWidth, (i + 1) * CellHeight, CellWidth, CellHeight);
        }
    }
}
