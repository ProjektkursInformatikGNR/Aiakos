using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;

namespace Aiakos
{
    /// <summary>
    /// Die Klasse <c>DataAdministration</c> stellt dem Nutzer eine Maske zur Eingabe und Einsicht der Daten zur Verfügung.
    /// </summary>
    public partial class DataAdministration : Form
    {
        /// <summary>
        /// Gibt an, ob der Nutzer die Änderungen speichern möchte.
        /// </summary>
		public bool Apply { get; private set; }

        


        private ContextMenuStrip context = new ContextMenuStrip(); //das Kontextmenü zur Tabellenmanipulation
        private ToolStripMenuItem copyMenu = new ToolStripMenuItem("Kopieren"), pasteMenu = new ToolStripMenuItem("Einfügen"), cutMenu = new ToolStripMenuItem("Ausschneiden"), removeMenu = new ToolStripMenuItem("Löschen"); //die Items im Kontextmenü
        private DataGridView view; //die aktuell ausgewählte Tabelle
        private Assembly assembly = Assembly.GetExecutingAssembly(); //das Tool zum Zugriff auf Dateien innerhalb der kompilierten Anwendung
        private Bitmap copyIcon, pasteIcon, cutIcon, removeIcon; //die Icons zur Tabellenmanipulation

        private const string defaultValue = "(keine)"; //die Standardangabe bei fehlenden Datenbankeinträgen

        public DataAdministration()
        {
            InitializeComponent(); //Initialisiert die Grafikkomponenten aus dem Designer.
            Apply = false;
            ShowInTaskbar = false;

            copyIcon = new Bitmap(assembly.GetManifestResourceStream("Aiakos.copy.png")); //Greift auf die Bitmaps aus der Executable zu.
            pasteIcon = new Bitmap(assembly.GetManifestResourceStream("Aiakos.paste.png"));
            cutIcon = new Bitmap(assembly.GetManifestResourceStream("Aiakos.cut.png"));
            removeIcon = new Bitmap(assembly.GetManifestResourceStream("Aiakos.remove.png"));

            //Initialisierung der Icons
            copyMenu.Image = copyIcon;
            copyMenu.ShortcutKeys = Keys.Control | Keys.C;
            copyMenu.Click += new EventHandler(CopyClick);
            context.Items.Add(copyMenu);
            pasteMenu.Image = pasteIcon;
            pasteMenu.ShortcutKeys = Keys.Control | Keys.V;
            pasteMenu.Click += new EventHandler(PasteClick);
            context.Items.Add(pasteMenu);
            cutMenu.Image = cutIcon;
            cutMenu.ShortcutKeys = Keys.Control | Keys.X;
            cutMenu.Click += new EventHandler(CutClick);
            context.Items.Add(cutMenu);
            removeMenu.Image = removeIcon;
            removeMenu.ShortcutKeys = Keys.Delete;
            removeMenu.Click += new EventHandler(RemoveClick);
            context.Items.Add(removeMenu);

            //Initialisierung der Kurstabelle
            courseView.ScrollBars = ScrollBars.Both;
            courseView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            courseView.GridColor = Color.Black;
            courseView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            courseView.RowHeadersVisible = false;
            courseView.CellMouseDown += CellMouseDown;

            foreach (KeyValuePair<int, Course> course in MainForm.Courses ?? new Dictionary<int, Course>()) //Einfügung der Daten
            {
                DataGridViewRow row = courseView.Rows[0].Clone() as DataGridViewRow;
                row.SetValues(new string[] {
                    course.Value.Name,
                    course.Value.MinYear + "-" + course.Value.MaxYear,
                    course.Value.MinStudents + "-" + course.Value.MaxStudents});
                row.Tag = course.Key;
                courseView.Rows.Add(row);
            }

            coursePage.Controls.Add(courseView);
            coursePage.TabIndex = 0;

            //Initialisierung der Schülertabelle
            studentView.ScrollBars = ScrollBars.Both;
            studentView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            studentView.GridColor = Color.Black;
            studentView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            studentView.RowHeadersVisible = false;
            studentView.CellMouseDown += CellMouseDown;
            studentView.CellClick += StudentView_CellClick;

            foreach (KeyValuePair<int, Student> student in MainForm.Students ?? new Dictionary<int, Student>()) //Einfügung der Daten
            {
                DataGridViewRow row = studentView.Rows[0].Clone() as DataGridViewRow;
                row.SetValues(new string[] {
                    student.Value.Name,
                    student.Value.DateOfBirth.ToShortDateString(),
                    student.Value.Form});
                row.Tag = student.Key;
                row.Cells[1].Tag = student.Value.DateOfBirth.ToShortDateString();
                studentView.Rows.Add(row);
            }

            studentPage.Controls.Add(studentView);
            studentPage.TabIndex = 0;

            //Initialisierung der Wahlentabelle
            choiceView.ScrollBars = ScrollBars.Both;
            choiceView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            choiceView.GridColor = Color.Black;
            choiceView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            choiceView.RowHeadersVisible = false;
            choiceView.CellMouseDown += CellMouseDown;

            (choiceView.Columns[0] as DataGridViewComboBoxColumn).Items.AddRange(MainForm.Students.Values.ToArray());
            (choiceView.Columns[1] as DataGridViewComboBoxColumn).Items.Add(defaultValue);
            (choiceView.Columns[1] as DataGridViewComboBoxColumn).Items.AddRange(MainForm.Courses.Values.ToArray());
            (choiceView.Columns[2] as DataGridViewComboBoxColumn).Items.Add(defaultValue);
            (choiceView.Columns[2] as DataGridViewComboBoxColumn).Items.AddRange(MainForm.Courses.Values.ToArray());
            (choiceView.Columns[3] as DataGridViewComboBoxColumn).Items.Add(defaultValue);
            (choiceView.Columns[3] as DataGridViewComboBoxColumn).Items.AddRange(MainForm.Courses.Values.ToArray());

            foreach (KeyValuePair<int, Choice> choice in MainForm.Choices) //Einfügung der Daten
            {
                DataGridViewRow row = choiceView.Rows[0].Clone() as DataGridViewRow;
                row.SetValues(new object[] {
                    MainForm.Students[choice.Value.StudentId],
                    choice.Value.CourseId1.HasValue ? MainForm.Courses[choice.Value.CourseId1.Value] as object : defaultValue,
                    choice.Value.CourseId2.HasValue ? MainForm.Courses[choice.Value.CourseId2.Value] as object : defaultValue,
                    choice.Value.CourseId3.HasValue ? MainForm.Courses[choice.Value.CourseId3.Value] as object : defaultValue});
                row.Tag = choice.Value.StudentId;
                choiceView.Rows.Add(row);
            }

            choicePage.Controls.Add(choiceView);
            choicePage.TabIndex = 0;

            Controls.Add(tabs);
        }

        /// <summary>
        /// Veranlasst auf Auffoderung des Nutzers, die aktuell markierten Datensätze zu löschen.
        /// </summary>
		/// <param name="sender">Auslöser des Events (hier das ToolStripMenuItem)</param>
		/// <param name="e">Informationen über das Event</param>
		private void RemoveClick(object sender, EventArgs e)
        {
            Remove();
        }

        /// <summary>
        /// Veranlasst auf Auffoderung des Nutzers, die aktuell markierten Datensätze auszuschneiden.
        /// </summary>
        /// <param name="sender">Auslöser des Events (hier das ToolStripMenuItem)</param>
        /// <param name="e">Informationen über das Event</param>
        private void CutClick(object sender, EventArgs e)
        {
            Cut();
        }

        /// <summary>
        /// Veranlasst auf Auffoderung des Nutzers, die Datensätze aus der Zwischenablage einzufügen.
        /// </summary>
        /// <param name="sender">Auslöser des Events (hier das ToolStripMenuItem)</param>
        /// <param name="e">Informationen über das Event</param>
        private void PasteClick(object sender, EventArgs e)
        {
            Paste();
        }

        /// <summary>
        /// Veranlasst auf Auffoderung des Nutzers, die aktuell markierten Datensätze in die Zwischenablage zu kopieren.
        /// </summary>
        /// <param name="sender">Auslöser des Events (hier das ToolStripMenuItem)</param>
        /// <param name="e">Informationen über das Event</param>
        private void CopyClick(object sender, EventArgs e)
        {
            Copy();
        }

        /// <summary>
        /// Öffnet nach Rechtsklick des Nutzers das Kontextmenü.
        /// </summary>
        /// <param name="sender">Auslöser des Events (hier das DataGridView)</param>
        /// <param name="e">Informationen über das Event</param>
		private void CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                view = sender as DataGridView;
                context.Show(sender as Control, e.Location);
            }
        }

        /// <summary>
        /// Öffnet den <see cref="DatePicker"/>, sobald der Nutzer ein für ein Datum ausgewiesenes Feld anklickt.
        /// </summary>
        /// <param name="sender">Auslöser des Events (hier das StudentView)</param>
        /// <param name="e">Informationen über das Event</param>
		private void StudentView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex != -1)
            {
                DatePicker datePicker = new DatePicker();

                if (studentView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                    datePicker.SelectedDate = studentView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                datePicker.ShowDialog();
                studentView.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = datePicker.SelectedDate;
                studentView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = datePicker.SelectedDate;

                if (datePicker.SelectedDate != null && e.RowIndex == studentView.Rows.Count - 1)
                {
                    var rows = studentView.Rows.Cast<DataGridViewRow>().ToArray();
                    var temp = rows[e.RowIndex];
                    rows[e.RowIndex] = rows[studentView.Rows.Count - 1];
                    rows[studentView.Rows.Count - 1] = temp;
                    studentView.Rows.Clear();
                    studentView.Rows.AddRange(rows);
                    studentView.Rows[0].Selected = false;
                    studentView.Rows[e.RowIndex].Selected = true;
                }
            }
        }

        /// <summary>
        /// Behandelt die Bestätigung des Nutzers zur Speicherung der Datenänderungen und schließt das Fenster, sollte kein Fehler auftreten.
        /// </summary>
        /// <param name="sender">Auslöser des Events (hier der Button)</param>
        /// <param name="e">Informationen über das Event</param>
        private void ConfirmClick(object sender, EventArgs e)
        {
            Apply = true;
            UpdateData();

            if (Apply)
                Close();
        }

        /// <summary>
        /// Behandelt die Bestätigung des Nutzers zur Speicherung, ohne das Fenster zu schließen.
        /// </summary>
        /// <param name="sender">Auslöser des Events (hier der Button)</param>
        /// <param name="e">Informationen über das Event</param>
        private void ApplyClick(object sender, EventArgs e)
        {
            UpdateData();
        }

        /// <summary>
        /// Schließt das Fenster auf Aufforderung des Nutzers.
        /// </summary>
        /// <param name="sender">Auslöser des Events (hier der Button)</param>
        /// <param name="e">Informationen über das Event</param>
        private void CancelClick(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Erinnert den Nutzer bei Aufforderung zum Schließen des Fensters, falls Daten nicht gespeichert sein sollten.
        /// </summary>
        /// <param name="sender">Auslöser des Events (hier die DataAdministration)</param>
        /// <param name="e">Informationen über das Event</param>
        private void DataAdministration_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Apply && MessageBox.Show("Sind Sie sicher?\n(Alle ungespeichert geänderten Daten gehen verloren!)", "Warnung!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                e.Cancel = true;
        }

        /// <summary>
        /// Kopiert die ausgewählten Zeilen in die Zwischenablage.
        /// </summary>
        private void Copy()
        {
            string text = "";

            foreach (DataGridViewRow row in view.SelectedRows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                    text += cell.Value + "\u0009"; //Fügt eine Markierung zum Zellenumbruch ein.

                text += "\u000D\u000A"; //Fügt eine Markierung zum Zeilenumbruch ein.
            }

            Clipboard.SetText(text, TextDataFormat.Text);
        }

        /// <summary>
        /// Fügt die Daten aus der Zwischenablage in der aktuellen Tabelle ein.
        /// </summary>
        private void Paste()
        {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) && view != choiceView)
            {
                List<DataGridViewRow> rows = new List<DataGridViewRow>();

                foreach (string row in Clipboard.GetText().Split('\u000D', '\u000A'))
                {
                    if (row.Length > 0)
                    {
                        DataGridViewRow dataRow = new DataGridViewRow();
                        dataRow.CreateCells(view, row.Split('\u0009'));
                        rows.Add(dataRow);
                    }
                }

                view.Rows.AddRange(rows.ToArray());
            }
        }

        /// <summary>
        /// Schneidet die aktuell markierten Zeilen durch Aufruf der Methoden <see cref="Copy"/> sowie <see cref="Remove"/> aus.
        /// </summary>
        private void Cut()
        {
            Copy();
            Remove();
        }

        /// <summary>
        /// Löscht die zurzeit ausgewählten Zeilen.
        /// </summary>
		private void Remove()
        {
            foreach (DataGridViewRow row in view.SelectedRows)
                view.Rows.Remove(row);
        }

        /// <summary>
        /// Aktualisiert die Datenbank durch Einfügen der Tabellendaten.
        /// </summary>
        private void UpdateData()
        {

            IDataAccess ida = new DataAccess(ServerConfiguration.DefaultServer);

            Apply = false;

            List<Course> courses = new List<Course>();
            foreach (DataGridViewRow row in courseView.Rows)
            {
                //Prüft die Kursdaten auf formale Richtigkeit.
                if (row.Cells["Column1"].Value == null && row.Cells["Column2"].Value == null && row.Cells["Column3"].Value == null)
                    continue;

                if (string.IsNullOrEmpty(row.Cells["Column1"].FormattedValue.ToString().Trim()))
                {
                    MessageBox.Show("Bitte tragen Sie einen Namen in Zeile " + (row.Index + 1) + " ein!", "Eingabefehler! - Kurse", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (row.Cells["Column2"].FormattedValue.ToString().Split('-').Length != 2 || !row.Cells["Column2"].FormattedValue.ToString().All(c => (char.IsDigit(c) || c == '-')) || row.Cells["Column2"].FormattedValue.ToString().StartsWith("-") || row.Cells["Column2"].FormattedValue.ToString().EndsWith("-"))
                {
                    MessageBox.Show("Bitte tragen Sie die Jahrgänge in Zeile " + (row.Index + 1) + " im korrekten Format (Jahr-Jahr) ein!", "Eingabefehler! - Kurse", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (row.Cells["Column3"].FormattedValue.ToString().Split('-').Length != 2 || !row.Cells["Column3"].FormattedValue.ToString().All(c => char.IsDigit(c) || c == '-') || row.Cells["Column3"].FormattedValue.ToString().StartsWith("-") || row.Cells["Column3"].FormattedValue.ToString().EndsWith("-"))
                {
                    MessageBox.Show("Bitte tragen Sie die Teilnehmerzahlen in Zeile " + (row.Index + 1) + " im korrekten Format (Minimum-Maximum) ein!", "Eingabefehler! - Kurse", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    foreach (DataGridViewRow row2 in courseView.Rows)
                    {
                        if (row.Cells["Column1"].FormattedValue.Equals(row2.Cells["Column1"].FormattedValue) && row != row2)
                        {
                            MessageBox.Show("Der Kurs \"" + row.Cells["Column1"].FormattedValue.ToString() + "\" in Zeile " + (row.Index + 1) + " kann nicht eindeutig von dem in Zeile " + (row2.Index + 1) + " unterschieden werden!", "Eingabefehler! - Kurse", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    courses.Add(new Course(
                        row.Cells["Column1"].Value.ToString(),
                        int.Parse(row.Cells["Column2"].Value.ToString().Split('-')[0]),
                        int.Parse(row.Cells["Column2"].Value.ToString().Split('-')[1]),
                        int.Parse(row.Cells["Column3"].Value.ToString().Split('-')[0]),
                        int.Parse(row.Cells["Column3"].Value.ToString().Split('-')[1]),
                        (int)(row.Tag ?? -1)
                    ));
                }
            }

            List<Student> students = new List<Student>();
            foreach (DataGridViewRow row in studentView.Rows)
            {
                //Prüft die Schülerdaten auf formale Richtigkeit.
                if (row.Cells["Column4"].Value == null && row.Cells["Column5"].Value == null && row.Cells["Column6"].Value == null)
                    continue;

                if (string.IsNullOrEmpty(row.Cells["Column4"].FormattedValue.ToString().Trim()))
                {
                    MessageBox.Show("Bitte tragen Sie einen Namen in Zeile " + (row.Index + 1) + " ein!", "Eingabefehler! - Schüler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (!DateTime.TryParse(row.Cells["Column5"].FormattedValue.ToString(), out DateTime d))
                {
                    MessageBox.Show("Bitte tragen Sie das Geburtsdatum in Zeile " + (row.Index + 1) + " im korrekten Format (YYYY-MM-DD oder DD.MM.YYYY) ein!", "Eingabefehler! - Schüler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (string.IsNullOrEmpty(row.Cells["Column6"].FormattedValue.ToString().Trim()))
                {
                    MessageBox.Show("Bitte tragen Sie die Klasse in Zeile " + (row.Index + 1) + "  ein!", "Eingabefehler! - Schüler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    foreach (DataGridViewRow row2 in studentView.Rows)
                    {
                        if (row.Cells["Column4"].FormattedValue.Equals(row2.Cells["Column4"].FormattedValue) && row.Cells["Column5"].FormattedValue.Equals(row2.Cells["Column5"].FormattedValue) && row != row2)
                        {
                            MessageBox.Show("Der Schüler \"" + row.Cells["Column4"].FormattedValue.ToString() + "\" in Zeile " + (row.Index + 1) + " kann nicht eindeutig von dem in Zeile " + (row2.Index + 1) + " unterschieden werden!", "Eingabefehler! - Schüler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    students.Add(new Student(
                        row.Cells["Column4"].Value.ToString(),
                        DateTime.Parse(row.Cells["Column5"].Value.ToString()),
                        row.Cells["Column6"].Value.ToString(),
                        (int)(row.Tag ?? -1)
                    ));
                }
            }

            List<Choice> choices = new List<Choice>();
            foreach (DataGridViewRow row in choiceView.Rows)
            {
                //Prüft die Wahldaten auf formale Richtigkeit.
                if (row.Cells["Column7"].Value == null && row.Cells["Column8"].Value == null && row.Cells["Column9"].Value == null && row.Cells["Column10"].Value == null)
                    continue;

                if (string.IsNullOrEmpty(row.Cells["Column7"].FormattedValue.ToString().Trim()))
                {
                    MessageBox.Show("Bitte wählen Sie einen Schüler in Zeile " + (row.Index + 1) + " aus!", "Eingabefehler! - Wahlen", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                foreach (DataGridViewCell cell1 in new[] { row.Cells["Column8"], row.Cells["Column9"], row.Cells["Column10"] })
                    foreach (DataGridViewCell cell2 in new[] { row.Cells["Column8"], row.Cells["Column9"], row.Cells["Column10"] })
                        if (cell1 != cell2 && cell1.Value != null && !cell1.FormattedValue.Equals(defaultValue) && cell1.FormattedValue.Equals(cell2.FormattedValue))
                        {
                            MessageBox.Show(string.Format("Der Schüler \"{0}\" kann nicht mehrmals den Kurs \"{1}\" wählen!", row.Cells["Column7"].Value as Student, cell1.FormattedValue), "Eingabefehler! - Wahlen", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                foreach (DataGridViewRow row2 in choiceView.Rows)
                {
                    if (row.Cells["Column7"].FormattedValue.Equals(row2.Cells["Column7"].FormattedValue) && row != row2)
                    {
                        MessageBox.Show(string.Format("Der Schüler \"{0}\" kann nicht mehrfach wählen!", row.Cells["Column7"].Value as Student), "Eingabefehler! - Wahlen", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                choices.Add(new Choice((row.Cells["Column7"].Value as Student).Id,
                    row.Cells["Column8"].Value is Course c1 ? c1.Id as int? : null,
                    row.Cells["Column9"].Value is Course c2 ? c2.Id as int? : null,
                    row.Cells["Column10"].Value is Course c3 ? c3.Id as int? : null));
            }

            Apply = true;
            ida.UpdateDatabase(ref students, ref courses, ref choices);
        }

        /// <summary>
        /// Sucht nach Auswahl eines Schülers bzw. Kurses aus einer ComboBox das dazugehörige Objekt.
        /// </summary>
        /// <param name="sender">Auslöser des Events (hier die ComboBoxCell)</param>
        /// <param name="e">Informationen über das Event</param>
		private void ChoiceView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (choiceView.Rows.Count > 0 && e.RowIndex >= 0 && choiceView.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewComboBoxCell cell && cell.Value != null && !cell.Value.Equals(defaultValue))
                switch (e.ColumnIndex)
                {
                    case 0: //erste Spalte --> Schüler
                        cell.Value = Array.Find(MainForm.Students.Values.ToArray(), s => s.ToString() == cell.Value.ToString());
                        break;
                    case int n when n >= 1 && n <= 3: //zweite bis vierte Spalte --> Kurs
                        cell.Value = Array.Find(MainForm.Courses.Values.ToArray(), c => c.ToString() == cell.Value.ToString());
                        break;
                    default: //sonst: Fehler
                        cell.Value = defaultValue;
                        break;
                }
        }
    }
}

