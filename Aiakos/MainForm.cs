using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Aiakos
{
	/// <summary>
	/// Die Klasse <c>MainForm</c> bildet das Hauptfenster mit seinen Funktionen ab.
	/// </summary>
    public partial class MainForm : Form
    {
		/// <summary>
		/// Name des Programms
		/// </summary>
		public const string AppName = "Aiakos";
        
        /// <summary>
        /// Passwort zur Verschlüsselung der Daten
        /// </summary>
        public static byte[] Key { get; private set; }

        /// <summary>
        /// IV-Ergänzung zur Verschlüsselung der Daten
        /// </summary>
        public static byte[] Salt { get; private set; }

        /// <summary>
        /// alle Schülerinnen und Schüler mit ihren Indizes
        /// </summary>
        public static Dictionary<int, Student> Students;

		/// <summary>
		/// alle Kurse mit ihren Indizes
		/// </summary>
		public static Dictionary<int, Course> Courses;

		/// <summary>
		/// alle Wahlen mit den Indizes der wählenden Schülerin bzw. des wählenden Schülers
		/// </summary>
		public static Dictionary<int, Choice> Choices;

		/// <summary>
		/// Verbindung zur Datenbank
		/// </summary>
		public static DataAccess Data;

        private Chart chart = new Chart(); //die Grafik zur Visualisierung der Daten
        private ChartArea chartArea = new ChartArea(); //der Container für die Grafik
        private Series[] series = new Series[3]; //die Datensätze (jeweils einen für eine Wahlpriorität)
        private Color[] colors = { Color.FromArgb(255, 0, 0, 255), Color.FromArgb(126, 0, 0, 255), Color.FromArgb(40, 0, 0, 255) }; //die Farben für die Säulen
        private Legend legend = new Legend("Legend"); //die Legende der Grafik
        private Title title; //die Überschrift der Grafik

        private List<string> courseNames = new List<string>(); //eine separate Auflistung der Kursnamen
        private List<int>[] choiceNumbers = new List<int>[3]; //Angaben zur Anzahl der Wahlen für einen Kurs mit gegebener Priorität
		private int highestColumn; //die Höhe der höchsten Säule im Diagramm

		/// <summary>
		/// Erzeugt ein neues Hauptfenster und initialisiert die Komponenten.
		/// </summary>
        public MainForm()
        {
            InitializeComponent();
			Text = AppName;
            WindowState = FormWindowState.Maximized;

            ManagementObject[] m = new ManagementObject[1];
            new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_processor").Get().CopyTo(m, 0);
            byte[] processorId = Encoding.Unicode.GetBytes((string)m[0]["ProcessorId"]);

            Key = new byte[32];
            for (int i = 0; i < Key.Length; i++)
                Key[i] = processorId[i % processorId.Length];

            Salt = new byte[16];
            for (int i = 0; i < Salt.Length; i++)
                Salt[i] = processorId[i % processorId.Length];

            if (ServerConfiguration.DefaultServer.Available)
                Initialise();
            else
				RequestServerData(false);
        }

        /// <summary>
        /// Initialisiert die Container und Objekte der GUI.
        /// </summary>
        private void Initialise()
        {
            Data = new DataAccess(ServerConfiguration.DefaultServer);
            Data.FillData(out Students, out Courses, out Choices); //Befüllung der Listen mit Datensätzen aus der Datenbank

            chart.ChartAreas.Clear();
            chart.Titles.Clear();
            chart.Series.Clear();
            chart.Annotations.Clear();
            chart.Legends.Clear();
            Controls.Clear();

            Controls.Add(menuStrip1);

            courseNames = new List<string>();
            foreach (KeyValuePair<int, Course> course in Courses)
                courseNames.Add(course.Value.Name);

            for (int priority = 0; priority < 3; priority++)
			{
                choiceNumbers[priority] = new List<int>();

                foreach (int courseId in Courses.Keys)
                    choiceNumbers[priority].Add(Data.GetChoiceNumber(courseId, priority));
            }
            
            chart.PrePaint += Chart_PrePaint;
            title = chart.Titles.Add(ServerConfiguration.DefaultServer.Name);
            title.Font = new Font("Arial", 16, FontStyle.Bold);

            chartArea.AxisX.Title = "Kurse";
            chartArea.AxisX.TitleFont = new Font("Arial", 10, FontStyle.Bold);
            chartArea.AxisY.TitleFont = new Font("Arial", 10, FontStyle.Bold);
            chartArea.AxisY.Title = "Anzahl der Schüler";
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chart.ChartAreas.Add(chartArea);

            for (int priority = 0; priority < 3; priority++)
            {
				series[priority] = new Series
				{
					Name = (priority + 1) + ". Wahl",
					XValueType = ChartValueType.String,
					Color = colors[priority]
				};
				series[priority].Points.DataBindXY(courseNames, choiceNumbers[priority]);
                series[priority].ChartType = SeriesChartType.StackedColumn;
                chart.Series.Add(series[priority]);

                ToolTip tooltip1 = new ToolTip();

				int count1 = 0;
                foreach (KeyValuePair<int, Course> course in Courses)
                {
                    string toolTip = course.Value.Name + " - " + (priority + 1) + ". Wahl:";

                    foreach (Choice c in Choices.Values)
                        if (c.Courses[priority] == course.Key)
                            foreach (KeyValuePair<int, Student> student in Students)
                                if (c.StudentId == student.Key)
                                    toolTip += "\n" + student.Value.ToString();

                    series[priority].Points[count1++].ToolTip = toolTip;
                }
            }

            highestColumn = 0;
			int count2 = 0;
            foreach (KeyValuePair<int, Course> course in Courses)
            {
				if (choiceNumbers[0][count2] + choiceNumbers[1][count2] + choiceNumbers[2][count2] > highestColumn)
                    highestColumn = choiceNumbers[0][count2] + choiceNumbers[1][count2] + choiceNumbers[2][count2];

                count2++;
            }

            foreach (KeyValuePair<int, Course> course in Courses)
                if (course.Value.MaxStudents > highestColumn)
                    highestColumn = course.Value.MaxStudents;
            chartArea.AxisY.Maximum = highestColumn;

            legend.Font = new Font("Arial", 10);
            legend.LegendItemOrder = LegendItemOrder.ReversedSeriesOrder;
            chart.Legends.Add(legend);

            chart.Invalidate();
            Controls.Add(chart);
        }

		/// <summary>
		/// Aktualisiert die Grafik auf Grundlage der Daten, indem das Diagramm erneut gezeichnet wird.
		/// </summary>
		/// <param name="sender">Auslöser des Events (hier die <c>MainForm</c>)</param>
		/// <param name="e">Informationen über das Event (hier v. a. absolute Koordinaten der Grafik)</param>
        private void Chart_PrePaint(object sender, ChartPaintEventArgs e)
        {
            float x0 = (float) e.ChartGraphics.GetPositionFromAxis(chartArea.Name, AxisName.X, 0);
            float y0 = (float) e.ChartGraphics.GetPositionFromAxis(chartArea.Name, AxisName.Y, 0);
            float dx = Math.Abs((float) e.ChartGraphics.GetPositionFromAxis(chartArea.Name, AxisName.X, 1) - x0);
            float dy = Math.Abs((float) e.ChartGraphics.GetPositionFromAxis(chartArea.Name, AxisName.Y, 1) - y0);

            float width = 0.8f * dx;
            Pen pen = new Pen(Color.Red, 1);
            SolidBrush brush = new SolidBrush(Color.FromArgb(2, Color.GreenYellow));

            for (int i = 0; i < Courses.Count; i++)
            {
                float x = x0 + (i + 1) * dx;

                float y1 = y0 - Courses.ElementAt(i).Value.MinStudents * dy;
                RectangleF r1 = e.ChartGraphics.GetAbsoluteRectangle(new RectangleF(x - width / 2, y1, width, 0));
                e.ChartGraphics.Graphics.DrawLine(pen, r1.X, r1.Y, r1.X + r1.Width, r1.Y);

                float y2 = y0 - Courses.ElementAt(i).Value.MaxStudents * dy;
                RectangleF r2 = e.ChartGraphics.GetAbsoluteRectangle(new RectangleF(x - width / 2, y2, width, 0));
                e.ChartGraphics.Graphics.DrawLine(pen, r2.X, r2.Y, r2.X + r2.Width, r2.Y);

                RectangleF r3 = new RectangleF(r1.X, r2.Y, r1.Width, r1.Y - r2.Y);
                e.ChartGraphics.Graphics.FillRectangle(brush, r3);
            }
        }

		/// <summary>
		/// Passt die Grafik an, sobald der Nutzer die Fenstergröße verändert.
		/// </summary>
		/// <param name="sender">Auslöser des Events (hier die MainForm)</param>
		/// <param name="e">Informationen über das Event</param>
        private void Form1_Resize(object sender, EventArgs e)
        {
            chart.Height = Height - 60;
            chart.Width = Width;
            chart.Top = 22;
            chart.Invalidate();
        }

		/// <summary>
		/// Beendet das Programm, sobald der Nutzer die dafür vorgesehene Schaltfläche anklickt.
		/// </summary>
		/// <param name="sender">Auslöser des Events (hier die MainForm)</param>
		/// <param name="e">Informationen über das Event</param>
		private void ExitClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

		/// <summary>
		/// Öffnet die Maske zur Eingabe der Serverdaten, sobald der Nutzer die dafür vorgesehene Schaltfläche anklickt.
		/// </summary>
		/// <param name="sender">Auslöser des Events (hier die MainForm)</param>
		/// <param name="e">Informationen über das Event</param>
		private void ServerConfigClick(object sender, EventArgs e)
        {
			RequestServerData(false);
        }

		/// <summary>
		/// Aktualisiert die Visualisierung der Daten, sobald der Nutzer die dafür vorgesehene Schaltfläche anklickt.
		/// </summary>
		/// <param name="sender">Auslöser des Events (hier die MainForm)</param>
		/// <param name="e">Informationen über das Event</param>
		private void DataUpdateClick(object sender, EventArgs e)
        {
            if (ServerConfiguration.DefaultServer.Available)
                Initialise();
            else
				RequestServerData(true);
        }

		/// <summary>
		/// Öffnet die Maske zur Bearbeitung der Datensätze, sobald der Nutzer die dafür vorgesehene Schaltfläche anklickt.
		/// </summary>
		/// <param name="sender">Auslöser des Events (hier die MainForm)</param>
		/// <param name="e">Informationen über das Event</param>
		private void DataAdminClick(object sender, EventArgs e)
        {
            DataAdministration dataAd = new DataAdministration();
            dataAd.ShowDialog();

            if (dataAd.Apply)
                Initialise();
        }

		/// <summary>
		/// Fordert den Nutzer auf, die Serverdaten in einer neuen Serverkonfigurationsmaske zu korrigieren.
		/// </summary>
		/// <param name="error">Ist <c>TRUE</c>, wenn eine fehlerbehaftete Verbindung aufzubauen versucht wurde, und andernfalls <c>FALSE</c>.</param>
		private void RequestServerData(bool error)
		{
			if (error)
				MessageBox.Show("Verbindung zum Server kann nicht aufgebaut werden!", "Verbindungsfehler!", MessageBoxButtons.OK, MessageBoxIcon.Error);

			ServerConfigurationGUI configGUI = new ServerConfigurationGUI();
			if (configGUI.Confirmed)
				Initialise();
		}
    }
}