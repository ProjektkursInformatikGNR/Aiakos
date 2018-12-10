using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiakos
{
    public interface IDataAccess
    {
        object[][] GetData(string cmd);
        void FillData(out Dictionary<int, Student> students, out Dictionary<int, Course> courses, out Dictionary<int, Choice> choices);
        void UpdateDatabase(ref List<Student> students, ref List<Course> coures, ref List<Choice> choices);
        int GetChoiceNumber(int courseId, int priority);
    }
}
