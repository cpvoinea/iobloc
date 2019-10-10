using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace iobloc.SDK
{
    public class FormRenderer : Form
    {
        static readonly Dictionary<int, Brush> FormBrush = new Dictionary<int, Brush> {
            {0, Brushes.Black},
            {1, Brushes.DarkBlue},
            {2, Brushes.DarkGreen},
            {3, Brushes.DarkCyan},
            {4, Brushes.DarkRed},
            {5, Brushes.DarkMagenta},
            {6, Brushes.DarkSlateGray},
            {7, Brushes.Gray},
            {8, Brushes.DarkGray},
            {9, Brushes.Blue},
            {10, Brushes.Green},
            {11, Brushes.Cyan},
            {12, Brushes.Red},
            {13, Brushes.Magenta},
            {14, Brushes.Yellow},
            {15, Brushes.White}
        }; 

        const int SCALE_FONT = 14;
        const int SCALE_HORIZONTAL = SCALE_FONT;
        const int SCALE_VERTICAL = SCALE_FONT + 8;
        readonly Brush BackgroundBrush = new SolidBrush(Color.FromKnownColor(KnownColor.Control));

        readonly IGame Game;
        Panel MainPanel;
        bool IsInitialized;
        int CellWidth, CellHeight;

        public FormRenderer(IGame game)
        {
            Game = game;
            SuspendLayout();
            InitializeControls();
            ResumeLayout(false);

            Game.Start();
            DrawAll();
        }

        void InitializeControls()
        {
            Font = new Font(Font.FontFamily, SCALE_FONT);
            Icon = Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            ShowIcon = false;
            DoubleBuffered = true;
            KeyPreview = true;
            MainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            ClientSize = new Size(Game.Width * SCALE_HORIZONTAL, Game.Height * SCALE_VERTICAL);
            Controls.Add(MainPanel);
        }

        public void DrawPane(Pane pane)
        {
            using (var g = MainPanel.CreateGraphics())
            {
                    for (int row = 0; row < pane.Height; row++)
                        for (int col = 0; col < pane.Width; col++)
                        {
                            var c = pane[row, col];

                            var b = c.Color == 0 ? BackgroundBrush : FormBrush[c.Color];
                            int x = (pane.FromCol + col) * CellWidth;
                            int y = (pane.FromRow + row) * CellHeight;
                            int xOff = col == 0 ? 1 : 0;
                            int yOff = row == 0 ? 1 : 0;
                                    g.FillRectangle(b, x + xOff, y + yOff, CellWidth - xOff, CellHeight - yOff);
                            if (c.IsCursor)
                                g.DrawLine(c.Color < 14 ? Pens.White : Pens.Black, x + CellWidth / 2, y + CellHeight / 2, x + CellWidth / 2 + 1, y + CellHeight / 2);
                        }

                g.DrawRectangle(Pens.Black, pane.FromCol * CellWidth, pane.FromRow * CellHeight, pane.Width * CellWidth, pane.Height * CellHeight);
            }
        }

        void DrawAll(bool force = false)
        {
            if (!IsInitialized)
                return;

            foreach (var p in Game.Panes)
                if (p.HasChanges || force)
                {
                    DrawPane(p);
                    p.Change(false);
                }
        }

        void HandleInput(Keys key)
        {
            Game.HandleInput(key); // handle if key is allowed
            DrawAll();
            if (!Game.IsRunning)
                Close();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Game.Stop(); // stop on Escape
                Close();
            }
            else
                HandleInput(e.KeyCode);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (!IsInitialized)
                return;

            if (Game.Width == 0 || Game.Height == 0)
                return;
            CellWidth = MainPanel.Width / Game.Width;
            CellHeight = MainPanel.Height / Game.Height;
            using (Graphics g = MainPanel.CreateGraphics())
                g.Clear(Color.FromKnownColor(KnownColor.Control));
            DrawAll(true);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            IsInitialized = true;
            OnSizeChanged(e);
        }
    }
}
