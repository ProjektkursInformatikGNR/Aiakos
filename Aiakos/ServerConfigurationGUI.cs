using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Reflection;
using System.IO;
using MySql.Data.MySqlClient;

namespace Aiakos
{
    public static class ServerConfigurationGUI
    {
        private static Form prompt;
        private static Button connect = new Button() { Text = "Verbinden", Left = 350, Width = 100, Top = 220 },
            apply = new Button() { Text = "Übernehmen", Left = 240, Width = 100, Top = 220 },
            cancel = new Button { Text = "Abbrechen", Left = 130, Width = 100, Top = 220 };
        private static ServerPanel defaultServer;
        private static bool confirmed;
        private static Server server;

        public static void showDialog(Server svr)
        {
            confirmed = false;
			server = svr;

            prompt = new Form();
            prompt.Width = 480;
            prompt.Height = 300;
            prompt.Text = "Serverkonfiguration";
            prompt.StartPosition = FormStartPosition.CenterScreen;
            prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
            prompt.MaximizeBox = false;
            prompt.MinimizeBox = false;
            prompt.ShowInTaskbar = false;

            defaultServer = new ServerPanel(server) { Text = "Standardmäßiger Server", Top = 10, Left = 10 };
            prompt.Controls.Add(defaultServer);

            connect.Click += (sender, e) =>
            {
                connectToServer();
            };
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

        public static void connectToServer()
        {
            if (!confirmed)
            {
                prompt.Cursor = Cursors.WaitCursor;

                confirmed = true;
                ServerConfiguration.DefaultServer = defaultServer.Server;
				ServerConfiguration.WriteServerData();

                if (DataAccess.ConnectionValid(defaultServer.Server))
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
                    showDialog(ServerConfiguration.DefaultServer);
                }
            }
        }

        public static bool Confirmation
        {
            get { return confirmed; }
        }
    }
}