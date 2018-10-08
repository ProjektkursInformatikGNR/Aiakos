using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;

namespace Aiakos
{
    public partial class DataAdministration : Form
    {
        bool apply;
        ContextMenuStrip context = new ContextMenuStrip();
        ToolStripMenuItem copyMenu = new ToolStripMenuItem("Kopieren"), pasteMenu = new ToolStripMenuItem("Einfügen"), cutMenu = new ToolStripMenuItem("Ausschneiden"), removeMenu = new ToolStripMenuItem("Löschen");
        DataGridView view;
        Assembly assembly = Assembly.GetExecutingAssembly();
        Bitmap copyIcon, pasteIcon, cutIcon, removeIcon;

        public DataAdministration(ref DataAccess da)
        {
            InitializeComponent();
            apply = false;
            ShowInTaskbar = false;

            copyIcon = new Bitmap(assembly.GetManifestResourceStream("Aiakos.copy.png"));
            pasteIcon = new Bitmap(assembly.GetManifestResourceStream("Aiakos.paste.png"));
            cutIcon = new Bitmap(assembly.GetManifestResourceStream("Aiakos.cut.png"));
            removeIcon = new Bitmap(assembly.GetManifestResourceStream("Aiakos.remove.png"));

            copyMenu.Image = copyIcon;
            copyMenu.ShortcutKeys = Keys.Control | Keys.C;
            copyMenu.Click += new EventHandler(copy_Click);
            context.Items.Add(copyMenu);
            pasteMenu.Image = pasteIcon;
            pasteMenu.ShortcutKeys = Keys.Control | Keys.V;
            pasteMenu.Click += new EventHandler(paste_Click);
            context.Items.Add(pasteMenu);
            cutMenu.Image = cutIcon;
            cutMenu.ShortcutKeys = Keys.Control | Keys.X;
            cutMenu.Click += new EventHandler(cutMenu_Click);
            context.Items.Add(cutMenu);
            removeMenu.Image = removeIcon;
            removeMenu.ShortcutKeys = Keys.Delete;
            removeMenu.Click += new EventHandler(removeMenu_Click);
            context.Items.Add(removeMenu);

            courseView.ScrollBars = ScrollBars.Both;
            courseView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            courseView.GridColor = Color.Black;
            courseView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            courseView.RowHeadersVisible = false;
            courseView.CellMouseDown += new DataGridViewCellMouseEventHandler(courseView_CellMouseDown);

            foreach (KeyValuePair<int, Course> course in Form1.Courses ?? new Dictionary<int, Course>())
            {
                DataGridViewRow row = (DataGridViewRow)courseView.Rows[0].Clone();
                row.SetValues(new string[] {
                    course.Value.Name,
                    course.Value.MinYear + "-" + course.Value.MaxYear,
                    course.Value.MinStudents + "-" + course.Value.MaxStudents});
                row.Tag = course.Key;
                courseView.Rows.Add(row);
            }

            coursePage.Controls.Add(courseView);
            coursePage.TabIndex = 0;

            studentView.ScrollBars = ScrollBars.Both;
            studentView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            studentView.GridColor = Color.Black;
            studentView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            studentView.RowHeadersVisible = false;
            studentView.CellMouseDown += new DataGridViewCellMouseEventHandler(studentView_CellMouseDown);
            studentView.CellClick += new DataGridViewCellEventHandler(studentView_CellClick);

            foreach (KeyValuePair<int, Student> student in Form1.Students ?? new Dictionary<int, Student>())
            {
                DataGridViewRow row = (DataGridViewRow)studentView.Rows[0].Clone();
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

            choiceView.ScrollBars = ScrollBars.Both;
            choiceView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            choiceView.GridColor = Color.Black;
            choiceView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            choiceView.RowHeadersVisible = false;
            choiceView.CellMouseDown += new DataGridViewCellMouseEventHandler(choiceView_CellMouseDown);

            ((DataGridViewComboBoxColumn)choiceView.Columns[0]).Items.AddRange(Form1.Students.Values.ToArray());
            ((DataGridViewComboBoxColumn)choiceView.Columns[1]).Items.AddRange(Form1.Courses.Values.ToArray());
			((DataGridViewComboBoxColumn)choiceView.Columns[2]).Items.AddRange(Form1.Courses.Values.ToArray());
			((DataGridViewComboBoxColumn)choiceView.Columns[3]).Items.AddRange(Form1.Courses.Values.ToArray());

			foreach (KeyValuePair<int, Choice> choice in Form1.Choices)
			{
				DataGridViewRow row = (DataGridViewRow)choiceView.Rows[0].Clone();
				row.SetValues(new object[] {
					Form1.Students[choice.Value.StudentId],
					Form1.Courses[choice.Value.CourseId1],
					Form1.Courses[choice.Value.CourseId2],
					Form1.Courses[choice.Value.CourseId3]});
				row.Tag = choice.Value.StudentId;
				choiceView.Rows.Add(row);
			}

			choicePage.Controls.Add(choiceView);
            choicePage.TabIndex = 0;

            Controls.Add(tabs);
        }

        void removeMenu_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in view.SelectedRows)
            {
                view.Rows.Remove(row);
            }
        }

        void cutMenu_Click(object sender, EventArgs e)
        {
            cut();
        }

        void paste_Click(object sender, EventArgs e)
        {
            paste();
        }

        void copy_Click(object sender, EventArgs e)
        {
            copy();
        }

        void courseView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                view = courseView;
                context.Show(sender as Control, e.Location);
            }
        }

        void studentView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                view = studentView;
                context.Show(sender as Control, e.Location);
            }
        }

        void choiceView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                view = choiceView;
                context.Show(sender as Control, e.Location);
            }
        }

        void studentView_CellClick(object sender, DataGridViewCellEventArgs e)
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

        void prompt_SizeChanged(object sender, System.EventArgs e)
        {
            tabs.Size = new Size(Width - 14, Height - 38);
            courseView.Size = new Size(Width - 14, Height - 38);
            studentView.Size = new Size(Width - 14, Height - 38);
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            apply = true;
            update();

            if (apply)
                this.Close();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            update();
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void DataAdministration_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!apply && MessageBox.Show("Sind Sie sicher?\n(Alle ungespeichert geänderten Daten gehen verloren!)", "Warnung!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void copy()
        {
            string text = "";

            foreach (DataGridViewRow row in view.SelectedRows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    text += cell.Value + "\u0009";
                }

                text += "\u000D\u000A";
            }

            Clipboard.SetText(text, TextDataFormat.Text);
        }

        private void paste()
        {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) && view != choiceView)
            {
                List<DataGridViewRow> rows = new List<DataGridViewRow>();

                foreach (string row in Clipboard.GetText().Split("\u000D\u000A".ToCharArray()))
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

        private void cut()
        {
            copy();

            foreach (DataGridViewRow row in view.SelectedRows)
            {
                view.Rows.Remove(row);
            }
        }

        private void update()
        {
            List<Course> courses = new List<Course>();

            foreach (DataGridViewRow row in courseView.Rows)
            {
                if (row.Cells["Column1"].Value != null
                    || row.Cells["Column2"].Value != null
                    || row.Cells["Column3"].Value != null)
                {
                    if (row.Cells["Column1"].FormattedValue.ToString().Trim().Equals(""))
                    {
                        MessageBox.Show("Bitte tragen Sie einen Namen in Zeile " + (row.Index + 1) + " ein!", "Eingabefehler! - Kurse", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        apply = false;
                        return;
                    }
                    else if (row.Cells["Column2"].FormattedValue.ToString().Split('-').Length != 2
                        || !row.Cells["Column2"].FormattedValue.ToString().All(character => (Char.IsDigit(character) || character == '-'))
                        || row.Cells["Column2"].FormattedValue.ToString().StartsWith("-")
                        || row.Cells["Column2"].FormattedValue.ToString().EndsWith("-"))
                    {
                        MessageBox.Show("Bitte tragen Sie die Jahrgänge in Zeile " + (row.Index + 1) + " im korrekten Format (Jahr-Jahr) ein!", "Eingabefehler! - Kurse", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        apply = false;
                        return;
                    }
                    else if (row.Cells["Column3"].FormattedValue.ToString().Split('-').Length != 2
                        || !row.Cells["Column3"].FormattedValue.ToString().All(character => Char.IsDigit(character) || character == '-')
                        || row.Cells["Column3"].FormattedValue.ToString().StartsWith("-")
                        || row.Cells["Column3"].FormattedValue.ToString().EndsWith("-"))
                    {
                        MessageBox.Show("Bitte tragen Sie die Teilnehmerzahlen in Zeile " + (row.Index + 1) + " im korrekten Format (Minimum-Maximum) ein!", "Eingabefehler! - Kurse", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        apply = false;
                        return;
                    }
                    else
                    {
                        foreach (DataGridViewRow row2 in courseView.Rows)
                        {
                            if (row.Cells["Column1"].FormattedValue.ToString() == row2.Cells["Column1"].FormattedValue.ToString()
                                && row != row2)
                            {
                                MessageBox.Show("Der Kurs \"" + row.Cells["Column1"].FormattedValue.ToString() + "\" in Zeile " + (row.Index + 1) + " kann nicht eindeutig von dem in Zeile " + (row2.Index + 1) + " unterschieden werden!", "Eingabefehler! - Kurse", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                apply = false;
                                return;
                            }
                        }
						
						courses.Add(new Course(
							row.Cells["Column1"].Value.ToString(),
							int.Parse(row.Cells["Column2"].Value.ToString().Split('-').ElementAt(0)),
							int.Parse(row.Cells["Column2"].Value.ToString().Split('-').ElementAt(1)),
							int.Parse(row.Cells["Column3"].Value.ToString().Split('-').ElementAt(0)),
							int.Parse(row.Cells["Column3"].Value.ToString().Split('-').ElementAt(1)),
							(int)(row.Tag ?? -1)
						));
					}
                }
            }

            List<Student> students = new List<Student>();

            foreach (DataGridViewRow row in studentView.Rows)
            {
                if (row.Cells["Column4"].Value != null
                    || row.Cells["Column5"].Value != null
                    || row.Cells["Column6"].Value != null)
                {
                    DateTime d = new DateTime();

                    if (row.Cells["Column4"].FormattedValue.ToString().Trim().Equals(""))
                    {
                        MessageBox.Show("Bitte tragen Sie einen Namen in Zeile " + (row.Index + 1) + " ein!", "Eingabefehler! - Schüler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        apply = false;
                        return;
                    }
                    else if (!DateTime.TryParse(row.Cells["Column5"].FormattedValue.ToString(), out d))
                    {
                        MessageBox.Show("Bitte tragen Sie das Geburtsdatum in Zeile " + (row.Index + 1) + " im korrekten Format (YYYY-MM-DD oder DD.MM.YYYY) ein!", "Eingabefehler! - Schüler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        apply = false;
                        return;
                    }
                    else if (row.Cells["Column6"].FormattedValue.ToString().Trim().Equals(""))
					{
                        MessageBox.Show("Bitte tragen Sie die Klasse in Zeile " + (row.Index + 1) + "  ein!", "Eingabefehler! - Schüler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        apply = false;
                        return;
                    }
                    else
                    {
                        foreach (DataGridViewRow row2 in studentView.Rows)
                        {
                            if (row.Cells["Column4"].FormattedValue.ToString() == row2.Cells["Column4"].FormattedValue.ToString()
                                && row.Cells["Column5"].FormattedValue.ToString() == row2.Cells["Column5"].FormattedValue.ToString()
                                && row != row2)
                            {
                                MessageBox.Show("Der Schüler \"" + row.Cells["Column4"].FormattedValue.ToString() + "\", geboren am " + DateTime.Parse(row.Cells["Column5"].FormattedValue.ToString()).ToLongDateString().Split(',')[0] + ", den" + DateTime.Parse(row.Cells["Column5"].FormattedValue.ToString()).ToLongDateString().Split(',')[1] + ", in Zeile " + (row.Index + 1) + " kann nicht eindeutig von dem in Zeile " + (row2.Index + 1) + " unterschieden werden!", "Eingabefehler! - Schüler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                apply = false;
                                return;
                            }
                        }
						
						students.Add(new Student(
							row.Cells["Column4"].Value.ToString(),
							DateTime.Parse(row.Cells["Column5"].Value.ToString()),
							row.Cells["Column6"].Value.ToString(),
							(int) (row.Tag ?? -1)
						));
					}
                }
            }

            List<Choice> choices = new List<Choice>();

            foreach (DataGridViewRow row in choiceView.Rows)
            {
                if (row.Cells["Column7"].Value != null
                    || row.Cells["Column8"].Value != null
                    || row.Cells["Column9"].Value != null)
                {
                    if (((Student)row.Cells["Column7"].Value) == null)
                    {
                        MessageBox.Show("Bitte wählen Sie einen Schüler in Zeile " + (row.Index + 1) + " aus!", "Eingabefehler! - Wahlen", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        apply = false;
                        return;
                    }
                    else if (((Course)row.Cells["Column8"].Value) == null)
                    {
                        MessageBox.Show("Bitte wählen Sie einen Kurs in Zeile " + (row.Index + 1) + " aus!", "Eingabefehler! - Wahlen", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        apply = false;
                        return;
					}
					else if (((Course)row.Cells["Column9"].Value) == null)
					{
						MessageBox.Show("Bitte wählen Sie einen Kurs in Zeile " + (row.Index + 1) + " aus!", "Eingabefehler! - Wahlen", MessageBoxButtons.OK, MessageBoxIcon.Error);
						apply = false;
						return;
					}
					else if (((Course)row.Cells["Column10"].Value) == null)
					{
						MessageBox.Show("Bitte wählen Sie einen Kurs in Zeile " + (row.Index + 1) + " aus!", "Eingabefehler! - Wahlen", MessageBoxButtons.OK, MessageBoxIcon.Error);
						apply = false;
						return;
					}
					else
                    {
                        foreach (DataGridViewRow row2 in choiceView.Rows)
                        {
                            if (row.Cells["Column7"].FormattedValue.ToString() == row2.Cells["Column7"].FormattedValue.ToString()
                                && row.Cells["Column8"].FormattedValue.ToString() == row2.Cells["Column8"].FormattedValue.ToString()
                                && row != row2)
                            {
                                MessageBox.Show("Der Schüler \"" + ((Student)row.Cells["Column7"].Value).Name + "\", geboren am " + ((Student)row.Cells["Column7"].Value).DateOfBirth.ToLongDateString().Split(',')[0] + ", den" + ((Student)row.Cells["Column7"].Value).DateOfBirth.ToLongDateString().Split(',')[1] + ", kann den Kurs \"" + row.Cells["Column8"].FormattedValue.ToString() + "\" in Zeile " + (row.Index + 1) + " bzw. " + (row2.Index + 1) + " nicht doppelt belegen!", "Eingabefehler! - Wahlen", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                apply = false;
                                return;
                            }
                        }

                        choices.Add(new Choice(((Student)row.Cells["Column7"].Value).Id,
                            ((Course)row.Cells["Column8"].Value).Id,
							((Course)row.Cells["Column9"].Value).Id,
							((Course)row.Cells["Column10"].Value).Id));
                    }
                }
            }

            Form1.da.UpdateDatabase(ref courses, ref students, ref choices);
        }

        public bool Apply
        {
            get { return apply; }
        }

        private void choiceView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (choiceView.Rows.Count > 0 && e.RowIndex >= 0)
            {
                DataGridViewComboBoxCell cell = choiceView.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewComboBoxCell;

                if (cell != null && cell.Value != null)
                {
                    switch (e.ColumnIndex)
                    {
                        case 0:
                            cell.Value = Array.Find(Form1.Students.Values.ToArray(), a => a.ToString() == cell.Value.ToString());
                            break;
                        case 1:
                            cell.Value = Array.Find(Form1.Courses.Values.ToArray(), a => a.ToString() == cell.Value.ToString());
                            break;
						case 2:
							cell.Value = Array.Find(Form1.Courses.Values.ToArray(), a => a.ToString() == cell.Value.ToString());
							break;
						case 3:
							cell.Value = Array.Find(Form1.Courses.Values.ToArray(), a => a.ToString() == cell.Value.ToString());
							break;
					}
                }
            }
        }
    }
}
