using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;
using UniversityZusha.funcions;
using System.IO;
using System.Drawing;



namespace UniversityZusha.forms
{
    public partial class SignIn : Form
    {
        // מחרוזת החיבור למסד הנתונים
        private static string connectionString = ConfigurationManager.ConnectionStrings["SchoolDbConnection"].ConnectionString;
        private byte[] defaultImageBytes;

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

        private int saveUser()
        {
            try
            {
                if (textBoxPassword.Text != textBoxPasswordCon.Text)
                {
                    MessageBox.Show("הסיסמאות אינן תואמות.");
                    return -1; // חזרה במקרה של שגיאה
                }

                String UserName = textBoxUserId.Text;
                String PasswordHash = PasswordHashHelper.HashPasswordHash(textBoxPassword.Text);
                String Role = comboBoxRole.Text;
                if (Role == "סטודנט")
                {
                    Role = "Student";
                }
                else if (Role == "מרצה")
                {
                    Role = "Lecturer";
                }
                else if (Role == "ראש מחלקה")
                {
                    Role = "DepartmentHead";
                }

                if (Role == "")
                {
                    MessageBox.Show("אנא בחר תפקיד.");
                    return -1; // חזרה במקרה של שגיאה
                }
                if (UserName == "" || PasswordHash == "")
                {
                    MessageBox.Show("אנא מלא את כל השדות.");
                    return -1; // חזרה במקרה של שגיאה
                }
                if (UserName.Length < 9)
                {
                    MessageBox.Show("שם משתמש חייב להיות תעודת זהות.");
                    return -1; // חזרה במקרה של שגיאה
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    String query = @"
                INSERT INTO Auth (UserName, PasswordHash, Role, RegistrationStatus) 
                VALUES (@UserName, @PasswordHash, @Role, @RegistrationStatus);
                SELECT SCOPE_IDENTITY();"; // החזרת ה-AuthId החדש שנוצר

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UserName", UserName);
                    cmd.Parameters.AddWithValue("@PasswordHash", PasswordHash);
                    cmd.Parameters.AddWithValue("@Role", Role);
                    cmd.Parameters.AddWithValue("@RegistrationStatus", "Pending");

                    // ביצוע השאילתה וקבלת ה-AuthId
                    int newAuthId = Convert.ToInt32(cmd.ExecuteScalar());

                    MessageBox.Show("המשתמש נוצר בהצלחה. AuthId: " + newAuthId);

                    return newAuthId; // מחזיר את ה-AuthId החדש
                }
            }
            catch (SqlException sqlEx)
            {
                // טיפול בשגיאות מסד הנתונים
                MessageBox.Show("שגיאה במסד הנתונים: " + sqlEx.Message);
                return -1; // חזרה במקרה של שגיאה
            }
            catch (Exception ex)
            {
                // טיפול בשגיאות כלליות
                MessageBox.Show("אירעה שגיאה: " + ex.Message);
                return -1; // חזרה במקרה של שגיאה
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
            int authId = saveUser(); // יצירת משתמש חדש בטבלת Auth
            if (authId == -1)
            {
                MessageBox.Show("שגיאה ביצירת משתמש.");
                return;
            }

            string selectedRole = comboBoxRole.Text.Trim();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                switch (selectedRole)
                {
                    case "סטודנט":
                        string studentName = textBoxStudentName.Text;
                        string studentPhone = textBoxStudentPhone.Text;
                        string studentEmail = textBoxStudentEmail.Text;
                        DateTime dateOfBirth = dateTimePickerStudentDateOfBirth.Value;

                        cmd.CommandText = @"
                    INSERT INTO Students (StudentID, AuthID, Name, PhoneNumber, Email, DateOfBirth, Image) 
                    VALUES (@AuthID, @StudentName, @StudentPhone, @StudentEmail, @DateOfBirth, @Image)";
                        cmd.Parameters.AddWithValue("@StudentID", authId);
                        cmd.Parameters.AddWithValue("@AuthID", authId);
                        cmd.Parameters.AddWithValue("@StudentName", studentName);
                        cmd.Parameters.AddWithValue("@StudentPhone", studentPhone);
                        cmd.Parameters.AddWithValue("@StudentEmail", studentEmail);
                        cmd.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                        cmd.Parameters.AddWithValue("@Image", defaultImageBytes); // התמונה הדיפולטית או התמונה שהועלתה
                        break;

                    case "מרצה":
                        string lecturerName = textBoxLecturerName.Text;
                        string lecturerPhone = textBoxLecturerPhone.Text;
                        string lecturerEmail = textBoxLecturerEmail.Text;
                        DateTime lecturerDateOfBirth = dateTimePickerLecturerDateOfBirth.Value;

                        cmd.CommandText = @"
                    INSERT INTO Lecturers (LecturerID, AuthID, Name, PhoneNumber, Email, DateOfBirth, Image) 
                    VALUES (@AuthID, @LecturerName, @LecturerPhone, @LecturerEmail, @DateOfBirth, @Image)";
                        cmd.Parameters.AddWithValue("@LecturerID", authId);
                        cmd.Parameters.AddWithValue("@AuthID", authId);
                        cmd.Parameters.AddWithValue("@LecturerName", lecturerName);
                        cmd.Parameters.AddWithValue("@LecturerPhone", lecturerPhone);
                        cmd.Parameters.AddWithValue("@LecturerEmail", lecturerEmail);
                        cmd.Parameters.AddWithValue("@DateOfBirth", lecturerDateOfBirth);
                        cmd.Parameters.AddWithValue("@Image", defaultImageBytes); // התמונה הדיפולטית או התמונה שהועלתה
                        break;

                    case "ראש מחלקה":
                        string headName = textBoxDepartmentHeadName.Text;
                        string headPhone = textBoxDepartmentHeadPhone.Text;
                        string headEmail = textBoxDepartmentHeadEmail.Text;
                        DateTime headDateOfBirth = dateTimePickerDepartmentHeadDateOfBirth.Value;

                        cmd.CommandText = @"
                    INSERT INTO DepartmentHeads (DepartmentHeadID, AuthID, Name, PhoneNumber, Email, DateOfBirth, Image, ManagedDepartmentID) 
                    VALUES (@AuthID, @HeadName, @HeadPhone, @HeadEmail, @DateOfBirth, @Image, @ManagedDepartmentID)";
                        cmd.Parameters.AddWithValue("@DepartmentHeadID", authId);
                        cmd.Parameters.AddWithValue("@AuthID", authId);
                        cmd.Parameters.AddWithValue("@HeadName", headName);
                        cmd.Parameters.AddWithValue("@HeadPhone", headPhone);
                        cmd.Parameters.AddWithValue("@HeadEmail", headEmail);
                        cmd.Parameters.AddWithValue("@DateOfBirth", headDateOfBirth);
                        cmd.Parameters.AddWithValue("@Image", defaultImageBytes); // התמונה הדיפולטית או התמונה שהועלתה
                        cmd.Parameters.AddWithValue("@ManagedDepartmentID", 1);
                        break;

                    default:
                        MessageBox.Show("אנא בחר תפקיד.");
                        return;
                }

                try
                {
                    cmd.ExecuteNonQuery(); // הוספת המשתמש החדש לפי התפקיד לטבלה המתאימה
                    MessageBox.Show("משתמש נוסף בהצלחה!");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("שגיאה במהלך הוספת המשתמש: " + ex.Message);
                }
            }
        }

        private void buttonAsUser_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
