using System.Threading;
using System.Windows.Forms;

namespace Aiakos
{
	/// <summary>
	/// Die Klasse <c>ServerConfigurationGUI</c> stellt ein Fenster zum Eingabedialog der Serverdaten dar.
	/// </summary>
    public class ServerConfigurationGUI : Form
	{
		public bool Confirmed { get; private set; }

		private Button _connect, _apply, _cancel; //Die Buttons in der Benutzeroberfläche
        private ServerPanel _serverPanel; //Die GroupBox mit den Eingabefeldern für die Serverdaten
		private Thread _checkConnection;

		/// <summary>
		/// Erzeugt ein neues Fenster zur Eingabe der Serverdaten.
		/// </summary>
		public ServerConfigurationGUI()
        {
            Confirmed = false;
			
            Width = 480;
            Height = 300;
            Text = "Serverkonfiguration";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
			
            Controls.Add(_serverPanel = new ServerPanel() { Text = "Datenbankinformationen", Top = 10, Left = 10 });

			_connect = new Button() { Text = "Verbinden", Left = 350, Width = 100, Top = 220 };
			_connect.Click += (sender, e) => StartConnect();
            Controls.Add(_connect);

			_apply = new Button() { Text = "Übernehmen", Left = 240, Width = 100, Top = 220 };
			_apply.Click += (sender, e) =>
            {
                ServerConfiguration.DefaultServer = _serverPanel.Result;
            };
            Controls.Add(_apply);

			_cancel = new Button { Text = "Abbrechen", Left = 130, Width = 100, Top = 220 };
			_cancel.Click += (sender, e) =>
            {
				if (_checkConnection != null && _checkConnection.IsAlive)
				{
					_checkConnection.Abort();
					CheckingFinished(true);
				}
				else
				{
					Controls.Clear();
					Close();
				}
            };
            Controls.Add(_cancel);
            CancelButton = _cancel;

            FormClosing += (sender, e) =>
			{
				if (_checkConnection != null && _checkConnection.IsAlive)
				{
					_checkConnection.Abort();
					CheckingFinished(true);
					e.Cancel = true;
				}
			};
			
			ShowDialog();
        }

		/// <summary>
		/// Beginnt, die Verbindung zum Server zu testen.
		/// </summary>
        public void StartConnect()
        {
            Cursor = Cursors.WaitCursor;
			_serverPanel.Enabled = false;
			_apply.Enabled = false;
			_connect.Enabled = false;

			_checkConnection = new Thread(new ThreadStart(() =>
			{
				Confirmed = _serverPanel.Result.Available;
				CheckingFinished(false);
			}));
			_checkConnection.Start();
		}

		/// <summary>
		/// Wird aufgerufen, wenn die Überprüfung der Serververbindung abgeschlossen ist.
		/// </summary>
		/// <param name="cancelled">Ist <code>TRUE</code>, wenn der Test abgebrochen wurde, oder <code>FALSE</code>, wenn er beendet wurde.</param>
		private void CheckingFinished(bool cancelled)
		{
			BeginInvoke(new MethodInvoker(() =>
			{
				if (Confirmed)
				{
					ServerConfiguration.DefaultServer = _serverPanel.Result;
					Controls.Clear();
					Close();
					Dispose();
				}
				else if (!cancelled)
					MessageBox.Show("Verbindung zum Server kann nicht aufgebaut werden!", "Verbindungsfehler!", MessageBoxButtons.OK, MessageBoxIcon.Error);

				Cursor = Cursors.Default;
				_serverPanel.Enabled = true;
				_apply.Enabled = true;
				_connect.Enabled = true;
			}));
		}
    }
}