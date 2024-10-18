using System;
using System.Collections.Generic;
using UniversityZusha.courses;

namespace UniversityZusha.users
{
    public class Student : User
    {
        public string StudentNumber { get; set; } // מספר סטודנט
        public string TrackInRold { get; set; } // מסלול הלימודים (כולל הקורסים)
        public List<string> CurrentSemesterCourses { get; set; } // רשימת הקורסים בסמסטר הנוכחי
        public string Specialization { get; set; } // התמחות (אם קיימת)
        public double CurrentCredits { get; private set; } // נק"ז נוכחי
        public double TotalCredits { get; set; } // נק"ז כולל
        public bool IsAssistant { get; set; } // האם מתרגל
        private List<Course> CoursesToAssist { get; set; } // רשימת קורסים לתרגול

        // בנאי של סטודנט
        public Student(string id, string name, string email, string phoneNumber, DateTime dateOfBirth,
                       DateTime enrollmentDate, Role role, byte[] image, string studentNumber,
                       Track track, List<string> currentSemesterCourses, string specialization,
                       double currentCredits)
            : base(id, name, email, phoneNumber, dateOfBirth, enrollmentDate, role, image)
        {
            StudentNumber = studentNumber;
            TrackInRold = track.TrackName;
            CurrentSemesterCourses = currentSemesterCourses;
            Specialization = specialization;
            CurrentCredits = currentCredits;
            TotalCredits = track.GetTotalCreditsRequired();
            IsAssistant = false;
            CoursesToAssist = new List<Course>();
        }

        /// <summary>
        /// מחזיר את אחוז ההתקדמות של הסטודנט במסלול.
        /// </summary>
        /// <returns>אחוז ההתקדמות</returns>
        public double GetProgressPercentage()
        {
            return (CurrentCredits / TotalCredits) * 100;
        }

        /// <summary>
        /// מוסיף נק"ז לנק"ז הנוכחי של הסטודנט.
        /// </summary>
        /// <param name="creditsToAdd">מספר נק"ז להוספה</param>
        public void AddCredits(double creditsToAdd)
        {
            if (creditsToAdd < 0)
            {
                throw new ArgumentException("Cannot add negative credits.");
            }
            CurrentCredits += creditsToAdd;
        }

        /// <summary>
        /// מוסיף קורס לרשימת הקורסים שהסטודנט מתרגל בהם.
        /// </summary>
        /// <param name="course">קורס לתרגול</param>
        public void AddCoursesToAssist(Course course)
        {
            if (!CoursesToAssist.Contains(course))
            {
                CoursesToAssist.Add(course);
            }
        }

        /// <summary>
        /// מחזיר את רשימת הקורסים שהסטודנט מתרגל בהם.
        /// </summary>
        /// <returns>רשימת הקורסים</returns>
        public List<Course> GetCoursesToAssist()
        {
            return CoursesToAssist;
        }
    }
}