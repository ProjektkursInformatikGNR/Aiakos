using System.Windows.Forms;

namespace Aiakos
{
    public class ServerPanel : GroupBox
    {
        private Label lName = new Label() { Text = "Name:", Left = 20, Width = 200, Top = 30 },
            lHost = new Label() { Text = "Host/IP-Adresse:", Left = 20, Width = 200, Top = 55 },
            lPort = new Label() { Text = "Port:", Left = 20, Width = 200, Top = 80 },
            lUserId = new Label() { Text = "Benutzer-ID:", Left = 20, Width = 200, Top = 105 },
            lPassword = new Label() { Text = "Passwort:", Left = 20, Width = 200, Top = 130 },
            lDatabase = new Label() { Text = "Datenbank:", Left = 20, Width = 200, Top = 155 };
        private TextBox tName = new TextBox() { Left = 220, Width = 200, Top = 30 },
            tHost = new TextBox() { Left = 220, Width = 200, Top = 55 },
            tPort = new TextBox() { Left = 220, Width = 200, Top = 80 },
            tUserId = new TextBox() { Left = 220, Width = 200, Top = 105 },
            tPassword = new TextBox() { Left = 220, Width = 200, Top = 130 },
            tDatabase = new TextBox() { Left = 220, Width = 200, Top = 155 };

        public ServerPanel()
        {
            Width = 450;
            Height = 190;

            Controls.Add(lName);
            Controls.Add(tName);
            tName.TextAlign = HorizontalAlignment.Center;
            tName.Text = ServerConfiguration.DefaultServer.Name;
            tName.KeyPress += new KeyPressEventHandler(KeyPress);
            Controls.Add(lHost);
            Controls.Add(tHost);
            tHost.TextAlign = HorizontalAlignment.Center;
            tHost.Text = ServerConfiguration.DefaultServer.Host;
            tHost.KeyPress += new KeyPressEventHandler(KeyPress);
            Controls.Add(lPort);
            Controls.Add(tPort);
            tPort.TextAlign = HorizontalAlignment.Center;
            tPort.Text = ServerConfiguration.DefaultServer.Port;
            tPort.KeyPress += new KeyPressEventHandler(KeyPress);
            Controls.Add(lUserId);
            Controls.Add(tUserId);
            tUserId.TextAlign = HorizontalAlignment.Center;
            tUserId.Text = ServerConfiguration.DefaultServer.UserId;
            tUserId.KeyPress += new KeyPressEventHandler(KeyPress);
            Controls.Add(lPassword);
            Controls.Add(tPassword);
            tPassword.UseSystemPasswordChar = true;
            tPassword.TextAlign = HorizontalAlignment.Center;
            tPassword.Text = ServerConfiguration.DefaultServer.Password;
            tPassword.KeyPress += new KeyPressEventHandler(KeyPress);
            Controls.Add(lDatabase);
            Controls.Add(tDatabase);
            tDatabase.TextAlign = HorizontalAlignment.Center;
            tDatabase.Text = ServerConfiguration.DefaultServer.Database;
            tDatabase.KeyPress += new KeyPressEventHandler(KeyPress);
        }

		new void KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char) Keys.Return)
				ServerConfigurationGUI.ConnectToServer();
		}

        public Server Server
        {
            get { return new Server(tName.Text, tHost.Text, tPort.Text, tUserId.Text, tPassword.Text, tDatabase.Text); }
        }
    }
}