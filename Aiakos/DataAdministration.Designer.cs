namespace Aiakos
{
	partial class DataAdministration
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.tabs = new System.Windows.Forms.TabControl();
            this.coursePage = new System.Windows.Forms.TabPage();
            this.courseView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.studentPage = new System.Windows.Forms.TabPage();
            this.studentView = new System.Windows.Forms.DataGridView();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.choicePage = new System.Windows.Forms.TabPage();
            this.choiceView = new System.Windows.Forms.DataGridView();
            this.Column7 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.tabs.SuspendLayout();
            this.coursePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.courseView)).BeginInit();
            this.studentPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.studentView)).BeginInit();
            this.choicePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.choiceView)).BeginInit();
            this.SuspendLayout();
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.coursePage);
            this.tabs.Controls.Add(this.studentPage);
            this.tabs.Controls.Add(this.choicePage);
            this.tabs.Location = new System.Drawing.Point(13, 13);
            this.tabs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(753, 516);
            this.tabs.TabIndex = 0;
            // 
            // coursePage
            // 
            this.coursePage.Controls.Add(this.courseView);
            this.coursePage.Location = new System.Drawing.Point(4, 25);
            this.coursePage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.coursePage.Name = "coursePage";
            this.coursePage.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.coursePage.Size = new System.Drawing.Size(745, 487);
            this.coursePage.TabIndex = 0;
            this.coursePage.Text = "Kurse";
            this.coursePage.UseVisualStyleBackColor = true;
            // 
            // courseView
            // 
            this.courseView.AllowUserToResizeRows = false;
            this.courseView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.courseView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.courseView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.courseView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.courseView.Location = new System.Drawing.Point(4, 4);
            this.courseView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.courseView.Name = "courseView";
            this.courseView.Size = new System.Drawing.Size(737, 479);
            this.courseView.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Name";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Jahrgänge";
            this.Column2.Name = "Column2";
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Teilnehmerzahlen";
            this.Column3.Name = "Column3";
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // studentPage
            // 
            this.studentPage.Controls.Add(this.studentView);
            this.studentPage.Location = new System.Drawing.Point(4, 25);
            this.studentPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.studentPage.Name = "studentPage";
            this.studentPage.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.studentPage.Size = new System.Drawing.Size(771, 500);
            this.studentPage.TabIndex = 1;
            this.studentPage.Text = "Schüler";
            this.studentPage.UseVisualStyleBackColor = true;
            // 
            // studentView
            // 
            this.studentView.AllowUserToResizeRows = false;
            this.studentView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.studentView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.studentView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column4,
            this.Column5,
            this.Column6});
            this.studentView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.studentView.Location = new System.Drawing.Point(4, 4);
            this.studentView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.studentView.Name = "studentView";
            this.studentView.Size = new System.Drawing.Size(763, 492);
            this.studentView.TabIndex = 0;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Name";
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Column5.HeaderText = "Geburtsdatum";
            this.Column5.Name = "Column5";
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Klasse";
            this.Column6.Name = "Column6";
            // 
            // choicePage
            // 
            this.choicePage.Controls.Add(this.choiceView);
            this.choicePage.Location = new System.Drawing.Point(4, 25);
            this.choicePage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.choicePage.Name = "choicePage";
            this.choicePage.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.choicePage.Size = new System.Drawing.Size(771, 500);
            this.choicePage.TabIndex = 2;
            this.choicePage.Text = "Wahlen";
            this.choicePage.UseVisualStyleBackColor = true;
            // 
            // choiceView
            // 
            this.choiceView.AllowUserToResizeRows = false;
            this.choiceView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.choiceView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.choiceView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column7,
            this.Column8,
            this.Column9,
            this.Column10});
            this.choiceView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.choiceView.Location = new System.Drawing.Point(4, 4);
            this.choiceView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.choiceView.Name = "choiceView";
            this.choiceView.Size = new System.Drawing.Size(763, 492);
            this.choiceView.TabIndex = 0;
            this.choiceView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.ChoiceView_CellValueChanged);
            // 
            // Column7
            // 
            this.Column7.FillWeight = 1F;
            this.Column7.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Column7.HeaderText = "Schüler";
            this.Column7.Name = "Column7";
            // 
            // Column8
            // 
            this.Column8.FillWeight = 1F;
            this.Column8.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Column8.HeaderText = "1. Wahl";
            this.Column8.Name = "Column8";
            // 
            // Column9
            // 
            this.Column9.FillWeight = 1F;
            this.Column9.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Column9.HeaderText = "2. Wahl";
            this.Column9.Name = "Column9";
            // 
            // Column10
            // 
            this.Column10.FillWeight = 1F;
            this.Column10.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Column10.HeaderText = "3. Wahl";
            this.Column10.Name = "Column10";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(659, 537);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 28);
            this.button1.TabIndex = 1;
            this.button1.Text = "Anwenden";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.ConfirmClick);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(537, 537);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(113, 28);
            this.button2.TabIndex = 2;
            this.button2.Text = "Übernehmen";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.ApplyClick);
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button3.Location = new System.Drawing.Point(416, 537);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(113, 28);
            this.button3.TabIndex = 3;
            this.button3.Text = "Abbrechen";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.CancelClick);
            // 
            // DataAdministration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button3;
            this.ClientSize = new System.Drawing.Size(779, 569);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataAdministration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Datenverwaltung";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DataAdministration_FormClosing);
            this.tabs.ResumeLayout(false);
            this.coursePage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.courseView)).EndInit();
            this.studentPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.studentView)).EndInit();
            this.choicePage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.choiceView)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabs;
		private System.Windows.Forms.TabPage coursePage;
		private System.Windows.Forms.TabPage studentPage;
		private System.Windows.Forms.DataGridView courseView;
		private System.Windows.Forms.DataGridView studentView;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
		private System.Windows.Forms.DataGridViewButtonColumn Column5;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
		private System.Windows.Forms.TabPage choicePage;
		private System.Windows.Forms.DataGridView choiceView;
		private System.Windows.Forms.DataGridViewComboBoxColumn Column7;
		private System.Windows.Forms.DataGridViewComboBoxColumn Column8;
		private System.Windows.Forms.DataGridViewComboBoxColumn Column9;
		private System.Windows.Forms.DataGridViewComboBoxColumn Column10;
	}
}