using System;
using System.Windows.Forms;

namespace Aiakos
{
	/// <summary>
	/// Die Klasse <c>Program</c> stellt die automatisch generierte Hauptklasse dar.
	/// </summary>
    public static class Program
    {
		/// <summary>
		/// Wird automatisch zum Programmstart aufgerufen und erzeugt lediglich das Hauptfenster der Klasse <seealso cref="MainForm"/>.
		/// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}