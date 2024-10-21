using System;
using System.Data;
using System.Windows.Forms;
using UniversityZusha.dbFunctions;

namespace UniversityZusha.forms
{
    public partial class AddEditTrackForm : Form
    {
        private int? trackID;
        private int departmentID;

        

        public AddEditTrackForm(int? trackID, int departmentID)
        {
            InitializeComponent();
            this.trackID = trackID;
            this.departmentID = departmentID;

            if (trackID.HasValue)
            {
                // מצב עריכה - טען את פרטי המסלול
                LoadTrackDetails(trackID.Value);
            }
        }

        

        private void LoadTrackDetails(int trackID)
        {
            DataTable trackDetails = DbFunctions.GetTrackByID(trackID);
            if (trackDetails.Rows.Count > 0)
            {
                DataRow row = trackDetails.Rows[0];
                textBoxTrackName.Text = row["TrackName"].ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxTrackName.Text))
            {
                MessageBox.Show("הכנס שם מסלול.");
                return;
            }

            if (trackID.HasValue)
            {
                // if textBoxTrackName.Text deddnt change - dont update
                if (textBoxTrackName.Text == DbFunctions.GetTrackNameByID(trackID.Value))
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }
                // עדכון מסלול קיים
                DbFunctions.UpdateTrack(trackID.Value, textBoxTrackName.Text);
            }
            else
            {
                // הוספת מסלול חדש
                DbFunctions.InsertTrack(textBoxTrackName.Text, departmentID);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}