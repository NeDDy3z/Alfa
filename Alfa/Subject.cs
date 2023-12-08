using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Alfa
{
    public class Subject
    {
        private string subjectName;
        private string classroom;
        private string teacher;
        private bool theory;

        public Subject(string subjectName, string classroom, string teacher, bool theory)
        {
            this.subjectName = subjectName;
            this.classroom = classroom;
            this.teacher = teacher;
            this.theory = theory;
        }

        public string SubjectName { get { return this.subjectName; } }
        public string Classroom { get { return this.classroom; } }
        public string Teacher { get { return this.teacher; } }
        public bool Theory { get { return this.theory; } }
        
        public static void LoadFromFile(List<Subject> list, string filePath)
        {
            string json = File.ReadAllText(filePath);
            List<Dictionary<string, object>> dictionaries =
                JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
            foreach (var dictionary in dictionaries)
            {
                list.Add(new Subject(
                    Convert.ToString(dictionary["subject"]), 
                    Convert.ToString(dictionary["classroom"]),
                    Convert.ToString(dictionary["teacher"]), 
                    Convert.ToBoolean(dictionary["theory"])));
            }
        }
    }
}