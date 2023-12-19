using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Alfa
{
    public class Subject
    {
        private string _subjectName;
        private string _classroom;
        private int _floor;
        private string _teacher;
        private bool _theory;

        public Subject(string subjectName, string classroom, int floor, string teacher, bool theory)
        {
            this._subjectName = subjectName;
            this._classroom = classroom;
            this._floor = floor;
            this._teacher = teacher;
            this._theory = theory;
        }
        
        public Subject()
        {
            this._subjectName = "";
            this._classroom = "";
            this._floor = 0;
            this._teacher = "";
            this._theory = false;
        }

        public string SubjectName { get { return this._subjectName; } }
        public string Classroom { get { return this._classroom; } }
        public int Floor { get { return this._floor; } }
        public string Teacher { get { return this._teacher; } }
        public bool Theory { get { return this._theory; } }
        
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