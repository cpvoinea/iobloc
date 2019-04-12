using System;
using System.Drawing;
using System.Windows.Forms;

namespace iobloc
{
    public class ImageFormRenderer<T> : FormRenderer<T>
    {
        private const int SCALE_HORIZONTAL = SCALE_FONT;
        private const int SCALE_VERTICAL = SCALE_FONT + 8;
        private readonly Brush _backgroundBrush = new SolidBrush(Color.FromKnownColor(KnownColor.Control));

        protected Panel MainPanel;
        protected int CellWidth, CellHeight;

        protected override void InitializeControls()
        {
            // _panel
            MainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            MainPanel.MouseClick += ControlMouseClick;
            MainPanel.MouseWheel += ControlMouseWheel;

            ClientSize = new Size(Game.Border.Width * SCALE_HORIZONTAL, Game.Border.Height * SCALE_VERTICAL);
            Controls.Add(MainPanel);
        }

        protected override string GetMenuKey(Control control, MouseEventArgs e)
        {
            int index = e.Y / CellHeight - 1;
            if (index < 10)
                return "D" + index;
            if (index >= Game.Panes[Pnl.Main].Text.Length)
                return "";
            string k = Game.Panes[Pnl.Main].Text[index].Substring(0, 2);
            if (k == "14")
                return "X";
            return k[0].ToString();
        }

        public override void DrawPane(Pane<T> pane)
        {
            using (var g = MainPanel.CreateGraphics())
            {
                if (pane.IsTextMode)
                {
                    g.FillRectangle(_backgroundBrush, pane.FromCol * CellWidth + 1, pane.FromRow * CellHeight + 1, pane.Width * CellWidth - 2, pane.Height * CellHeight - 2);
                    for (int row = 0; row < pane.Text.Length && row < pane.Height; row++)
                    {
                        string text = pane.Text[row];
                        if (string.IsNullOrEmpty(text))
                            continue;
                        g.DrawString(text, Font, Brushes.Black, pane.FromCol * CellWidth, (pane.FromRow + row) * CellHeight);
                    }
                }
                else
                {
                    for (int row = 0; row < pane.Height; row++)
                        for (int col = 0; col < pane.Width; col++)
                        {
                            int c = int.Parse(pane[row, col].ToString());
                            var b = c == 0 ? _backgroundBrush : RenderMapping.FormBrush[c < 0 ? -c : c];
                            int x = (pane.FromCol + col) * CellWidth;
                            int y = (pane.FromRow + row) * CellHeight;
                            int xOff = col == 0 ? 1 : 0;
                            int yOff = row == 0 ? 1 : 0;
                            g.FillRectangle(b, x + xOff, y + yOff, CellWidth - xOff, CellHeight - yOff);
                            if (c < 0)
                                g.DrawEllipse(Pens.White, x + 1, y + 1, CellWidth - 3, CellHeight - 3);
                        }
                }

                g.DrawRectangle(Pens.Black, pane.FromCol * CellWidth, pane.FromRow * CellHeight, pane.Width * CellWidth, pane.Height * CellHeight);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (!IsInitialized)
                return;

            CellWidth = MainPanel.Width / Game.Border.Width;
            CellHeight = MainPanel.Height / Game.Border.Height;
            using (Graphics g = MainPanel.CreateGraphics())
                g.Clear(Color.FromKnownColor(KnownColor.Control));
            DrawAll(true);
        }
    }
}
