using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using UniversityZusha.dbFunctions;

namespace UniversityZusha.forms
{
    public partial class AssignCoursesToStudentForm : Form
    {
        private int studentID;
        private int departmentHeadID;
        private int trackID;
        private DataGridView dataGridViewCourses;
        private Button btnSave;
        private static string connectionString = ConfigurationManager.ConnectionStrings["SchoolDbConnection"].ConnectionString;

        public AssignCoursesToStudentForm(int studentID, int departmentHeadID)
        {
            this.studentID = studentID;
            this.departmentHeadID = departmentHeadID;
            this.trackID = DbFunctions.GetStudentTrackID(studentID);

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
                        CASE WHEN sc.StudentID IS NOT NULL THEN 1 ELSE 0 END AS IsAssigned
                    FROM Courses c
                    INNER JOIN TrackCourses tc ON c.CourseID = tc.CourseID
                    LEFT JOIN StudentCourses sc ON c.CourseID = sc.CourseID AND sc.StudentID = @StudentID
                    WHERE tc.TrackID = @TrackID
                    GROUP BY c.CourseID, c.CourseName, sc.StudentID
                ";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentID);
                    cmd.Parameters.AddWithValue("@TrackID", trackID);

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
                    // הסרת שיוכים קיימים
                    string deleteQuery = "DELETE FROM StudentCourses WHERE StudentID = @StudentID";
                    using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, con, transaction))
                    {
                        deleteCmd.Parameters.AddWithValue("@StudentID", studentID);
                        deleteCmd.ExecuteNonQuery();
                    }

                    // הוספת שיוכים חדשים
                    foreach (DataGridViewRow row in dataGridViewCourses.Rows)
                    {
                        if (row.Cells["IsAssigned"].Value != DBNull.Value && Convert.ToBoolean(row.Cells["IsAssigned"].Value))
                        {
                            if (row.Cells["CourseID"].Value != DBNull.Value)
                            {
                                int courseID = Convert.ToInt32(row.Cells["CourseID"].Value);
                                string insertQuery = "INSERT INTO StudentCourses (StudentID, CourseID) VALUES (@StudentID, @CourseID)";
                                using (SqlCommand insertCmd = new SqlCommand(insertQuery, con, transaction))
                                {
                                    insertCmd.Parameters.AddWithValue("@StudentID", studentID);
                                    insertCmd.Parameters.AddWithValue("@CourseID", courseID);
                                    insertCmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                // Handle the case when CourseID is null or display a meaningful message
                                MessageBox.Show("Course ID is missing for one of the rows.");
                            }
                        }
                    }

                    transaction.Commit();
                    MessageBox.Show("הקורסים שויכו לסטודנט בהצלחה.");
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
