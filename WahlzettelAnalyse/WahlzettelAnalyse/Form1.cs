using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MySql.Data.MySqlClient;
using System.Drawing.Drawing2D;

namespace WahlzettelAnalyse
{
    public partial class Form1 : Form
    {
        public static Dictionary<int, Student> students;
        public static Dictionary<int, Course> courses;
        public static Dictionary<int, Choice> choices;

		GuiCfg cfg = new GuiCfg(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Roaming\\WahlzettelAnalyse\\cfg.xml");
        public static DataAccess da;

        Chart chart = new Chart();
        ChartArea chartArea = new ChartArea();
        Series[] series = new Series[3];
        Color[] colors = { Color.FromArgb(255, 0, 0, 255), Color.FromArgb(126, 0, 0, 255), Color.FromArgb(40, 0, 0, 255) };
        HorizontalLineAnnotation[][] annotations;
        Legend legend = new Legend("Legend");
        Title title;

        List<string> courseNames = new List<string>();
        List<int>[] choiceNumbers = new List<int>[3];
        int highestColumn, count;

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            cfg.readConfigFile();

            if (DataAccess.ConnectionValid(cfg.DefaultServer))
            {
                this.init();
            }
            else
            {
                MessageBox.Show("Verbindung zum Server kann nicht aufgebaut werden!", "Verbindungsfehler!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ServerConfiguration.showDialog(cfg);

                if (ServerConfiguration.Confirmation)
                {
                    cfg = ServerConfiguration.Configuration;
                    this.init();
                }
            }
        }

        private void init()
        {
            da = new DataAccess(cfg.DefaultServer);
            da.FillData(out students, out courses, out choices);

            chart.ChartAreas.Clear();
            chart.Titles.Clear();
            chart.Series.Clear();
            chart.Annotations.Clear();
            chart.Legends.Clear();
            this.Controls.Clear();

            this.Controls.Add(menuStrip1);

            courseNames = new List<string>();
            foreach (KeyValuePair<int, Course> course in courses)
            {
                courseNames.Add(course.Value.Name);
            }

            for (int priority = 0; priority < 3; priority++)
            {
                choiceNumbers[priority] = new List<int>();

                foreach (KeyValuePair<int, Course> course in courses)
                {
                    choiceNumbers[priority].Add(da.GetChoiceNumber(course.Key, priority));
                }
            }

            annotations = new HorizontalLineAnnotation[courses.Count][];
            
            chart.PrePaint += Chart_PostPaint;
            title = chart.Titles.Add(cfg.DefaultServer.Name);
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

                foreach (KeyValuePair<int, Course> course in courses)
                {
                    string toolTip = course.Value.Name + " - " + (priority + 1) + ". Wahl:";

                    foreach (Choice c in choices.Values)
                    {
                        if (c.Courses[priority] == course.Key)
                        {
                            foreach (KeyValuePair<int, Student> student in students)
                            {
                                if (c.StudentId == student.Key)
                                    toolTip += "\n" + student.Value.ToString();
                            }
                        }
                    }

                    series[priority].Points[count++].ToolTip = toolTip;
                }
            }

            highestColumn = 0;
            count = 0;

            foreach (KeyValuePair<int, Course> course in courses)
            {
                if (choiceNumbers[0][count] + choiceNumbers[1][count] + choiceNumbers[2][count] > highestColumn)
                {
                    highestColumn = choiceNumbers[0][count] + choiceNumbers[1][count] + choiceNumbers[2][count];
                }

                count++;
            }

            foreach (KeyValuePair<int, Course> course in courses)
            {
                if (course.Value.MaxStudents > highestColumn)
                {
                    highestColumn = course.Value.MaxStudents;
                }
            }
            chartArea.AxisY.Maximum = highestColumn;

            legend.Font = new Font("Arial", 10);
            legend.LegendItemOrder = LegendItemOrder.ReversedSeriesOrder;
            chart.Legends.Add(legend);

            chart.Invalidate();
            this.Controls.Add(chart);
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

            for (int i = 0; i < courses.Count; i++)
            {
                float x = x0 + (i + 1) * dx;

                float y1 = y0 - courses.ElementAt(i).Value.MinStudents * dy;
                RectangleF r1 = e.ChartGraphics.GetAbsoluteRectangle(new RectangleF(x - width / 2, y1, width, 0));
                e.ChartGraphics.Graphics.DrawLine(pen, r1.X, r1.Y, r1.X + r1.Width, r1.Y);

                float y2 = y0 - courses.ElementAt(i).Value.MaxStudents * dy;
                RectangleF r2 = e.ChartGraphics.GetAbsoluteRectangle(new RectangleF(x - width / 2, y2, width, 0));
                e.ChartGraphics.Graphics.DrawLine(pen, r2.X, r2.Y, r2.X + r2.Width, r2.Y);

                RectangleF r3 = new RectangleF(r1.X, r2.Y, r1.Width, r1.Y - r2.Y);
                e.ChartGraphics.Graphics.FillRectangle(brush, r3);
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.Height >= 60)
                chart.Height = this.Height - 60;
            chart.Width = this.Width;
            chart.Top = 22;
            chart.Invalidate();
        }

        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void serverkonfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ServerConfiguration.showDialog(cfg);

            if (ServerConfiguration.Confirmation)
            {
                cfg = ServerConfiguration.Configuration;
                init();
            }
        }

        private void datenAktualisierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DataAccess.ConnectionValid(cfg.DefaultServer))
            {
                this.init();
            }
            else
            {
                MessageBox.Show("Verbindung zum Server kann nicht aufgebaut werden!", "Verbindungsfehler!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ServerConfiguration.showDialog(cfg);

                if (ServerConfiguration.Confirmation)
                {
                    cfg = ServerConfiguration.Configuration;
                    this.init();
                }
            }
        }

        private void datenverwaltungToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataAdministration dataAd = new DataAdministration(ref da);
            dataAd.ShowDialog();

            if (dataAd.Apply)
                this.init();
        }
    }
}
