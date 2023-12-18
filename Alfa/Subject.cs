using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using Newtonsoft.Json;

namespace Alfa
{
    public class Subject
    {
        private string subjectName;
        private string classroom;
        private int floor;
        private string teacher;
        private bool theory;

        public Subject(string subjectName, string classroom, int floor, string teacher, bool theory)
        {
            this.subjectName = subjectName;
            this.classroom = classroom;
            this.floor = floor;
            this.teacher = teacher;
            this.theory = theory;
        }
        
        public Subject()
        {
            this.subjectName = "";
            this.classroom = "";
            this.floor = 0;
            this.teacher = "";
            this.theory = false;
        }

        public string SubjectName { get { return this.subjectName; } }
        public string Classroom { get { return this.classroom; } }
        public int Floor { get { return this.floor; } }
        public string Teacher { get { return this.teacher; } }
        public bool Theory { get { return this.theory; } }
        
        public static List<Subject> LoadFromFile(string filePath)
        {
            string json = File.ReadAllText(filePath);
            List<Dictionary<string, object>> dictionaries = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
            List<Subject> list = new List<Subject>();
            
            foreach (var dictionary in dictionaries)
            {
                list.Add(new Subject(
                        Convert.ToString(dictionary["subject"]), 
                        Convert.ToString(dictionary["classroom"]),
                        Convert.ToInt32(dictionary["floor"]),
                        Convert.ToString(dictionary["teacher"]), 
                        Convert.ToBoolean(dictionary["theory"])));   
            }
            return list;
        }
    }
}