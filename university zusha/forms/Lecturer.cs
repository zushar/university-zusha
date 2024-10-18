using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniversityZusha.messageFuncions;
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

        private void Lecturer_FormClosing(object sender, FormClosingEventArgs e)
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
            MessageFuncions messageFuncions = new MessageFuncions(LecturerId);
            messageFuncions.InitializeMessagesTab(tabMessages);
            tabControl.TabPages.Add(tabMessages);

            TabPage tabPersonalInfo = new TabPage("מידע אישי");
            PersonalInfo.InitializePersonalInfoTab(tabPersonalInfo, LecturerId, "Lecturer");
            tabControl.TabPages.Add(tabPersonalInfo);

            TabPage tabLogout = new TabPage("התנתק");
            InitializeLogoutTab(tabLogout);
            tabControl.TabPages.Add(tabLogout);

            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;

            this.Controls.Add(tabControl);
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
