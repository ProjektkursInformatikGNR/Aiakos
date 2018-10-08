using MySql.Data.MySqlClient;
using System;
using System.Runtime.InteropServices;

namespace Aiakos
{
	/// <summary>
	/// Die Klasse <c>Server</c> bildet ein Struktur von Attributen einer Serververbindung ab.
	/// </summary>
    public class Server
    {
		/// <summary>
		/// Erzeugt eine neue Instanz einer Serverstruktur.
		/// </summary>
		/// <param name="name">Name des Servers (frei wählbar)</param>
		/// <param name="host">Host/IP-Adresse des Servers</param>
		/// <param name="port">Port des Servers (MySQL-Standard: 3306)</param>
		/// <param name="userId">User-ID zur Authorisierung des Datenzugriffs</param>
		/// <param name="password">Passwort zur jeweiligen User-ID</param>
		/// <param name="database">Name der Datenbank innerhalb des Servers</param>
		public Server(string name, string host, string port, string userId, string password, string database)
        {
            Name = name;
            Host = host;
            Port = port;
            UserId = userId;
            Password = password;
            Database = database;
        }

		/// <summary>
		/// Name des Servers (frei wählbar)
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Host/IP-Adresse des Servers
		/// </summary>
		public string Host { get; set; }

		/// <summary>
		/// Port des Servers (MySQL-Standard: 3306)
		/// </summary>
		public string Port { get; set; }

		/// <summary>
		/// User-ID zur Authorisierung des Datenzugriffs
		/// </summary>
		public string UserId { get; set; }

		/// <summary>
		/// Passwort zur jeweiligen User-ID
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// Name der Datenbank innerhalb des Servers
		/// </summary>
		public string Database { get; set; }

		/// <summary>
		/// Stellt die Informationen über die Serververbindung im für einen MySQL-Server vorgeschriebenen Format dar.
		/// </summary>
		/// <returns>Serverinformationen</returns>
		public override string ToString()
		{
			return string.Format("server={0};uid={1};pwd={2};database={3};port={4};", Host, UserId, Password, Database, Port);
		}

		/// <summary>
		/// Importierte Methode zur Überprüfung der Internetverbindung; greift auf eine externe DLL-Bibliothek zu.
		/// </summary>
		/// <param name="Description">Detailliertere Informationen zur Internetverbindung</param>
		/// <param name="ReservedValue">REservierter Parameter; muss 0 sein</param>
		/// <returns>Liefert <code>TRUE</code>, wenn eine Internetverbindung aufgebaut werden kann und <code>FALSE</code>, wenn derzeit keine Verbindung verfügbar ist.</returns>
		[DllImport("wininet.dll")]
		private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

		/// <summary>
		/// Gibt über die externe Methode <seealso cref="InternetGetConnectedState(out int, int)"/> die Verfügbarkeit einer Internetverbindung wieder.
		/// </summary>
		public bool InternetConnectionAvailable
		{
			get
			{
				return InternetGetConnectedState(out int desc, 0);
			}
		}

		/// <summary>
		/// Versucht, eine Verbindung zum Server aufzubauen, und gibt den Erfolg wieder.
		/// </summary>
		public bool ServerAvailable
		{
			get
			{
				if (!InternetConnectionAvailable)
					return false;

				try
				{
					MySqlConnection con = new MySqlConnection(ToString());
					con.Open();
					con.CreateCommand();
					con.Close();
				}
				catch (Exception)
				{
					return false;
				}

				return true;
			}
		}
	}
}