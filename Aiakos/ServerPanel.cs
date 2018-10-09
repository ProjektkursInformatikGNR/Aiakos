using System.Windows.Forms;

namespace Aiakos
{
	/// <summary>
	/// Die Klasse <c>ServerPanel</c> gibt dem Benutzer die Möglichkeit, die Serverdaten einzugeben.
	/// </summary>
    public class ServerPanel : GroupBox
    {
		/// <summary>
		/// Die Labels zur Kennzeichnung der Eingabefelder
		/// </summary>
        private readonly Label _lName = new Label() { Text = "Name:", Left = 20, Width = 200, Top = 30 },
            _lPort = new Label() { Text = "Port:", Left = 20, Width = 200, Top = 80 },
            _lHost = new Label() { Text = "Host/IP-Adresse:", Left = 20, Width = 200, Top = 55 },
            _lUserId = new Label() { Text = "Benutzer-ID:", Left = 20, Width = 200, Top = 105 },
            _lPassword = new Label() { Text = "Passwort:", Left = 20, Width = 200, Top = 130 },
            _lDatabase = new Label() { Text = "Datenbank:", Left = 20, Width = 200, Top = 155 };

		/// <summary>
		/// Die Textfelder zur Eingabe der Serverdaten
		/// </summary>
        private readonly TextBox _tName = new TextBox() { Left = 220, Width = 200, Top = 30 },
            _tHost = new TextBox() { Left = 220, Width = 200, Top = 55 },
            _tPort = new TextBox() { Left = 220, Width = 200, Top = 80 },
            _tUserId = new TextBox() { Left = 220, Width = 200, Top = 105 },
            _tPassword = new TextBox() { Left = 220, Width = 200, Top = 130 },
            _tDatabase = new TextBox() { Left = 220, Width = 200, Top = 155 };

		/// <summary>
		/// Erzeugt eine neue <seealso cref="GroupBox"/> mit den vorgegebenen Eingabefeldern.
		/// </summary>
        public ServerPanel()
        {
            Width = 450;
            Height = 190;

            Controls.Add(_lName);
            Controls.Add(_tName);
            _tName.TextAlign = HorizontalAlignment.Center;
            _tName.Text = ServerConfiguration.DefaultServer.Name;
            _tName.KeyPress += KeyPress;
            Controls.Add(_lHost);
            Controls.Add(_tHost);
            _tHost.TextAlign = HorizontalAlignment.Center;
            _tHost.Text = ServerConfiguration.DefaultServer.Host;
            _tHost.KeyPress += KeyPress;
            Controls.Add(_lPort);
            Controls.Add(_tPort);
            _tPort.TextAlign = HorizontalAlignment.Center;
            _tPort.Text = ServerConfiguration.DefaultServer.Port;
            _tPort.KeyPress += KeyPress;
            Controls.Add(_lUserId);
            Controls.Add(_tUserId);
            _tUserId.TextAlign = HorizontalAlignment.Center;
            _tUserId.Text = ServerConfiguration.DefaultServer.UserId;
            _tUserId.KeyPress += KeyPress;
            Controls.Add(_lPassword);
            Controls.Add(_tPassword);
            _tPassword.UseSystemPasswordChar = true;
            _tPassword.TextAlign = HorizontalAlignment.Center;
            _tPassword.Text = ServerConfiguration.DefaultServer.Password;
			_tPassword.KeyPress += KeyPress;
			Controls.Add(_lDatabase);
            Controls.Add(_tDatabase);
            _tDatabase.TextAlign = HorizontalAlignment.Center;
            _tDatabase.Text = ServerConfiguration.DefaultServer.Database;
            _tDatabase.KeyPress += KeyPress;
        }

		/// <summary>
		/// Beim Drücken der Entertaste soll versucht werden, sich mit dem Server zu verbinden.
		/// </summary>
		/// <param name="sender">Informationen über den Auslöser des Events (hier: das jeweilige Textfeld)</param>
		/// <param name="e">detaillierte Informationen über den Tastendruck</param>
		new void KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char) Keys.Return)
				(Parent as ServerConfigurationGUI).StartConnect();
		}

		/// <summary>
		/// Das aus den eingegebenen Daten erzeugte Server-Objekt
		/// </summary>
        public Server Result
        {
            get { return new Server(_tName.Text, _tHost.Text, _tPort.Text, _tUserId.Text, _tPassword.Text, _tDatabase.Text); }
        }
    }
}