using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace iobloc
{
    // Entry point to application, handle initialization and disposal
    class Launcher : Form
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

        public Launcher()
        {
            SuspendLayout();
            //
            // _btnSettings
            //
            _btnSettings = new Button
            {
                Dock = DockStyle.Fill,
                Text = "&Settings"
            };
            _btnSettings.Click += BtnSettings_Click;
            //
            // _btnConsole
            //
            _btnConsole = new Button
            {
                Dock = DockStyle.Fill,
                Text = "&Text"
            };
            // _btnConsole.Enabled = false;
            _btnConsole.Click += BtnConsole_Click;
            //
            // _btnForm
            //
            _btnForm = new Button
            {
                Dock = DockStyle.Fill,
                Text = "&Grid"
            };
            _btnForm.Click += BtnForm_Click;
            //
            // _btnImage
            //
            _btnImage = new Button
            {
                Dock = DockStyle.Fill,
                Text = "&Image"
            };
            _btnImage.Click += BtnImage_Click;
            //
            // _btnExit
            //
            _btnExit = new Button
            {
                Dock = DockStyle.Fill,
                Text = "E&xit"
            };
            _btnExit.Click += BtnExit_Click;
            //
            // _link
            //
            _link = new LinkLabel
            {
                Dock = DockStyle.Fill,
                Text = "iObloc v3.0",
                TextAlign = ContentAlignment.MiddleCenter
            };
            _link.Click += Link_Click;
            // 
            // _panel
            // 
            _panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 6
            };
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

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(Serializer.SettingsFileName))
                    Serializer.Save();
                StartProcess(Serializer.SettingsFileName);
            }
            catch { }
        }

        private void BtnConsole_Click(object sender, EventArgs e)
        {
            Launch(RenderType.Console);
        }

        private void BtnForm_Click(object sender, EventArgs e)
        {
            Launch(RenderType.TableForm);
        }

        private void BtnImage_Click(object sender, EventArgs e)
        {
            Launch(RenderType.ImageForm);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Link_Click(object sender, EventArgs e)
        {
            StartProcess("https://github.com/cpvoinea/iobloc/releases");
        }

        private void StartProcess(string command)
        {
            try { Process.Start(new ProcessStartInfo("cmd", $"/c start {command}") { CreateNoWindow = true }); }
            catch { }
        }

        public void Launch(RenderType renderType)
        {
            try
            {
                Serializer.Load();
                IGame menu = Serializer.GetGame((int)GameType.Menu);
                IGame game = menu;

                while (game != null)
                {
                    using (IRenderer renderer = GetRenderer(renderType))
                    {
                        renderer.Run(game);
                        if (renderer is Form)
                            (renderer as Form).ShowDialog(this);
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

        public static IRenderer GetRenderer(RenderType renderType)
        {
            switch (renderType)
            {
                case RenderType.Console: return new ConsoleRenderer();
                case RenderType.TableForm: return new TableFormRenderer();
                case RenderType.ImageForm: return new ImageFormRenderer();
                default: return null;
            }
        }

        public static Form Form(GameType gameType, RenderType renderType = RenderType.ImageForm)
        {
            Serializer.Load();
            IGame game = Serializer.GetGame((int)gameType);
            IRenderer renderer = GetRenderer(renderType);
            renderer.Run(game);
            return renderer as Form;
        }

        public static void Console(GameType gameType)
        {
            Serializer.Load();
            IGame game = Serializer.GetGame((int)gameType);
            ConsoleRenderer renderer = new ConsoleRenderer();
            renderer.Run(game);
        }
    }
}
