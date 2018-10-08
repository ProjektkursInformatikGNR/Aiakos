namespace Aiakos
{
	/// <summary>
	/// Die Klasse <c>Course</c> bildet einen Stärken- bzw. Förderkurs oder eine AG ab.
	/// </summary>
    public class Course
    {
		/// <summary>
		/// Erzeugt einen neuen Course.
		/// </summary>
		/// <param name="name">Name des Course</param>
		/// <param name="minYear">Mindestjahrgang der am Course teilnehmenden Schülerinnen bzw. Schüler</param>
		/// <param name="maxYear">Höchstjahrgang der am Course teilnehmenden Schülerinnen bzw. Schüler</param>
		/// <param name="minStudents">Mindestteilnehmerzahl des Course</param>
		/// <param name="maxStudents">Höchstteilnehmerzahl des Course</param>
		/// <param name="id">Datenbank-ID des Course</param>
		public Course(string name, int minYear, int maxYear, int minStudents, int maxStudents, int id = -1)
        {
            Name = name;
            Id = id;
            MinYear = minYear;
            MaxYear = maxYear;
            MinStudents = minStudents;
            MaxStudents = maxStudents;
        }

		/// <summary>
		/// Datenbank-ID des Course
		/// </summary>
		public int Id { get; }

		/// <summary>
		/// Name des Course
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Mindestjahrgang der am Course teilnehmenden Schülerinnen bzw. Schüler
		/// </summary>
		public int MinYear { get; set; }

		/// <summary>
		/// Höchstjahrgang der am Course teilnehmenden Schülerinnen bzw. Schüler
		/// </summary>
		public int MaxYear { get; set; }

		/// <summary>
		/// Mindestteilnehmerzahl des Course
		/// </summary>
		public int MinStudents { get; set; }

		/// <summary>
		/// Höchstteilnehmerzahl des Course
		/// </summary>
		public int MaxStudents { get; set; }

		/// <summary>
		/// Vergleichsoperation: Die Übereinstimmung der Datenbank-IDs genügt als Vergleich.
		/// </summary>
		/// <param name="obj">Vergleichsobjekt</param>
		/// <returns></returns>
		public override bool Equals(object obj)
        {
			return obj is Course && Id == (obj as Course).Id;
        }

		/// <summary>
		/// Gibt den standardmäßig anzuzeigenden Text als Beschreibung der Instanz wieder.
		/// </summary>
		/// <returns>Beschreibung der Instanz</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}