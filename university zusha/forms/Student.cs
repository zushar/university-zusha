using System;
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

        //todo: add CurrentCredits and TotalCredits to the personal info tab
        private void InitializePersonalInfoTab(TabPage tab)
        {
            PersonalInfo.InitializePersonalInfoTab(tab, StudentId, "Lecturer");

            // הוספת שדות להתמחות וכוכבים
            Label lblSpecialization = new Label() { Text = "התמחות:", Location = new Point(20, 220) };
            TextBox txtSpecialization = new TextBox() { Location = new Point(150, 220), Width = 200 };

            Button btnUpdateSpecialization = new Button() { Text = "עדכן התמחות", Location = new Point(150, 300) };
            btnUpdateSpecialization.Click += (sender, e) => DbFunctions.UpdateLecturerSpecialization(StudentId, txtSpecialization.Text);

            tab.Controls.Add(lblSpecialization);
            tab.Controls.Add(txtSpecialization);
            tab.Controls.Add(btnUpdateSpecialization);

            // טעינת ערכים נוכחיים
            var lecturerInfo = DbFunctions.GetLecturerInfo(StudentId);
            txtSpecialization.Text = lecturerInfo.Specialization ?? string.Empty;
        }

        //Todo: add giving stars to lecturers
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
            dgvCourses.ReadOnly = true;

            // הוספת עמודות לטבלה
            dgvCourses.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "CourseName", HeaderText = "שם קורס", Width = 200 });
            dgvCourses.Columns.Add(new DataGridViewColumn() { DataPropertyName = "Credits", HeaderText = "נקודות זכות", Width = 100 });
            dgvCourses.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "LecturerName", HeaderText = "שם מרצה", Width = 200 });

            // טעינת הנתונים לטבלה
            dgvCourses.DataSource = DbFunctions.GetStudentCourses(StudentId);

            // הוספת הטבלה ללשונית
            tab.Controls.Add(dgvCourses);
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
