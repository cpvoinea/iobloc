using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace iobloc
{
    public class FormRunner : Form
    {
        private const int SCALE_FONT = 14;
        private const int SCALE_HORIZONTAL = SCALE_FONT + 2;
        private const int SCALE_VERTICAL = SCALE_FONT + 8;
        private static readonly Dictionary<int, Color> COLOR_MAP = new Dictionary<int, Color> {
            {0, Color.Black},
            {1, Color.DarkBlue},
            {2, Color.DarkGreen},
            {3, Color.DarkCyan},
            {4, Color.DarkRed},
            {5, Color.DarkMagenta},
            {6, Color.DarkSlateGray},
            {7, Color.Gray},
            {8, Color.DarkGray},
            {9, Color.Blue},
            {10, Color.Green},
            {11, Color.Cyan},
            {12, Color.Red},
            {13, Color.Magenta},
            {14, Color.Yellow},
            {15, Color.White}
        };
        private static readonly Dictionary<Keys, string> KEYS_MAP = new Dictionary<Keys, string>{
            {Keys.Left, UIKey.LeftArrow},
            {Keys.Right, UIKey.RightArrow},
            {Keys.Up, UIKey.UpArrow},
            {Keys.Down, UIKey.DownArrow},
            {Keys.Escape, UIKey.Escape},
            {Keys.Enter, UIKey.Enter},
            {Keys.Space, "Spacebar"}
        };

        private readonly TableLayoutPanel _grid = new TableLayoutPanel();
        private readonly Timer _timer = new Timer();
        private IGame _game = null;
        private bool _isPaused = false;

        public FormRunner(IGame game)
        {
            _game = game;
            InitializeComponent();
            Run();
        }

        #region UI

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // _grid
            // 
            this._grid.Dock = DockStyle.Fill;
            this._grid.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            this._grid.Margin = new Padding(0);
            this._grid.Padding = new Padding(0);
            this._grid.ColumnCount = _game.Border.Width;
            this._grid.RowCount = _game.Border.Height;
            float h = 100f / this._grid.ColumnCount;
            float v = 100f / this._grid.RowCount;

            this._grid.ColumnStyles.Clear();
            for (int i = 0; i < this._grid.ColumnCount; i++)
                this._grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, h));

            this._grid.RowStyles.Clear();
            for (int i = 0; i < this._grid.RowCount; i++)
                this._grid.RowStyles.Add(new RowStyle(SizeType.Percent, v));

            for (int i = 0; i < this._grid.ColumnCount * this._grid.RowCount; i++)
                this._grid.Controls.Add(new Label()
                {
                    AutoSize = false,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0),
                    Padding = new Padding(0),
                    TextAlign = ContentAlignment.MiddleCenter
                });

            for (int row = 0; row < _game.Border.Height; row++)
                for (int col = 0; col < _game.Border.Width; col++)
                    if (_game.Border[row, col] > 0)
                    {
                        var c = this._grid.GetControlFromPosition(col, row);
                        c.Text = ((char)_game.Border[row, col]).ToString();
                    }
            //
            // _timer
            //
            this._timer.Tick += FrameTimer_Tick;
            if (_game.FrameInterval > 0)
                _timer.Interval = _game.FrameInterval;
            // 
            // FormRunner
            // 
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font(Font.FontFamily, SCALE_FONT);
            this.ClientSize = new System.Drawing.Size(this._grid.ColumnCount * SCALE_HORIZONTAL, this._grid.RowCount * SCALE_VERTICAL);
            this.DoubleBuffered = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Controls.Add(this._grid);
            this.Name = "FormRunner";
            this.Text = "iObloc";
            this.ResumeLayout(false);
        }

        public void DrawPane(Pane pane)
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
                            Cell(pane, row, col).BackColor = COLOR_MAP[pane[row, col]];
            }
        }

        private void DrawAll()
        {
            _grid.SuspendLayout();
            foreach (var p in _game.Panes.Values)
                if (p.HasChanges)
                {
                    DrawPane(p);
                    p.Change(false);
                }
            _grid.ResumeLayout(true);
        }

        private Control Cell(Pane pane, int row, int col)
        {
            return _grid.GetControlFromPosition(pane.FromCol + col, pane.FromRow + row);
        }

        #endregion

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (_isPaused)
            {
                _game.TogglePause();
                DrawAll();
                _isPaused = false;
                return;
            }

            string key = KEYS_MAP.ContainsKey(e.KeyCode) ? KEYS_MAP[e.KeyCode] : e.KeyCode.ToString();

            if (key == UIKey.Escape)
            {
                _game.Stop(); // stop on Escape
                this.Close();
            }
            else if (_game.AllowedKeys.Contains(key))
            {
                _game.HandleInput(key); // handle if key is allowed
                DrawAll();
                if (!_game.IsRunning)
                    this.Close();
            }
            else
            {
                _isPaused = true;
                _game.TogglePause();
                DrawAll();
            }
        }

        private void FrameTimer_Tick(object sender, EventArgs args)
        {
            if (!_game.IsRunning)
            {
                _timer.Stop();
                this.Close();
                return;
            }

            if (_isPaused)
                return;

            _game.NextFrame();
            DrawAll();
        }

        public void Run()
        {
            _game.Start();
            DrawAll();
            if (!_game.IsRunning)
                return;

            if (_game.FrameInterval > 0)
                _timer.Start();
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
