using System;
using System.Drawing;
using System.Windows.Forms;

namespace iobloc
{
    public class ImageFormRenderer : FormRenderer
    {
        private Panel _panel;

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

        protected override string GetMenuKey(Control control, MouseEventArgs e)
        {
            int index = e.Y / (_panel.Height / Game.Border.Height) - 1;
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
            int cellWidth = _panel.Width / Game.Border.Width;
            int cellHeight = _panel.Height / Game.Border.Height;
            using (var g = _panel.CreateGraphics())
            {
                //g.FillRectangle(Brushes.White, pane.FromCol * cellWidth, pane.FromRow * cellHeight, pane.Width * cellWidth, pane.Height * cellHeight);
                g.DrawRectangle(Pens.Black, pane.FromCol * cellWidth, pane.FromRow * cellHeight, pane.Width * cellWidth, pane.Height * cellHeight);

                if (pane.IsTextMode)
                {
                    for (int row = 0; row < pane.Text.Length && row < pane.Height; row++)
                    {
                        string text = pane.Text[row];
                        if (string.IsNullOrEmpty(text))
                            continue;
                        g.DrawString(text, Font, Brushes.Black, pane.FromCol * cellWidth, (pane.FromRow + row) * cellHeight);
                    }
                }
                else
                {
                    for (int row = 0; row < pane.Height; row++)
                        for (int col = 0; col < pane.Width; col++)
                        {
                            int c = pane[row, col];
                            if (c != 0)
                            {
                                int x = (pane.FromCol + col) * cellWidth;
                                int y = (pane.FromRow + row) * cellHeight;
                                g.FillRectangle(RenderMapping.FormBrush[c < 0 ? -c : c], x, y, cellWidth, cellHeight);
                                if (c < 0)
                                    g.DrawEllipse(Pens.White, x + 1, y + 1, cellWidth - 3, cellHeight - 3);
                            }
                        }
                }
            }

            _panel.Show();
        }
    }
}
