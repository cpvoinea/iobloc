using System;
using System.Drawing;
using System.Windows.Forms;

namespace iobloc
{
    public class ImageFormRenderer : FormRenderer
    {
        private Panel _panel;
        private int _cellWidth;
        private int _cellHeight;

        protected override void InitializeControls()
        {
            //
            // _panel
            //
            _panel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            Controls.Add(_panel);
            ControlBox = false;
            ShowInTaskbar = true;
            WindowState = FormWindowState.Maximized;
        }

        public override void DrawPane(Pane pane)
        {
            if (pane.IsTextMode)
                return;
            _panel.BackColor = Color.White;

            Graphics g = _panel.CreateGraphics();
            int cursorX = 0, cursorY = 0;
            for (int row = 0; row < pane.Height; row++)
                for (int col = 0; col < pane.Width; col++)
                {
                    int c = pane[row, col];
                    if (c != 0)
                    {
                        bool cursor = false;
                        if (c < 0)
                        {
                            c = -c;
                            cursor = true;
                        }
                        g.FillRectangle(RenderMapping.FormBrush[c], col * _cellWidth, row * _cellHeight, _cellWidth, _cellHeight);
                        if (cursor)
                        {
                            cursorX = col * _cellWidth;
                            cursorY = row * _cellHeight;
                        }
                    }
                }
            g.DrawEllipse(Pens.White, cursorX, cursorY, _cellWidth, _cellHeight);
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
            _cellWidth = _panel.Width / Game.Border.Width;
            _cellHeight = _panel.Height / Game.Border.Height;
        }
    }
}