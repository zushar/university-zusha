using System.Collections.Generic;
using UniversityZusha.courses;

public class Track
{
    public string TrackName { get; set; } // שם המסלול
    public List<Course> Courses { get; set; } // רשימת הקורסים במסלול

    // בנאי
    public Track(string trackName, List<Course> courses)
    {
        TrackName = trackName;
        Courses = courses;
    }

    /// <summary>
    /// מחזיר את סך נקודות הזכות הנדרשות לסיום המסלול (נק"ז כולל)
    /// </summary>
    /// <returns>נק"ז כולל</returns>
    public double GetTotalCreditsRequired()
    {
        double totalCredits = 0;
        foreach (var course in Courses)
        {
            totalCredits += course.Credits;
        }
        return totalCredits;
    }
}