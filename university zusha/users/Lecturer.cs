using System;
using System.Collections.Generic;
using UniversityZusha.courses;

namespace UniversityZusha.users
{
    /// <summary>
    /// מחלקה המייצגת מרצה, יורשת ממחלקת User.
    /// </summary>
    public class Lecturer : User
    {
        public string EmployeeNumber { get; set; } // מספר עובד
        public List<Course> CoursesTaught { get; set; } // רשימת קורסים שהמרצה מלמד
        public string Specialization { get; set; } // התמחות
        public double Stars { get; set; } // מספר כוכבים מסטודנטים

        // בנאי של מרצה
        public Lecturer(string id, string name, string email, string phoneNumber, DateTime dateOfBirth,
                        DateTime enrollmentDate, Role role, byte[] image, string employeeNumber,
                        List<Course> coursesTaught, string specialization, double stars)
            : base(id, name, email, phoneNumber, dateOfBirth, enrollmentDate, role, image)
        {
            EmployeeNumber = employeeNumber;
            CoursesTaught = coursesTaught;
            Specialization = specialization;
            Stars = stars;
        }

        /// <summary>
        /// מוסיף ציון חדש לדירוג המרצה ומעדכן את ממוצע הכוכבים.
        /// </summary>
        /// <param name="newRating">הדירוג החדש מהסטודנטים</param>
        public void AddRating(double newRating)
        {
            // ניתן להוסיף לוגיקה לעדכון ממוצע הכוכבים על בסיס דירוגים
            Stars = Stars + newRating;
        }
    }
}