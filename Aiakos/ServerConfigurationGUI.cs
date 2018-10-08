using System.Windows.Forms;

namespace Aiakos
{
    public static class ServerConfigurationGUI
    {
        private static Form prompt;
        private static Button connect = new Button() { Text = "Verbinden", Left = 350, Width = 100, Top = 220 },
            apply = new Button() { Text = "Übernehmen", Left = 240, Width = 100, Top = 220 },
            cancel = new Button { Text = "Abbrechen", Left = 130, Width = 100, Top = 220 };
        private static ServerPanel defaultServer;

        public static void ShowDialog()
        {
            Confirmed = false;

            prompt = new Form();
            prompt.Width = 480;
            prompt.Height = 300;
            prompt.Text = "Serverkonfiguration";
            prompt.StartPosition = FormStartPosition.CenterScreen;
            prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
            prompt.MaximizeBox = false;
            prompt.MinimizeBox = false;
            prompt.ShowInTaskbar = false;

            defaultServer = new ServerPanel() { Text = "Standardmäßiger Server", Top = 10, Left = 10 };
            prompt.Controls.Add(defaultServer);

            connect.Click += (sender, e) => ConnectToServer();
            prompt.Controls.Add(connect);

            apply.Click += (sender, e) =>
            {
                ServerConfiguration.DefaultServer = defaultServer.Server;
				ServerConfiguration.WriteServerData();
                prompt.Controls.Clear();
                prompt.Close();
            };
            prompt.Controls.Add(apply);

            cancel.Click += (sender, e) =>
            {
                prompt.Controls.Clear();
                prompt.Close();
            };
            prompt.Controls.Add(cancel);

            prompt.ShowDialog();
        }

        public static void ConnectToServer()
        {
            if (!Confirmed)
            {
                prompt.Cursor = Cursors.WaitCursor;

                Confirmed = true;
                ServerConfiguration.DefaultServer = defaultServer.Server;
				ServerConfiguration.WriteServerData();

                if (defaultServer.Server.ServerAvailable)
                {
                    prompt.Controls.Clear();
                    prompt.Close();
                    prompt.Cursor = Cursors.Default;
                }
                else
                {
                    MessageBox.Show("Verbindung zum Server kann nicht aufgebaut werden!", "Verbindungsfehler!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    prompt.Controls.Clear();
                    prompt.Close();
                    prompt.Cursor = Cursors.Default;
                    ShowDialog();
                }
            }
        }

        public static bool Confirmed { get; private set; }
    }
}