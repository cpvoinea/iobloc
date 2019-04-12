using System;
using System.Drawing;
using System.Windows.Forms;

namespace iobloc
{
    public class ImageFormRenderer : FormRenderer
    {
        private Panel _panel;
        private bool _isInitialized;
        private int _width, _height;

        protected override Control InitializeControls()
        {
            // _panel
            _panel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            _panel.MouseClick += ControlMouseClick;
            _panel.MouseWheel += ControlMouseWheel;

            return _panel;
        }

        private void ChangeSize()
        {
            _width = _panel.Width / Game.Border.Width;
            _height = _panel.Height / Game.Border.Height;
            if (_isInitialized)
            {
                ClearPanel();
                DrawAll(true);
            }
        }

        private void ClearPanel()
        {
            using (Graphics g = _panel.CreateGraphics())
            {
                g.Clear(Color.FromKnownColor(KnownColor.Control));
            }
        }

        protected override void Toggling()
        {
            ClearPanel();
        }

        protected override string GetMenuKey(Control control, MouseEventArgs e)
        {
            int index = e.Y / _height - 1;
            if (index < 10)
                return "D" + index;
            if (index >= Game.Panes[Pnl.Main].Text.Length)
                return "";
            string k = Game.Panes[Pnl.Main].Text[index].Substring(0, 2);
            if (k == "14")
                return "X";
            return k[0].ToString();
        }

        public override void DrawPane(Pane pane)
        {
            if (!_isInitialized)
                return;

            SuspendLayout();
            using (var g = _panel.CreateGraphics())
            {
                if (pane.IsTextMode)
                {
                    for (int row = 0; row < pane.Text.Length && row < pane.Height; row++)
                    {
                        string text = pane.Text[row];
                        if (string.IsNullOrEmpty(text))
                            continue;
                        g.DrawString(text, Font, Brushes.Black, pane.FromCol * _width, (pane.FromRow + row) * _height);
                    }
                }
                else
                {
                    for (int row = 0; row < pane.Height; row++)
                        for (int col = 0; col < pane.Width; col++)
                        {
                            int c = pane[row, col];
                            var b = c == 0 ? new SolidBrush(Color.FromKnownColor(KnownColor.Control)) : RenderMapping.FormBrush[c < 0 ? -c : c];
                            int x = (pane.FromCol + col) * _width;
                            int y = (pane.FromRow + row) * _height;
                            g.FillRectangle(b, x, y, _width, _height);
                            if (c < 0)
                                g.DrawEllipse(Pens.White, x + 1, y + 1, _width - 3, _height - 3);
                        }
                }

                g.DrawRectangle(Pens.Black, pane.FromCol * _width, pane.FromRow * _height, pane.Width * _width, pane.Height * _height);
            }
            ResumeLayout(true);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            ChangeSize();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e); _isInitialized = true;
            DrawAll(true);
        }
    }
}
