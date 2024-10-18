using System;
using System.Collections.Generic;

namespace UniversityZusha.users
{
    /// <summary>
    /// מחלקה המייצגת ראש מחלקה, יורשת ממחלקת User.
    /// </summary>
    public class DepartmentHead : User
    {
        public string EmployeeNumber { get; set; } // מספר עובד
        public List<Track> ResponsibleTracks { get; set; } // רשימת מסלולים שאחראי עליהם
        public List<Lecturer> Teachers { get; set; } // רשימת מרצים ומתרגלים
        public List<Student> Students { get; set; } // רשימת סטודנטים

        // בנאי של ראש מחלקה
        public DepartmentHead(string id, string name, string email, string phoneNumber, DateTime dateOfBirth,
                              DateTime enrollmentDate, Role role, byte[] image, string employeeNumber,
                              List<Track> responsibleTracks, List<Lecturer> teachers, List<Student> students)
            : base(id, name, email, phoneNumber, dateOfBirth, enrollmentDate, role, image)
        {
            EmployeeNumber = employeeNumber;
            ResponsibleTracks = responsibleTracks;
            Teachers = teachers;
            Students = students;
        }

        /// <summary>
        /// מוסיף מסלול חדש שאחראי עליו.
        /// </summary>
        /// <param name="track">המסלול להוספה</param>
        public void AddTrack(Track track)
        {
            if (!ResponsibleTracks.Contains(track))
            {
                ResponsibleTracks.Add(track);
            }
        }

        /// <summary>
        /// מוסיף מרצה חדש לרשימת המרצים.
        /// </summary>
        /// <param name="lecturer">המרצה להוספה</param>
        public void AddTeacher(Lecturer lecturer)
        {
            if (!Teachers.Contains(lecturer))
            {
                Teachers.Add(lecturer);
            }
        }

        /// <summary>
        /// מוסיף סטודנט חדש לרשימת הסטודנטים.
        /// </summary>
        /// <param name="student">הסטודנט להוספה</param>
        public void AddStudent(Student student)
        {
            if (!Students.Contains(student))
            {
                Students.Add(student);
            }
        }
    }
}