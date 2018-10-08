namespace Aiakos
{
	partial class Form1
	{
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Windows Form-Designer generierter Code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung.
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.dateiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.datenAktualisierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.beendenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.einstellungenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.datenverwaltungToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.serverkonfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.analyseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.wahlzuweisungVorschlagenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.dateiToolStripMenuItem,
			this.einstellungenToolStripMenuItem,
			this.analyseToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(884, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// dateiToolStripMenuItem
			// 
			this.dateiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.datenAktualisierenToolStripMenuItem,
			this.beendenToolStripMenuItem});
			this.dateiToolStripMenuItem.Name = "dateiToolStripMenuItem";
			this.dateiToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
			this.dateiToolStripMenuItem.Text = "Datei";
			// 
			// datenAktualisierenToolStripMenuItem
			// 
			this.datenAktualisierenToolStripMenuItem.Name = "datenAktualisierenToolStripMenuItem";
			this.datenAktualisierenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.datenAktualisierenToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.datenAktualisierenToolStripMenuItem.Text = "Daten aktualisieren";
			this.datenAktualisierenToolStripMenuItem.Click += new System.EventHandler(this.datenAktualisierenToolStripMenuItem_Click);
			// 
			// beendenToolStripMenuItem
			// 
			this.beendenToolStripMenuItem.Name = "beendenToolStripMenuItem";
			this.beendenToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
			this.beendenToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.beendenToolStripMenuItem.Text = "Beenden";
			this.beendenToolStripMenuItem.Click += new System.EventHandler(this.beendenToolStripMenuItem_Click);
			// 
			// einstellungenToolStripMenuItem
			// 
			this.einstellungenToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.datenverwaltungToolStripMenuItem,
			this.serverkonfigurationToolStripMenuItem});
			this.einstellungenToolStripMenuItem.Name = "einstellungenToolStripMenuItem";
			this.einstellungenToolStripMenuItem.Size = new System.Drawing.Size(90, 20);
			this.einstellungenToolStripMenuItem.Text = "Einstellungen";
			// 
			// datenverwaltungToolStripMenuItem
			// 
			this.datenverwaltungToolStripMenuItem.Name = "datenverwaltungToolStripMenuItem";
			this.datenverwaltungToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D)));
			this.datenverwaltungToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
			this.datenverwaltungToolStripMenuItem.Text = "Datenverwaltung";
			this.datenverwaltungToolStripMenuItem.Click += new System.EventHandler(this.datenverwaltungToolStripMenuItem_Click);
			// 
			// serverkonfigurationToolStripMenuItem
			// 
			this.serverkonfigurationToolStripMenuItem.Name = "serverkonfigurationToolStripMenuItem";
			this.serverkonfigurationToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.S)));
			this.serverkonfigurationToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
			this.serverkonfigurationToolStripMenuItem.Text = "Serverkonfiguration";
			this.serverkonfigurationToolStripMenuItem.Click += new System.EventHandler(this.serverkonfigurationToolStripMenuItem_Click);
			// 
			// analyseToolStripMenuItem
			// 
			this.analyseToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.wahlzuweisungVorschlagenToolStripMenuItem});
			this.analyseToolStripMenuItem.Name = "analyseToolStripMenuItem";
			this.analyseToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
			this.analyseToolStripMenuItem.Text = "Analyse";
			// 
			// wahlzuweisungVorschlagenToolStripMenuItem
			// 
			this.wahlzuweisungVorschlagenToolStripMenuItem.Name = "wahlzuweisungVorschlagenToolStripMenuItem";
			this.wahlzuweisungVorschlagenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F8;
			this.wahlzuweisungVorschlagenToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
			this.wahlzuweisungVorschlagenToolStripMenuItem.Text = "Wahlzuweisung vorschlagen";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(884, 662);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "Wahlzettel-Analyse";
			this.Click += new System.EventHandler(this.beendenToolStripMenuItem_Click);
			this.Resize += new System.EventHandler(this.Form1_Resize);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem einstellungenToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem dateiToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem beendenToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem serverkonfigurationToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem analyseToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem wahlzuweisungVorschlagenToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem datenAktualisierenToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem datenverwaltungToolStripMenuItem;
	}
}

