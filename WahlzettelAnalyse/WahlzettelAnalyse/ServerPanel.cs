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

namespace WahlzettelAnalyse
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

        public ServerPanel(Server server)
        {
            this.Width = 450;
            this.Height = 190;

            this.Controls.Add(lName);
            this.Controls.Add(tName);
            tName.TextAlign = HorizontalAlignment.Center;
            tName.Text = server.Name;
            tName.KeyPress += new KeyPressEventHandler(tName_KeyPress);
            this.Controls.Add(lHost);
            this.Controls.Add(tHost);
            tHost.TextAlign = HorizontalAlignment.Center;
            tHost.Text = server.Host;
            tHost.KeyPress += new KeyPressEventHandler(tHost_KeyPress);
            this.Controls.Add(lPort);
            this.Controls.Add(tPort);
            tPort.TextAlign = HorizontalAlignment.Center;
            tPort.Text = server.Port;
            tPort.KeyPress += new KeyPressEventHandler(tPort_KeyPress);
            this.Controls.Add(lUserId);
            this.Controls.Add(tUserId);
            tUserId.TextAlign = HorizontalAlignment.Center;
            tUserId.Text = server.UserId;
            tUserId.KeyPress += new KeyPressEventHandler(tUserId_KeyPress);
            this.Controls.Add(lPassword);
            this.Controls.Add(tPassword);
            tPassword.UseSystemPasswordChar = true;
            tPassword.TextAlign = HorizontalAlignment.Center;
            tPassword.Text = server.Password;
            tPassword.KeyPress += new KeyPressEventHandler(tPassword_KeyPress);
            this.Controls.Add(lDatabase);
            this.Controls.Add(tDatabase);
            tDatabase.TextAlign = HorizontalAlignment.Center;
            tDatabase.Text = server.Database;
            tDatabase.KeyPress += new KeyPressEventHandler(tDatabase_KeyPress);
        }

        void tDatabase_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                ServerConfiguration.connectToServer();
        }

        void tPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                ServerConfiguration.connectToServer();
        }

        void tUserId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                ServerConfiguration.connectToServer();
        }

        void tPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                ServerConfiguration.connectToServer();
        }

        void tHost_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                ServerConfiguration.connectToServer();
        }

        void tName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                ServerConfiguration.connectToServer();
        }

        public Server Server
        {
            get { return new Server(tName.Text, tHost.Text, tPort.Text, tUserId.Text, tPassword.Text, tDatabase.Text); }
        }
    }
}
