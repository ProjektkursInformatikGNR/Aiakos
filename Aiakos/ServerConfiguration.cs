using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Security.Cryptography;
using System.IO;

namespace Aiakos
{
	/// <summary>
	/// Die Klasse Config verwaltet die Kommunikation zwischen der Software und der verschlüsselten Config-Datei mit den Serverdaten.
	/// </summary>
    public static class ServerConfiguration
    {
		public static string ConfigFile { get; private set; } //Der Dateiname zum Auslesen bzw. Schreiben der Config-Datei

		private static Server _defServer;
		/// <summary>
		/// Die als Standard festgelegten Serverdaten
		/// </summary>
		public static Server DefaultServer
		{
			get
			{
				if (_defServer != null)
					return _defServer;

				Dictionary<string, string> config = ReadConfig();

				if (config != null)
					foreach (KeyValuePair<string, string> kvp in config)
					{
						if (kvp.Key.Equals("ServerInfo"))
						{
							string[] data = kvp.Value.Split(';');
							return _defServer = new Server(data[0], data[1], data[2], data[3], data[4], data[5]);
						}
					}

				return _defServer = new Server("", "", "", "", "", "");
			}

			set
			{
				_defServer = value;
				WriteConfig(new Dictionary<string, string>
				{
					{ "ServerInfo", string.Join(";", _defServer.Name, _defServer.Host, _defServer.Port, _defServer.UserId, _defServer.Password, _defServer.Database) }
				});
			}
		}

		/// <summary>
		/// Zum Programmstart werden die Dateipfade initialisiert.
		/// </summary>
		static ServerConfiguration() => Initialise();

		/// <summary>
		/// Initialisiert den FileStream zur Config-Datei im AppData-Ordner.
		/// </summary>
		public static void Initialise()
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Roaming\Aiakos\";
			ConfigFile = path + "cfg.xml";

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			if (!File.Exists(ConfigFile))
				File.Create(ConfigFile);

			_defServer = null;
		}

		/// <summary>
		/// Schreibt die übergebenen Serverdaten verschlüsselt in die Config-Datei.
		/// </summary>
		/// <param name="elements">Die einzutragenden Serverdaten</param>
		private static void WriteConfig(Dictionary<string, string> elements)
        {
			using (XmlTextWriter writer = new XmlTextWriter(ConfigFile, Encoding.UTF8) { Formatting = Formatting.Indented })
			{
				writer.WriteStartDocument();
				writer.WriteStartElement("config");

				foreach (KeyValuePair<string, string> element in elements)
					writer.WriteElementString(element.Key, Encrypt(element.Value));

				writer.WriteEndDocument();
				writer.Flush();
			}
        }

		/// <summary>
		/// Liest die Serverdaten aus der Config-Datei und gibt sie zurück.
		/// </summary>
		/// <returns>Die Serverdaten aus der Config-Datei</returns>
        private static Dictionary<string, string> ReadConfig()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

			using (XmlReader reader = new XmlTextReader(ConfigFile))
			{
				try
				{
					reader.Read();
				}
				catch (Exception)
				{
					return null;
				}

				while (reader.Read())
				{
					if (!reader.IsStartElement() || reader.Name.Equals("config"))
						continue;

                    string name = reader.Name, decryptedValue;
                    if (!string.IsNullOrEmpty(decryptedValue = Decrypt(reader.ReadElementContentAsString())))
					    result.Add(name, decryptedValue);
				}
			}

            return result.Count > 0 ? result : null;
        }

		/// <summary>
		/// Verschlüsselt den gegebenen String.
		/// </summary>
		/// <param name="input">Der unverschlüsselte String</param>
		/// <returns>Der verschlüsselte String</returns>
        private static string Encrypt(string input)
        {
			if (string.IsNullOrEmpty(input))
				return null;

            string result = null;

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = MainForm.Key;
                aesAlg.IV = MainForm.Salt;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        swEncrypt.Write(input);

                    result = Convert.ToBase64String( msEncrypt.ToArray());
                }
            }

            return result;
        }

		/// <summary>
		/// Entschlüsselt den gegebenen String.
		/// </summary>
		/// <param name="input">Der verschlüsselte String</param>
		/// <returns>Der unverschlüsselte String</returns>
        private static string Decrypt(string input)
        {
			if (string.IsNullOrEmpty(input))
				return null;

            string result = null;

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = MainForm.Key;
                aesAlg.IV = MainForm.Salt;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                try
                {
                    using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(input)))
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        result = srDecrypt.ReadToEnd();
                }
                catch (Exception) { }
            }

            return result;
        }
    }
}