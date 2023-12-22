using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Alfa
{
    /// <summary>
    /// Represents a subject with details such as name, classroom, floor, teacher, and theory status.
    /// </summary>
    public class Subject
    {
        private string _subjectName;
        private string _classroom;
        private int _floor;
        private string _teacher;
        private bool _theory;

        /// <summary>
        /// Initializes a new instance of the <see cref="Subject"/> class with specified parameters.
        /// </summary>
        /// <param name="subjectName">The name of the subject.</param>
        /// <param name="classroom">The classroom assigned to the subject.</param>
        /// <param name="floor">The floor where the subject is scheduled.</param>
        /// <param name="teacher">The teacher assigned to the subject.</param>
        /// <param name="theory">Indicates whether the subject is theoretical or practical.</param>
        public Subject(string subjectName, string classroom, int floor, string teacher, bool theory)
        {
            this._subjectName = subjectName;
            this._classroom = classroom;
            this._floor = floor;
            this._teacher = teacher;
            this._theory = theory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Subject"/> class with default values.
        /// </summary>
        public Subject()
        {
            this._subjectName = "";
            this._classroom = "";
            this._floor = 0;
            this._teacher = "";
            this._theory = false;
        }

        /// <summary>
        /// Gets the name of the subject.
        /// </summary>
        public string SubjectName => this._subjectName;

        /// <summary>
        /// Gets the assigned classroom for the subject.
        /// </summary>
        public string Classroom => this._classroom;

        /// <summary>
        /// Gets the floor where the subject is scheduled.
        /// </summary>
        public int Floor => this._floor;

        /// <summary>
        /// Gets the teacher assigned to the subject.
        /// </summary>
        public string Teacher => this._teacher;

        /// <summary>
        /// Gets a value indicating whether the subject is theoretical.
        /// </summary>
        public bool Theory => this._theory;

        /// <summary>
        /// Loads a list of subjects from a JSON file.
        /// </summary>
        /// <param name="filePath">The path to the JSON file containing subject data.</param>
        /// <returns>A list of subjects loaded from the JSON file.</returns>
        public static List<Subject> LoadFromFile(string filePath)
        {
            string json = File.ReadAllText(filePath);
            List<Dictionary<string, object>> dictionaries =
                JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
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