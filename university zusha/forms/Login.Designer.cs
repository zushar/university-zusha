namespace UniversityZusha
{
    partial class Login
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
            this.labelIdNum = new System.Windows.Forms.Label();
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelTop = new System.Windows.Forms.Label();
            this.textBoxIdNum = new System.Windows.Forms.TextBox();
            this.textBoxPasswordHash = new System.Windows.Forms.TextBox();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.buttonSubscribe = new System.Windows.Forms.Button();
            this.labelPasswordHash = new System.Windows.Forms.Label();
            this.labelErrer = new System.Windows.Forms.Label();
            this.panelImag = new System.Windows.Forms.Panel();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelIdNum
            // 
            this.labelIdNum.AutoSize = true;
            this.labelIdNum.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIdNum.Location = new System.Drawing.Point(280, 184);
            this.labelIdNum.Name = "labelIdNum";
            this.labelIdNum.Size = new System.Drawing.Size(142, 31);
            this.labelIdNum.TabIndex = 0;
            this.labelIdNum.Text = "id numbor";
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.Cyan;
            this.panelTop.Controls.Add(this.labelTop);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(784, 80);
            this.panelTop.TabIndex = 1;
            // 
            // labelTop
            // 
            this.labelTop.AutoSize = true;
            this.labelTop.Font = new System.Drawing.Font("Microsoft YaHei", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTop.Location = new System.Drawing.Point(269, 24);
            this.labelTop.Name = "labelTop";
            this.labelTop.Size = new System.Drawing.Size(297, 31);
            this.labelTop.TabIndex = 3;
            this.labelTop.Text = "התחברות לאוניברסיטה";
            // 
            // textBoxIdNum
            // 
            this.textBoxIdNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxIdNum.Location = new System.Drawing.Point(459, 192);
            this.textBoxIdNum.MaxLength = 9;
            this.textBoxIdNum.Name = "textBoxIdNum";
            this.textBoxIdNum.Size = new System.Drawing.Size(200, 24);
            this.textBoxIdNum.TabIndex = 1;
            // 
            // textBoxPasswordHash
            // 
            this.textBoxPasswordHash.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPasswordHash.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.textBoxPasswordHash.Location = new System.Drawing.Point(459, 239);
            this.textBoxPasswordHash.MaxLength = 10;
            this.textBoxPasswordHash.Name = "textBoxPasswordHash";
            this.textBoxPasswordHash.Size = new System.Drawing.Size(200, 24);
            this.textBoxPasswordHash.TabIndex = 2;
            this.textBoxPasswordHash.UseSystemPasswordChar = true;
            // 
            // buttonLogin
            // 
            this.buttonLogin.BackColor = System.Drawing.Color.Cyan;
            this.buttonLogin.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonLogin.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLogin.Location = new System.Drawing.Point(261, 322);
            this.buttonLogin.Margin = new System.Windows.Forms.Padding(2);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(187, 61);
            this.buttonLogin.TabIndex = 3;
            this.buttonLogin.Text = "התחברות";
            this.buttonLogin.UseVisualStyleBackColor = false;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // buttonSubscribe
            // 
            this.buttonSubscribe.BackColor = System.Drawing.Color.Cyan;
            this.buttonSubscribe.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSubscribe.Location = new System.Drawing.Point(485, 322);
            this.buttonSubscribe.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSubscribe.Name = "buttonSubscribe";
            this.buttonSubscribe.Size = new System.Drawing.Size(187, 61);
            this.buttonSubscribe.TabIndex = 4;
            this.buttonSubscribe.Text = "להרשמה";
            this.buttonSubscribe.UseVisualStyleBackColor = false;
            this.buttonSubscribe.Click += new System.EventHandler(this.buttonSubscribe_Click);
            // 
            // labelPasswordHash
            // 
            this.labelPasswordHash.AutoSize = true;
            this.labelPasswordHash.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPasswordHash.Location = new System.Drawing.Point(280, 231);
            this.labelPasswordHash.Name = "labelPasswordHash";
            this.labelPasswordHash.Size = new System.Drawing.Size(130, 31);
            this.labelPasswordHash.TabIndex = 4;
            this.labelPasswordHash.Text = "Password";
            // 
            // labelErrer
            // 
            this.labelErrer.AutoSize = true;
            this.labelErrer.Font = new System.Drawing.Font("Microsoft Tai Le", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelErrer.ForeColor = System.Drawing.Color.Red;
            this.labelErrer.Location = new System.Drawing.Point(480, 105);
            this.labelErrer.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelErrer.Name = "labelErrer";
            this.labelErrer.Size = new System.Drawing.Size(0, 26);
            this.labelErrer.TabIndex = 11;
            // 
            // panelImag
            // 
            this.panelImag.BackgroundImage = global::UniversityZusha.Properties.Resources.loginPhoto;
            this.panelImag.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelImag.Location = new System.Drawing.Point(0, 80);
            this.panelImag.Name = "panelImag";
            this.panelImag.Size = new System.Drawing.Size(230, 380);
            this.panelImag.TabIndex = 3;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.panelImag);
            this.Controls.Add(this.labelErrer);
            this.Controls.Add(this.buttonSubscribe);
            this.Controls.Add(this.buttonLogin);
            this.Controls.Add(this.textBoxPasswordHash);
            this.Controls.Add(this.labelPasswordHash);
            this.Controls.Add(this.textBoxIdNum);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.labelIdNum);
            this.Name = "Login";
            this.Text = "login";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelIdNum;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label labelTop;
        private System.Windows.Forms.TextBox textBoxIdNum;
        private System.Windows.Forms.Panel panelImag;
        private System.Windows.Forms.TextBox textBoxPasswordHash;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.Button buttonSubscribe;
        private System.Windows.Forms.Label labelPasswordHash;
        private System.Windows.Forms.Label labelErrer;
    }
}

