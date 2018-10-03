using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WahlzettelAnalyse
{
    public partial class DatePicker : Form
    {
        private string selectedDate = null;

        public DatePicker()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
        }

        public string SelectedDate
        {
            get { return selectedDate; }
            set { DateTime d = new DateTime(); dateTimePicker1.Value = DateTime.TryParse(value, out d) ? d : DateTime.Today; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            selectedDate = dateTimePicker1.Value.ToShortDateString();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dateTimePicker1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                selectedDate = dateTimePicker1.Value.ToShortDateString();
                this.Close();
            }
        }
    }
}
