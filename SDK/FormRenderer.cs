using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace iobloc
{
    public class FormRenderer : Form, IRenderer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private static readonly Dictionary<int, Color> COLOR = new Dictionary<int, Color> {
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
        private readonly TableLayoutPanel _grid = new TableLayoutPanel();
        private readonly System.Timers.Timer _timer = new System.Timers.Timer();
        private string _key = null;

        public event LoopHandler NextInLoop;

        public FormRenderer()
        {
            InitializeComponent();
        }

        #region Windows Form Designer

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // _grid
            // 
            this._grid.Dock = DockStyle.Fill;
            this._grid.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            // 
            // FormRenderer
            // 
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new System.Drawing.Size(250, 500);
            this.Controls.Add(this._grid);
            this.Name = "FormRenderer";
            this.Text = "iobloc";
            this.ResumeLayout(false);
        }

        #endregion

        private void OnNextInLoop(object sender, System.Timers.ElapsedEventArgs args)
        {
            if (NextInLoop != null)
                NextInLoop();
        }

        public void StartLoop(int interval)
        {
            StopLoop();
            if (interval == 0)
                return;

            _timer.Interval = interval;
            _timer.Elapsed += OnNextInLoop;
            _timer.Enabled = true;
            _timer.Start();
            Application.DoEvents();
        }

        public void StopLoop()
        {
            _timer.Stop();
            _timer.Enabled = false;
            _timer.Elapsed -= OnNextInLoop;
        }

        [STAThread]
        public void DrawBorder(Border border)
        {
            _grid.ColumnCount = border.Width;
            _grid.RowCount = border.Height;
            float h = 100f / _grid.ColumnCount;
            float v = 100f / _grid.RowCount;

            _grid.ColumnStyles.Clear();
            for (int i = 0; i < _grid.ColumnCount; i++)
                _grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, h));

            _grid.RowStyles.Clear();
            for (int i = 0; i < _grid.RowCount; i++)
                _grid.RowStyles.Add(new RowStyle(SizeType.Percent, v));

            for (int i = 0; i < _grid.ColumnCount * _grid.RowCount; i++)
                _grid.Controls.Add(new Label()
                {
                    AutoSize = false,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0),
                    TextAlign = ContentAlignment.MiddleCenter
                });

            for (int row = 0; row < border.Height; row++)
                for (int col = 0; col < border.Width; col++)
                    if (border[row, col] > 0)
                    {
                        var c = _grid.GetControlFromPosition(col, row);
                        c.BackColor = Color.White;
                        c.Text = ((char)border[row, col]).ToString();
                    }

            this.Show();
        }

        public void DrawPane(Pane pane)
        {
            _grid.Invoke((MethodInvoker)delegate
            {
                SuspendLayout();
                for (int row = 0; row < pane.Height; row++)
                    for (int col = 0; col < pane.Width; col++)
                    {
                        var c = Cell(pane, row, col);
                        c.BackColor = Color.FromKnownColor(KnownColor.Control);
                        c.Text = string.Empty;
                    }

                if (pane.IsTextMode)
                {
                    for (int row = 0; row < pane.Text.Length && row < pane.Height; row++)
                        for (int col = 0; col < pane.Text[row].Length && col < pane.Width; col++)
                        {
                            var c = Cell(pane, row, col);
                            c.Text = pane.Text[row][col].ToString();
                        }
                }
                else
                {
                    for (int row = 0; row < pane.Height; row++)
                        for (int col = 0; col < pane.Width; col++)
                            if (pane[row, col] > 0)
                                Cell(pane, row, col).BackColor = COLOR[pane[row, col]];
                }
                ResumeLayout(true);
                Refresh();
            });
        }

        private Control Cell(Pane pane, int row, int col)
        {
            return _grid.GetControlFromPosition(pane.FromCol + col, pane.FromRow + row);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            _key = e.KeyCode.ToString();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            _key = null;
        }

        public string InputWait()
        {
            return _key;
        }

        public string Input()
        {
            return _key;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            StopLoop();
            _timer.Dispose();
            base.Dispose(disposing);
        }
    }
}
