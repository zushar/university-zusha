using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;

namespace UniversityZusha.forms
{
    public partial class Admin : Form
    {
        // מחרוזת החיבור למסד הנתונים
        private static string connectionString = ConfigurationManager.ConnectionStrings["SchoolDbConnection"].ConnectionString;
        private int curentUserId;
        private Login loginForm;
        private bool isLoggingOut = false;
        public Admin(int authId, Login loginForm)
        {   
            this.curentUserId = authId;
            this.loginForm = loginForm;
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(Admin_FormClosed);
            this.Load += new System.EventHandler(this.Admin_Load);
            LoadUnapprovedDepartmentHeads(); // קריאה לפונקציה שמציגה את ראשי המחלקות הלא מאושרים
            LoadDepartmentHeads(); // קריאה לפונקציה שמציגה את ראשי המחלקות
            LoadDepartments(); // קריאה לפונקציה שמציגה את רשימת המחלקות
        }

        private void LoadUnapprovedDepartmentHeads()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = @"
                    SELECT a.AuthID, a.UserName, a.RegistrationStatus, dh.Name, dh.DateOfBirth, dh.PhoneNumber, dh.Email, dh.ManagedDepartmentID, d.DepartmentName
                    FROM Auth a
                    JOIN DepartmentHeads dh ON a.AuthID = dh.AuthID
                    JOIN Departments d ON dh.ManagedDepartmentID = d.DepartmentID
                    WHERE a.Role = 'DepartmentHead' AND a.RegistrationStatus = 'Pending'";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // הצגת הנתונים ב-DataGridView
                    dataGridViewUnapprovedDepartmentHeads.DataSource = dt;

                    // בדוק אם העמודות כבר קיימות כדי למנוע הוספה כפולה
                    if (!dataGridViewUnapprovedDepartmentHeads.Columns.Contains("Approve"))
                    {
                        // הוספת כפתור אישור
                        DataGridViewButtonColumn approveButton = new DataGridViewButtonColumn();
                        approveButton.Name = "Approve";
                        approveButton.HeaderText = "אשר";
                        approveButton.Text = "אשר";
                        approveButton.UseColumnTextForButtonValue = true;
                        dataGridViewUnapprovedDepartmentHeads.Columns.Add(approveButton);
                    }

                    if (!dataGridViewUnapprovedDepartmentHeads.Columns.Contains("Reject"))
                    {
                        // הוספת כפתור דחייה
                        DataGridViewButtonColumn rejectButton = new DataGridViewButtonColumn();
                        rejectButton.Name = "Reject";
                        rejectButton.HeaderText = "דחה";
                        rejectButton.Text = "דחה";
                        rejectButton.UseColumnTextForButtonValue = true;
                        dataGridViewUnapprovedDepartmentHeads.Columns.Add(rejectButton);
                    }

                    // הסתרת עמודת AuthID אם אינך מעוניין להציג אותה
                    dataGridViewUnapprovedDepartmentHeads.Columns["AuthID"].Visible = false;
                }
            }
        }

        // חיבור אירוע CellContentClick ל-DataGridView
        private void Admin_Load(object sender, EventArgs e)
        {
            dataGridViewUnapprovedDepartmentHeads.CellContentClick += dataGridViewUnapprovedDepartmentHeads_CellContentClick;
            dataGridViewDepartmentHeads.CellContentClick += dataGridViewDepartmentHeads_CellContentClick;
        }

        private void dataGridViewUnapprovedDepartmentHeads_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            string columnName = dataGridViewUnapprovedDepartmentHeads.Columns[e.ColumnIndex].Name;

            if (columnName == "Approve")
            {
                int authID = Convert.ToInt32(dataGridViewUnapprovedDepartmentHeads.Rows[e.RowIndex].Cells["AuthID"].Value);
                ApproveDepartmentHead(authID);
                LoadUnapprovedDepartmentHeads();
                LoadDepartmentHeads();
            }
            else if (columnName == "Reject")
            {
                int authID = Convert.ToInt32(dataGridViewUnapprovedDepartmentHeads.Rows[e.RowIndex].Cells["AuthID"].Value);
                RejectDepartmentHead(authID);
                LoadUnapprovedDepartmentHeads();
                LoadDepartmentHeads();
            }
        }

        private void dataGridViewDepartmentHeads_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            string columnName = dataGridViewDepartmentHeads.Columns[e.ColumnIndex].Name;
            int authID = Convert.ToInt32(dataGridViewDepartmentHeads.Rows[e.RowIndex].Cells["AuthID"].Value);

            if (columnName == "SetPending")
            {
                SetPendingDepartmentHead(authID);
                LoadDepartmentHeads(); // רענון הרשימה
                LoadUnapprovedDepartmentHeads(); // רענון הרשימה של הלא מאושרים
            }
            else if (columnName == "Reject")
            {
                DialogResult result = MessageBox.Show("האם אתה בטוח שברצונך לדחות את ראש המחלקה?", "אישור דחייה", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    RejectDepartmentHead(authID);
                    LoadDepartmentHeads(); // רענון הרשימה
                    LoadUnapprovedDepartmentHeads(); // רענון הרשימה של הלא מאושרים
                }
            }
        }

        private void SetPendingDepartmentHead(int authID)
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

        private void ApproveDepartmentHead(int authID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "UPDATE Auth SET RegistrationStatus = 'Approved', ApprovedByID = @ApprovedByID WHERE AuthID = @AuthID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@AuthID", authID);
                        cmd.Parameters.AddWithValue("@ApprovedByID", this.curentUserId);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("ראש המחלקה אושר בהצלחה.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה: " + ex.Message);
            }
        }

        private void RejectDepartmentHead(int authID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "UPDATE Auth SET RegistrationStatus = 'Rejected', ApprovedByID = @ApprovedByID WHERE AuthID = @AuthID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@AuthID", authID);
                        cmd.Parameters.AddWithValue("@ApprovedByID", this.curentUserId);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("ראש המחלקה נדחה.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה: " + ex.Message);
            }
        }

        private void LoadDepartmentHeads()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = @"
                    SELECT a.AuthID, a.UserName, a.RegistrationStatus, dh.Name, dh.DateOfBirth, dh.PhoneNumber, dh.Email, dh.ManagedDepartmentID, d.DepartmentName
                    FROM Auth a
                    JOIN DepartmentHeads dh ON a.AuthID = dh.AuthID
                    JOIN Departments d ON dh.ManagedDepartmentID = d.DepartmentID
                    WHERE a.Role = 'DepartmentHead' AND a.RegistrationStatus NOT IN ('Pending')";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // הצגת הנתונים ב-DataGridView
                    dataGridViewDepartmentHeads.DataSource = dt;

                    dataGridViewDepartmentHeads.ReadOnly = false;
                    dataGridViewDepartmentHeads.Columns["AuthID"].Visible = false;

                    // המרת ManagedDepartmentID ל-ComboBoxColumn
                    if (dataGridViewDepartmentHeads.Columns.Contains("DepartmentComboBox"))
                    {
                        dataGridViewDepartmentHeads.Columns.Remove("DepartmentComboBox");
                    }

                    // המרת ManagedDepartmentID ל-ComboBoxColumn
                    DataGridViewComboBoxColumn departmentComboBox = new DataGridViewComboBoxColumn();
                    departmentComboBox.Name = "DepartmentComboBox";
                    departmentComboBox.HeaderText = "מחלקה";
                    departmentComboBox.DataPropertyName = "ManagedDepartmentID";
                    departmentComboBox.DisplayMember = "DepartmentName";
                    departmentComboBox.ValueMember = "DepartmentID";

                    // קבלת רשימת המחלקות
                    string deptQuery = "SELECT DepartmentID, DepartmentName FROM Departments";
                    SqlDataAdapter deptDa = new SqlDataAdapter(deptQuery, con);
                    DataTable deptDt = new DataTable();
                    deptDa.Fill(deptDt);

                    departmentComboBox.DataSource = deptDt;

                    int columnIndex = dataGridViewDepartmentHeads.Columns["ManagedDepartmentID"].Index;
                    dataGridViewDepartmentHeads.Columns.Insert(columnIndex, departmentComboBox);

                    dataGridViewDepartmentHeads.Columns["ManagedDepartmentID"].Visible = false;

                    // הוספת כפתורי 'Pending' ו-'Reject' אם הם עדיין לא קיימים
                    if (!dataGridViewDepartmentHeads.Columns.Contains("SetPending"))
                    {
                        DataGridViewButtonColumn pendingButton = new DataGridViewButtonColumn();
                        pendingButton.Name = "SetPending";
                        pendingButton.HeaderText = "הגדר כממתין";
                        pendingButton.Text = "ממתין";
                        pendingButton.UseColumnTextForButtonValue = true;
                        dataGridViewDepartmentHeads.Columns.Add(pendingButton);
                    }

                    if (!dataGridViewDepartmentHeads.Columns.Contains("Reject"))
                    {
                        DataGridViewButtonColumn rejectButton = new DataGridViewButtonColumn();
                        rejectButton.Name = "Reject";
                        rejectButton.HeaderText = "דחה";
                        rejectButton.Text = "דחה";
                        rejectButton.UseColumnTextForButtonValue = true;
                        dataGridViewDepartmentHeads.Columns.Add(rejectButton);
                    }

                    // הסתרת עמודת AuthID אם אינך מעוניין להציג אותה
                    dataGridViewDepartmentHeads.Columns["AuthID"].Visible = false;
                }
            }
        }



        private void LoadDepartments()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT DepartmentID, DepartmentName FROM Departments"; // הצגת רשימת מחלקות

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // הצגת רשימת המחלקות ב-DataGridView
                    dataGridViewDepartments.DataSource = dt;
                    dataGridViewDepartments.Columns["DepartmentID"].ReadOnly = true;
                }
            }
        }

        private void Admin_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!isLoggingOut)
            {
                // המשתמש סוגר את הטופס ללא התנתקות - נסגור את האפליקציה
                Application.Exit();
            }
        }

        private void buttonDepartmentHeadSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                foreach (DataGridViewRow row in dataGridViewDepartmentHeads.Rows)
                {
                    if (row.IsNewRow) continue;

                    int authID = Convert.ToInt32(row.Cells["AuthID"].Value);
                    int managedDepartmentID = Convert.ToInt32(row.Cells["DepartmentComboBox"].Value);

                    string query = "UPDATE DepartmentHeads SET ManagedDepartmentID = @ManagedDepartmentID WHERE AuthID = @AuthID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ManagedDepartmentID", managedDepartmentID);
                        cmd.Parameters.AddWithValue("@AuthID", authID);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("השינויים בראשי המחלקות נשמרו בהצלחה!");

                // טעינת הנתונים מחדש
                LoadDepartmentHeads();
            }
        }

        private void buttonDepartmentsSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                foreach (DataGridViewRow row in dataGridViewDepartments.Rows)
                {
                    if (row.IsNewRow) continue;

                    var departmentIDValue = row.Cells["DepartmentID"].Value;
                    string departmentName = row.Cells["DepartmentName"].Value?.ToString();

                    if (string.IsNullOrWhiteSpace(departmentName))
                    {
                        // אם שם המחלקה ריק, דלג על השורה או הצג הודעת שגיאה
                        continue;
                    }

                    if (departmentIDValue == null || departmentIDValue == DBNull.Value || Convert.ToInt32(departmentIDValue) == 0)
                    {
                        // הוספת מחלקה חדשה
                        string insertQuery = "INSERT INTO Departments (DepartmentName) VALUES (@DepartmentName);";
                        using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@DepartmentName", departmentName);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // עדכון מחלקה קיימת
                        int departmentID = Convert.ToInt32(departmentIDValue);

                        string updateQuery = "UPDATE Departments SET DepartmentName = @DepartmentName WHERE DepartmentID = @DepartmentID";
                        using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@DepartmentName", departmentName);
                            cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                MessageBox.Show("השינויים במחלקות נשמרו בהצלחה!");

                // טעינת המחלקות מחדש כדי לעדכן את ה-DataGridView
                LoadDepartments();
                LoadDepartmentHeads();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // סימון שהמשתמש מתנתק
            isLoggingOut = true;

            // איפוס טופס ההתחברות
            loginForm.ResetForm();
            loginForm.Show();

            // סגירת הטופס הנוכחי
            this.Close();
        }
    }
}
