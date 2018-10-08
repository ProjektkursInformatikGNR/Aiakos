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
        private const string _key = "0A43284D63694186A4881CBF341ACE02"; //Die Verschlüsselungskeys
        private const string _salt = "5FA40A1EB95D4614";

		public static readonly string ConfigFile; //Der Dateiname zum Auslesen bzw. Schreiben der Config-Datei

		public static Server DefaultServer { get; set; } //Die zu verwendende Server-Instanz

		/// <summary>
		/// Initialisiert den FileStream zur Config-Datei im AppData-Ordner.
		/// </summary>
		static ServerConfiguration()
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Roaming\Aiakos\";
			ConfigFile = path + "cfg.xml";

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			if (!File.Exists(ConfigFile))
				File.Create(ConfigFile);

			DefaultServer = new Server("", "", "", "", "", "");
		}

		/// <summary>
		/// Extrahiert die Serverdaten aus der Server-Instanz und schreibt sie in die Config-Datei.
		/// </summary>
		public static void WriteServerData()
		{
			WriteConfig(new Dictionary<string, string>
			{
				{ "default", string.Format("{0};{1};{2};{3};{4};{5}", DefaultServer.Name, DefaultServer.Host, DefaultServer.Port, DefaultServer.UserId, DefaultServer.Password, DefaultServer.Database) }
			});
		}

		/// <summary>
		/// Liest die Config-Datei aus und hinterlegt die Daten in der Server-Instanz zurück.
		/// </summary>
		public static void ReadServerData()
		{
			Dictionary<string, string> config = ReadConfig();

			if (config == null)
				return;

			foreach (KeyValuePair<string, string> kvp in config)
			{
				if (kvp.Key.Equals("default"))
				{
					string[] data = kvp.Value.Split(';');
					DefaultServer = new Server(data[0], data[1], data[2], data[3], data[4], data[5]);
				}
			}
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
				catch (Exception e)
				{
					return null;
				}

				while (reader.Read())
				{
					if (!reader.IsStartElement() || reader.Name.Equals("config"))
						continue;

					result.Add(reader.Name, Decrypt(reader.ReadElementContentAsString()));
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
                aesAlg.Key = Encoding.Default.GetBytes(_key);
                aesAlg.IV = Encoding.ASCII.GetBytes(_salt);
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
                aesAlg.Key = Encoding.ASCII.GetBytes(_key);
                aesAlg.IV = Encoding.ASCII.GetBytes(_salt);
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(input)))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    result = srDecrypt.ReadToEnd();
            }

            return result;
        }
    }
}