
namespace UniversityZusha.courses
{
    public class Course
    {
        public int CourseID { get; set; } // מזהה ייחודי לקורס
        public string CourseName { get; set; } // שם הקורס
        public double Credits { get; set; } // נקודות זכות עבור הקורס

        // בנאי
        public Course(int courseID, string courseName, double credits)
        {
            CourseID = courseID;
            CourseName = courseName;
            Credits = credits;
        }
    }
}
