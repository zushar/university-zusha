namespace UniversityZusha.forms
{
    partial class SignIn
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
            this.panelImag = new System.Windows.Forms.Panel();
            this.panelLecturer = new System.Windows.Forms.Panel();
            this.buttonUpLodeImighLecture = new System.Windows.Forms.Button();
            this.textBoxLecturerPhone = new System.Windows.Forms.TextBox();
            this.textBoxLecturerEmail = new System.Windows.Forms.TextBox();
            this.textBoxLecturerName = new System.Windows.Forms.TextBox();
            this.dateTimePickerLecturerDateOfBirth = new System.Windows.Forms.DateTimePicker();
            this.labelLecturerEmail = new System.Windows.Forms.Label();
            this.labelLecturerPhone = new System.Windows.Forms.Label();
            this.labelLecturerName = new System.Windows.Forms.Label();
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelTop = new System.Windows.Forms.Label();
            this.panelDepartmentHead = new System.Windows.Forms.Panel();
            this.buttonDepartmentHeadImighUpLode = new System.Windows.Forms.Button();
            this.textBoxDepartmentHeadPhone = new System.Windows.Forms.TextBox();
            this.textBoxDepartmentHeadEmail = new System.Windows.Forms.TextBox();
            this.textBoxDepartmentHeadName = new System.Windows.Forms.TextBox();
            this.dateTimePickerDepartmentHeadDateOfBirth = new System.Windows.Forms.DateTimePicker();
            this.labelDepartmentHeadEmail = new System.Windows.Forms.Label();
            this.labelDepartmentHeadPhone = new System.Windows.Forms.Label();
            this.labelDepartmentHeadName = new System.Windows.Forms.Label();
            this.panelStudent = new System.Windows.Forms.Panel();
            this.buttonUploadStudentImage = new System.Windows.Forms.Button();
            this.textBoxStudentPhone = new System.Windows.Forms.TextBox();
            this.textBoxStudentEmail = new System.Windows.Forms.TextBox();
            this.textBoxStudentName = new System.Windows.Forms.TextBox();
            this.dateTimePickerStudentDateOfBirth = new System.Windows.Forms.DateTimePicker();
            this.labelStudentEmail = new System.Windows.Forms.Label();
            this.labelStudentPhone = new System.Windows.Forms.Label();
            this.labelStudentName = new System.Windows.Forms.Label();
            this.buttonAsUser = new System.Windows.Forms.Button();
            this.buttonSignIn = new System.Windows.Forms.Button();
            this.labelUserId = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelPasswordCn = new System.Windows.Forms.Label();
            this.comboBoxRole = new System.Windows.Forms.ComboBox();
            this.textBoxUserId = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxPasswordCon = new System.Windows.Forms.TextBox();
            this.fillByToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.panelLecturer.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panelDepartmentHead.SuspendLayout();
            this.panelStudent.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelImag
            // 
            this.panelImag.BackgroundImage = global::UniversityZusha.Properties.Resources.loginPhoto;
            this.panelImag.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelImag.Location = new System.Drawing.Point(0, 80);
            this.panelImag.Name = "panelImag";
            this.panelImag.Size = new System.Drawing.Size(230, 380);
            this.panelImag.TabIndex = 4;
            // 
            // panelLecturer
            // 
            this.panelLecturer.Controls.Add(this.buttonUpLodeImighLecture);
            this.panelLecturer.Controls.Add(this.textBoxLecturerPhone);
            this.panelLecturer.Controls.Add(this.textBoxLecturerEmail);
            this.panelLecturer.Controls.Add(this.textBoxLecturerName);
            this.panelLecturer.Controls.Add(this.dateTimePickerLecturerDateOfBirth);
            this.panelLecturer.Controls.Add(this.labelLecturerEmail);
            this.panelLecturer.Controls.Add(this.labelLecturerPhone);
            this.panelLecturer.Controls.Add(this.labelLecturerName);
            this.panelLecturer.Location = new System.Drawing.Point(399, 90);
            this.panelLecturer.Name = "panelLecturer";
            this.panelLecturer.Size = new System.Drawing.Size(380, 319);
            this.panelLecturer.TabIndex = 21;
            this.panelLecturer.Visible = false;
            // 
            // buttonUpLodeImighLecture
            // 
            this.buttonUpLodeImighLecture.BackColor = System.Drawing.Color.Cyan;
            this.buttonUpLodeImighLecture.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonUpLodeImighLecture.Location = new System.Drawing.Point(216, 199);
            this.buttonUpLodeImighLecture.Name = "buttonUpLodeImighLecture";
            this.buttonUpLodeImighLecture.Size = new System.Drawing.Size(129, 54);
            this.buttonUpLodeImighLecture.TabIndex = 20;
            this.buttonUpLodeImighLecture.Text = "העלאת תמונה";
            this.buttonUpLodeImighLecture.UseVisualStyleBackColor = false;
            this.buttonUpLodeImighLecture.Click += new System.EventHandler(this.buttonUpLodeImighLecture_Click);
            // 
            // textBoxLecturerPhone
            // 
            this.textBoxLecturerPhone.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLecturerPhone.Location = new System.Drawing.Point(38, 99);
            this.textBoxLecturerPhone.Name = "textBoxLecturerPhone";
            this.textBoxLecturerPhone.Size = new System.Drawing.Size(132, 25);
            this.textBoxLecturerPhone.TabIndex = 19;
            // 
            // textBoxLecturerEmail
            // 
            this.textBoxLecturerEmail.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLecturerEmail.Location = new System.Drawing.Point(38, 172);
            this.textBoxLecturerEmail.Name = "textBoxLecturerEmail";
            this.textBoxLecturerEmail.Size = new System.Drawing.Size(132, 25);
            this.textBoxLecturerEmail.TabIndex = 18;
            // 
            // textBoxLecturerName
            // 
            this.textBoxLecturerName.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLecturerName.Location = new System.Drawing.Point(38, 33);
            this.textBoxLecturerName.Name = "textBoxLecturerName";
            this.textBoxLecturerName.Size = new System.Drawing.Size(132, 25);
            this.textBoxLecturerName.TabIndex = 17;
            // 
            // dateTimePickerLecturerDateOfBirth
            // 
            this.dateTimePickerLecturerDateOfBirth.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerLecturerDateOfBirth.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePickerLecturerDateOfBirth.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerLecturerDateOfBirth.Location = new System.Drawing.Point(38, 228);
            this.dateTimePickerLecturerDateOfBirth.Name = "dateTimePickerLecturerDateOfBirth";
            this.dateTimePickerLecturerDateOfBirth.Size = new System.Drawing.Size(132, 25);
            this.dateTimePickerLecturerDateOfBirth.TabIndex = 3;
            this.dateTimePickerLecturerDateOfBirth.Value = new System.DateTime(2024, 10, 10, 0, 0, 0, 0);
            // 
            // labelLecturerEmail
            // 
            this.labelLecturerEmail.AutoSize = true;
            this.labelLecturerEmail.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLecturerEmail.Location = new System.Drawing.Point(77, 150);
            this.labelLecturerEmail.Name = "labelLecturerEmail";
            this.labelLecturerEmail.Size = new System.Drawing.Size(47, 19);
            this.labelLecturerEmail.TabIndex = 2;
            this.labelLecturerEmail.Text = "איימיל";
            // 
            // labelLecturerPhone
            // 
            this.labelLecturerPhone.AutoSize = true;
            this.labelLecturerPhone.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLecturerPhone.Location = new System.Drawing.Point(68, 77);
            this.labelLecturerPhone.Name = "labelLecturerPhone";
            this.labelLecturerPhone.Size = new System.Drawing.Size(82, 19);
            this.labelLecturerPhone.TabIndex = 1;
            this.labelLecturerPhone.Text = "מספר טלפון";
            // 
            // labelLecturerName
            // 
            this.labelLecturerName.AutoSize = true;
            this.labelLecturerName.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLecturerName.Location = new System.Drawing.Point(89, 11);
            this.labelLecturerName.Name = "labelLecturerName";
            this.labelLecturerName.Size = new System.Drawing.Size(67, 19);
            this.labelLecturerName.TabIndex = 0;
            this.labelLecturerName.Text = "שם מרצה";
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.Cyan;
            this.panelTop.Controls.Add(this.labelTop);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(784, 80);
            this.panelTop.TabIndex = 5;
            // 
            // labelTop
            // 
            this.labelTop.AutoSize = true;
            this.labelTop.Font = new System.Drawing.Font("Microsoft YaHei", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTop.Location = new System.Drawing.Point(269, 24);
            this.labelTop.Name = "labelTop";
            this.labelTop.Size = new System.Drawing.Size(275, 31);
            this.labelTop.TabIndex = 3;
            this.labelTop.Text = "הרשמה לאוניברסיטה";
            // 
            // panelDepartmentHead
            // 
            this.panelDepartmentHead.Controls.Add(this.buttonDepartmentHeadImighUpLode);
            this.panelDepartmentHead.Controls.Add(this.textBoxDepartmentHeadPhone);
            this.panelDepartmentHead.Controls.Add(this.textBoxDepartmentHeadEmail);
            this.panelDepartmentHead.Controls.Add(this.textBoxDepartmentHeadName);
            this.panelDepartmentHead.Controls.Add(this.dateTimePickerDepartmentHeadDateOfBirth);
            this.panelDepartmentHead.Controls.Add(this.labelDepartmentHeadEmail);
            this.panelDepartmentHead.Controls.Add(this.labelDepartmentHeadPhone);
            this.panelDepartmentHead.Controls.Add(this.labelDepartmentHeadName);
            this.panelDepartmentHead.Location = new System.Drawing.Point(391, 93);
            this.panelDepartmentHead.Name = "panelDepartmentHead";
            this.panelDepartmentHead.Size = new System.Drawing.Size(380, 319);
            this.panelDepartmentHead.TabIndex = 21;
            this.panelDepartmentHead.Visible = false;
            // 
            // buttonDepartmentHeadImighUpLode
            // 
            this.buttonDepartmentHeadImighUpLode.BackColor = System.Drawing.Color.Cyan;
            this.buttonDepartmentHeadImighUpLode.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDepartmentHeadImighUpLode.Location = new System.Drawing.Point(216, 199);
            this.buttonDepartmentHeadImighUpLode.Name = "buttonDepartmentHeadImighUpLode";
            this.buttonDepartmentHeadImighUpLode.Size = new System.Drawing.Size(129, 54);
            this.buttonDepartmentHeadImighUpLode.TabIndex = 20;
            this.buttonDepartmentHeadImighUpLode.Text = "העלאת תמונה";
            this.buttonDepartmentHeadImighUpLode.UseVisualStyleBackColor = false;
            this.buttonDepartmentHeadImighUpLode.Click += new System.EventHandler(this.buttonDepartmentHeadImighUpLode_Click);
            // 
            // textBoxDepartmentHeadPhone
            // 
            this.textBoxDepartmentHeadPhone.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDepartmentHeadPhone.Location = new System.Drawing.Point(38, 99);
            this.textBoxDepartmentHeadPhone.Name = "textBoxDepartmentHeadPhone";
            this.textBoxDepartmentHeadPhone.Size = new System.Drawing.Size(132, 25);
            this.textBoxDepartmentHeadPhone.TabIndex = 19;
            // 
            // textBoxDepartmentHeadEmail
            // 
            this.textBoxDepartmentHeadEmail.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDepartmentHeadEmail.Location = new System.Drawing.Point(38, 172);
            this.textBoxDepartmentHeadEmail.Name = "textBoxDepartmentHeadEmail";
            this.textBoxDepartmentHeadEmail.Size = new System.Drawing.Size(132, 25);
            this.textBoxDepartmentHeadEmail.TabIndex = 18;
            // 
            // textBoxDepartmentHeadName
            // 
            this.textBoxDepartmentHeadName.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDepartmentHeadName.Location = new System.Drawing.Point(38, 33);
            this.textBoxDepartmentHeadName.Name = "textBoxDepartmentHeadName";
            this.textBoxDepartmentHeadName.Size = new System.Drawing.Size(132, 25);
            this.textBoxDepartmentHeadName.TabIndex = 17;
            // 
            // dateTimePickerDepartmentHeadDateOfBirth
            // 
            this.dateTimePickerDepartmentHeadDateOfBirth.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerDepartmentHeadDateOfBirth.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePickerDepartmentHeadDateOfBirth.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerDepartmentHeadDateOfBirth.Location = new System.Drawing.Point(38, 228);
            this.dateTimePickerDepartmentHeadDateOfBirth.Name = "dateTimePickerDepartmentHeadDateOfBirth";
            this.dateTimePickerDepartmentHeadDateOfBirth.Size = new System.Drawing.Size(132, 25);
            this.dateTimePickerDepartmentHeadDateOfBirth.TabIndex = 3;
            this.dateTimePickerDepartmentHeadDateOfBirth.Value = new System.DateTime(2024, 10, 10, 0, 0, 0, 0);
            // 
            // labelDepartmentHeadEmail
            // 
            this.labelDepartmentHeadEmail.AutoSize = true;
            this.labelDepartmentHeadEmail.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDepartmentHeadEmail.Location = new System.Drawing.Point(79, 150);
            this.labelDepartmentHeadEmail.Name = "labelDepartmentHeadEmail";
            this.labelDepartmentHeadEmail.Size = new System.Drawing.Size(47, 19);
            this.labelDepartmentHeadEmail.TabIndex = 2;
            this.labelDepartmentHeadEmail.Text = "איימיל";
            // 
            // labelDepartmentHeadPhone
            // 
            this.labelDepartmentHeadPhone.AutoSize = true;
            this.labelDepartmentHeadPhone.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDepartmentHeadPhone.Location = new System.Drawing.Point(68, 77);
            this.labelDepartmentHeadPhone.Name = "labelDepartmentHeadPhone";
            this.labelDepartmentHeadPhone.Size = new System.Drawing.Size(82, 19);
            this.labelDepartmentHeadPhone.TabIndex = 1;
            this.labelDepartmentHeadPhone.Text = "מספר טלפון";
            // 
            // labelDepartmentHeadName
            // 
            this.labelDepartmentHeadName.AutoSize = true;
            this.labelDepartmentHeadName.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDepartmentHeadName.Location = new System.Drawing.Point(50, 11);
            this.labelDepartmentHeadName.Name = "labelDepartmentHeadName";
            this.labelDepartmentHeadName.Size = new System.Drawing.Size(109, 19);
            this.labelDepartmentHeadName.TabIndex = 0;
            this.labelDepartmentHeadName.Text = "שם ראש מחלקה";
            // 
            // panelStudent
            // 
            this.panelStudent.Controls.Add(this.buttonUploadStudentImage);
            this.panelStudent.Controls.Add(this.textBoxStudentPhone);
            this.panelStudent.Controls.Add(this.textBoxStudentEmail);
            this.panelStudent.Controls.Add(this.textBoxStudentName);
            this.panelStudent.Controls.Add(this.dateTimePickerStudentDateOfBirth);
            this.panelStudent.Controls.Add(this.labelStudentEmail);
            this.panelStudent.Controls.Add(this.labelStudentPhone);
            this.panelStudent.Controls.Add(this.labelStudentName);
            this.panelStudent.Location = new System.Drawing.Point(399, 86);
            this.panelStudent.Name = "panelStudent";
            this.panelStudent.Size = new System.Drawing.Size(380, 319);
            this.panelStudent.TabIndex = 19;
            this.panelStudent.Visible = false;
            // 
            // buttonUploadStudentImage
            // 
            this.buttonUploadStudentImage.BackColor = System.Drawing.Color.Cyan;
            this.buttonUploadStudentImage.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonUploadStudentImage.Location = new System.Drawing.Point(216, 199);
            this.buttonUploadStudentImage.Name = "buttonUploadStudentImage";
            this.buttonUploadStudentImage.Size = new System.Drawing.Size(129, 54);
            this.buttonUploadStudentImage.TabIndex = 20;
            this.buttonUploadStudentImage.Text = "העלאת תמונה";
            this.buttonUploadStudentImage.UseVisualStyleBackColor = false;
            this.buttonUploadStudentImage.Click += new System.EventHandler(this.buttonUploadStudentImage_Click);
            // 
            // textBoxStudentPhone
            // 
            this.textBoxStudentPhone.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxStudentPhone.Location = new System.Drawing.Point(38, 99);
            this.textBoxStudentPhone.Name = "textBoxStudentPhone";
            this.textBoxStudentPhone.Size = new System.Drawing.Size(132, 25);
            this.textBoxStudentPhone.TabIndex = 19;
            // 
            // textBoxStudentEmail
            // 
            this.textBoxStudentEmail.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxStudentEmail.Location = new System.Drawing.Point(38, 172);
            this.textBoxStudentEmail.Name = "textBoxStudentEmail";
            this.textBoxStudentEmail.Size = new System.Drawing.Size(132, 25);
            this.textBoxStudentEmail.TabIndex = 18;
            // 
            // textBoxStudentName
            // 
            this.textBoxStudentName.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxStudentName.Location = new System.Drawing.Point(38, 33);
            this.textBoxStudentName.Name = "textBoxStudentName";
            this.textBoxStudentName.Size = new System.Drawing.Size(132, 25);
            this.textBoxStudentName.TabIndex = 17;
            // 
            // dateTimePickerStudentDateOfBirth
            // 
            this.dateTimePickerStudentDateOfBirth.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerStudentDateOfBirth.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePickerStudentDateOfBirth.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerStudentDateOfBirth.Location = new System.Drawing.Point(38, 228);
            this.dateTimePickerStudentDateOfBirth.Name = "dateTimePickerStudentDateOfBirth";
            this.dateTimePickerStudentDateOfBirth.Size = new System.Drawing.Size(132, 25);
            this.dateTimePickerStudentDateOfBirth.TabIndex = 3;
            this.dateTimePickerStudentDateOfBirth.Value = new System.DateTime(2024, 10, 10, 0, 0, 0, 0);
            // 
            // labelStudentEmail
            // 
            this.labelStudentEmail.AutoSize = true;
            this.labelStudentEmail.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStudentEmail.Location = new System.Drawing.Point(77, 150);
            this.labelStudentEmail.Name = "labelStudentEmail";
            this.labelStudentEmail.Size = new System.Drawing.Size(47, 19);
            this.labelStudentEmail.TabIndex = 2;
            this.labelStudentEmail.Text = "איימיל";
            // 
            // labelStudentPhone
            // 
            this.labelStudentPhone.AutoSize = true;
            this.labelStudentPhone.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStudentPhone.Location = new System.Drawing.Point(68, 77);
            this.labelStudentPhone.Name = "labelStudentPhone";
            this.labelStudentPhone.Size = new System.Drawing.Size(82, 19);
            this.labelStudentPhone.TabIndex = 1;
            this.labelStudentPhone.Text = "מספר טלפון";
            // 
            // labelStudentName
            // 
            this.labelStudentName.AutoSize = true;
            this.labelStudentName.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStudentName.Location = new System.Drawing.Point(89, 11);
            this.labelStudentName.Name = "labelStudentName";
            this.labelStudentName.Size = new System.Drawing.Size(81, 19);
            this.labelStudentName.TabIndex = 0;
            this.labelStudentName.Text = " שם סטודנט";
            // 
            // buttonAsUser
            // 
            this.buttonAsUser.BackColor = System.Drawing.Color.Cyan;
            this.buttonAsUser.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonAsUser.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAsUser.Location = new System.Drawing.Point(230, 410);
            this.buttonAsUser.Margin = new System.Windows.Forms.Padding(2);
            this.buttonAsUser.Name = "buttonAsUser";
            this.buttonAsUser.Size = new System.Drawing.Size(164, 50);
            this.buttonAsUser.TabIndex = 8;
            this.buttonAsUser.Text = "משתמש קיים";
            this.buttonAsUser.UseVisualStyleBackColor = false;
            this.buttonAsUser.Click += new System.EventHandler(this.buttonAsUser_Click);
            // 
            // buttonSignIn
            // 
            this.buttonSignIn.BackColor = System.Drawing.Color.Cyan;
            this.buttonSignIn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonSignIn.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSignIn.Location = new System.Drawing.Point(620, 410);
            this.buttonSignIn.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSignIn.Name = "buttonSignIn";
            this.buttonSignIn.Size = new System.Drawing.Size(164, 50);
            this.buttonSignIn.TabIndex = 9;
            this.buttonSignIn.Text = "להרשמה";
            this.buttonSignIn.UseVisualStyleBackColor = false;
            this.buttonSignIn.Click += new System.EventHandler(this.buttonSignIn_Click);
            // 
            // labelUserId
            // 
            this.labelUserId.AutoSize = true;
            this.labelUserId.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUserId.Location = new System.Drawing.Point(236, 97);
            this.labelUserId.Name = "labelUserId";
            this.labelUserId.Size = new System.Drawing.Size(160, 19);
            this.labelUserId.TabIndex = 10;
            this.labelUserId.Text = "מספר זהות(שם משתמש)";
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPassword.Location = new System.Drawing.Point(290, 163);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(49, 19);
            this.labelPassword.TabIndex = 11;
            this.labelPassword.Text = "סיסמה";
            // 
            // labelPasswordCn
            // 
            this.labelPasswordCn.AutoSize = true;
            this.labelPasswordCn.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPasswordCn.Location = new System.Drawing.Point(271, 236);
            this.labelPasswordCn.Name = "labelPasswordCn";
            this.labelPasswordCn.Size = new System.Drawing.Size(89, 19);
            this.labelPasswordCn.TabIndex = 12;
            this.labelPasswordCn.Text = "אישור סיסמה";
            // 
            // comboBoxRole
            // 
            this.comboBoxRole.AutoCompleteCustomSource.AddRange(new string[] {
            "סטודנט",
            "מרצה",
            "ראש מחלקה"});
            this.comboBoxRole.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxRole.FormattingEnabled = true;
            this.comboBoxRole.Items.AddRange(new object[] {
            "סטודנט",
            "מרצה",
            "ראש מחלקה"});
            this.comboBoxRole.Location = new System.Drawing.Point(252, 312);
            this.comboBoxRole.Name = "comboBoxRole";
            this.comboBoxRole.Size = new System.Drawing.Size(132, 27);
            this.comboBoxRole.TabIndex = 15;
            this.comboBoxRole.Text = "בחירת תפקיד";
            this.comboBoxRole.SelectedIndexChanged += new System.EventHandler(this.comboBoxRole_SelectedIndexChanged);
            // 
            // textBoxUserId
            // 
            this.textBoxUserId.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxUserId.Location = new System.Drawing.Point(252, 119);
            this.textBoxUserId.Name = "textBoxUserId";
            this.textBoxUserId.Size = new System.Drawing.Size(132, 25);
            this.textBoxUserId.TabIndex = 16;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPassword.Location = new System.Drawing.Point(252, 185);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(132, 25);
            this.textBoxPassword.TabIndex = 17;
            // 
            // textBoxPasswordCon
            // 
            this.textBoxPasswordCon.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPasswordCon.Location = new System.Drawing.Point(252, 258);
            this.textBoxPasswordCon.Name = "textBoxPasswordCon";
            this.textBoxPasswordCon.Size = new System.Drawing.Size(132, 25);
            this.textBoxPasswordCon.TabIndex = 18;
            // 
            // fillByToolStripButton
            // 
            this.fillByToolStripButton.Name = "fillByToolStripButton";
            this.fillByToolStripButton.Size = new System.Drawing.Size(23, 23);
            // 
            // SignIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.panelLecturer);
            this.Controls.Add(this.panelDepartmentHead);
            this.Controls.Add(this.panelStudent);
            this.Controls.Add(this.textBoxPasswordCon);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.textBoxUserId);
            this.Controls.Add(this.comboBoxRole);
            this.Controls.Add(this.labelPasswordCn);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.labelUserId);
            this.Controls.Add(this.buttonSignIn);
            this.Controls.Add(this.buttonAsUser);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelImag);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "SignIn";
            this.Text = "SignIn";
            this.panelLecturer.ResumeLayout(false);
            this.panelLecturer.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelDepartmentHead.ResumeLayout(false);
            this.panelDepartmentHead.PerformLayout();
            this.panelStudent.ResumeLayout(false);
            this.panelStudent.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelImag;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label labelTop;
        private System.Windows.Forms.Button buttonAsUser;
        private System.Windows.Forms.Button buttonSignIn;
        private System.Windows.Forms.Label labelUserId;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelPasswordCn;
        private System.Windows.Forms.ComboBox comboBoxRole;
        private System.Windows.Forms.TextBox textBoxUserId;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxPasswordCon;
        private System.Windows.Forms.Panel panelStudent;
        private System.Windows.Forms.Label labelStudentEmail;
        private System.Windows.Forms.Label labelStudentPhone;
        private System.Windows.Forms.Label labelStudentName;
        private System.Windows.Forms.ToolStripButton fillByToolStripButton;
        private System.Windows.Forms.DateTimePicker dateTimePickerStudentDateOfBirth;
        private System.Windows.Forms.TextBox textBoxStudentPhone;
        private System.Windows.Forms.TextBox textBoxStudentEmail;
        private System.Windows.Forms.TextBox textBoxStudentName;
        private System.Windows.Forms.Button buttonUploadStudentImage;
        private System.Windows.Forms.Panel panelLecturer;
        private System.Windows.Forms.Panel panelDepartmentHead;
        private System.Windows.Forms.Button buttonDepartmentHeadImighUpLode;
        private System.Windows.Forms.TextBox textBoxDepartmentHeadPhone;
        private System.Windows.Forms.TextBox textBoxDepartmentHeadEmail;
        private System.Windows.Forms.TextBox textBoxDepartmentHeadName;
        private System.Windows.Forms.DateTimePicker dateTimePickerDepartmentHeadDateOfBirth;
        private System.Windows.Forms.Label labelDepartmentHeadEmail;
        private System.Windows.Forms.Label labelDepartmentHeadPhone;
        private System.Windows.Forms.Label labelDepartmentHeadName;
        private System.Windows.Forms.Button buttonUpLodeImighLecture;
        private System.Windows.Forms.TextBox textBoxLecturerPhone;
        private System.Windows.Forms.TextBox textBoxLecturerEmail;
        private System.Windows.Forms.TextBox textBoxLecturerName;
        private System.Windows.Forms.DateTimePicker dateTimePickerLecturerDateOfBirth;
        private System.Windows.Forms.Label labelLecturerEmail;
        private System.Windows.Forms.Label labelLecturerPhone;
        private System.Windows.Forms.Label labelLecturerName;
    }
}