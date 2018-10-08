using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Aiakos
{
    public partial class MainForm : Form
    {
		public const string AppName = "Aiakos";

        public static Dictionary<int, Student> Students;
        public static Dictionary<int, Course> Courses;
        public static Dictionary<int, Choice> Choices;
		
        public static DataAccess Data;

        private Chart chart = new Chart();
        private ChartArea chartArea = new ChartArea();
        private Series[] series = new Series[3];
        private Color[] colors = { Color.FromArgb(255, 0, 0, 255), Color.FromArgb(126, 0, 0, 255), Color.FromArgb(40, 0, 0, 255) };
        private HorizontalLineAnnotation[][] annotations;
        private Legend legend = new Legend("Legend");
        private Title title;

        private List<string> courseNames = new List<string>();
        private List<int>[] choiceNumbers = new List<int>[3];
        private int highestColumn, count;

        public MainForm()
        {
            InitializeComponent();
			Text = AppName;
            WindowState = FormWindowState.Maximized;
			ServerConfiguration.ReadServerData();

            if (ServerConfiguration.DefaultServer.ServerAvailable)
                Initialise();
            else
				RequestServerData(false);
        }

        private void Initialise()
        {
            Data = new DataAccess(ServerConfiguration.DefaultServer);
            Data.FillData(out Students, out Courses, out Choices);

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

            annotations = new HorizontalLineAnnotation[Courses.Count][];
            
            chart.PrePaint += Chart_PostPaint;
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
                series[priority] = new Series();
                series[priority].Name = (priority + 1) + ". Wahl";
                series[priority].XValueType = ChartValueType.String;
                series[priority].Color = colors[priority];
                series[priority].Points.DataBindXY(courseNames, choiceNumbers[priority]);
                series[priority].ChartType = SeriesChartType.StackedColumn;
                chart.Series.Add(series[priority]);

                ToolTip tooltip1 = new ToolTip();
                count = 0;

                foreach (KeyValuePair<int, Course> course in Courses)
                {
                    string toolTip = course.Value.Name + " - " + (priority + 1) + ". Wahl:";

                    foreach (Choice c in Choices.Values)
                        if (c.Courses[priority] == course.Key)
                            foreach (KeyValuePair<int, Student> student in Students)
                                if (c.StudentId == student.Key)
                                    toolTip += "\n" + student.Value.ToString();

                    series[priority].Points[count++].ToolTip = toolTip;
                }
            }

            highestColumn = 0;
            count = 0;

            foreach (KeyValuePair<int, Course> course in Courses)
            {
                if (choiceNumbers[0][count] + choiceNumbers[1][count] + choiceNumbers[2][count] > highestColumn)
                    highestColumn = choiceNumbers[0][count] + choiceNumbers[1][count] + choiceNumbers[2][count];

                count++;
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

        private void Chart_PostPaint(object sender, ChartPaintEventArgs e)
        {
            float x0 = (float)e.ChartGraphics.GetPositionFromAxis(chartArea.Name, AxisName.X, 0);
            float y0 = (float)e.ChartGraphics.GetPositionFromAxis(chartArea.Name, AxisName.Y, 0);
            float dx = Math.Abs((float)e.ChartGraphics.GetPositionFromAxis(chartArea.Name, AxisName.X, 1) - x0);
            float dy = Math.Abs((float)e.ChartGraphics.GetPositionFromAxis(chartArea.Name, AxisName.Y, 1) - y0);

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

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (Height >= 60)
                chart.Height = Height - 60;
            chart.Width = Width;
            chart.Top = 22;
            chart.Invalidate();
        }

        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void serverkonfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
			RequestServerData(false);
        }

        private void datenAktualisierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ServerConfiguration.DefaultServer.ServerAvailable)
                Initialise();
            else
				RequestServerData(true);
        }

        private void datenverwaltungToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataAdministration dataAd = new DataAdministration(ref Data);
            dataAd.ShowDialog();

            if (dataAd.Apply)
                Initialise();
        }

		private void RequestServerData(bool error)
		{
			if (error)
				MessageBox.Show("Verbindung zum Server kann nicht aufgebaut werden!", "Verbindungsfehler!", MessageBoxButtons.OK, MessageBoxIcon.Error);

			ServerConfigurationGUI.ShowDialog();
			if (ServerConfigurationGUI.Confirmed)
				Initialise();
		}
    }
}