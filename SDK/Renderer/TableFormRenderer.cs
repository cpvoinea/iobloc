using System.Windows.Forms;
using System.Drawing;

namespace iobloc
{
    public class TableFormRenderer : FormRenderer
    {
        private const int SCALE_HORIZONTAL = SCALE_FONT + 2;
        private const int SCALE_VERTICAL = SCALE_FONT + 8;

        private TableLayoutPanel _grid;

        protected override void InitializeControls()
        {
            // 
            // _grid
            // 
            _grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                Margin = new Padding(0),
                Padding = new Padding(0),
                ColumnCount = Game.Border.Width,
                RowCount = Game.Border.Height
            };
            float h = 100f / _grid.ColumnCount;
            float v = 100f / _grid.RowCount;

            _grid.ColumnStyles.Clear();
            for (int i = 0; i < _grid.ColumnCount; i++)
                _grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, h));

            _grid.RowStyles.Clear();
            for (int i = 0; i < _grid.RowCount; i++)
                _grid.RowStyles.Add(new RowStyle(SizeType.Percent, v));

            for (int i = 0; i < _grid.ColumnCount * _grid.RowCount; i++)
            {
                var fill = new Label()
                {
                    AutoSize = false,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0),
                    Padding = new Padding(0),
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                fill.MouseClick += OnMouseDown;
                fill.MouseWheel += OnMouseWheel;

                _grid.Controls.Add(fill);
            }

            for (int row = 0; row < Game.Border.Height; row++)
                for (int col = 0; col < Game.Border.Width; col++)
                    if (Game.Border[row, col] > 0)
                    {
                        var c = _grid.GetControlFromPosition(col, row);
                        c.Text = ((char)Game.Border[row, col]).ToString();
                    }

            ClientSize = new Size(_grid.ColumnCount * SCALE_HORIZONTAL, _grid.RowCount * SCALE_VERTICAL);
            Controls.Add(_grid);
        }

        public override void DrawPane(Pane pane)
        {
            for (int row = 0; row < pane.Height; row++)
                for (int col = 0; col < pane.Width; col++)
                {
                    var c = Cell(pane, row, col);
                    c.BackColor = Color.FromKnownColor(KnownColor.Control);
                    c.Text = string.Empty;
                }

            if (pane.IsTextMode)
            {
                int start = (pane.Height - pane.Text.Length) / 2;
                if (start < 0) start = 0;
                for (int row = 0; row < pane.Text.Length && start + row < pane.Height; row++)
                {
                    string text = pane.Text[row];
                    if (string.IsNullOrEmpty(text))
                        continue;
                    int left = (pane.Width - text.Length) / 2;
                    for (int col = 0; col < text.Length && left + col < pane.Width; col++)
                    {
                        var c = Cell(pane, start + row, left + col);
                        c.Text = text[col].ToString();
                    }
                }
            }
            else
            {
                for (int row = 0; row < pane.Height; row++)
                    for (int col = 0; col < pane.Width; col++)
                        if (pane[row, col] > 0)
                            Cell(pane, row, col).BackColor = RenderMapping.FormColor[pane[row, col]];
            }
        }

        private Control Cell(Pane pane, int row, int col)
        {
            return _grid.GetControlFromPosition(pane.FromCol + col, pane.FromRow + row);
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (IsPaused)
            {
                Pause(false);
                return;
            }

            string key = null;
            if (Game is Menu) // in menu, get clicked item
            {
                int g = _grid.GetPositionFromControl(sender as Control).Row - 1;
                key = "D" + (g < 10 ? g.ToString() : "");
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

        private void OnMouseWheel(object sender, MouseEventArgs e)
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
    }
}