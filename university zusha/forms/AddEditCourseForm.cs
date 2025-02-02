﻿using System;
using System.Data;
using System.Windows.Forms;
using UniversityZusha.dbFunctions;

namespace UniversityZusha.forms
{
    public partial class AddEditCourseForm : Form
    {
        private int? courseID;
        private int trackID;

        public AddEditCourseForm(int? courseID, int trackID)
        {
            InitializeComponent();
            this.courseID = courseID;
            this.trackID = trackID;

            if (courseID.HasValue)
            {
                // מצב עריכה - טען את פרטי הקורס
                LoadCourseDetails(courseID.Value);
            }

            // טעינת רשימת המרצים אם יש צורך
            // LoadLecturers();
        }

        private void LoadCourseDetails(int courseID)
        {
            DataTable courseDetails = DbFunctions.GetCourseByID(courseID);
            if (courseDetails.Rows.Count > 0)
            {
                DataRow row = courseDetails.Rows[0];
                textBoxCourseName.Text = row["CourseName"].ToString();
                numericUpDownCredits.Value = Convert.ToInt32(row["Credits"]);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxCourseName.Text))
            {
                MessageBox.Show("הכנס שם קורס.");
                return;
            }

            if (courseID.HasValue)
            {
                // Update existing course
                DbFunctions.UpdateCourse(courseID.Value, textBoxCourseName.Text, (int)numericUpDownCredits.Value);
                DbFunctions.UpdateTrackTotalCredits(trackID);
            }
            else
            {
                // Insert new course
                int newCourseID = DbFunctions.InsertCourse(textBoxCourseName.Text, (int)numericUpDownCredits.Value);
                // Link course to track
                DbFunctions.InsertTrackCourse(trackID, newCourseID);
                DbFunctions.UpdateTrackTotalCredits(trackID);
            }

            // Update the TotalCredits for all students in the updated track
            DbFunctions.UpdateStudentsTotalCreditsByTrack(trackID);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
