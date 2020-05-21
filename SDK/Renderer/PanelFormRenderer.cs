using System.Drawing;
using System.Windows.Forms;

namespace iobloc
{
    public class PanelFormRenderer : FormRenderer
    {
        private const int SCALE_HORIZONTAL = SCALE_FONT;
        private const int SCALE_VERTICAL = SCALE_FONT + 8;
        private readonly Brush _backgroundBrush = new SolidBrush(Color.FromKnownColor(KnownColor.Control));

        protected Panel MainPanel;
        protected int CellWidth, CellHeight;

        protected override void InitializeControls()
        {
            Icon = Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            DoubleBuffered = true;
            KeyPreview = true;
            Text = "";
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

        public override void DrawPane(Pane<PaneCell> pane)
        {
            using var g = MainPanel.CreateGraphics();
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
                        var c = pane[row, col];

                        var b = c.Color == 0 ? _backgroundBrush : RenderMapping.FormBrush[c.Color];
                        int x = (pane.FromCol + col) * CellWidth;
                        int y = (pane.FromRow + row) * CellHeight;
                        int xOff = col == 0 ? 1 : 0;
                        int yOff = row == 0 ? 1 : 0;
                        //g.FillRectangle(_backgroundBrush, x + xOff, y + yOff, CellWidth - xOff, CellHeight - yOff);
                        switch (c.Shape)
                        {
                            case CellShape.Block:
                                g.FillRectangle(b, x + xOff, y + yOff, CellWidth - xOff, CellHeight - yOff);
                                break;
                            case CellShape.Elipse:
                                g.FillEllipse(b, x + xOff, y + yOff, CellWidth - xOff, CellHeight - yOff);
                                break;
                        }
                        if (c.IsCursor)
                            g.DrawLine(c.Color < 14 ? Pens.White : Pens.Black, x + CellWidth / 2, y + CellHeight / 2, x + CellWidth / 2 + 1, y + CellHeight / 2);
                        if (c.Char != '\0')
                            g.DrawString(c.Char.ToString(), Font, c.Color < 8 && c.Color > 0 ? Brushes.White : Brushes.Black, x + 3, y + 3);
                    }
            }

            g.DrawRectangle(Pens.Black, pane.FromCol * CellWidth, pane.FromRow * CellHeight, pane.Width * CellWidth, pane.Height * CellHeight);
        }

        protected override void OnSizeChanged(System.EventArgs e)
        {
            base.OnSizeChanged(e);
            if (!IsInitialized)
                return;

            if (Game.Border.Width == 0 || Game.Border.Height == 0)
                return;
            CellWidth = MainPanel.Width / Game.Border.Width;
            CellHeight = MainPanel.Height / Game.Border.Height;
            using (Graphics g = MainPanel.CreateGraphics())
                g.Clear(Color.FromKnownColor(KnownColor.Control));
            DrawAll(true);
        }
    }
}
