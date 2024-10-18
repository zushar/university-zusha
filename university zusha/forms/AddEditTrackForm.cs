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
                numericUpDownTotalCredits.Value = Convert.ToInt32(row["TotalCredits"]);
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
                // עדכון מסלול קיים
                DbFunctions.UpdateTrack(trackID.Value, textBoxTrackName.Text, (int)numericUpDownTotalCredits.Value);
            }
            else
            {
                // הוספת מסלול חדש
                DbFunctions.InsertTrack(textBoxTrackName.Text, departmentID, (int)numericUpDownTotalCredits.Value);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}