using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Security.Cryptography;
using System.IO;

namespace WahlzettelAnalyse
{
    public abstract class Config
    {
        private static string _key = "0A43284D63694186A4881CBF341ACE02";
        private static string _salt = "5FA40A1EB95D4614";

        protected string _filename;

        protected void writeConfig(Dictionary<string,string> elements)
        {
            try { File.Delete(_filename); }
            catch { }

            XmlTextWriter writer;

            try { writer = new XmlTextWriter(_filename, Encoding.UTF8); }
            catch (DirectoryNotFoundException) 
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_filename));
                writer = new XmlTextWriter(_filename, Encoding.UTF8);
            }

            writer.Formatting = Formatting.Indented;


            writer.WriteStartDocument();
            writer.WriteStartElement("config");

            foreach (KeyValuePair<string, string> element in elements)
            {
                writer.WriteElementString(element.Key, encrypt(element.Value));
            }

            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }

        protected Dictionary<string, string> readConfig()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            XmlReader reader;

            if (File.Exists(_filename))
                reader = new XmlTextReader(_filename);
            else
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(_filename));
                return result;
            }

            using (reader)
            {
                while (reader.Read())
                {
                    if (!reader.IsStartElement())
                        continue;


                    if (reader.Name == "config")
                        continue;

                    result.Add(reader.Name, decrypt(reader.ReadElementContentAsString()));
                }
            }
            if (result.Count <= 0)
                return null;

            return result;
        }

        protected static string encrypt(string input)
        {
            if (input == null || input.Length <= 0)
                throw new ArgumentNullException("input");

            string result = null;

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Encoding.Default.GetBytes(_key);
                aesAlg.IV = Encoding.ASCII.GetBytes(_salt);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(input);
                        }
                        result = Convert.ToBase64String( msEncrypt.ToArray());
                    }
                }
            }

            return result;
        }

        protected static string decrypt(string input)
        {
            if (input == null || input.Length <= 0)
                throw new ArgumentNullException("input");

            string result = "";

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Encoding.ASCII.GetBytes(_key);
                aesAlg.IV = Encoding.ASCII.GetBytes(_salt);

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(input)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            result = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return result;
        }
    }
}
