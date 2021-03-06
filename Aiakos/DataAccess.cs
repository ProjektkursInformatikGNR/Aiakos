﻿using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace Aiakos
{
	/// <summary>
	/// Die Klasse <c>DataAccess</c> bezieht die Daten aus der MySQL-Datenbank und konvertiert sie.
	/// </summary>
    public class DataAccess : IDataAccess
    {
        private MySqlConnection connection; //Die Verbindung zum MySQL-Server

		/// <summary>
		/// Erzeugt einen neuen Datenzugriff anhand der übergegebenen Serverdaten.
		/// </summary>
		/// <param name="server">Die Informationen zum Server und zur SQL-Datenbank</param>
        public DataAccess(Server server)
        {
            connection = new MySqlConnection(server.ToString());
        }
		
		/// <summary>
		/// Gibt die Ergebnisse einer SQL-Abfrage in einer Matrix wieder.
		/// </summary>
		/// <param name="cmd">Der auszuführende SQL-Befehl</param>
		/// <returns>Die Rückgabe der Datenbank</returns>
        public object[][] GetData(string cmd)
		{
			connection.Open();
			MySqlDataReader reader = new MySqlCommand(cmd, connection).ExecuteReader();
			DataTable data = new DataTable();
			data.Load(reader);
			reader.Close();
			connection.Close();
			return Array.ConvertAll(data.Select(), row => row.ItemArray);
		}

		/// <summary>
		/// Füllt die übergebenen Dictionaries mit den Daten zur Schülerschaft, zum Wahlangebot und zu den getroffenen Wahlen durch Zugriff auf die MySQL-Datenbank.
		/// </summary>
		/// <param name="students">Speicherort für die Schülerdaten</param>
		/// <param name="courses">Speicherort für das Kursangebot</param>
		/// <param name="choices">Speicherort für die Wahlen</param>
        public void FillData(out Dictionary<int, Student> students, out Dictionary<int, Course> courses, out Dictionary<int, Choice> choices)
        {
			students = new Dictionary<int, Student>();
			foreach (object[] row in GetData("SELECT * FROM students;"))
				students.Add(int.Parse(row[0].ToString()),
					new Student(row[1].ToString(),
					DateTime.Parse(row[2].ToString()),
					row[3].ToString(),
					int.Parse(row[0].ToString())
				));

			courses = new Dictionary<int, Course>();
			foreach (object[] row in GetData("SELECT * FROM courses;"))
                courses.Add(int.Parse(row[0].ToString()),
                    new Course(row[1].ToString(),
                    int.Parse(row[2].ToString()),
                    int.Parse(row[3].ToString()),
                    int.Parse(row[4].ToString()),
                    int.Parse(row[5].ToString()),
					int.Parse(row[0].ToString())
				));

			choices = new Dictionary<int, Choice>();
            foreach (object[] row in GetData("SELECT * FROM choices;"))
				choices.Add(int.Parse(row[0].ToString()),
					new Choice(int.Parse(row[0].ToString()),
					string.IsNullOrEmpty(row[1]?.ToString()) ? null : int.Parse(row[1].ToString()) as int?,
					string.IsNullOrEmpty(row[2]?.ToString()) ? null : int.Parse(row[2].ToString()) as int?,
					string.IsNullOrEmpty(row[3]?.ToString()) ? null : int.Parse(row[3].ToString()) as int?
				));
        }

		/// <summary>
		/// Aktualisiert die Datenbank durch Ergänzen der Datensätze aus der Datenverwaltung.
		/// </summary>
		/// <param name="students">Die neuen Schülerdaten</param>
		/// <param name="courses">Die neuen Kurse</param>
		/// <param name="choices">Die neuen Wahlen</param>
		public void UpdateDatabase(ref List<Student> students, ref List<Course> courses, ref List<Choice> choices)
        {
            connection.Open();

			new MySqlCommand("DELETE FROM courses;", connection).ExecuteNonQuery();
			foreach (Course course in courses)
				new MySqlCommand(string.Format("REPLACE INTO courses SET {0}`name`='{1}', `minYear`={2}, `maxYear`={3}, `minStudents`={4}, `maxStudents`={5};", course.Id == -1 ? "" : $"`id`={course.Id}, ", course.Name, course.MinYear, course.MaxYear, course.MinStudents, course.MaxStudents), connection).ExecuteNonQuery();

			new MySqlCommand("DELETE FROM students;", connection).ExecuteNonQuery();
			foreach (Student student in students)
				new MySqlCommand(string.Format("REPLACE INTO students SET {0}`name`='{1}', `dateOfBirth`='{2}', `form`='{3}';", student.Id == -1 ? "" : $"`id`={student.Id}, ", student.Name, student.DateOfBirth.ToString("yyyy-MM-dd"), student.Form), connection).ExecuteNonQuery();

			new MySqlCommand("DELETE FROM choices;", connection).ExecuteNonQuery();
			foreach (Choice choice in choices)
				new MySqlCommand(string.Format("REPLACE INTO choices SET `studentId`={0}, `courseId1`={1}, `courseId2`={2}, `courseId3`={3};", choice.StudentId, choice.CourseId1.HasValue ? choice.CourseId1.Value.ToString() : "NULL", choice.CourseId2.HasValue ? choice.CourseId2.Value.ToString() : "NULL", choice.CourseId3.HasValue ? choice.CourseId3.Value.ToString() : "NULL"), connection).ExecuteNonQuery();

            connection.Close();
        }

		/// <summary>
		/// Gibt für einen Kurs und die Wahlrangfolge die Anzahl der Wahlen an.
		/// </summary>
		/// <param name="courseId"></param>
		/// <param name="priority"></param>
		/// <returns></returns>
        public int GetChoiceNumber(int courseId, int priority)
        {
			return int.Parse(GetData($"SELECT COUNT(*) FROM choices WHERE `courseId{priority + 1}`={courseId};")[0][0].ToString());
        }
    }
}