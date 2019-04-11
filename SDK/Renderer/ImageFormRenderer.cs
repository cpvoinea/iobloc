using System;
using System.Drawing;
using System.Windows.Forms;

namespace iobloc
{
    public class ImageFormRenderer : FormRenderer
    {
        private const int SCALE_HORIZONTAL = SCALE_FONT + 6;
        private const int SCALE_VERTICAL = SCALE_FONT + 9;

        private Panel _panel;
        private Graphics g;
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
            _panel.MouseClick += ControlMouseClick;
            _panel.MouseWheel += ControlMouseWheel;

            ClientSize = new Size(Game.Border.Width * SCALE_HORIZONTAL, Game.Border.Height * SCALE_VERTICAL);
            Controls.Add(_panel);
        }

        protected override string GetMenuKey(Control control, MouseEventArgs e)
        {
            int index = e.Y / _cellHeight - 1;
            if (index < 10)
                return "D" + index;
            if (index >= Game.Panes[Pnl.Main].Text.Length)
                return "";
            string k = Game.Panes[Pnl.Main].Text[index].Substring(0, 2);
            if (k == "14")
                return "X";
            return k[0].ToString();
        }

        protected override void SetSize()
        {
            _cellWidth = _panel.Width / Game.Border.Width;
            _cellHeight = _panel.Height / Game.Border.Height;
        }

        public override void DrawPane(Pane pane)
        {
            SetSize();
            using (var g = _panel.CreateGraphics())
            {
                g.FillRectangle(Brushes.White, pane.FromCol * _cellWidth, pane.FromRow * _cellHeight, pane.Width * _cellWidth, pane.Height * _cellHeight);
                g.DrawRectangle(Pens.Black, pane.FromCol * _cellWidth, pane.FromRow * _cellHeight, pane.Width * _cellWidth, pane.Height * _cellHeight);

                if (pane.IsTextMode)
                {
                    int start = (pane.Height - pane.Text.Length) / 2;
                    if (start < 0) start = 0;
                    start += pane.FromRow;
                    for (int row = 0; row < pane.Text.Length && start + row < pane.FromRow + pane.Height; row++)
                    {
                        string text = pane.Text[row];
                        if (string.IsNullOrEmpty(text))
                            continue;
                        int left = (pane.Width - text.Length) / 2;
                        if (left < 0) left = 0;
                        left += pane.FromCol;
                        for (int col = 0; col < text.Length && left + col < pane.FromCol + pane.Width; col++)
                        {
                            int x = (left + col) * _cellWidth;
                            int y = (start + row) * _cellHeight;
                            g.DrawString(text[col].ToString(), Font, Brushes.Black, x, y);
                        }
                    }
                }
                else
                {
                    bool cursor = false;
                    int cursorX = 0, cursorY = 0;
                    for (int row = 0; row < pane.Height; row++)
                        for (int col = 0; col < pane.Width; col++)
                        {
                            int c = pane[row, col];
                            if (c != 0)
                            {
                                if (c < 0)
                                {
                                    c = -c;
                                    cursor = true;
                                }
                                int x = (pane.FromCol + col) * _cellWidth;
                                int y = (pane.FromRow + row) * _cellHeight;
                                g.FillRectangle(RenderMapping.FormBrush[c], x, y, _cellWidth, _cellHeight);
                                if (cursor)
                                {
                                    cursorX = x + 1;
                                    cursorY = y + 1;
                                }
                            }
                        }
                    if (cursor)
                        g.DrawEllipse(Pens.White, cursorX, cursorY, _cellWidth - 3, _cellHeight - 3);
                }
            }
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
            SetSize();
            using (var g = _panel.CreateGraphics())
                g.Clear(Color.FromKnownColor(KnownColor.Control));
            DrawAll(true);
        }
    }
}