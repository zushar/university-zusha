using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using UniversityZusha.dbFunctions;

namespace UniversityZusha.forms
{
    public partial class AssignCoursesForm : Form
    {
        private int lecturerID;
        private int departmentHeadID;
        private int departmentID;
        private DataGridView dataGridViewCourses;
        private Button btnSave;
        private static string connectionString = ConfigurationManager.ConnectionStrings["SchoolDbConnection"].ConnectionString;

        public AssignCoursesForm(int lecturerID, int departmentHeadID)
        {
            this.lecturerID = lecturerID;
            this.departmentHeadID = departmentHeadID;
            this.departmentID = DbFunctions.GetDepartmentID(departmentHeadID);

            InitializeComponent();
            LoadCourses();
        }

        private void LoadCourses()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = @"
                    SELECT 
                        c.CourseID,
                        c.CourseName,
                        CASE WHEN lc.LecturerID IS NOT NULL THEN 1 ELSE 0 END AS IsAssigned
                    FROM Courses c
                    INNER JOIN TrackCourses tc ON c.CourseID = tc.CourseID
                    INNER JOIN Tracks t ON tc.TrackID = t.TrackID
                    LEFT JOIN LecturersCourses lc ON c.CourseID = lc.CourseID AND lc.LecturerID = @LecturerID
                    WHERE t.DepartmentID = @DepartmentID
                    GROUP BY c.CourseID, c.CourseName, lc.LecturerID
                ";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@LecturerID", lecturerID);
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentID);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridViewCourses.DataSource = dt;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
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
                    foreach (DataGridViewRow row in dataGridViewCourses.Rows)
                    {
                        bool isAssigned = Convert.ToBoolean(row.Cells["IsAssigned"].Value);
                        if (isAssigned)
                        {
                            int courseID = Convert.ToInt32(row.Cells["CourseID"].Value);
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
                    MessageBox.Show("הקורסים שויכו למרצה בהצלחה.");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("אירעה שגיאה: " + ex.Message);
                }
            }
        }
    }
}
