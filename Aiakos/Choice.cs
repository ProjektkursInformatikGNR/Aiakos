﻿namespace Aiakos
{
	/// <summary>
	/// Die Klasse <c>Choice</c> bildet eine Zusammenstellung der Wahlen einer Schülerin bzw. eines Schülers ab.
	/// </summary>
    public class Choice
    {
		/// <summary>
		/// Erzeugt eine neue Choice.
		/// </summary>
		/// <param name="studentId">Datenbank-ID des Student</param>
		/// <param name="courseId1">Datenbank-ID des erstgewählten Course</param>
		/// <param name="courseId2">Datenbank-ID des zweitgewählten Course</param>
		/// <param name="courseId3">Datenbank-ID des drittgewählten Course</param>
		public Choice(int studentId, int? courseId1, int? courseId2, int? courseId3)
        {
			StudentId = studentId;
			CourseId1 = courseId1;
			CourseId2 = courseId2;
			CourseId3 = courseId3;
        }

		/// <summary>
		/// Datenbank-ID des Student
		/// </summary>
		public int StudentId { get; set; }

		/// <summary>
		/// Datenbank-ID des erstgewählten Course
		/// </summary>
		public int? CourseId1 { get; set; }

		/// <summary>
		/// Datenbank-ID des zweitgewählten Course
		/// </summary>
		public int? CourseId2 { get; set; }

		/// <summary>
		/// Datenbank-ID des drittgewählten Course
		/// </summary>
		public int? CourseId3 { get; set; }

		/// <summary>
		/// Gibt die Datenbank-IDs aller gewählten Courses als Array nach aufsteigender Priorität geordnet wieder.
		/// </summary>
		public int?[] Courses
		{
			get
			{
				return new int?[] { CourseId1, CourseId2, CourseId3 };
			}
		}

		/// <summary>
		/// Vergleichsoperation: Die Übereinstimmung der Datenbank-IDs des Student genügt als Vergleich, da eine Schülerin bzw. ein Schüler nur einmal wählen können sollte.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			return obj is Choice o && o.StudentId == StudentId;
		}

		/// <summary>
		/// Gibt den standardmäßig anzuzeigenden Text als Beschreibung der Instanz wieder.
		/// </summary>
		/// <returns>Beschreibung der Instanz</returns>
		public override string ToString()
		{
			return string.Format("Wahl von {0}", MainForm.Students[StudentId].ToString());
		}

		/// <summary>
		/// Hash-Code zur eindeutigen Zuordnung
		/// </summary>
		/// <returns>Hash-Code</returns>
		public override int GetHashCode()
		{
			return StudentId;
		}
	}
}