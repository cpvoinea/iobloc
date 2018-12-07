using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace iobloc
{
    // Entry point to application, handle initialization and disposal
    class Engine : Form
    {
        private const int SCALE_FONT = 18;
        private const int SCALE_HORIZONTAL = 8;
        private const int SCALE_VERTICAL = 12;
        private TableLayoutPanel _panel;
        private Button _btnSettings;
        private Button _btnConsole;
        private Button _btnForm;
        private Button _btnExit;
        private LinkLabel _link;

        public Engine()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // _panel
            // 
            this._panel = new TableLayoutPanel();
            this._panel.Dock = DockStyle.Fill;
            this._panel.ColumnCount = 1;
            this._panel.RowCount = 5;
            this._panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            float p = 100f / this._panel.RowCount;
            for (int i = 0; i < this._panel.RowCount; i++)
                this._panel.RowStyles.Add(new RowStyle(SizeType.Percent, p));
            //
            // buttons
            //
            this._btnSettings = new Button();
            this._btnSettings.Dock = DockStyle.Fill;
            this._btnSettings.Text = "&Settings";
            this._btnSettings.Click += btnSettings_Click;
            this._panel.Controls.Add(this._btnSettings);
            this._btnConsole = new Button();
            this._btnConsole.Dock = DockStyle.Fill;
            this._btnConsole.Text = "&Console";
            this._btnConsole.Click += btnConsole_Click;
            this._panel.Controls.Add(this._btnConsole);
            this._btnForm = new Button();
            this._btnForm.Dock = DockStyle.Fill;
            this._btnForm.Text = "&Form";
            this._btnForm.Click += btnForm_Click;
            this._panel.Controls.Add(this._btnForm);
            this._btnExit = new Button();
            this._btnExit.Dock = DockStyle.Fill;
            this._btnExit.Text = "E&xit";
            this._btnExit.Click += btnExit_Click;
            //
            // _link
            //
            this._link = new LinkLabel();
            this._link.Dock = DockStyle.Fill;
            this._link.Text = "iObloc v3.0";
            this._link.TextAlign = ContentAlignment.MiddleCenter;
            this._link.Click += link_Click;
            this._panel.Controls.Add(_link);
            this._panel.Controls.Add(this._btnExit);
            // 
            // FormRunner
            // 
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font(Font.FontFamily, SCALE_FONT);
            this.ClientSize = new System.Drawing.Size(SCALE_FONT * SCALE_HORIZONTAL, SCALE_FONT * SCALE_VERTICAL);
            this.AcceptButton = _btnForm;
            this.CancelButton = _btnExit;
            this.ShowIcon = false;
            this.ShowInTaskbar = true;
            this.Controls.Add(this._panel);
            this.Name = "Engine";
            this.Text = "iObloc";
            this.ResumeLayout(false);
        }

        private void btnSettings_Click(object sender, EventArgs args)
        {
            try
            {
                if (!File.Exists(Serializer.SettingsFileName))
                    Serializer.Save();
                StartProcess(Serializer.SettingsFileName);
            }
            catch { }
        }

        private void btnConsole_Click(object sender, EventArgs args)
        {
            Start(false);
        }

        private void btnForm_Click(object sender, EventArgs args)
        {
            Start(true);
        }

        private void btnExit_Click(object sender, EventArgs args)
        {
            Application.Exit();
        }

        private void link_Click(object sender, EventArgs args)
        {
            StartProcess("https://github.com/cpvoinea/iobloc/releases");
        }

        private void StartProcess(string command)
        {
            try { Process.Start(new ProcessStartInfo("cmd", $"/c start {command}") { CreateNoWindow = true }); }
            catch { }
        }

        private void Start(bool isForms)
        {
            try
            {
                Serializer.Load();

                IGame menu = Serializer.GetGame((int)GameType.Menu);
                IGame game = menu;
                while (game != null)
                {
                    if (isForms)
                    {
                        Cursor = Cursors.WaitCursor;
                        var form = new FormRunner(game);
                        Cursor = Cursors.Default;
                        form.ShowDialog(this);
                    }
                    else
                    {
                        ConsoleRunner.Initialize();
                        ConsoleRunner.Run(game);
                        ConsoleRunner.Exit();
                    }

                    if (game is IBaseGame) // base game selected
                        game = (game as IBaseGame).Next; // continue to next game
                    else if (game != menu) // return to menu
                        game = menu;
                    else // exit
                        game = null;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                try { Serializer.Save(); }
                catch { }
            }
        }
    }
}
