using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;

namespace UniversityZusha.users
{
    /// <summary>
    /// תפקיד המשתמש במערכת.
    /// </summary>
    public enum Role
    {
        Student,
        TeachingAssistant,
        Lecturer,
        DepartmentHead
    }

    /// <summary>
    /// מחלקה אבסטרקטית המייצגת משתמש במערכת.
    /// </summary>
    public abstract class User
    {
        public string ID { get; set; } 
        public string Name { get; set; } 
        public DateTime DateOfBirth { get; set; } 
        public string PhoneNumber { get; set; } 
        public string Email { get; set; } 
        public byte[] image { get; set; }
        public Role Role { get; set; } 
        public DateTime EnrollmentDate { get; set; } 

        protected User(string id, string name, string email, string phoneNumber,
                       DateTime dateOfBirth, DateTime enrollmentDate, Role role, byte[] image)
        {
            ID = id;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            EnrollmentDate = enrollmentDate;
            Role = role;
            this.image = image;
        }

        /// <summary>
        /// מחשב את הגיל של המשתמש על בסיס תאריך הלידה.
        /// </summary>
        /// <returns>גיל בשנים.</returns>
        public int GetAge()
        {
            int age = DateTime.Now.Year - DateOfBirth.Year;
            if (DateTime.Now.Date < DateOfBirth.Date.AddYears(age))
                age--;
            return age;
        }

        /// <summary>
        /// מחזיר מחרוזת של הודעות ששיכות למשתמש.
        /// </summary>
        /// <returns>רשימת הודעות.</returns>
        public List<string> GetMessages()
        {
            List<string> messages = new List<string>();

            string connectionString = ConfigurationManager.ConnectionStrings["SchoolDbConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT MessageText FROM Messages WHERE UserID = @UserID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", this.ID);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    messages.Add(reader["MessageText"].ToString());
                }
            }

            return messages;
        }
    }
}
