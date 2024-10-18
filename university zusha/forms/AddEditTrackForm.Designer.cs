using System.Drawing;
using System.Windows.Forms;

namespace UniversityZusha.forms
{
    partial class AddEditTrackForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        // רכיבי הממשק
        private Label labelTrackName;
        private TextBox textBoxTrackName;
        private Label labelTotalCredits;
        private NumericUpDown numericUpDownTotalCredits;
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
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "AddEditTrackForm";

            // הגדרות כלליות לטופס
            this.Text = trackID.HasValue ? "עריכת מסלול" : "הוספת מסלול";
            this.Size = new Size(400, 250);
            this.StartPosition = FormStartPosition.CenterParent;

            // יצירת הרכיבים
            labelTrackName = new Label() { Text = "שם מסלול:", Location = new Point(20, 20), AutoSize = true };
            textBoxTrackName = new TextBox() { Location = new Point(120, 20), Width = 200 };

            labelTotalCredits = new Label() { Text = "סך נקודות זכות:", Location = new Point(20, 60), AutoSize = true };
            numericUpDownTotalCredits = new NumericUpDown() { Location = new Point(120, 60), Width = 200, Minimum = 0, Maximum = 500 };

            btnSave = new Button() { Text = "שמור", Location = new Point(250, 100), Width = 70 };
            btnSave.Click += btnSave_Click;

            // הוספת הרכיבים לטופס
            this.Controls.Add(labelTrackName);
            this.Controls.Add(textBoxTrackName);
            this.Controls.Add(labelTotalCredits);
            this.Controls.Add(numericUpDownTotalCredits);
            this.Controls.Add(btnSave);
        }

        #endregion
    }
}