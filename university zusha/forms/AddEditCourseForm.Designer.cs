using System.Drawing;
using System.Windows.Forms;

namespace UniversityZusha.forms
{
    partial class AddEditCourseForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        // רכיבי הממשק
        private Label labelCourseName;
        private TextBox textBoxCourseName;
        private Label labelCredits;
        private NumericUpDown numericUpDownCredits;
        private Button btnSave;

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
            this.Text = courseID.HasValue ? "עריכת קורס" : "הוספת קורס";
            this.Size = new Size(400, 200);
            this.StartPosition = FormStartPosition.CenterParent;

            // יצירת הרכיבים
            labelCourseName = new Label() { Text = "שם קורס:", Location = new Point(20, 20), AutoSize = true };
            textBoxCourseName = new TextBox() { Location = new Point(120, 20), Width = 200 };

            labelCredits = new Label() { Text = "נקודות זכות:", Location = new Point(20, 60), AutoSize = true };
            numericUpDownCredits = new NumericUpDown() { Location = new Point(120, 60), Width = 200, Minimum = 0, Maximum = 20 };

            btnSave = new Button() { Text = "שמור", Location = new Point(250, 100), Width = 70 };
            btnSave.Click += btnSave_Click;

            // הוספת הרכיבים לטופס
            this.Controls.Add(labelCourseName);
            this.Controls.Add(textBoxCourseName);
            this.Controls.Add(labelCredits);
            this.Controls.Add(numericUpDownCredits);
            this.Controls.Add(btnSave);
        }

        #endregion
    }
}