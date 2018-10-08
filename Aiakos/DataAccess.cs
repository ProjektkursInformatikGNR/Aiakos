﻿using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace Aiakos
{
    public class DataAccess
    {
        private MySqlConnection connection;

        public DataAccess(Server server)
        {
            connection = new MySqlConnection(string.Format("server={0};uid={1};pwd={2};database={3};port={4};", server.Host, server.UserId, server.Password, server.Database, server.Port));
        }
		
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
            {
				choices.Add(int.Parse(row[0].ToString()),
					new Choice(int.Parse(row[0].ToString()),
					int.Parse(row[1].ToString()),
					int.Parse(row[2].ToString()),
					int.Parse(row[3].ToString())
				));
			}
        }

        public void UpdateDatabase(ref List<Course> courses, ref List<Student> students, ref List<Choice> choices)
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
				new MySqlCommand(string.Format("REPLACE INTO choices SET `studentId`={0}, `courseId1`={1}, `courseId2`={2}, `courseId3`={3};", choice.StudentId, choice.CourseId1, choice.CourseId2, choice.CourseId3), connection).ExecuteNonQuery();

            connection.Close();
        }

        public int GetChoiceNumber(int courseId, int priority)
        {
            int count = 0;

            foreach (Choice choice in Form1.Choices.Values)
                if (choice.Courses[priority] == courseId)
                    count++;

            return count;
        }

        public static bool ConnectionValid(Server server)
        {
            try
            {
                MySqlConnection con = new MySqlConnection(string.Format("server={0};uid={1};pwd={2};database={3};port={4};", server.Host, server.UserId, server.Password, server.Database, server.Port));
				con.Open();
                con.CreateCommand();
                con.Close();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}