using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using UniversityZusha.dbFunctions;
using UniversityZusha.messageFuncions;
using UniversityZusha.PersonalInfoFunctions;

namespace UniversityZusha.forms
{
    public partial class DepartmentHead : Form
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["SchoolDbConnection"].ConnectionString;
        private int departmentHeadID;
        private Login loginForm;
        private bool isLoggingOut = false;
        private int previousTabIndex = 0;
        private DataGridView dgvTracks;
        private DataGridView dgvCourses;
        private DataGridView dataGridViewUnapprovedLecturers;
        private DataGridView dataGridViewLecturers;
        private DataGridView dataGridViewUnapprovedStudents;
        private DataGridView dataGridViewApprovedStudents;
        private DataGridView dataGridViewStudents;
        private DataGridView dataGridViewAssistantCourses;

        public DepartmentHead(int departmentHeadID, Login loginForm)
        {
            this.departmentHeadID = departmentHeadID;
            this.loginForm = loginForm;
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(DepartmentHead_FormClosing);
            InitializeTabs();
        }

        private void DepartmentHead_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isLoggingOut)
            {
                // המשתמש סוגר את הטופס ללא התנתקות - נסגור את האפליקציה
                Application.Exit();
            }
            else
            {
                // המשתמש התנתק - לא נעשה כלום, טופס ההתחברות כבר מוצג
            }
        }

        private void InitializeTabs()
        {
            TabControl tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;

            TabPage tabTracksAndCourses = new TabPage("מסלולים וקורסים");
            InitializeTrackCourseTab(tabTracksAndCourses);
            tabControl.TabPages.Add(tabTracksAndCourses);

            TabPage tabLecturers = new TabPage("מרצים");
            InitializeLecturersTab(tabLecturers);
            tabControl.TabPages.Add(tabLecturers);

            TabPage tabLecAdd = new TabPage("מתרגלים");
            InitializeStudentAssistentTab(tabLecAdd);
            tabControl.TabPages.Add(tabLecAdd);

            TabPage tabStudents = new TabPage("סטודנטים");
            InitializeStudentsTab(tabStudents);
            tabControl.TabPages.Add(tabStudents);

            TabPage tabMessages = new TabPage("הודעות");
            MessageFuncions messageFuncions = new MessageFuncions(departmentHeadID);
            messageFuncions.InitializeMessagesTab(tabMessages);
            tabControl.TabPages.Add(tabMessages);

            TabPage tabPersonalInfo = new TabPage("מידע אישי");
            PersonalInfo.InitializePersonalInfoTab(tabPersonalInfo, departmentHeadID, "departmenthead");
            tabControl.TabPages.Add(tabPersonalInfo);

            TabPage tabLogout = new TabPage("התנתק");
            InitializeLogoutTab(tabLogout);
            tabControl.TabPages.Add(tabLogout);

            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;

            this.Controls.Add(tabControl);
        }

        private void InitializeStudentAssistentTab(TabPage tab)
        {
            // יצירת DataGridView לרשימת הסטודנטים
            dataGridViewStudents = new DataGridView();
            dataGridViewStudents.Dock = DockStyle.Top;
            dataGridViewStudents.Height = 200;
            dataGridViewStudents.AutoGenerateColumns = false;
            dataGridViewStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewStudents.MultiSelect = false;

            // הגדרת עמודות לרשימת הסטודנטים
            dataGridViewStudents.Columns.Add(new DataGridViewTextBoxColumn() { Name = "StudentID", HeaderText = "מזהה סטודנט", DataPropertyName = "StudentID" });
            dataGridViewStudents.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Name", HeaderText = "שם", DataPropertyName = "Name" });
            dataGridViewStudents.Columns.Add(new DataGridViewTextBoxColumn() { Name = "TrackName", HeaderText = "מסלול", DataPropertyName = "TrackName" });  
            dataGridViewStudents.Columns.Add(new DataGridViewCheckBoxColumn() { Name = "IsAssistant", HeaderText = "מתרגל?", DataPropertyName = "IsAssistant", TrueValue = true, FalseValue = false, IndeterminateValue = false });

            // יצירת DataGridView לרשימת הקורסים של הסטודנט הנבחר
            dataGridViewAssistantCourses = new DataGridView();
            dataGridViewAssistantCourses.Dock = DockStyle.Bottom;
            dataGridViewAssistantCourses.Height = 200;
            dataGridViewAssistantCourses.AutoGenerateColumns = false;
            dataGridViewAssistantCourses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // הגדרת עמודות לרשימת הקורסים
            dataGridViewAssistantCourses.Columns.Add(new DataGridViewTextBoxColumn() { Name = "CourseID", HeaderText = "מזהה קורס", DataPropertyName = "CourseID" });
            dataGridViewAssistantCourses.Columns.Add(new DataGridViewTextBoxColumn() { Name = "CourseName", HeaderText = "שם הקורס", DataPropertyName = "CourseName" });
            dataGridViewAssistantCourses.Columns.Add(new DataGridViewCheckBoxColumn() { Name = "IsAssistantFor", HeaderText = "מתרגל בקורס?", DataPropertyName = "IsAssistantFor" });

            // הוספת כפתור לעדכון סטטוס המתרגל
            Button btnUpdateAssistantStatus = new Button();
            btnUpdateAssistantStatus.Text = "עדכן סטטוס מתרגל";
            btnUpdateAssistantStatus.Dock = DockStyle.Bottom;
            btnUpdateAssistantStatus.Click += BtnUpdateAssistantStatus_Click;

            // הוספת הרכיבים ללשונית
            tab.Controls.Add(dataGridViewStudents);
            tab.Controls.Add(dataGridViewAssistantCourses);
            tab.Controls.Add(btnUpdateAssistantStatus);

            // טעינת נתוני הסטודנטים
            LoadStudents();

            // הוספת אירוע לבחירת סטודנט
            dataGridViewStudents.SelectionChanged += DataGridViewStudents_SelectionChanged;
        }

        private void LoadStudents()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = @"
                    SELECT s.StudentID, s.Name, t.TrackName, 
                    CASE WHEN EXISTS (SELECT 1 FROM StudentAssistantCourses WHERE StudentID = s.StudentID)
                    THEN 1 ELSE 0 END AS IsAssistant
                    FROM Students s
                    INNER JOIN Tracks t ON s.TrackID = t.TrackID
                    WHERE t.DepartmentID = @DepartmentID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@DepartmentID", DbFunctions.GetDepartmentID(departmentHeadID));
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewStudents.DataSource = dt;
                }
            }
        }

        private void DataGridViewStudents_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewStudents.SelectedRows.Count > 0)
            {
                int studentID = Convert.ToInt32(dataGridViewStudents.SelectedRows[0].Cells["StudentID"].Value);
                LoadStudentCourses(studentID);
            }
        }

        private void LoadStudentCourses(int studentID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = @"
                    SELECT c.CourseID, c.CourseName, 
                    CASE WHEN sac.StudentID IS NOT NULL THEN 1 ELSE 0 END AS IsAssistantFor
                    FROM StudentCourses sc
                    INNER JOIN Courses c ON sc.CourseID = c.CourseID
                    LEFT JOIN StudentAssistantCourses sac ON sc.StudentID = sac.StudentID AND sc.CourseID = sac.CourseID
                    WHERE sc.StudentID = @StudentID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentID);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewAssistantCourses.DataSource = dt;
                }
            }
        }

        private void BtnUpdateAssistantStatus_Click(object sender, EventArgs e)
        {
            UpdateStudentAssistantStatus();
        }

        private void UpdateStudentAssistantStatus()
        {
            if (dataGridViewStudents.SelectedRows.Count == 0) return;

            int studentID = Convert.ToInt32(dataGridViewStudents.SelectedRows[0].Cells["StudentID"].Value);

            // לוודא שהערך נלקח נכון מהעמודה
            bool isAssistant = Convert.ToBoolean(dataGridViewStudents.SelectedRows[0].Cells["IsAssistant"].EditedFormattedValue);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        // עדכון הסטטוס הכללי של הסטודנט בטבלת Students
                        string updateStudentQuery = "UPDATE Students SET IsAssistant = @IsAssistant WHERE StudentID = @StudentID";
                        using (SqlCommand updateStudentCmd = new SqlCommand(updateStudentQuery, con, transaction))
                        {
                            updateStudentCmd.Parameters.AddWithValue("@IsAssistant", isAssistant);
                            updateStudentCmd.Parameters.AddWithValue("@StudentID", studentID);
                            updateStudentCmd.ExecuteNonQuery();
                        }

                        // מחיקת כל הרשומות הקיימות של הסטודנט בטבלת StudentAssistantCourses
                        string deleteAllQuery = "DELETE FROM StudentAssistantCourses WHERE StudentID = @StudentID";
                        using (SqlCommand deleteAllCmd = new SqlCommand(deleteAllQuery, con, transaction))
                        {
                            deleteAllCmd.Parameters.AddWithValue("@StudentID", studentID);
                            deleteAllCmd.ExecuteNonQuery();
                        }

                        // הוספת הקורסים שבהם הסטודנט מתרגל
                        if (isAssistant)
                        {
                            foreach (DataGridViewRow row in dataGridViewAssistantCourses.Rows)
                            {
                                int courseID = Convert.ToInt32(row.Cells["CourseID"].Value);
                                bool isAssistantFor = Convert.ToBoolean(row.Cells["IsAssistantFor"].EditedFormattedValue);

                                if (isAssistantFor)
                                {
                                    string insertQuery = @"
                                IF NOT EXISTS (SELECT 1 FROM StudentAssistantCourses WHERE StudentID = @StudentID AND CourseID = @CourseID)
                                BEGIN
                                    INSERT INTO StudentAssistantCourses (StudentID, CourseID) VALUES (@StudentID, @CourseID)
                                END";
                                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, con, transaction))
                                    {
                                        insertCmd.Parameters.AddWithValue("@StudentID", studentID);
                                        insertCmd.Parameters.AddWithValue("@CourseID", courseID);
                                        insertCmd.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                        transaction.Commit();
                        MessageBox.Show("סטטוס המתרגל עודכן בהצלחה.");
                        LoadStudents();
                        LoadStudentCourses(studentID);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"אירעה שגיאה בעדכון סטטוס המתרגל: {ex.Message}");
                    }
                }
            }
        }


        private void InitializeLogoutTab(TabPage tab)
        {
            // יצירת כפתור התנתקות
            Button logoutButton = new Button();
            logoutButton.Text = "התנתק";
            logoutButton.AutoSize = true;
            logoutButton.Location = new Point(20, 20);
            logoutButton.Click += LogoutButton_Click;

            // הוספת הכפתור ללשונית
            tab.Controls.Add(logoutButton);
        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            // סימון שהמשתמש מתנתק
            isLoggingOut = true;

            // איפוס טופס ההתחברות
            loginForm.ResetForm();
            loginForm.Show();

            // סגירת הטופס הנוכחי
            this.Close();
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            if (tabControl.SelectedTab.Text == "התנתק")
            {
                // אופציונלי: לשאול את המשתמש אם הוא בטוח
                DialogResult result = MessageBox.Show("האם אתה בטוח שברצונך להתנתק?", "אישור התנתקות", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // סימון שהמשתמש מתנתק
                    isLoggingOut = true;

                    // איפוס טופס ההתחברות
                    loginForm.ResetForm();
                    loginForm.Show();

                    // סגירת הטופס הנוכחי
                    this.Close();
                }
                else
                {
                    // חזרה ללשונית הקודמת
                    tabControl.SelectedIndex = previousTabIndex;
                }
            }
            else
            {
                // שמירת האינדקס של הלשונית הנוכחית
                previousTabIndex = tabControl.SelectedIndex;
            }
        }

        private void InitializeTrackCourseTab(TabPage tab)
        {
            // יצירת DataGridViews
            dgvTracks = new DataGridView();
            dgvCourses = new DataGridView();

            // הגדרות ל-dgvTracks
            dgvTracks.Dock = DockStyle.Top;
            dgvTracks.Height = 150;
            dgvTracks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTracks.MultiSelect = false;
            dgvTracks.AllowUserToAddRows = false;
            dgvTracks.ReadOnly = true;
            dgvTracks.AutoGenerateColumns = true;

            // הגדרות ל-dgvCourses
            dgvCourses.Dock = DockStyle.Fill;
            dgvCourses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCourses.MultiSelect = false;
            dgvCourses.AllowUserToAddRows = false;
            dgvCourses.ReadOnly = true;
            dgvCourses.AutoGenerateColumns = false; // שינינו ל-False כדי להגדיר עמודות ידנית

            dgvCourses.Columns.Clear();
            dgvCourses.Columns.Add(new DataGridViewTextBoxColumn() { Name = "CourseID", DataPropertyName = "CourseID", Visible = false });
            dgvCourses.Columns.Add(new DataGridViewTextBoxColumn() { Name = "CourseName", HeaderText = "שם הקורס", DataPropertyName = "CourseName" });
            dgvCourses.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Credits", HeaderText = "נקודות זכות", DataPropertyName = "Credits" });
            dgvCourses.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Lecturers", HeaderText = "מרצים", DataPropertyName = "Lecturers" });

            // כפתורים לניהול מסלולים
            Button btnAddTrack = new Button() { Text = "הוסף מסלול" };
            btnAddTrack.Click += BtnAddTrack_Click;
            Button btnEditTrack = new Button() { Text = "ערוך מסלול" };
            btnEditTrack.Click += BtnEditTrack_Click;
            Button btnDeleteTrack = new Button() { Text = "מחק מסלול" };
            btnDeleteTrack.Click += BtnDeleteTrack_Click;

            // פאנל לכפתורי ניהול מסלולים
            Panel panelTrackButtons = new Panel();
            panelTrackButtons.Height = 40;
            panelTrackButtons.Dock = DockStyle.Top;
            panelTrackButtons.Controls.Add(btnAddTrack);
            panelTrackButtons.Controls.Add(btnEditTrack);
            panelTrackButtons.Controls.Add(btnDeleteTrack);

            // מיקום הכפתורים בתוך הפאנל
            btnAddTrack.Location = new Point(10, 5);
            btnEditTrack.Location = new Point(120, 5);
            btnDeleteTrack.Location = new Point(230, 5);

            // כפתורים לניהול קורסים
            Button btnAddCourse = new Button() { Text = "הוסף קורס" };
            btnAddCourse.Click += BtnAddCourse_Click;
            Button btnEditCourse = new Button() { Text = "ערוך קורס" };
            btnEditCourse.Click += BtnEditCourse_Click;
            Button btnDeleteCourse = new Button() { Text = "מחק קורס" };
            btnDeleteCourse.Click += BtnDeleteCourse_Click;

            // פאנל לכפתורי ניהול קורסים
            Panel panelCourseButtons = new Panel();
            panelCourseButtons.Height = 40;
            panelCourseButtons.Dock = DockStyle.Top;
            panelCourseButtons.Controls.Add(btnAddCourse);
            panelCourseButtons.Controls.Add(btnEditCourse);
            panelCourseButtons.Controls.Add(btnDeleteCourse);

            // מיקום הכפתורים בתוך הפאנל
            btnAddCourse.Location = new Point(10, 5);
            btnEditCourse.Location = new Point(120, 5);

            // קבלת ה-DepartmentID
            int departmentID = DbFunctions.GetDepartmentID(departmentHeadID);

            // שליפת המסלולים
            dgvTracks.DataSource = DbFunctions.GetTracks(departmentID);

            // אירוע לבחירת מסלול
            dgvTracks.SelectionChanged += (s, e) =>
            {
                if (dgvTracks.SelectedRows.Count > 0)
                {
                    int trackID = Convert.ToInt32(dgvTracks.SelectedRows[0].Cells["TrackID"].Value);
                    dgvCourses.DataSource = DbFunctions.GetCourses(trackID);
                }
            };

            // טעינת הקורסים של המסלול הראשון אם קיים
            if (dgvTracks.Rows.Count > 0)
            {
                dgvTracks.Rows[0].Selected = true;
                int trackID = Convert.ToInt32(dgvTracks.Rows[0].Cells["TrackID"].Value);
                dgvCourses.DataSource = DbFunctions.GetCourses(trackID);
            }

            // הוספת הרכיבים ללשונית בסדר הנכון
            tab.Controls.Add(dgvCourses);
            tab.Controls.Add(panelCourseButtons);
            tab.Controls.Add(dgvTracks);
            tab.Controls.Add(panelTrackButtons);
        }

        private void BtnAddTrack_Click(object sender, EventArgs e)
        {
            int departmentID = DbFunctions.GetDepartmentID(departmentHeadID);
            // פתיחת טופס להוספת מסלול חדש
            AddEditTrackForm addTrackForm = new AddEditTrackForm(null, departmentID);
            if (addTrackForm.ShowDialog() == DialogResult.OK)
            {
                // רענון הטבלה לאחר הוספה
                dgvTracks.DataSource = DbFunctions.GetTracks(departmentID);
            }
        }

        private void BtnEditTrack_Click(object sender, EventArgs e)
        {
            if (dgvTracks.SelectedRows.Count > 0)
            {
                int trackID = Convert.ToInt32(dgvTracks.SelectedRows[0].Cells["TrackID"].Value);
                int departmentID = DbFunctions.GetDepartmentID(departmentHeadID);
                // פתיחת טופס לעריכת המסלול
                AddEditTrackForm editTrackForm = new AddEditTrackForm(trackID, departmentID);
                if (editTrackForm.ShowDialog() == DialogResult.OK)
                {
                    // רענון הטבלה לאחר עריכה
                    dgvTracks.DataSource = DbFunctions.GetTracks(departmentID);
                }
            }
            else
            {
                MessageBox.Show("בחר מסלול לעריכה.");
            }
        }

        private void BtnDeleteTrack_Click(object sender, EventArgs e)
        {
            if (dgvTracks.SelectedRows.Count > 0)
            {
                int trackID = Convert.ToInt32(dgvTracks.SelectedRows[0].Cells["TrackID"].Value);
                DialogResult result = MessageBox.Show("האם אתה בטוח שברצונך למחוק את המסלול?", "אישור מחיקה", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    DbFunctions.DeleteTrack(trackID);
                    int departmentID = DbFunctions.GetDepartmentID(departmentHeadID);
                    dgvTracks.DataSource = DbFunctions.GetTracks(departmentID);
                    dgvCourses.DataSource = null; // ניקוי טבלת הקורסים
                }
            }
            else
            {
                MessageBox.Show("בחר מסלול למחיקה.");
            }
        }

        private void BtnAddCourse_Click(object sender, EventArgs e)
        {
            if (dgvTracks.SelectedRows.Count > 0)
            {
                int trackID = Convert.ToInt32(dgvTracks.SelectedRows[0].Cells["TrackID"].Value);
                // פתיחת טופס להוספת קורס חדש
                AddEditCourseForm addCourseForm = new AddEditCourseForm(null, trackID);
                if (addCourseForm.ShowDialog() == DialogResult.OK)
                {
                    dgvCourses.DataSource = DbFunctions.GetCourses(trackID);
                }
            }
            else
            {
                MessageBox.Show("בחר מסלול להוספת קורס.");
            }
        }

        private void BtnEditCourse_Click(object sender, EventArgs e)
        {
            if (dgvCourses.SelectedRows.Count > 0)
            {
                int courseID = Convert.ToInt32(dgvCourses.SelectedRows[0].Cells["CourseID"].Value);
                int trackID = Convert.ToInt32(dgvTracks.SelectedRows[0].Cells["TrackID"].Value);
                // פתיחת טופס לעריכת הקורס
                AddEditCourseForm editCourseForm = new AddEditCourseForm(courseID, trackID);
                if (editCourseForm.ShowDialog() == DialogResult.OK)
                {
                    dgvCourses.DataSource = DbFunctions.GetCourses(trackID);
                }
            }
            else
            {
                MessageBox.Show("בחר קורס לעריכה.");
            }
        }

        private void BtnDeleteCourse_Click(object sender, EventArgs e)
        {
            if (dgvCourses.SelectedRows.Count > 0)
            {
                int courseID = Convert.ToInt32(dgvCourses.SelectedRows[0].Cells["CourseID"].Value);
                int trackID = Convert.ToInt32(dgvTracks.SelectedRows[0].Cells["TrackID"].Value);
                DialogResult result = MessageBox.Show("האם אתה בטוח שברצונך למחוק את הקורס?", "אישור מחיקה", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    DbFunctions.DeleteCourse(courseID, trackID);
                    dgvCourses.DataSource = DbFunctions.GetCourses(trackID);
                }
            }
            else
            {
                MessageBox.Show("בחר קורס למחיקה.");
            }
        }

        private void InitializeLecturersTab(TabPage tab)
        {
            // יצירת TabControl עם שתי לשוניות: מרצים ממתינים ומרצים מאושרים
            TabControl lecturersTabControl = new TabControl();
            lecturersTabControl.Dock = DockStyle.Fill;

            TabPage tabUnapprovedLecturers = new TabPage("מרצים ממתינים");
            TabPage tabApprovedLecturers = new TabPage("מרצים מאושרים");

            // אתחול הלשוניות
            InitializeUnapprovedLecturersTab(tabUnapprovedLecturers);
            InitializeApprovedLecturersTab(tabApprovedLecturers);

            lecturersTabControl.TabPages.Add(tabUnapprovedLecturers);
            lecturersTabControl.TabPages.Add(tabApprovedLecturers);

            tab.Controls.Add(lecturersTabControl);
        }

        private void InitializeUnapprovedLecturersTab(TabPage tab)
        {
            dataGridViewUnapprovedLecturers = new DataGridView();
            dataGridViewUnapprovedLecturers.Dock = DockStyle.Fill;
            dataGridViewUnapprovedLecturers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewUnapprovedLecturers.MultiSelect = false;
            dataGridViewUnapprovedLecturers.AllowUserToAddRows = false;
            dataGridViewUnapprovedLecturers.ReadOnly = true;
            dataGridViewUnapprovedLecturers.AutoGenerateColumns = false;

            // הגדרת עמודות
            dataGridViewUnapprovedLecturers.Columns.Add(new DataGridViewTextBoxColumn() { Name = "AuthID", DataPropertyName = "AuthID", Visible = false });
            dataGridViewUnapprovedLecturers.Columns.Add(new DataGridViewTextBoxColumn() { Name = "LecturerID", DataPropertyName = "LecturerID", Visible = false });
            dataGridViewUnapprovedLecturers.Columns.Add(new DataGridViewTextBoxColumn() { Name = "UserName", HeaderText = "שם משתמש", DataPropertyName = "UserName" });
            dataGridViewUnapprovedLecturers.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Name", HeaderText = "שם מלא", DataPropertyName = "Name" });
            dataGridViewUnapprovedLecturers.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Email", HeaderText = "דוא\"ל", DataPropertyName = "Email" });
            dataGridViewUnapprovedLecturers.Columns.Add(new DataGridViewTextBoxColumn() { Name = "PhoneNumber", HeaderText = "טלפון", DataPropertyName = "PhoneNumber" });

            // הוספת כפתורי אישור ודחייה
            DataGridViewButtonColumn approveButton = new DataGridViewButtonColumn();
            approveButton.Name = "Approve";
            approveButton.HeaderText = "אשר";
            approveButton.Text = "אשר";
            approveButton.UseColumnTextForButtonValue = true;
            dataGridViewUnapprovedLecturers.Columns.Add(approveButton);

            DataGridViewButtonColumn rejectButton = new DataGridViewButtonColumn();
            rejectButton.Name = "Reject";
            rejectButton.HeaderText = "דחה";
            rejectButton.Text = "דחה";
            rejectButton.UseColumnTextForButtonValue = true;
            dataGridViewUnapprovedLecturers.Columns.Add(rejectButton);

            // טעינת הנתונים
            LoadUnapprovedLecturers();

            // אירוע ללחיצה על הכפתורים
            dataGridViewUnapprovedLecturers.CellContentClick += DataGridViewUnapprovedLecturers_CellContentClick;

            tab.Controls.Add(dataGridViewUnapprovedLecturers);
        }

        private void LoadUnapprovedLecturers()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = @"
            SELECT a.AuthID, l.LecturerID, a.UserName, l.Name, l.Email, l.PhoneNumber
            FROM Auth a
            INNER JOIN Lecturers l ON a.AuthID = l.AuthID
            WHERE a.Role = 'Lecturer' AND a.RegistrationStatus = 'Pending'
        ";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridViewUnapprovedLecturers.DataSource = dt;
                }
            }
        }

        private void DataGridViewUnapprovedLecturers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            string columnName = dataGridViewUnapprovedLecturers.Columns[e.ColumnIndex].Name;

            if (columnName == "Approve")
            {
                int authID = Convert.ToInt32(dataGridViewUnapprovedLecturers.Rows[e.RowIndex].Cells["AuthID"].Value);
                ApproveLecturer(authID);
                LoadUnapprovedLecturers();
                LoadApprovedLecturers();
            }
            else if (columnName == "Reject")
            {
                int authID = Convert.ToInt32(dataGridViewUnapprovedLecturers.Rows[e.RowIndex].Cells["AuthID"].Value);
                RejectLecturer(authID);
                LoadUnapprovedLecturers();
                LoadApprovedLecturers();
            }
        }

        private void ApproveLecturer(int authID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "UPDATE Auth SET RegistrationStatus = 'Approved' WHERE AuthID = @AuthID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@AuthID", authID);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("המרצה אושר בהצלחה.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה: " + ex.Message);
            }
        }

        private void RejectLecturer(int authID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "UPDATE Auth SET RegistrationStatus = 'Rejected' WHERE AuthID = @AuthID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@AuthID", authID);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("המרצה נדחה.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה: " + ex.Message);
            }
        }

        private void InitializeApprovedLecturersTab(TabPage tab)
        {
            dataGridViewLecturers = new DataGridView();
            dataGridViewLecturers.Dock = DockStyle.Fill;
            dataGridViewLecturers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewLecturers.MultiSelect = false;
            dataGridViewLecturers.AllowUserToAddRows = false;
            dataGridViewLecturers.ReadOnly = true;
            dataGridViewLecturers.AutoGenerateColumns = false;

            // הגדרת עמודות
            dataGridViewLecturers.Columns.Add(new DataGridViewTextBoxColumn() { Name = "AuthID", DataPropertyName = "AuthID", Visible = false });
            dataGridViewLecturers.Columns.Add(new DataGridViewTextBoxColumn() { Name = "LecturerID", DataPropertyName = "LecturerID", Visible = false });
            dataGridViewLecturers.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Name", HeaderText = "שם מלא", DataPropertyName = "Name" });
            dataGridViewLecturers.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Email", HeaderText = "דוא\"ל", DataPropertyName = "Email" });
            dataGridViewLecturers.Columns.Add(new DataGridViewTextBoxColumn() { Name = "PhoneNumber", HeaderText = "טלפון", DataPropertyName = "PhoneNumber" });
            dataGridViewLecturers.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Courses", HeaderText = "קורסים", DataPropertyName = "Courses" });

            // הוספת כפתורי 'הגדר כממתין' ו-'דחה'
            DataGridViewButtonColumn setPendingButton = new DataGridViewButtonColumn();
            setPendingButton.Name = "SetPending";
            setPendingButton.HeaderText = "הגדר כממתין";
            setPendingButton.Text = "ממתין";
            setPendingButton.UseColumnTextForButtonValue = true;
            dataGridViewLecturers.Columns.Add(setPendingButton);

            DataGridViewButtonColumn rejectButton = new DataGridViewButtonColumn();
            rejectButton.Name = "Reject";
            rejectButton.HeaderText = "דחה";
            rejectButton.Text = "דחה";
            rejectButton.UseColumnTextForButtonValue = true;
            dataGridViewLecturers.Columns.Add(rejectButton);

            // הוספת כפתור 'שיוך קורסים'
            DataGridViewButtonColumn assignCoursesButton = new DataGridViewButtonColumn();
            assignCoursesButton.Name = "AssignCourses";
            assignCoursesButton.HeaderText = "שיוך קורסים";
            assignCoursesButton.Text = "שיוך קורסים";
            assignCoursesButton.UseColumnTextForButtonValue = true;
            dataGridViewLecturers.Columns.Add(assignCoursesButton);

            // טעינת הנתונים
            LoadApprovedLecturers();

            // אירוע ללחיצה על הכפתורים
            dataGridViewLecturers.CellContentClick += DataGridViewLecturers_CellContentClick;

            tab.Controls.Add(dataGridViewLecturers);
        }


        private void LoadApprovedLecturers()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                int departmentID = DbFunctions.GetDepartmentID(departmentHeadID);

                string query = @"
            SELECT 
                l.AuthID,
                l.LecturerID,
                l.Name,
                l.Email,
                l.PhoneNumber,
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
            WHERE a.Role = 'Lecturer' AND a.RegistrationStatus = 'Approved'
            GROUP BY l.AuthID, l.LecturerID, l.Name, l.Email, l.PhoneNumber
        ";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridViewLecturers.DataSource = dt;
                }
            }
        }

        private void DataGridViewLecturers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            string columnName = dataGridViewLecturers.Columns[e.ColumnIndex].Name;
            int authID = Convert.ToInt32(dataGridViewLecturers.Rows[e.RowIndex].Cells["AuthID"].Value);

            if (columnName == "SetPending")
            {
                DbFunctions.SetPendingLecturer(authID);
                LoadApprovedLecturers();
                LoadUnapprovedLecturers();
            }
            else if (columnName == "Reject")
            {
                DialogResult result = MessageBox.Show("האם אתה בטוח שברצונך לדחות את המרצה?", "אישור דחייה", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    RejectLecturer(authID);
                    LoadApprovedLecturers();
                    LoadUnapprovedLecturers();
                }
            }
            else if (columnName == "AssignCourses")
            {
                int lecturerID = Convert.ToInt32(dataGridViewLecturers.Rows[e.RowIndex].Cells["LecturerID"].Value);
                AssignCoursesToLecturer(lecturerID);
            }
        }

        private void AssignCoursesToLecturer(int lecturerID)
        {
            AssignCoursesForm assignCoursesForm = new AssignCoursesForm(lecturerID, departmentHeadID);
            if (assignCoursesForm.ShowDialog() == DialogResult.OK)
            {
                LoadApprovedLecturers();
            }
        }

        private void InitializeStudentsTab(TabPage tab)
        {
            // יצירת TabControl עם שתי לשוניות: סטודנטים ממתינים וסטודנטים מאושרים
            TabControl studentsTabControl = new TabControl();
            studentsTabControl.Dock = DockStyle.Fill;

            TabPage tabUnapprovedStudents = new TabPage("סטודנטים ממתינים");
            TabPage tabApprovedStudents = new TabPage("סטודנטים מאושרים");

            // אתחול הלשוניות
            InitializeUnapprovedStudentsTab(tabUnapprovedStudents);
            InitializeApprovedStudentsTab(tabApprovedStudents);

            studentsTabControl.TabPages.Add(tabUnapprovedStudents);
            studentsTabControl.TabPages.Add(tabApprovedStudents);

            tab.Controls.Add(studentsTabControl);
        }

        private void InitializeUnapprovedStudentsTab(TabPage tab)
        {
            dataGridViewUnapprovedStudents = new DataGridView();
            dataGridViewUnapprovedStudents.Dock = DockStyle.Fill;
            dataGridViewUnapprovedStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewUnapprovedStudents.MultiSelect = false;
            dataGridViewUnapprovedStudents.AllowUserToAddRows = false;
            dataGridViewUnapprovedStudents.ReadOnly = false; // אפשר עריכה ב-DataGridView
            dataGridViewUnapprovedStudents.AutoGenerateColumns = false;

            // הגדרת עמודות
            DataGridViewTextBoxColumn authIDColumn = new DataGridViewTextBoxColumn() { Name = "AuthID", DataPropertyName = "AuthID", Visible = false, ReadOnly = true };
            dataGridViewUnapprovedStudents.Columns.Add(authIDColumn);

            DataGridViewTextBoxColumn studentIDColumn = new DataGridViewTextBoxColumn() { Name = "StudentID", DataPropertyName = "StudentID", Visible = false, ReadOnly = true };
            dataGridViewUnapprovedStudents.Columns.Add(studentIDColumn);

            DataGridViewTextBoxColumn userNameColumn = new DataGridViewTextBoxColumn() { Name = "UserName", HeaderText = "שם משתמש", DataPropertyName = "UserName", ReadOnly = true };
            dataGridViewUnapprovedStudents.Columns.Add(userNameColumn);

            DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn() { Name = "Name", HeaderText = "שם מלא", DataPropertyName = "Name", ReadOnly = true };
            dataGridViewUnapprovedStudents.Columns.Add(nameColumn);

            DataGridViewTextBoxColumn emailColumn = new DataGridViewTextBoxColumn() { Name = "Email", HeaderText = "דוא\"ל", DataPropertyName = "Email", ReadOnly = true };
            dataGridViewUnapprovedStudents.Columns.Add(emailColumn);

            DataGridViewTextBoxColumn phoneNumberColumn = new DataGridViewTextBoxColumn() { Name = "PhoneNumber", HeaderText = "טלפון", DataPropertyName = "PhoneNumber", ReadOnly = true };
            dataGridViewUnapprovedStudents.Columns.Add(phoneNumberColumn);

            // הוספת ComboBox לשיוך מסלול
            DataGridViewComboBoxColumn trackComboBox = new DataGridViewComboBoxColumn();
            trackComboBox.Name = "TrackID";
            trackComboBox.HeaderText = "מסלול";
            trackComboBox.DataPropertyName = "TrackID";
            trackComboBox.DisplayMember = "TrackName";
            trackComboBox.ValueMember = "TrackID";
            trackComboBox.DataSource = DbFunctions.GetAllTracks();
            trackComboBox.ReadOnly = false; // אפשר עריכה בעמודה זו
            dataGridViewUnapprovedStudents.Columns.Add(trackComboBox);

            // הוספת כפתורי אישור ודחייה
            DataGridViewButtonColumn approveButton = new DataGridViewButtonColumn();
            approveButton.Name = "Approve";
            approveButton.HeaderText = "אשר";
            approveButton.Text = "אשר";
            approveButton.UseColumnTextForButtonValue = true;
            approveButton.ReadOnly = true; // כפתור לא ניתן לעריכה
            dataGridViewUnapprovedStudents.Columns.Add(approveButton);

            DataGridViewButtonColumn rejectButton = new DataGridViewButtonColumn();
            rejectButton.Name = "Reject";
            rejectButton.HeaderText = "דחה";
            rejectButton.Text = "דחה";
            rejectButton.UseColumnTextForButtonValue = true;
            rejectButton.ReadOnly = true; // כפתור לא ניתן לעריכה
            dataGridViewUnapprovedStudents.Columns.Add(rejectButton);

            // טעינת הנתונים
            LoadUnapprovedStudents();

            // אירוע ללחיצה על הכפתורים
            dataGridViewUnapprovedStudents.CellContentClick += DataGridViewUnapprovedStudents_CellContentClick;

            tab.Controls.Add(dataGridViewUnapprovedStudents);
        }


        private void LoadUnapprovedStudents()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = @"
            SELECT 
                a.AuthID, 
                s.StudentID, 
                a.UserName, 
                s.Name, 
                s.Email, 
                s.PhoneNumber,
                s.TrackID
            FROM Auth a
            INNER JOIN Students s ON a.AuthID = s.AuthID
            WHERE a.Role = 'Student' AND a.RegistrationStatus = 'Pending'
        ";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridViewUnapprovedStudents.DataSource = dt;
                }
            }
        }

        private void DataGridViewUnapprovedStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            string columnName = dataGridViewUnapprovedStudents.Columns[e.ColumnIndex].Name;

            if (columnName == "Approve")
            {
                int authID = Convert.ToInt32(dataGridViewUnapprovedStudents.Rows[e.RowIndex].Cells["AuthID"].Value);

                // קבלת הערך של TrackID
                object trackIDValue = dataGridViewUnapprovedStudents.Rows[e.RowIndex].Cells["TrackID"].Value;

                // בדיקה אם הערך הוא null או DBNull
                if (trackIDValue == null || trackIDValue == DBNull.Value)
                {
                    MessageBox.Show("יש לבחור מסלול לפני אישור הסטודנט.");
                    return;
                }

                int trackID = Convert.ToInt32(trackIDValue);

                // המשך הקוד...
                ApproveStudent(authID, trackID);
                LoadUnapprovedStudents();
                LoadApprovedStudents();
            }
            else if (columnName == "Reject")
            {
                int authID = Convert.ToInt32(dataGridViewUnapprovedStudents.Rows[e.RowIndex].Cells["AuthID"].Value);
                RejectStudent(authID);
                LoadUnapprovedStudents();
                LoadApprovedStudents();
            }
        }


        private void ApproveStudent(int authID, int trackID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    // עדכון סטטוס והרשמת המסלול
                    string query = @"
                UPDATE Auth SET RegistrationStatus = 'Approved' WHERE AuthID = @AuthID;
                UPDATE Students SET TrackID = @TrackID WHERE AuthID = @AuthID;
            ";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@AuthID", authID);
                        cmd.Parameters.AddWithValue("@TrackID", trackID);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("הסטודנט אושר בהצלחה.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה: " + ex.Message);
            }
        }

        private void RejectStudent(int authID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "UPDATE Auth SET RegistrationStatus = 'Rejected' WHERE AuthID = @AuthID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@AuthID", authID);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("הסטודנט נדחה.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה: " + ex.Message);
            }
        }

        private void InitializeApprovedStudentsTab(TabPage tab)
        {
            dataGridViewApprovedStudents = new DataGridView();
            dataGridViewApprovedStudents.Dock = DockStyle.Fill;
            dataGridViewApprovedStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewApprovedStudents.MultiSelect = false;
            dataGridViewApprovedStudents.AllowUserToAddRows = false;
            dataGridViewApprovedStudents.ReadOnly = true;
            dataGridViewApprovedStudents.AutoGenerateColumns = false;

            // הגדרת עמודות
            dataGridViewApprovedStudents.Columns.Add(new DataGridViewTextBoxColumn() { Name = "AuthID", DataPropertyName = "AuthID", Visible = false });
            dataGridViewApprovedStudents.Columns.Add(new DataGridViewTextBoxColumn() { Name = "StudentID", DataPropertyName = "StudentID", Visible = false });
            dataGridViewApprovedStudents.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Name", HeaderText = "שם מלא", DataPropertyName = "Name" });
            dataGridViewApprovedStudents.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Email", HeaderText = "דוא\"ל", DataPropertyName = "Email" });
            dataGridViewApprovedStudents.Columns.Add(new DataGridViewTextBoxColumn() { Name = "PhoneNumber", HeaderText = "טלפון", DataPropertyName = "PhoneNumber" });
            dataGridViewApprovedStudents.Columns.Add(new DataGridViewTextBoxColumn() { Name = "TrackName", HeaderText = "מסלול", DataPropertyName = "TrackName" });
            dataGridViewApprovedStudents.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Courses", HeaderText = "קורסים", DataPropertyName = "Courses" });

            // הוספת כפתורי 'הגדר כממתין' ו-'דחה'
            DataGridViewButtonColumn setPendingButton = new DataGridViewButtonColumn();
            setPendingButton.Name = "SetPending";
            setPendingButton.HeaderText = "הגדר כממתין";
            setPendingButton.Text = "ממתין";
            setPendingButton.UseColumnTextForButtonValue = true;
            dataGridViewApprovedStudents.Columns.Add(setPendingButton);

            DataGridViewButtonColumn rejectButton = new DataGridViewButtonColumn();
            rejectButton.Name = "Reject";
            rejectButton.HeaderText = "דחה";
            rejectButton.Text = "דחה";
            rejectButton.UseColumnTextForButtonValue = true;
            dataGridViewApprovedStudents.Columns.Add(rejectButton);

            // הוספת כפתור 'שיוך קורסים'
            DataGridViewButtonColumn assignCoursesButton = new DataGridViewButtonColumn();
            assignCoursesButton.Name = "AssignCourses";
            assignCoursesButton.HeaderText = "שיוך קורסים";
            assignCoursesButton.Text = "שיוך קורסים";
            assignCoursesButton.UseColumnTextForButtonValue = true;
            dataGridViewApprovedStudents.Columns.Add(assignCoursesButton);

            // טעינת הנתונים
            LoadApprovedStudents();

            // אירוע ללחיצה על הכפתורים
            dataGridViewApprovedStudents.CellContentClick += DataGridViewApprovedStudents_CellContentClick;

            tab.Controls.Add(dataGridViewApprovedStudents);
        }

        private void LoadApprovedStudents()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                int departmentID = DbFunctions.GetDepartmentID(departmentHeadID);

                string query = @"
            SELECT 
                a.AuthID,
                s.StudentID,
                s.Name,
                s.Email,
                s.PhoneNumber,
                t.TrackName,
                STUFF((
                    SELECT ', ' + c.CourseName
                    FROM StudentCourses sc
                    INNER JOIN Courses c ON sc.CourseID = c.CourseID
                    WHERE sc.StudentID = s.StudentID
                    FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS Courses
            FROM Students s
            INNER JOIN Auth a ON s.AuthID = a.AuthID
            INNER JOIN Tracks t ON s.TrackID = t.TrackID
            WHERE a.Role = 'Student' AND a.RegistrationStatus = 'Approved' AND t.DepartmentID = @DepartmentID
        ";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridViewApprovedStudents.DataSource = dt;
                }
            }
        }

        private void DataGridViewApprovedStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            string columnName = dataGridViewApprovedStudents.Columns[e.ColumnIndex].Name;
            int authID = Convert.ToInt32(dataGridViewApprovedStudents.Rows[e.RowIndex].Cells["AuthID"].Value);
            int studentID = Convert.ToInt32(dataGridViewApprovedStudents.Rows[e.RowIndex].Cells["StudentID"].Value);

            if (columnName == "SetPending")
            {
                DbFunctions.SetPendingStudent(authID);
                LoadApprovedStudents();
                LoadUnapprovedStudents();
            }
            else if (columnName == "Reject")
            {
                DialogResult result = MessageBox.Show("האם אתה בטוח שברצונך לדחות את הסטודנט?", "אישור דחייה", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    DbFunctions.RejectStudent(authID);
                    LoadApprovedStudents();
                    LoadUnapprovedStudents();
                }
            }
            else if (columnName == "AssignCourses")
            {
                AssignCoursesToStudent(studentID);
            }
        }

        private void AssignCoursesToStudent(int studentID)
        {
            AssignCoursesToStudentForm assignCoursesForm = new AssignCoursesToStudentForm(studentID, departmentHeadID);
            if (assignCoursesForm.ShowDialog() == DialogResult.OK)
            {
                LoadApprovedStudents();
            }
        }

    }
}
