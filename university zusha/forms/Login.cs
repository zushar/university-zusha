using System;
using System.Data;
using System.Windows.Forms;
using UniversityZusha.funcions;
using System.Data.SqlClient;
using System.Configuration;
using UniversityZusha.forms;

namespace UniversityZusha
{
    public partial class Login : Form
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["SchoolDbConnection"].ConnectionString;

        public Login()
        {
            InitializeComponent();
            CheckAndCreateFirstAdmin();
        }

        /// <summary>
        /// Checks if the first admin exists in the system and creates one if not.
        /// </summary>
        private void CheckAndCreateFirstAdmin()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string checkAdminQuery = "SELECT COUNT(*) FROM Auth WHERE Role = 'Admin'";
                    SqlCommand checkAdminCommand = new SqlCommand(checkAdminQuery, connection);
                    int adminCount = (int)checkAdminCommand.ExecuteScalar();
                    if (adminCount > 0)

                    {

                        Console.WriteLine("אדמין כבר קיים במערכת.");

                        return;

                    }
                    string adminPassword = "password";
                    string hashedPassword = PasswordHashHelper.HashPasswordHash(adminPassword);
                    string insertAuthQuery = @"INSERT INTO Auth (AuthID, UserName, PasswordHash, Role, RegistrationStatus) 
                                                       VALUES (@AuthID, @UserName, @PasswordHash, @Role, @RegistrationStatus)";
                    SqlCommand authCommand = new SqlCommand(insertAuthQuery, connection);
                    authCommand.Parameters.AddWithValue("@AuthID", 1);
                    authCommand.Parameters.AddWithValue("@UserName", "admin");

                    authCommand.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                    authCommand.Parameters.AddWithValue("@Role", "Admin");
                    authCommand.Parameters.AddWithValue("@RegistrationStatus", "Approved");
                    authCommand.ExecuteNonQuery();
                    string selectAuthIDQuery = "SELECT TOP 1 AuthID FROM Auth WHERE UserName = @UserName ORDER BY AuthID DESC";
                    SqlCommand selectAuthIDCommand = new SqlCommand(selectAuthIDQuery, connection);
                    selectAuthIDCommand.Parameters.AddWithValue("@UserName", "admin");
                    int adminAuthID = (int)selectAuthIDCommand.ExecuteScalar();
                    string insertDepartmentHeadQuery = @"INSERT INTO DepartmentHeads (AuthID, Name, DateOfBirth, PhoneNumber, Email, ManagedDepartmentID) 
                                                                 VALUES (@AuthID, @Name, @DateOfBirth, @PhoneNumber, @Email,, @ManagedDepartmentID)";
                    SqlCommand departmentHeadCommand = new SqlCommand(insertDepartmentHeadQuery, connection);

                    departmentHeadCommand.Parameters.AddWithValue("@AuthID", adminAuthID);

                    departmentHeadCommand.Parameters.AddWithValue("@Name", "Admin Name");

                    departmentHeadCommand.Parameters.AddWithValue("@DateOfBirth", new DateTime(1980, 1, 1));

                    departmentHeadCommand.Parameters.AddWithValue("@PhoneNumber", "050-1234567");

                    departmentHeadCommand.Parameters.AddWithValue("@Email", "admin@example.com");

                    departmentHeadCommand.Parameters.AddWithValue("@ManagedDepartmentID", 1);

                    departmentHeadCommand.ExecuteNonQuery();
                    Console.WriteLine("אדמין ראשון נוסף בהצלחה!");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("שגיאה במסד הנתונים: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Event handler for the buttonLogin click event.
        /// Attempts to log in the user based on the entered username and password.
        /// If the login is successful, it hides the current form and opens a new form based on the user's role.
        /// If the login fails, it displays an error message.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                try
                {
                    string txtUserName = textBoxIdNum.Text;
                    string txtPasswordHash = textBoxPasswordHash.Text;

                    using (SqlCommand cnn = new SqlCommand("SELECT AuthID, UserName, PasswordHash, Role FROM Auth WHERE UserName = @UserName", con))
                    {
                        cnn.Parameters.AddWithValue("@UserName", txtUserName);
                        SqlDataAdapter da = new SqlDataAdapter(cnn);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            string storedHash = dt.Rows[0]["PasswordHash"].ToString();
                            string userRole = dt.Rows[0]["Role"].ToString();
                            int userId = Convert.ToInt32(dt.Rows[0]["AuthID"]); // Use Convert.ToInt32 for safety

                            // Verify the password
                            if (PasswordHashHelper.VerifyPasswordHash(txtPasswordHash, storedHash))
                            {
                                // Successful login
                                this.Hide(); // Hide the current form

                                if (userRole == "Admin")
                                {
                                    Admin adminForm = new Admin(userId, this);
                                    adminForm.Show();
                                }
                                else if (userRole == "DepartmentHead")
                                {
                                    DepartmentHead departmentHeadForm = new DepartmentHead(userId, this);
                                    departmentHeadForm.Show();
                                }
                                else if (userRole == "Lecturer")
                                {
                                    Lecturer lecturerForm = new Lecturer(userId, this);
                                    lecturerForm.Show();
                                }
                                else if (userRole == "Student")
                                {
                                    Student studentForm = new Student(userId, this);
                                    studentForm.Show();
                                }
                            }
                            else
                            {
                                labelErrer.Text = "ת.ז או סיסמה שגואים";
                            }
                        }
                        else
                        {
                            labelErrer.Text = "ת.ז או סיסמה שגואים";
                        }
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("שגיאה במסד הנתונים: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Event handler for the buttonSubscribe click event.
        /// Opens the SignIn form as a dialog.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void buttonSubscribe_Click(object sender, EventArgs e)
        {
            SignIn signIn = new SignIn();
            signIn.ShowDialog();
        }

        public void ResetForm()
        {
            textBoxIdNum.Text = string.Empty;
            textBoxPasswordHash.Text = string.Empty;
            labelErrer.Text = string.Empty;
            // אתחול נוסף לפי הצורך
        }
    }
}
