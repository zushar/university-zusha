using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;
using UniversityZusha.funcions;
using System.IO;

namespace UniversityZusha.forms
{
    public partial class SignIn : Form
    {
        // מחרוזת החיבור למסד הנתונים
        private static string connectionString = ConfigurationManager.ConnectionStrings["SchoolDbConnection"].ConnectionString;
        private byte[] defaultImageBytes;
        int authID;

        public SignIn()
        {
            InitializeComponent();
            defaultImageBytes = LoadDefaultImage(); // Load default image at initialization
        }

        // Function to load the default image
        private byte[] LoadDefaultImage()
        {
            // נתיב התמונה הדיפולטית
            string imagePath = @"C:\Users\123zo\source\repos\university zusha\university zusha\Resources\avatar-image.jpg";
            if (!File.Exists(imagePath))
            {
                MessageBox.Show("תמונת ברירת המחדל לא נמצאה בנתיב: " + imagePath);
                return null;
            }

            try
            {
                // המרת התמונה למערך בייטים
                byte[] imageBytes = File.ReadAllBytes(imagePath);

                // בדוק אם מערך הבייטים אינו ריק
                if (imageBytes == null || imageBytes.Length == 0)
                {
                    MessageBox.Show("התמונה הדיפולטית ריקה.");
                    return null;
                }

                return imageBytes; // החזר את מערך הבייטים
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאה בעת טעינת התמונה הדיפולטית: " + ex.Message);
                return null;
            }
        }




        // General function for uploading an image
        private void UploadImage(ref byte[] imageBytes)
        {
            byte[] uploadedImageBytes = SelectImage();

            // If a new image is uploaded, replace the current one
            if (uploadedImageBytes != null)
            {
                imageBytes = uploadedImageBytes;
                MessageBox.Show("התמונה הועלתה בהצלחה!");
            }
            else
            {
                MessageBox.Show("לא נבחרה תמונה. משתמש בתמונה הדיפולטית.");
            }
        }

        // Image selection dialog and binary conversion
        private byte[] SelectImage()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp|All Files|*.*"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string imgLocation = dialog.FileName;
                return File.ReadAllBytes(imgLocation); // Return image as byte array
            }

            return null;
        }


        // אירוע לחיצה על כפתור "העלה תמונה" של סטודנט
        public void buttonUploadStudentImage_Click(object sender, EventArgs e)
        {
            UploadImage(ref defaultImageBytes);
        }

        // אירוע לחיצה על כפתור "העלה תמונה" של מרצה
        private void buttonUpLodeImighLecture_Click(object sender, EventArgs e)
        {
            UploadImage(ref defaultImageBytes);
        }

        // אירוע לחיצה על כפתור "העלה תמונה" של ראש מחלקה
        private void buttonDepartmentHeadImighUpLode_Click(object sender, EventArgs e)
        {
            UploadImage(ref defaultImageBytes);
        }

        // פונקציה להמרת תמונה לבינארי מתוך קובץ
        private byte[] ImaghBynare()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp|All Files|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string imgLocation = dialog.FileName;
                return File.ReadAllBytes(imgLocation);
            }

            return null;
        }

        private int SaveUser()
        {
            try
            {
                // Validate password confirmation
                if (textBoxPassword.Text != textBoxPasswordCon.Text)
                {
                    MessageBox.Show("הסיסמאות אינן תואמות.");
                    return -1; // Return in case of error
                }

                string userName = textBoxUserId.Text.Trim();

                // Validate UserName is numeric
                if (!int.TryParse(userName, out int authID))
                {
                    MessageBox.Show("תעודת זהות חייבת להיות מספרית.");
                    return -1; // Return in case of error
                }

                // Hash the password
                string passwordHash = PasswordHashHelper.HashPasswordHash(textBoxPassword.Text);

                // Map role from Hebrew to English
                string role = comboBoxRole.Text.Trim();
                switch (role)
                {
                    case "סטודנט":
                        role = "Student";
                        break;
                    case "מרצה":
                        role = "Lecturer";
                        break;
                    case "ראש מחלקה":
                        role = "DepartmentHead";
                        break;
                    default:
                        role = "";
                        break;
                }

                // Validate role selection
                if (string.IsNullOrEmpty(role))
                {
                    MessageBox.Show("אנא בחר תפקיד.");
                    return -1; // Return in case of error
                }

                // Validate required fields
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passwordHash))
                {
                    MessageBox.Show("אנא מלא את כל השדות.");
                    return -1; // Return in case of error
                }

                // Validate UserName length (assuming it's a national ID)
                if (userName.Length < 9)
                {
                    MessageBox.Show("שם משתמש חייב להיות תעודת זהות.");
                    return -1; // Return in case of error
                }

                // Insert into Auth table
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = @"
                INSERT INTO Auth (AuthID, UserName, PasswordHash, Role, RegistrationStatus) 
                VALUES (@AuthID, @UserName, @PasswordHash, @Role, @RegistrationStatus);
            ";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@AuthID", authID);
                        cmd.Parameters.AddWithValue("@UserName", userName);
                        cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                        cmd.Parameters.AddWithValue("@Role", role);
                        cmd.Parameters.AddWithValue("@RegistrationStatus", "Pending");

                        cmd.ExecuteNonQuery(); // Execute the insert command
                    }
                }

                return authID; // Return the new AuthID
            }
            catch (SqlException sqlEx)
            {
                // Log the AuthID if needed for debugging
                MessageBox.Show($"AuthID: {authID}\nשגיאה במסד הנתונים: {sqlEx.Message}");
                return -1; // Return in case of error
            }
            catch (Exception ex)
            {
                // Handle general errors
                MessageBox.Show("אירעה שגיאה: " + ex.Message);
                return -1; // Return in case of error
            }
        }

        private void comboBoxRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedRole = comboBoxRole.Text.Trim();

            switch (selectedRole)
            {
                case "סטודנט":
                    panelStudent.Visible = true;
                    panelStudent.BringToFront();
                    panelLecturer.Visible = false;
                    panelDepartmentHead.Visible = false;
                    break;

                case "מרצה":
                    panelLecturer.BringToFront();
                    panelLecturer.Visible = true;
                    panelStudent.Visible = false;
                    panelDepartmentHead.Visible = false;
                    break;

                case "ראש מחלקה":
                    panelDepartmentHead.BringToFront();
                    panelDepartmentHead.Visible = true;
                    panelStudent.Visible = false;
                    panelLecturer.Visible = false;    
                    break;

                default:
                    panelStudent.Visible = false;
                    panelLecturer.Visible = false;
                    panelDepartmentHead.Visible = false;
                    break;
            }
        }

        private void buttonSignIn_Click(object sender, EventArgs e)
        {
            int authId = SaveUser(); // Create a new user in the Auth table

            if (authId == -1)
            {
                // If SaveUser failed, exit the method
                MessageBox.Show("שגיאה ביצירת משתמש.");
                return;
            }

            string selectedRole = comboBoxRole.Text.Trim();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;

                    try
                    {
                        switch (selectedRole)
                        {
                            case "סטודנט":
                                string studentName = textBoxStudentName.Text.Trim();
                                string studentPhone = textBoxStudentPhone.Text.Trim();
                                string studentEmail = textBoxStudentEmail.Text.Trim();
                                DateTime dateOfBirth = dateTimePickerStudentDateOfBirth.Value;

                                cmd.CommandText = @"
                            INSERT INTO Students (StudentID, AuthID, Name, PhoneNumber, Email, DateOfBirth, Image) 
                            VALUES (@StudentID, @AuthID, @StudentName, @StudentPhone, @StudentEmail, @DateOfBirth, @Image)";
                                cmd.Parameters.AddWithValue("@StudentID", authId);
                                cmd.Parameters.AddWithValue("@AuthID", authId);
                                cmd.Parameters.AddWithValue("@StudentName", studentName);
                                cmd.Parameters.AddWithValue("@StudentPhone", studentPhone);
                                cmd.Parameters.AddWithValue("@StudentEmail", studentEmail);
                                cmd.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                                cmd.Parameters.AddWithValue("@Image", defaultImageBytes); // Ensure defaultImageBytes is defined
                                break;

                            case "מרצה":
                                string lecturerName = textBoxLecturerName.Text.Trim();
                                string lecturerPhone = textBoxLecturerPhone.Text.Trim();
                                string lecturerEmail = textBoxLecturerEmail.Text.Trim();
                                DateTime lecturerDateOfBirth = dateTimePickerLecturerDateOfBirth.Value;

                                cmd.CommandText = @"
                            INSERT INTO Lecturers (LecturerID, AuthID, Name, PhoneNumber, Email, DateOfBirth, Image) 
                            VALUES (@LecturerID, @AuthID, @LecturerName, @LecturerPhone, @LecturerEmail, @DateOfBirth, @Image)";
                                cmd.Parameters.AddWithValue("@LecturerID", authId);
                                cmd.Parameters.AddWithValue("@AuthID", authId);
                                cmd.Parameters.AddWithValue("@LecturerName", lecturerName);
                                cmd.Parameters.AddWithValue("@LecturerPhone", lecturerPhone);
                                cmd.Parameters.AddWithValue("@LecturerEmail", lecturerEmail);
                                cmd.Parameters.AddWithValue("@DateOfBirth", lecturerDateOfBirth);
                                cmd.Parameters.AddWithValue("@Image", defaultImageBytes); // Ensure defaultImageBytes is defined
                                break;

                            case "ראש מחלקה":
                                string headName = textBoxDepartmentHeadName.Text.Trim();
                                string headPhone = textBoxDepartmentHeadPhone.Text.Trim();
                                string headEmail = textBoxDepartmentHeadEmail.Text.Trim();
                                DateTime headDateOfBirth = dateTimePickerDepartmentHeadDateOfBirth.Value;

                                cmd.CommandText = @"
                            INSERT INTO DepartmentHeads (DepartmentHeadID, AuthID, Name, PhoneNumber, Email, DateOfBirth, Image, ManagedDepartmentID) 
                            VALUES (@DepartmentHeadID, @AuthID, @HeadName, @HeadPhone, @HeadEmail, @DateOfBirth, @Image, @ManagedDepartmentID)";
                                cmd.Parameters.AddWithValue("@DepartmentHeadID", authId);
                                cmd.Parameters.AddWithValue("@AuthID", authId);
                                cmd.Parameters.AddWithValue("@HeadName", headName);
                                cmd.Parameters.AddWithValue("@HeadPhone", headPhone);
                                cmd.Parameters.AddWithValue("@HeadEmail", headEmail);
                                cmd.Parameters.AddWithValue("@DateOfBirth", headDateOfBirth);
                                cmd.Parameters.AddWithValue("@Image", defaultImageBytes); // Ensure defaultImageBytes is defined
                                cmd.Parameters.AddWithValue("@ManagedDepartmentID", 1); // Consider making this dynamic
                                break;

                            default:
                                MessageBox.Show("אנא בחר תפקיד.");
                                return;
                        }

                        cmd.ExecuteNonQuery(); // Insert the user into the respective table
                        MessageBox.Show("משתמש נוסף בהצלחה!");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("שגיאה במהלך הוספת המשתמש: " + ex.Message);
                    }
                    finally
                    {
                        // Clear parameters to avoid issues if the method is called again
                        cmd.Parameters.Clear();
                    }
                }
            }
        }


        private void buttonAsUser_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
