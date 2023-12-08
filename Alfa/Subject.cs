using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Alfa
{
    public class Subject
    {
        private string subject;
        private string classroom;
        private string teacher;
        private string theory;

        public Subject(string subject, string classroom, string teacher, bool theory)
        {
            this.subject = subject;
            this.classroom = classroom;
            this.teacher = teacher;
            this.theory = this.theory;
        }

        public override string ToString()
        {
            return this.subject;
        }

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