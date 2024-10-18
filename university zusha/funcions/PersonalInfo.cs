using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace UniversityZusha.PersonalInfoFunctions
{
    internal class PersonalInfo
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["SchoolDbConnection"].ConnectionString;

        /// <summary>
        /// Initializes the personal information tab with all necessary controls and loads user data.
        /// </summary>
        /// <param name="tab">The TabPage to initialize</param>
        /// <param name="UserID">The ID of the current user</param>
        /// <param name="userRole">The role of the current user</param>
        public static void InitializePersonalInfoTab(TabPage tab, int UserID, string userRole)
        {
            Label labelName = new Label() { Text = "שם", Location = new Point(20, 20) };
            TextBox textBoxName = new TextBox() { Location = new Point(150, 20), Width = 200 };

            Label labelDateOfBirth = new Label() { Text = "תאריך לידה", Location = new Point(20, 60) };
            DateTimePicker datePickerBirthDate = new DateTimePicker() { Location = new Point(150, 60), Width = 200 };

            Label labelPhone = new Label() { Text = "טלפון", Location = new Point(20, 100) };
            TextBox textBoxPhone = new TextBox() { Location = new Point(150, 100), Width = 200 };

            Label labelEmail = new Label() { Text = "דוא\"ל", Location = new Point(20, 140) };
            TextBox textBoxEmail = new TextBox() { Location = new Point(150, 140), Width = 200 };

            PictureBox pictureBox = new PictureBox()
            {
                Location = new Point(400, 20),
                Size = new Size(150, 150),
                BorderStyle = BorderStyle.Fixed3D,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            Button buttonSavePersonalInfo = new Button() { Text = "שמור", Location = new Point(150, 180) };
            buttonSavePersonalInfo.Click += (s, e) => SavePersonalInfo(textBoxName.Text, datePickerBirthDate.Value, textBoxPhone.Text, textBoxEmail.Text, pictureBox, UserID, userRole);

            Button buttonChooseImage = new Button() { Text = "בחר תמונה", Location = new Point(400, 180) };
            buttonChooseImage.Click += (s, e) => ChooseImage(pictureBox);

            tab.Controls.AddRange(new Control[] { labelName, textBoxName, labelDateOfBirth, datePickerBirthDate,
                                                  labelPhone, textBoxPhone, labelEmail, textBoxEmail,
                                                  buttonSavePersonalInfo, pictureBox, buttonChooseImage });

            LoadPersonalInfo(textBoxName, datePickerBirthDate, textBoxPhone, textBoxEmail, pictureBox, UserID, userRole);
        }

        /// <summary>
        /// Opens a file dialog for the user to choose an image and sets it to the PictureBox.
        /// </summary>
        /// <param name="pictureBox">The PictureBox to set the chosen image to</param>
        public static void ChooseImage(PictureBox pictureBox)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "בחר תמונה";
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox.Image = Image.FromFile(openFileDialog.FileName);
                }
            }
        }

        /// <summary>
        /// Loads personal information for a user from the database and populates the form controls.
        /// </summary>
        /// <param name="textBoxName">TextBox for user's name</param>
        /// <param name="datePickerBirthDate">DateTimePicker for user's birth date</param>
        /// <param name="textBoxPhone">TextBox for user's phone number</param>
        /// <param name="textBoxEmail">TextBox for user's email</param>
        /// <param name="pictureBox">PictureBox for user's image</param>
        /// <param name="UserID">The ID of the current user</param>
        /// <param name="userRole">The role of the current user</param>
        public static void LoadPersonalInfo(TextBox textBoxName, DateTimePicker datePickerBirthDate, TextBox textBoxPhone, TextBox textBoxEmail, PictureBox pictureBox, int UserID, string userRole)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string tableName = GetTableNameByRole(userRole);
                string query = $"SELECT Name, DateOfBirth, PhoneNumber, Email, Image FROM {tableName} WHERE AuthID = @AuthID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@AuthID", UserID);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBoxName.Text = reader["Name"].ToString();

                            if (!reader.IsDBNull(reader.GetOrdinal("DateOfBirth")))
                            {
                                datePickerBirthDate.Value = Convert.ToDateTime(reader["DateOfBirth"]);
                            }

                            textBoxPhone.Text = reader["PhoneNumber"].ToString();
                            textBoxEmail.Text = reader["Email"].ToString();

                            if (!reader.IsDBNull(reader.GetOrdinal("Image")))
                            {
                                byte[] imageData = (byte[])reader["Image"];
                                if (imageData != null && imageData.Length > 0)
                                {
                                    try
                                    {
                                        using (MemoryStream ms = new MemoryStream(imageData))
                                        {
                                            Image image = Image.FromStream(ms);
                                            pictureBox.Image = new Bitmap(image);
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        // Handle or log the exception as appropriate
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Saves the user's personal information to the database.
        /// </summary>
        /// <param name="name">User's name</param>
        /// <param name="birthDate">User's birth date</param>
        /// <param name="phone">User's phone number</param>
        /// <param name="email">User's email</param>
        /// <param name="pictureBox">PictureBox containing user's image</param>
        /// <param name="UserID">The ID of the current user</param>
        /// <param name="userRole">The role of the current user</param>
        public static void SavePersonalInfo(string name, DateTime birthDate, string phone, string email, PictureBox pictureBox, int UserID, string userRole)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string tableName = GetTableNameByRole(userRole);
                    string query = $@"
            UPDATE {tableName}
            SET Name = @Name, DateOfBirth = @DateOfBirth, PhoneNumber = @Phone, Email = @Email";

                    bool imageChanged = HasImageChanged(pictureBox, UserID, userRole);

                    if (imageChanged)
                    {
                        query += ", Image = @Image";
                    }

                    query += " WHERE AuthID = @AuthID";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@DateOfBirth", birthDate);
                        cmd.Parameters.AddWithValue("@Phone", phone);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@AuthID", UserID);

                        if (imageChanged && pictureBox.Image != null)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                System.Drawing.Imaging.ImageFormat format = pictureBox.Image.RawFormat;
                                if (format == null || !System.Drawing.Imaging.ImageFormat.Jpeg.Equals(format))
                                {
                                    format = System.Drawing.Imaging.ImageFormat.Png;
                                }
                                pictureBox.Image.Save(ms, format);
                                byte[] imageBytes = ms.ToArray();
                                cmd.Parameters.AddWithValue("@Image", imageBytes);
                            }
                        }

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                // Handle or log the exception as appropriate
            }
        }

        /// <summary>
        /// Gets the appropriate table name based on the user's role.
        /// </summary>
        /// <param name="userRole">The role of the user</param>
        /// <returns>The name of the database table corresponding to the user's role</returns>
        private static string GetTableNameByRole(string userRole)
        {
            switch (userRole.ToLower())
            {
                case "departmenthead":
                    return "DepartmentHeads";
                case "lecturer":
                    return "Lecturers";
                case "student":
                    return "Students";
                default:
                    throw new ArgumentException("תפקיד לא חוקי", nameof(userRole));
            }
        }

        /// <summary>
        /// Checks if the image in the PictureBox has changed compared to the one stored in the database.
        /// </summary>
        /// <param name="pictureBox">PictureBox containing the potentially new image</param>
        /// <param name="UserID">The ID of the current user</param>
        /// <param name="userRole">The role of the current user</param>
        /// <returns>True if the image has changed, false otherwise</returns>
        private static bool HasImageChanged(PictureBox pictureBox, int UserID, string userRole)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string tableName = GetTableNameByRole(userRole);
                    string query = $"SELECT Image FROM {tableName} WHERE AuthID = @AuthID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@AuthID", UserID);
                        object result = cmd.ExecuteScalar();

                        if (result == null || result == DBNull.Value)
                        {
                            return pictureBox.Image != null;
                        }

                        byte[] currentImageBytes = (byte[])result;

                        if (pictureBox.Image == null)
                        {
                            return true;
                        }

                        using (MemoryStream ms = new MemoryStream())
                        {
                            System.Drawing.Imaging.ImageFormat format = pictureBox.Image.RawFormat;
                            if (format == null || !System.Drawing.Imaging.ImageFormat.Jpeg.Equals(format))
                            {
                                format = System.Drawing.Imaging.ImageFormat.Png;
                            }
                            pictureBox.Image.Save(ms, format);
                            byte[] newImageBytes = ms.ToArray();
                            return !CompareByteArrays(currentImageBytes, newImageBytes);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Handle or log the exception as appropriate
                return false;
            }
        }

        /// <summary>
        /// Compares two byte arrays for equality.
        /// </summary>
        /// <param name="array1">First byte array</param>
        /// <param name="array2">Second byte array</param>
        /// <returns>True if the arrays are equal, false otherwise</returns>
        private static bool CompareByteArrays(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}