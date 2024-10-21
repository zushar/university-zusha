using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using UniversityZusha.dbFunctions;
using UniversityZusha.messageFuncions;
using UniversityZusha.PersonalInfoFunctions;

namespace UniversityZusha.forms
{
    public partial class Student : Form
    {
        private int StudentId;
        private Login loginForm;
        private bool isLoggingOut = false;
        private int previousTabIndex = 0;
        public Student(int studentId, Login loginForm)
        {
            InitializeComponent();
            StudentId = studentId;
            this.loginForm = loginForm;
            this.FormClosing += new FormClosingEventHandler(Student_FormClosing);
            InitializeTabs();
        }
        private void Student_FormClosing(object sender, FormClosingEventArgs e)
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

            TabPage tabMessages = new TabPage("הודעות");
            MessageFuncions messageFuncions = new MessageFuncions(StudentId);
            messageFuncions.InitializeMessagesTab(tabMessages);
            tabControl.TabPages.Add(tabMessages);

            TabPage tabPersonalInfo = new TabPage("מידע אישי");
            InitializePersonalInfoTab(tabPersonalInfo);
            tabControl.TabPages.Add(tabPersonalInfo);

            TabPage tabCourses = new TabPage("קורסים");
            InitializCoursesTab(tabCourses);
            tabControl.TabPages.Add(tabCourses);

            TabPage tabLogout = new TabPage("התנתק");
            InitializeLogoutTab(tabLogout);
            tabControl.TabPages.Add(tabLogout);

            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;

            this.Controls.Add(tabControl);
        }

        private void InitializePersonalInfoTab(TabPage tab)
        {
            // Initialize the personal info tab with existing personal info
            PersonalInfo.InitializePersonalInfoTab(tab, StudentId, "Student");

            // Add labels for CurrentCredits and TotalCredits
            Label lblCurrentCredits = new Label() { Text = "נקודות זכות נוכחיות:", Location = new Point(20, 220) };
            Label lblTotalCredits = new Label() { Text = "סה\"כ נקודות זכות:", Location = new Point(20, 260) };
            Label lblCurrentCreditsValue = new Label() { Location = new Point(150, 220), Width = 100 };
            Label lblTotalCreditsValue = new Label() { Location = new Point(150, 260), Width = 100 };

            // Retrieve the current credits and total credits for the student
            var studentInfo = DbFunctions.GetStudentInfo(StudentId); // This function should retrieve Student's CurrentCredits and TotalCredits
            lblCurrentCreditsValue.Text = studentInfo.CurrentCredits.ToString();
            lblTotalCreditsValue.Text = studentInfo.TotalCredits.ToString();

            // Add labels to the tab
            tab.Controls.Add(lblCurrentCredits);
            tab.Controls.Add(lblCurrentCreditsValue);
            tab.Controls.Add(lblTotalCredits);
            tab.Controls.Add(lblTotalCreditsValue);
        }

        private void InitializCoursesTab(TabPage tab)
        {
            // טבלת קורסים
            DataGridView dgvCourses = new DataGridView();
            dgvCourses.Dock = DockStyle.Fill;
            dgvCourses.AutoGenerateColumns = false;
            dgvCourses.AllowUserToAddRows = false;
            dgvCourses.AllowUserToDeleteRows = false;
            dgvCourses.AllowUserToOrderColumns = false;
            dgvCourses.AllowUserToResizeColumns = false;
            dgvCourses.AllowUserToResizeRows = false;
            dgvCourses.ReadOnly = false; // Set to false to allow editing in the rating column

            // הוספת עמודות לטבלה
            dgvCourses.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "CourseName",
                HeaderText = "שם קורס",
                Width = 200
            });
            dgvCourses.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Credits",
                HeaderText = "נקודות זכות",
                Width = 100
            });
            dgvCourses.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "LecturerName",
                HeaderText = "שם מרצה",
                Width = 200
            });

            // Hidden column for LecturerID (make sure this has the Name property)
            dgvCourses.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "LecturerID",
                HeaderText = "LecturerID",
                Visible = false,
                Name = "LecturerID"  // Set the name explicitly
            });

            // הוספת עמודת דירוג מרצה (כוכבים)
            DataGridViewComboBoxColumn ratingColumn = new DataGridViewComboBoxColumn();
            ratingColumn.HeaderText = "דרג מרצה";
            ratingColumn.Items.AddRange(1, 2, 3, 4, 5); // Allow students to select a rating between 1 and 5
            ratingColumn.Name = "Rating"; // Set the name of the rating column
            dgvCourses.Columns.Add(ratingColumn);

            // טעינת הנתונים לטבלה
            dgvCourses.DataSource = DbFunctions.GetStudentCourses(StudentId);

            // Set default value for the rating column to 5
            foreach (DataGridViewRow row in dgvCourses.Rows)
            {
                row.Cells["Rating"].Value = 5; // Default rating is 5
            }

            // הוספת הטבלה ללשונית
            tab.Controls.Add(dgvCourses);

            // הוספת כפתור לשליחת דירוגים
            Button btnSubmitRatings = new Button() { Text = "שלח דירוגים", Dock = DockStyle.Bottom };
            btnSubmitRatings.Click += (sender, e) => SubmitRatings(dgvCourses);
            tab.Controls.Add(btnSubmitRatings);
        }


        private void dgvCourses_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception != null)
            {
                MessageBox.Show("An error occurred: " + e.Exception.Message);
                e.ThrowException = false; // Prevent the crash
            }
        }

        private void SubmitRatings(DataGridView dgvCourses)
        {
            foreach (DataGridViewRow row in dgvCourses.Rows)
            {
                // Get lecturer ID and rating from the grid using the column name "LecturerID"
                int lecturerID = Convert.ToInt32(row.Cells["LecturerID"].Value); // Ensure this matches the exact column name

                if (row.Cells["Rating"].Value != null) // Assuming "Rating" is the rating column
                {
                    int rating = Convert.ToInt32(row.Cells["Rating"].Value);

                    // Save the rating to the database
                    DbFunctions.SaveLecturerRating(lecturerID, rating);
                }
            }

            MessageBox.Show("הדירוגים נשמרו בהצלחה.");
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
    }
}
