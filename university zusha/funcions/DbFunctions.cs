using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;

namespace UniversityZusha.dbFunctions
{
    public class DbFunctions
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["SchoolDbConnection"].ConnectionString;

        // פונקציה לשליפת ה-DepartmentID של ראש המחלקה
        public static int GetDepartmentID(int departmentHeadID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT ManagedDepartmentID FROM DepartmentHeads WHERE AuthID = @AuthID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@AuthID", departmentHeadID);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        throw new Exception("לא נמצא DepartmentID עבור ראש המחלקה הנוכחי.");
                    }
                }
            }
        }

        // פונקציה לשליפת מסלולים לפי DepartmentID
        public static DataTable GetTracks(int departmentID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = "SELECT TrackID, TrackName, TotalCredits FROM Tracks WHERE DepartmentID = @DepartmentID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentID);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    return dt;
                }
            }
        }

        // פונקציה לשליפת מסלול לפי TrackID
        public static DataTable GetTrackByID(int trackID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = "SELECT TrackID, TrackName, TotalCredits FROM Tracks WHERE TrackID = @TrackID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@TrackID", trackID);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    return dt;
                }
            }
        }


        public static int GetTrackTotalCredits(int TrackID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT TotalCredits FROM Tracks WHERE TrackID = @TrackID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@TrackID", TrackID);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        throw new Exception("לא נמצאו נקודות זיכוי עבור המסלול הנוכחי.");
                    }
                }
            }
        }

        public static void UpdateStudentTotalCredits(int authID, int trackID)
        {
            try
            {
                // Get the total credits for the track
                int totalCredits = GetTrackTotalCredits(trackID);

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Update the student's TotalCredits based on the track
                    string query = @"
                UPDATE Students SET TotalCredits = @TotalCredits WHERE AuthID = @AuthID;
            ";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@AuthID", authID);
                        cmd.Parameters.AddWithValue("@TotalCredits", totalCredits);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("הנקודות זיכוי של הסטודנט עודכנו בהצלחה.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה במהלך עדכון הנקודות זיכוי: " + ex.Message);
            }
        }


        public static String GetTrackNameByID(int trackID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = "SELECT TrackID, TrackName, TotalCredits FROM Tracks WHERE TrackID = @TrackID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@TrackID", trackID);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt.Columns["TrackName"].ToString();
                }
            }
        }

        // פונקציה לשליפת קורס לפי CourseID
        public static DataTable GetCourseByID(int courseID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = "SELECT CourseID, CourseName, Credits FROM Courses WHERE CourseID = @CourseID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CourseID", courseID);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    return dt;
                }
            }
        }

        // פונקציה להוספת מסלול חדש
        public static void InsertTrack(string trackName, int departmentID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "INSERT INTO Tracks (TrackName, DepartmentID, TotalCredits) VALUES (@TrackName, @DepartmentID, @TotalCredits)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@TrackName", trackName);
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
                    cmd.Parameters.AddWithValue("@TotalCredits", 0);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // פונקציה לעדכון מסלול קיים
        public static void UpdateTrack(int trackID, string trackName)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "UPDATE Tracks SET TrackName = @TrackNames WHERE TrackID = @TrackID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@TrackName", trackName);
                    cmd.Parameters.AddWithValue("@TrackID", trackID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // פונקציה למחיקת מסלול
        public static void DeleteTrack(int trackID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                // מחיקת קישורים בטבלת TrackCourses
                string deleteTrackCoursesQuery = "DELETE FROM TrackCourses WHERE TrackID = @TrackID";
                using (SqlCommand cmd1 = new SqlCommand(deleteTrackCoursesQuery, con))
                {
                    cmd1.Parameters.AddWithValue("@TrackID", trackID);
                    cmd1.ExecuteNonQuery();
                }

                // מחיקת המסלול עצמו
                string deleteTrackQuery = "DELETE FROM Tracks WHERE TrackID = @TrackID";
                using (SqlCommand cmd2 = new SqlCommand(deleteTrackQuery, con))
                {
                    cmd2.Parameters.AddWithValue("@TrackID", trackID);
                    cmd2.ExecuteNonQuery();
                }
            }
        }

        // פונקציה להוספת קורס חדש
        public static int InsertCourse(string courseName, int credits)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "INSERT INTO Courses (CourseName, Credits) VALUES (@CourseName, @Credits); SELECT SCOPE_IDENTITY();";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CourseName", courseName);
                    cmd.Parameters.AddWithValue("@Credits", credits);
                    int newCourseID = Convert.ToInt32(cmd.ExecuteScalar());
                    return newCourseID;
                }
            }
        }

        // פונקציה להוספת קישור בין קורס למסלול
        public static void InsertTrackCourse(int trackID, int courseID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string insertTrackCourseQuery = "INSERT INTO TrackCourses (TrackID, CourseID) VALUES (@TrackID, @CourseID)";
                using (SqlCommand cmd = new SqlCommand(insertTrackCourseQuery, con))
                {
                    cmd.Parameters.AddWithValue("@TrackID", trackID);
                    cmd.Parameters.AddWithValue("@CourseID", courseID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // פונקציה לעדכון קורס קיים
        public static void UpdateCourse(int courseID, string courseName, int credits)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "UPDATE Courses SET CourseName = @CourseName, Credits = @Credits WHERE CourseID = @CourseID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CourseName", courseName);
                    cmd.Parameters.AddWithValue("@Credits", credits);
                    cmd.Parameters.AddWithValue("@CourseID", courseID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // פונקציה לעדכן נקודות זיכוי של מסלול בהסתמך על הנקודות זיכוי של כול הקורסים המשויכים למסלול
        public static void UpdateTrackTotalCredits(int trackID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = @"
                    UPDATE t
                    SET t.TotalCredits = ISNULL((
                        SELECT SUM(Credits)
                        FROM Courses c
                        INNER JOIN TrackCourses tc ON c.CourseID = tc.CourseID
                        WHERE tc.TrackID = @TrackID
                    ), 0)
                    FROM Tracks t
                    WHERE t.TrackID = @TrackID
                ";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@TrackID", trackID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateStudentsTotalCreditsByTrack(int trackID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    // Query to update TotalCredits for all students in the specified track
                    string query = @"
                UPDATE s
                SET s.TotalCredits = t.TotalCredits
                FROM Students s
                INNER JOIN Tracks t ON s.TrackID = t.TrackID
                WHERE s.TrackID = @TrackID;
            ";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@TrackID", trackID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה במהלך עדכון הנקודות זיכוי של הסטודנטים: " + ex.Message);
            }
        }

        // פונקציה למחיקת קורס
        public static void DeleteCourse(int courseID, int trackID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                // מחיקת קישורים בטבלת TrackCourses
                string deleteTrackCoursesQuery = "DELETE FROM TrackCourses WHERE CourseID = @CourseID AND TrackID = @TrackID";
                using (SqlCommand cmd1 = new SqlCommand(deleteTrackCoursesQuery, con))
                {
                    cmd1.Parameters.AddWithValue("@CourseID", courseID);
                    cmd1.Parameters.AddWithValue("@TrackID", trackID);
                    cmd1.ExecuteNonQuery();
                }

                // בדיקה אם הקורס משויך למסלולים אחרים
                string checkCourseInOtherTracks = "SELECT COUNT(*) FROM TrackCourses WHERE CourseID = @CourseID";
                using (SqlCommand cmd2 = new SqlCommand(checkCourseInOtherTracks, con))
                {
                    cmd2.Parameters.AddWithValue("@CourseID", courseID);
                    int count = Convert.ToInt32(cmd2.ExecuteScalar());
                    if (count == 0)
                    {
                        // הקורס אינו משויך למסלולים אחרים - ניתן למחוק
                        string deleteCourseQuery = "DELETE FROM Courses WHERE CourseID = @CourseID";
                        using (SqlCommand cmd3 = new SqlCommand(deleteCourseQuery, con))
                        {
                            cmd3.Parameters.AddWithValue("@CourseID", courseID);
                            cmd3.ExecuteNonQuery();
                        }
                    }
                }
                UpdateTrackTotalCredits(trackID);
            }
        }

        // פונקציה לעדכון סטטוס של מרצה
        public static void UpdateLecturerStatus(int lecturerID, string newStatus)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = @"
                    UPDATE a
                    SET a.RegistrationStatus = @Status
                    FROM Auth a
                    INNER JOIN Lecturers l ON a.AuthID = l.AuthID
                    WHERE l.LecturerID = @LecturerID
                ";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@LecturerID", lecturerID);
                    cmd.Parameters.AddWithValue("@Status", newStatus);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // פונקציה לשליפת מרצים (ממתינים או מאושרים) לפי DepartmentID
        public static DataTable GetLecturers(int departmentID, string registrationStatus)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = @"
                    SELECT 
                        l.AuthID,
                        l.LecturerID,
                        l.Name,
                        l.Email,
                        l.PhoneNumber,
                        a.RegistrationStatus,
                        STUFF((
                            SELECT ', ' + c.CourseName
                            FROM LecturersCourses lc
                            INNER JOIN Courses c ON lc.CourseID = c.CourseID
                            INNER JOIN TrackCourses tc ON c.CourseID = tc.CourseID
                            INNER JOIN Tracks t ON tc.TrackID = t.TrackID
                            WHERE lc.LecturerID = l.LecturerID AND t.DepartmentID = @DepartmentID
                            FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS Courses
                    FROM Lecturers l
                    INNER JOIN Auth a ON l.AuthID = a.AuthID
                    WHERE a.Role = 'Lecturer' AND a.RegistrationStatus = @RegistrationStatus
                    GROUP BY l.AuthID, l.LecturerID, l.Name, l.Email, l.PhoneNumber, a.RegistrationStatus
                ";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
                    cmd.Parameters.AddWithValue("@RegistrationStatus", registrationStatus);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    return dt;
                }
            }
        }

        // פונקציה לשליפת קורסים לשיוך למרצה
        public static DataTable GetCourses(int trackID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = @"
            SELECT 
                c.CourseID, 
                c.CourseName, 
                c.Credits,
                STUFF((
                    SELECT ', ' + l.Name
                    FROM LecturersCourses lc
                    INNER JOIN Lecturers l ON lc.LecturerID = l.LecturerID
                    WHERE lc.CourseID = c.CourseID
                    FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS Lecturers
            FROM Courses c
            INNER JOIN TrackCourses tc ON c.CourseID = tc.CourseID
            WHERE tc.TrackID = @TrackID
        ";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@TrackID", trackID);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    return dt;
                }
            }
        }


        // פונקציה לשמירת שיוך קורסים למרצה
        public static void SaveLecturerCourseAssignments(int lecturerID, int departmentID, DataTable assignedCourses)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    // הסרת שיוכים קיימים במחלקה זו
                    string deleteQuery = @"
                        DELETE lc
                        FROM LecturersCourses lc
                        INNER JOIN Courses c ON lc.CourseID = c.CourseID
                        INNER JOIN TrackCourses tc ON c.CourseID = tc.CourseID
                        INNER JOIN Tracks t ON tc.TrackID = t.TrackID
                        WHERE lc.LecturerID = @LecturerID AND t.DepartmentID = @DepartmentID
                    ";
                    using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, con, transaction))
                    {
                        deleteCmd.Parameters.AddWithValue("@LecturerID", lecturerID);
                        deleteCmd.Parameters.AddWithValue("@DepartmentID", departmentID);
                        deleteCmd.ExecuteNonQuery();
                    }

                    // הוספת שיוכים חדשים
                    foreach (DataRow row in assignedCourses.Rows)
                    {
                        bool isAssigned = Convert.ToBoolean(row["IsAssigned"]);
                        if (isAssigned)
                        {
                            int courseID = Convert.ToInt32(row["CourseID"]);
                            string insertQuery = "INSERT INTO LecturersCourses (LecturerID, CourseID) VALUES (@LecturerID, @CourseID)";
                            using (SqlCommand insertCmd = new SqlCommand(insertQuery, con, transaction))
                            {
                                insertCmd.Parameters.AddWithValue("@LecturerID", lecturerID);
                                insertCmd.Parameters.AddWithValue("@CourseID", courseID);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public static void SetPendingLecturer(int authID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "UPDATE Auth SET RegistrationStatus = 'Pending' WHERE AuthID = @AuthID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@AuthID", authID);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("הסטטוס שונה ל-'Pending'.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה: " + ex.Message);
            }
        }

        public static DataTable GetAllTracks()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT TrackID, TrackName FROM Tracks";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public static void SetPendingStudent(int authID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "UPDATE Auth SET RegistrationStatus = 'Pending' WHERE AuthID = @AuthID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@AuthID", authID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void RejectStudent(int authID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "UPDATE Auth SET RegistrationStatus = 'Rejected' WHERE AuthID = @AuthID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@AuthID", authID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static int GetStudentTrackID(int studentID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT TrackID FROM Students WHERE StudentID = @StudentID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentID);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        throw new Exception("לא נמצא TrackID עבור הסטודנט הנוכחי.");
                    }
                }
            }
        }

        public static DataTable SearchUsers(string searchTerm, string searchType, string currentUserRole, int currentUserID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = @"
                SELECT 
                    a.AuthID, 
                    a.UserName,
                    a.Role,
                    CASE 
                        WHEN a.Role = 'Lecturer' THEN l.Name
                        WHEN a.Role = 'Student' THEN s.Name
                        WHEN a.Role = 'DepartmentHead' THEN dh.Name
                        ELSE a.UserName
                    END AS FullName,
                    CASE 
                        WHEN a.Role = 'Lecturer' THEN l.Email
                        WHEN a.Role = 'Student' THEN s.Email
                        WHEN a.Role = 'DepartmentHead' THEN dh.Email
                        ELSE NULL
                    END AS Email,
                    CASE
                        WHEN a.Role = 'Lecturer' THEN l.LecturerID
                        WHEN a.Role = 'Student' THEN s.StudentID
                        ELSE NULL
                    END AS UserNumber
                FROM Auth a
                LEFT JOIN Lecturers l ON a.AuthID = l.AuthID
                LEFT JOIN Students s ON a.AuthID = s.AuthID
                LEFT JOIN DepartmentHeads dh ON a.AuthID = dh.AuthID
                WHERE (a.UserName LIKE @SearchTerm 
                    OR l.Name LIKE @SearchTerm 
                    OR s.Name LIKE @SearchTerm 
                    OR dh.Name LIKE @SearchTerm
                    OR CAST(l.LecturerID AS NVARCHAR) = @SearchTerm
                    OR CAST(s.StudentID AS NVARCHAR) = @SearchTerm)
                    AND (@SearchType = 'All' OR a.Role = @SearchType)
                    AND ((@CurrentUserRole = 'DepartmentHead') OR (a.AuthID != @CurrentUserID))
            ";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                    cmd.Parameters.AddWithValue("@SearchType", searchType);
                    cmd.Parameters.AddWithValue("@CurrentUserRole", currentUserRole);
                    cmd.Parameters.AddWithValue("@CurrentUserID", currentUserID);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }
        /// <summary>
        /// מחזיר את רשימת הקורסים של מרצה מסוים.
        /// </summary>
        /// <param name="lecturerId">מזהה המרצה</param>
        /// <returns>DataTable עם פרטי הקורסים</returns>
        public static DataTable GetLecturerCourses(int lecturerId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = @"
                SELECT c.CourseName, c.Credits
                FROM Courses c
                INNER JOIN LecturersCourses lc ON c.CourseID = lc.CourseID
                WHERE lc.LecturerID = @LecturerID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@LecturerID", lecturerId);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        /// <summary>
        /// מחזיר את רשימת הסטודנטים הרשומים לקורסים של מרצה מסוים.
        /// </summary>
        /// <param name="lecturerId">מזהה המרצה</param>
        /// <returns>DataTable עם פרטי הסטודנטים והקורסים</returns>
        public static DataTable GetLecturerStudentsWithGrades(int lecturerId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = @"
            SELECT DISTINCT s.Name AS StudentName, c.CourseName, sc.Grade
            FROM Students s
            INNER JOIN StudentCourses sc ON s.StudentID = sc.StudentID
            INNER JOIN Courses c ON sc.CourseID = c.CourseID
            INNER JOIN LecturersCourses lc ON c.CourseID = lc.CourseID
            WHERE lc.LecturerID = @LecturerID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@LecturerID", lecturerId);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public static (string Specialization, float AverageStars) GetLecturerInfo(int lecturerId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT Specialization, AverageStars FROM Lecturers WHERE LecturerID = @LecturerID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@LecturerID", lecturerId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (
                                reader["Specialization"] == DBNull.Value ? string.Empty : reader["Specialization"].ToString(),
                                reader["AverageStars"] == DBNull.Value ? 0 : Convert.ToSingle(reader["AverageStars"])
                            );
                        }
                    }
                }
            }
            return (string.Empty, 0);
        }

        public static void UpdateLecturerSpecialization(int lecturerId, string specialization)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "UPDATE Lecturers SET Specialization = @Specialization WHERE LecturerID = @LecturerID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@LecturerID", lecturerId);
                    cmd.Parameters.AddWithValue("@Specialization", specialization);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// מחזיר את כול הקורסים של סטודנט כולל השמות של המרצים של הקורסים וכולל הנקודות זיכוי
        /// לוקח את הקורסים של הסטודנט מטבלת StudentCourses על ידי שימוש במזהה של הסטודנט
        /// לוקח את נקודות זיכוי מטבלת Courses על ידי שימוש במזהה של הקורס מטבלת StudentCourses
        /// לוקח את המרצה על ידי שימוש במזהה של הקורס מטבלת LecturersCourses
        /// </summary>
        /// <param name="StudentId">מזהה סטודנט</param>
        /// <returns>DataTable עם שם הקורס נקודות זיכוי ומזהה מרצה</returns>
        public static DataTable GetStudentCourses(int studentId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = @"
        SELECT c.CourseName, c.Credits, l.Name AS LecturerName, l.LecturerID
        FROM StudentCourses sc
        INNER JOIN Courses c ON sc.CourseID = c.CourseID
        INNER JOIN LecturersCourses lc ON c.CourseID = lc.CourseID
        INNER JOIN Lecturers l ON lc.LecturerID = l.LecturerID
        WHERE sc.StudentID = @StudentID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }


        public static void SaveLecturerRating( int lecturerId, int rating)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // Add the new rating to the lecturer's total stars
                string query = @"
                UPDATE Lecturers
                SET AverageStars = AverageStars + @Rating
                WHERE LecturerID = @LecturerID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@LecturerID", lecturerId);
                    cmd.Parameters.AddWithValue("@Rating", rating);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static (int CurrentCredits, int TotalCredits) GetStudentInfo(int studentId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                // Query to retrieve current and total credits for the student
                string query = @"
            SELECT 
                s.CurrentCredits, 
                t.TotalCredits 
            FROM Students s
            INNER JOIN Tracks t ON s.TrackID = t.TrackID
            WHERE s.StudentID = @StudentID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int currentCredits = reader["CurrentCredits"] != DBNull.Value ? Convert.ToInt32(reader["CurrentCredits"]) : 0;
                            int totalCredits = reader["TotalCredits"] != DBNull.Value ? Convert.ToInt32(reader["TotalCredits"]) : 0;

                            return (currentCredits, totalCredits);
                        }
                        else
                        {
                            throw new Exception("לא נמצאו פרטים עבור הסטודנט הנוכחי.");
                        }
                    }
                }
            }
        }


    }
}
