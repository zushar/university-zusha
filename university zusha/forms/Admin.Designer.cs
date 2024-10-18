namespace UniversityZusha.forms
{
    partial class Admin
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelTop = new System.Windows.Forms.Label();
            this.dataGridViewUnapprovedDepartmentHeads = new System.Windows.Forms.DataGridView();
            this.dataGridViewDepartmentHeads = new System.Windows.Forms.DataGridView();
            this.dataGridViewDepartments = new System.Windows.Forms.DataGridView();
            this.buttonDepartmentHeadSave = new System.Windows.Forms.Button();
            this.buttonDepartmentsSave = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUnapprovedDepartmentHeads)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDepartmentHeads)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDepartments)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.Cyan;
            this.panelTop.Controls.Add(this.labelTop);
            this.panelTop.Controls.Add(this.menuStrip1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1241, 80);
            this.panelTop.TabIndex = 6;
            // 
            // labelTop
            // 
            this.labelTop.AutoSize = true;
            this.labelTop.Font = new System.Drawing.Font("Microsoft YaHei", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTop.Location = new System.Drawing.Point(368, 9);
            this.labelTop.Name = "labelTop";
            this.labelTop.Size = new System.Drawing.Size(209, 31);
            this.labelTop.TabIndex = 3;
            this.labelTop.Text = "ברוך הבא אדמין";
            // 
            // dataGridViewUnapprovedDepartmentHeads
            // 
            this.dataGridViewUnapprovedDepartmentHeads.BackgroundColor = System.Drawing.Color.Cyan;
            this.dataGridViewUnapprovedDepartmentHeads.ColumnHeadersHeight = 46;
            this.dataGridViewUnapprovedDepartmentHeads.Location = new System.Drawing.Point(149, 109);
            this.dataGridViewUnapprovedDepartmentHeads.Name = "dataGridViewUnapprovedDepartmentHeads";
            this.dataGridViewUnapprovedDepartmentHeads.RowHeadersWidth = 82;
            this.dataGridViewUnapprovedDepartmentHeads.Size = new System.Drawing.Size(742, 194);
            this.dataGridViewUnapprovedDepartmentHeads.TabIndex = 0;
            // 
            // dataGridViewDepartmentHeads
            // 
            this.dataGridViewDepartmentHeads.BackgroundColor = System.Drawing.Color.Cyan;
            this.dataGridViewDepartmentHeads.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDepartmentHeads.Location = new System.Drawing.Point(149, 346);
            this.dataGridViewDepartmentHeads.Name = "dataGridViewDepartmentHeads";
            this.dataGridViewDepartmentHeads.RowHeadersWidth = 82;
            this.dataGridViewDepartmentHeads.Size = new System.Drawing.Size(742, 228);
            this.dataGridViewDepartmentHeads.TabIndex = 10;
            // 
            // dataGridViewDepartments
            // 
            this.dataGridViewDepartments.BackgroundColor = System.Drawing.Color.Cyan;
            this.dataGridViewDepartments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDepartments.Location = new System.Drawing.Point(915, 109);
            this.dataGridViewDepartments.Name = "dataGridViewDepartments";
            this.dataGridViewDepartments.RowHeadersWidth = 82;
            this.dataGridViewDepartments.Size = new System.Drawing.Size(314, 465);
            this.dataGridViewDepartments.TabIndex = 11;
            // 
            // buttonDepartmentHeadSave
            // 
            this.buttonDepartmentHeadSave.Location = new System.Drawing.Point(12, 109);
            this.buttonDepartmentHeadSave.Name = "buttonDepartmentHeadSave";
            this.buttonDepartmentHeadSave.Size = new System.Drawing.Size(131, 58);
            this.buttonDepartmentHeadSave.TabIndex = 7;
            this.buttonDepartmentHeadSave.Text = "Department Head \r\nSave Changes";
            this.buttonDepartmentHeadSave.Click += new System.EventHandler(this.buttonDepartmentHeadSave_Click);
            // 
            // buttonDepartmentsSave
            // 
            this.buttonDepartmentsSave.Location = new System.Drawing.Point(12, 191);
            this.buttonDepartmentsSave.Name = "buttonDepartmentsSave";
            this.buttonDepartmentsSave.Size = new System.Drawing.Size(131, 60);
            this.buttonDepartmentsSave.TabIndex = 8;
            this.buttonDepartmentsSave.Text = "Departments\r\n Save Changes";
            this.buttonDepartmentsSave.UseVisualStyleBackColor = true;
            this.buttonDepartmentsSave.Click += new System.EventHandler(this.buttonDepartmentsSave_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logoutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1241, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.logoutToolStripMenuItem.Text = "logout";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "logut";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // Admin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1241, 614);
            this.Controls.Add(this.dataGridViewDepartments);
            this.Controls.Add(this.dataGridViewDepartmentHeads);
            this.Controls.Add(this.buttonDepartmentsSave);
            this.Controls.Add(this.buttonDepartmentHeadSave);
            this.Controls.Add(this.dataGridViewUnapprovedDepartmentHeads);
            this.Controls.Add(this.panelTop);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Admin";
            this.Text = "Admin";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUnapprovedDepartmentHeads)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDepartmentHeads)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDepartments)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label labelTop;
        private System.Windows.Forms.DataGridView dataGridViewUnapprovedDepartmentHeads;
        private System.Windows.Forms.DataGridView dataGridViewDepartmentHeads;
        private System.Windows.Forms.DataGridView dataGridViewDepartments;
        private System.Windows.Forms.Button buttonDepartmentHeadSave;
        private System.Windows.Forms.Button buttonDepartmentsSave;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem logoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    }
}
