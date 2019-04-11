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
        private const int SCALE_VERTICAL = 15;
        private readonly TableLayoutPanel _panel;
        private readonly Button _btnSettings;
        private readonly Button _btnConsole;
        private readonly Button _btnForm;
        private readonly Button _btnImage;
        private readonly Button _btnExit;
        private readonly LinkLabel _link;

        public Engine()
        {
            SuspendLayout();
            //
            // _btnSettings
            //
            _btnSettings = new Button();
            _btnSettings.Dock = DockStyle.Fill;
            _btnSettings.Text = "&Settings";
            _btnSettings.Click += btnSettings_Click;
            //
            // _btnConsole
            //
            _btnConsole = new Button();
            _btnConsole.Dock = DockStyle.Fill;
            _btnConsole.Text = "&Console";
            // _btnConsole.Enabled = false;
            _btnConsole.Click += btnConsole_Click;
            //
            // _btnForm
            //
            _btnForm = new Button();
            _btnForm.Dock = DockStyle.Fill;
            _btnForm.Text = "&Form";
            _btnForm.Click += btnForm_Click;
            //
            // _btnImage
            //
            _btnImage = new Button();
            _btnImage.Dock = DockStyle.Fill;
            _btnImage.Text = "&Image";
            _btnImage.Click += btnImage_Click;
            //
            // _btnExit
            //
            _btnExit = new Button();
            _btnExit.Dock = DockStyle.Fill;
            _btnExit.Text = "E&xit";
            _btnExit.Click += btnExit_Click;
            //
            // _link
            //
            _link = new LinkLabel();
            _link.Dock = DockStyle.Fill;
            _link.Text = "iObloc v3.0";
            _link.TextAlign = ContentAlignment.MiddleCenter;
            _link.Click += link_Click;
            // 
            // _panel
            // 
            _panel = new TableLayoutPanel();
            _panel.Dock = DockStyle.Fill;
            _panel.ColumnCount = 1;
            _panel.RowCount = 6;
            _panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            float p = 100f / _panel.RowCount;
            for (int i = 0; i < _panel.RowCount; i++)
                _panel.RowStyles.Add(new RowStyle(SizeType.Percent, p));
            _panel.Controls.Add(_btnSettings);
            _panel.Controls.Add(_btnConsole);
            _panel.Controls.Add(_btnForm);
            _panel.Controls.Add(_btnImage);
            _panel.Controls.Add(_link);
            _panel.Controls.Add(_btnExit);
            // 
            // FormRunner
            // 
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font(Font.FontFamily, SCALE_FONT);
            ClientSize = new System.Drawing.Size(SCALE_FONT * SCALE_HORIZONTAL, SCALE_FONT * SCALE_VERTICAL);
            AcceptButton = _btnImage;
            CancelButton = _btnExit;
            ShowIcon = false;
            ShowInTaskbar = true;
            Controls.Add(_panel);
            Name = "Engine";
            Text = "iObloc";
            ResumeLayout(false);
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(Serializer.SettingsFileName))
                    Serializer.Save();
                StartProcess(Serializer.SettingsFileName);
            }
            catch { }
        }

        private void btnConsole_Click(object sender, EventArgs e)
        {
            Start(RenderType.Console);
        }

        private void btnForm_Click(object sender, EventArgs e)
        {
            Start(RenderType.TableForm);
        }

        private void btnImage_Click(object sender, EventArgs e)
        {
            Start(RenderType.ImageForm);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void link_Click(object sender, EventArgs e)
        {
            StartProcess("https://github.com/cpvoinea/iobloc/releases");
        }

        private void StartProcess(string command)
        {
            try { Process.Start(new ProcessStartInfo("cmd", $"/c start {command}") { CreateNoWindow = true }); }
            catch { }
        }

        private void Start(RenderType render)
        {
            try
            {
                Serializer.Load();

                IGame menu = Serializer.GetGame((int)GameType.Menu);
                IGame game = menu;
                while (game != null)
                {
                    switch (render)
                    {
                        case RenderType.ImageForm:
                            Cursor = Cursors.WaitCursor;
                            using (var form = new ImageFormRenderer())
                            {
                                Cursor = Cursors.Default;
                                form.Run(game);
                                form.ShowDialog(this);
                            }
                            break;
                        case RenderType.TableForm:
                            Cursor = Cursors.WaitCursor;
                            using (var form = new TableFormRenderer())
                            {
                                Cursor = Cursors.Default;
                                form.Run(game);
                                form.ShowDialog(this);
                            }
                            break;
                        case RenderType.Console:
                            using (var console = new ConsoleRenderer())
                                console.Run(game);
                            break;
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
