using System;
using System.Windows.Forms;

namespace Aiakos
{
    /// <summary>
    /// Die Klasse <c>DatePicker</c> erzeugt für den Nutzer eine Maske zur Eingabe eines Datums.
    /// </summary>
    public partial class DatePicker : Form
    {
        private string selectedDate = null; //das zurzeit ausgewählte Datum

        /// <summary>
        /// Erzeugt eine neue Benutzermaske zur Eingabe eines Datums.
        /// </summary>
        public DatePicker()
        {
            InitializeComponent(); //initialisiert die Grafikkomponenten aus dem Designer
            ShowInTaskbar = false; //kein Symbol in der Taskleiste
        }

        /// <summary>
        /// Das zurzeit ausgewählte Datum
        /// </summary>
        public string SelectedDate
        {
            get { return selectedDate; }
            set { dateTimePicker1.Value = DateTime.TryParse(value, out DateTime d) ? d : DateTime.Today; } //Überprüfung der Richtigkeit des Datumsformats
        }

        /// <summary>
        /// Behandelt das Auswählen der Schaltfäche zur Bestätigung des Datums durch den Nutzer, indem das Datum zwischengespeichert und das Fenster geschlossen wird.
        /// </summary>
		/// <param name="sender">Auslöser des Events (hier der Button)</param>
		/// <param name="e">Informationen über das Event</param>
        private void ConfirmClick(object sender, EventArgs e)
        {
            selectedDate = dateTimePicker1.Value.ToShortDateString();
            Close();
        }

        /// <summary>
        /// Schließt auf Aufforderung des Nutzers das Fenster.
        /// </summary>
		/// <param name="sender">Auslöser des Events (hier der Button)</param>
		/// <param name="e">Informationen über das Event</param>
        private void CancelClick(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Behandelt die Bestätigung ebenso wie <see cref="ConfirmClick(object, EventArgs)"/>.
        /// </summary>
		/// <param name="sender">Auslöser des Events (hier der DateTimePicker)</param>
		/// <param name="e">Informationen über das Event</param>
        private void DateTimePicker_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                ConfirmClick(null, EventArgs.Empty);
        }
    }
}