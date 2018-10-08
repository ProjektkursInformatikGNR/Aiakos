using System;

namespace Aiakos
{
	/// <summary>
	/// Die Klasse <c>Student</c> bildet eine Schülerin bzw. einen Schüler mit ihren bzw. seinen Attributen ab.
	/// </summary>
	public class Student
	{
		/// <summary>
		/// Erzeugt einen neuen Student.
		/// </summary>
		/// <param name="name">Name des Student</param>
		/// <param name="dateOfBirth">Geburtsdatum des Student</param>
		/// <param name="form">Klasse/Kurs des Student</param>
		/// <param name="id">Datenbank-ID des Student</param>
		public Student(string name, DateTime dateOfBirth, string form, int id = -1)
		{
			Id = id;
			Name = name;
			DateOfBirth = dateOfBirth;
			Form = form;
		}

		/// <summary>
		/// Datenbank-ID des Student
		/// </summary>
		public int Id { get; }

		/// <summary>
		/// Name des Student
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Geburtsdatum des Student
		/// </summary>
		public DateTime DateOfBirth { get; set; }

		/// <summary>
		/// Klasse/Kurs des Student
		/// </summary>
		public string Form { get; set; }

		/// <summary>
		/// Vergleichsoperation: Die Übereinstimmung der Datenbank-IDs genügt als Vergleich.
		/// </summary>
		/// <param name="obj">Vergleichsobjekt</param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			return obj is Student && Id == (obj as Student).Id;
		}

		/// <summary>
		/// Gibt den standardmäßig anzuzeigenden Text als Beschreibung der Instanz wieder.
		/// </summary>
		/// <returns>Beschreibung der Instanz</returns>
		public override string ToString()
		{
			return string.Format("{0} (geb. {1})", Name, DateOfBirth.ToShortDateString());
		}
	}
}