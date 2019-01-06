using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Aiakos
{
    public class Calculator
    {
        public static void Assign(out List<S> sOutput, out List<C> cOutput)
        {
            C.CDict = new Dictionary<int, C>();
            cOutput = new List<C>(Array.ConvertAll(new List<int>(MainForm.Courses.Keys).ToArray(), c => new C(c)));
            S.SDict = new Dictionary<int, S>();
            List<S> unassignedStudents = new List<S>(Array.ConvertAll(new List<int>(MainForm.Students.Keys).ToArray(), s => new S(s)));
            sOutput = new List<S>(unassignedStudents);

            for (int priority = 0; unassignedStudents.Count > 0 && priority < 3; priority++)
            {
                List<int> assignedStudents = new List<int>();

                for (int i = 0; i < unassignedStudents.Count; i++)
                {
                    S student = unassignedStudents[i];
                    if (student.Preferences.Count <= priority)
                        continue;

                    C course = C.CDict[student.Preferences[priority]];
                    if (course.AssignedStudents.Count < course.Course.MaxStudents)
                    {
                        student.AssignedCourse = course.Course.Id;
                        student.Satisfaction = -priority;
                        course.AssignedStudents.Add(student.Student.Id);
                        unassignedStudents.RemoveAt(i--);
                        assignedStudents.Add(student.Student.Id);
                    }
                }

                for (int i = 0; i < unassignedStudents.Count; i++)
                {
                    S loser = unassignedStudents[i];
                    if (loser.Preferences.Count <= priority + 1)
                        continue;

                    int course = loser.Preferences[priority + 1];
                    for (int j = 0; j < assignedStudents.Count; j++)
                    {
                        S winner = S.SDict[assignedStudents[j]];

                        if (winner.AssignedCourse == course)
                        {
                            loser.AssignedCourse = course;
                            winner.AssignedCourse = null;
                            loser.Satisfaction = -priority - 1;
                            winner.Satisfaction = -10;
                            C.CDict[course].AssignedStudents.Add(loser.Student.Id);
                            C.CDict[course].AssignedStudents.Remove(winner.Student.Id);
                            unassignedStudents.RemoveAt(i);
                            unassignedStudents.Add(winner);
                            assignedStudents.RemoveAt(j);
                            break;
                        }
                    }
                }
            }

            if (unassignedStudents.Count == 0)
                return;

            Dictionary<int, List<int>> freeCourses = new Dictionary<int, List<int>>();
            foreach (C course in cOutput)
                if (course.AssignedStudents.Count < course.Course.MaxStudents)
                    for (int year = course.Course.MinYear; year < course.Course.MaxYear; year++)
                    {
                        if (!freeCourses.ContainsKey(year))
                            freeCourses.Add(year, new List<int>());
                        freeCourses[year].Add(course.Course.Id);
                    }

            int seed = 0;
            while (unassignedStudents.Count > 0 && Array.TrueForAll(new List<List<int>>(freeCourses.Values).ToArray(), l => l.Count > 0))
            {
                S student = unassignedStudents[0];
                int year = 0;
                foreach (char c in student.Student.Form)
                    if (char.IsDigit(c))
                        year = 10 * year + int.Parse(c + "");
                    else
                        break;

                if (!freeCourses.ContainsKey(year))
                {
                    unassignedStudents.RemoveAt(0);
                    continue;
                }

                Random random = new Random(++seed);
                C course = C.CDict[freeCourses[year][random.Next(freeCourses[year].Count)]];
                student.AssignedCourse = course.Course.Id;
                student.Satisfaction = -5;
                course.AssignedStudents.Add(student.Student.Id);
                unassignedStudents.RemoveAt(0);

                if (course.AssignedStudents.Count >= course.Course.MaxStudents)
                    foreach (List<int> l in freeCourses.Values)
                        l.Remove(course.Course.Id);
            }
        }

        public static void Print()
        {
            ResultDisplay rd = new ResultDisplay();
            Assign(out List<S> students, out List<C> courses);

            rd.tbResult.AppendText("Kurslisten:" + Environment.NewLine + Environment.NewLine);
            foreach (C course in courses)
                rd.tbResult.Text += string.Format("{0} ({1} Teilnehmer): {2}" + Environment.NewLine, course.Course, course.AssignedStudents.Count, string.Join<Student>(", ", Array.ConvertAll(course.AssignedStudents.ToArray(), s => S.SDict[s].Student)));

            rd.tbResult.AppendText(Environment.NewLine + Environment.NewLine + "Schülerzuordnungen:" + Environment.NewLine + Environment.NewLine);
            double satisfaction = 0.0;
            foreach (S student in students)
            {
                satisfaction += student.Satisfaction;
                rd.tbResult.AppendText(string.Format("{0}: {1}\t| Zufriedenheit: {2}" + Environment.NewLine, student.Student, student.AssignedCourse.HasValue ? C.CDict[student.AssignedCourse.Value].Course.ToString() : DataAdministration.DefaultValue, student.Satisfaction));
            }
            
            rd.tbResult.AppendText(Environment.NewLine + Environment.NewLine + "durchschnittliche Zufriedenheit: " + satisfaction / students.Count);
            rd.ShowDialog();
        }
    }

    public class S
    {
        public static Dictionary<int, S> SDict;

        public S(int id)
        {
            Student = MainForm.Students[id];
            AssignedCourse = null;
            Satisfaction = -10;

            Preferences = new List<int>();
            if (MainForm.Choices.ContainsKey(id))
                foreach (int? i in MainForm.Choices[id].Courses)
                    if (i.HasValue)
                        Preferences.Add(i.Value);

            SDict.Add(id, this);
        }

        public Student Student;
        public int? AssignedCourse;
        public List<int> Preferences;
        public int Satisfaction;
    }

    public class C
    {
        public static Dictionary<int, C> CDict;

        public C(int id)
        {
            Course = MainForm.Courses[id];
            AssignedStudents = new List<int>();
            CDict.Add(id, this);
        }

        public Course Course;
        public List<int> AssignedStudents;
    }
}