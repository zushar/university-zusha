using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using UniversityZusha.messageFuncions;
using UniversityZusha.dbFunctions;
using UniversityZusha.PersonalInfoFunctions;
using UniversityZusha.users;

namespace UniversityZusha.forms
{
    public partial class Lecturer : Form
    {
        private int LecturerId;
        private Login loginForm;
        private bool isLoggingOut = false;
        private int previousTabIndex = 0;

        public Lecturer(int lecturerId, Login loginForm)
        {
            InitializeComponent();
            LecturerId = lecturerId;
            this.loginForm = loginForm;
            this.FormClosing += new FormClosingEventHandler(Lecturer_FormClosing);
            InitializeTabs();
        }

        private void InitializeTabs()
        {
            TabControl tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;

            // מידע אישי
            TabPage tabPersonalInfo = new TabPage("מידע אישי");
            InitializePersonalInfoTab(tabPersonalInfo);
            tabControl.TabPages.Add(tabPersonalInfo);

            // קורסים
            TabPage tabCourses = new TabPage("קורסים");
            InitializeCoursesTab(tabCourses);
            tabControl.TabPages.Add(tabCourses);

            // סטודנטים
            TabPage tabStudents = new TabPage("סטודנטים");
            InitializeStudentsTab(tabStudents);
            tabControl.TabPages.Add(tabStudents);

            // הודעות
            TabPage tabMessages = new TabPage("הודעות");
            MessageFuncions messageFuncions = new MessageFuncions(LecturerId);
            messageFuncions.InitializeMessagesTab(tabMessages);
            tabControl.TabPages.Add(tabMessages);

            // התנתקות
            TabPage tabLogout = new TabPage("התנתק");
            InitializeLogoutTab(tabLogout);
            tabControl.TabPages.Add(tabLogout);

            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
            this.Controls.Add(tabControl);
        }

        private void InitializePersonalInfoTab(TabPage tab)
        {
            PersonalInfo.InitializePersonalInfoTab(tab, LecturerId, "Lecturer");

            // הוספת שדות להתמחות וכוכבים
            Label lblSpecialization = new Label() { Text = "התמחות:", Location = new Point(20, 220) };
            TextBox txtSpecialization = new TextBox() { Location = new Point(150, 220), Width = 200 };
            Label lblStars = new Label() { Text = "כוכבים:", Location = new Point(20, 260) };
            Label lblStarsValue = new Label() { Location = new Point(150, 260), Width = 200 };

            Button btnUpdateSpecialization = new Button() { Text = "עדכן התמחות", Location = new Point(150, 300) };
            btnUpdateSpecialization.Click += (sender, e) => DbFunctions.UpdateLecturerSpecialization(LecturerId, txtSpecialization.Text);

            tab.Controls.Add(lblSpecialization);
            tab.Controls.Add(txtSpecialization);
            tab.Controls.Add(lblStars);
            tab.Controls.Add(lblStarsValue);
            tab.Controls.Add(btnUpdateSpecialization);

            // טעינת ערכים נוכחיים
            var lecturerInfo = DbFunctions.GetLecturerInfo(LecturerId);
            txtSpecialization.Text = lecturerInfo.Specialization ?? string.Empty;
            lblStarsValue.Text = lecturerInfo.AverageStars.ToString("F2");
        }

        private void InitializeCoursesTab(TabPage tab)
        {
            DataGridView dgvCourses = new DataGridView();
            dgvCourses.Dock = DockStyle.Fill;
            dgvCourses.AutoGenerateColumns = false;

            dgvCourses.Columns.Add(new DataGridViewTextBoxColumn() { Name = "CourseName", HeaderText = "שם הקורס", DataPropertyName = "CourseName" });
            dgvCourses.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Credits", HeaderText = "נקודות זכות", DataPropertyName = "Credits" });

            // טעינת הקורסים של המרצה
            dgvCourses.DataSource = DbFunctions.GetLecturerCourses(LecturerId);

            tab.Controls.Add(dgvCourses);
        }

        //Dodo: add the ability to grade students and update their grades in StudentCourses table and update currentCredits in Students table bast on courses credits
        private void InitializeStudentsTab(TabPage tab)
        {
            DataGridView dgvStudents = new DataGridView();
            dgvStudents.Dock = DockStyle.Fill;
            dgvStudents.AutoGenerateColumns = false;

            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn() { Name = "StudentName", HeaderText = "שם הסטודנט", DataPropertyName = "StudentName" });
            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn() { Name = "CourseName", HeaderText = "שם הקורס", DataPropertyName = "CourseName" });

            // טעינת הסטודנטים הרשומים לקורסים של המרצה
            dgvStudents.DataSource = DbFunctions.GetLecturerStudents(LecturerId);

            tab.Controls.Add(dgvStudents);
        }

        private void InitializeLogoutTab(TabPage tab)
        {
            Button logoutButton = new Button();
            logoutButton.Text = "התנתק";
            logoutButton.AutoSize = true;
            logoutButton.Location = new Point(20, 20);
            logoutButton.Click += LogoutButton_Click;
            tab.Controls.Add(logoutButton);
        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            isLoggingOut = true;
            loginForm.ResetForm();
            loginForm.Show();
            this.Close();
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            if (tabControl.SelectedTab.Text == "התנתק")
            {
                DialogResult result = MessageBox.Show("האם אתה בטוח שברצונך להתנתק?", "אישור התנתקות", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    isLoggingOut = true;
                    loginForm.ResetForm();
                    loginForm.Show();
                    this.Close();
                }
                else
                {
                    tabControl.SelectedIndex = previousTabIndex;
                }
            }
            else
            {
                previousTabIndex = tabControl.SelectedIndex;
            }
        }

        private void Lecturer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isLoggingOut)
            {
                Application.Exit();
            }
        }
    }
}