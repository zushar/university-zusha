using System.Drawing;
using System.Windows.Forms;

namespace UniversityZusha.forms
{
    partial class AssignCoursesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Text = "שיוך קורסים למרצה";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterParent;

            dataGridViewCourses = new DataGridView();
            dataGridViewCourses.Dock = DockStyle.Top;
            dataGridViewCourses.Height = 300;
            dataGridViewCourses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCourses.MultiSelect = false;
            dataGridViewCourses.AllowUserToAddRows = false;
            dataGridViewCourses.AutoGenerateColumns = false;

            dataGridViewCourses.Columns.Add(new DataGridViewTextBoxColumn() { Name = "CourseID", DataPropertyName = "CourseID", Visible = false });
            dataGridViewCourses.Columns.Add(new DataGridViewTextBoxColumn() { Name = "CourseName", HeaderText = "שם קורס", DataPropertyName = "CourseName" });

            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.Name = "IsAssigned";
            checkBoxColumn.HeaderText = "משויך";
            checkBoxColumn.DataPropertyName = "IsAssigned";
            dataGridViewCourses.Columns.Add(checkBoxColumn);

            btnSave = new Button() { Text = "שמור", Location = new Point(500, 320), Width = 70 };
            btnSave.Click += btnSave_Click;

            this.Controls.Add(dataGridViewCourses);
            this.Controls.Add(btnSave);
        }

        #endregion
    }
}