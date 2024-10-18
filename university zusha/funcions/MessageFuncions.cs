using System;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;
using UniversityZusha.dbFunctions;
using UniversityZusha.users;
using System.Linq;

namespace UniversityZusha.messageFuncions
{
    internal class MessageFuncions
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["SchoolDbConnection"].ConnectionString;
        private int UserID;
        private string currentUserRole;
        private DataGridView dataGridViewInbox;
        private ComboBox comboBoxRecipient;
        private TextBox textBoxSubject;
        private TextBox textBoxBody;
        private TabControl messagesTabControl;
        private DataGridView dataGridViewMessages;
        private Button btnSortByDate;
        private Button btnSortByFavorite;
        private Button btnSortBySenderImportance;
        private TextBox textBoxSearch;
        private ComboBox comboBoxSearchType;
        private DataGridView dataGridViewSearchResults;
        private DataGridView dataGridViewOutbox;

        public MessageFuncions(int UserID)
        {
            this.UserID = UserID;
            this.currentUserRole = GetUserRole(UserID);
        }

        /// <summary>
        /// מאתחל את הלשונית הראשית של ההודעות
        /// </summary>
        public void InitializeMessagesTab(TabPage tab)
        {
            TabControl messagesTabControl = new TabControl();
            messagesTabControl.Dock = DockStyle.Fill;

            TabPage tabInbox = new TabPage("תיבת דואר נכנס");
            TabPage tabOutbox = new TabPage("תיבת דואר יוצא");
            TabPage tabSendMessage = new TabPage("שלח הודעה");
            TabPage tabSearch = new TabPage("חיפוש אנשים");

            InitializeInboxTab(tabInbox);
            InitializeOutboxTab(tabOutbox); // פונקציה חדשה שנגדיר
            InitializeSendMessageTab(tabSendMessage);
            InitializeSearchTab(tabSearch);

            messagesTabControl.TabPages.Add(tabInbox);
            messagesTabControl.TabPages.Add(tabOutbox);
            messagesTabControl.TabPages.Add(tabSendMessage);
            messagesTabControl.TabPages.Add(tabSearch);

            tab.Controls.Add(messagesTabControl);
        }

        public void InitializeOutboxTab(TabPage tabOutbox)
        {
            dataGridViewOutbox = new DataGridView();
            dataGridViewOutbox.Dock = DockStyle.Fill;
            dataGridViewOutbox.AutoGenerateColumns = false;

            // הגדרת עמודות
            dataGridViewOutbox.Columns.Add(new DataGridViewTextBoxColumn() { Name = "MessageID", DataPropertyName = "MessageID", Visible = false });
            dataGridViewOutbox.Columns.Add(new DataGridViewTextBoxColumn() { Name = "SentDate", HeaderText = "תאריך שליחה", DataPropertyName = "SentDate" });
            dataGridViewOutbox.Columns.Add(new DataGridViewTextBoxColumn() { Name = "RecipientName", HeaderText = "נמען", DataPropertyName = "RecipientName" });
            dataGridViewOutbox.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Subject", HeaderText = "נושא", DataPropertyName = "Subject" });

            // הוספת כפתור לצפייה בהודעה
            DataGridViewButtonColumn viewMessageColumn = new DataGridViewButtonColumn();
            viewMessageColumn.Name = "ViewMessage";
            viewMessageColumn.HeaderText = "צפה בהודעה";
            viewMessageColumn.Text = "צפה";
            viewMessageColumn.UseColumnTextForButtonValue = true;
            dataGridViewOutbox.Columns.Add(viewMessageColumn);

            dataGridViewOutbox.CellContentClick += DataGridViewOutbox_CellContentClick;

            tabOutbox.Controls.Add(dataGridViewOutbox);

            // טעינת ההודעות
            LoadOutboxMessages();
        }

        /// <summary>
        /// Loads outbox messages for the current user and populates the dataGridViewOutbox.
        /// </summary>
        private void LoadOutboxMessages()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = @"
        SELECT 
            m.MessageID,
            m.SentDate,
            m.Subject,
            CASE 
                WHEN a.Role = 'Lecturer' THEN l.Name
                WHEN a.Role = 'Student' THEN s.Name
                WHEN a.Role = 'DepartmentHead' THEN dh.Name
                ELSE a.UserName
            END AS RecipientName
        FROM Messages m
        INNER JOIN Auth a ON m.RecipientID = a.AuthID
        LEFT JOIN Lecturers l ON a.AuthID = l.AuthID
        LEFT JOIN Students s ON a.AuthID = s.AuthID
        LEFT JOIN DepartmentHeads dh ON a.AuthID = dh.AuthID
        WHERE m.SenderID = @SenderID
        ORDER BY m.SentDate DESC
    ";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@SenderID", UserID);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (!dt.Columns.Contains("MessageID"))
                        {
                            // Log this error instead of showing a MessageBox
                            Console.WriteLine("העמודה MessageID חסרה בתוצאות השאילתה.");
                            return;
                        }

                        dataGridViewOutbox.DataSource = dt;

                        // For debugging purposes
                        string columnNames = string.Join(", ", dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
                        Console.WriteLine($"Columns in DataTable: {columnNames}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"אירעה שגיאה בטעינת הודעות יוצאות: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Handles the cell click event for the outbox DataGridView.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A DataGridViewCellEventArgs that contains the event data.</param>
        private void DataGridViewOutbox_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dataGridViewOutbox.Columns[e.ColumnIndex].Name == "ViewMessage")
            {
                if (dataGridViewOutbox.Columns.Contains("MessageID"))
                {
                    int messageID = Convert.ToInt32(dataGridViewOutbox.Rows[e.RowIndex].Cells["MessageID"].Value);
                    ViewOutboxMessage(messageID);
                }
                else
                {
                    // Log this error instead of showing a MessageBox
                    Console.WriteLine("לא ניתן לצפות בהודעה. מזהה ההודעה חסר.");
                    string columnNames = string.Join(", ", dataGridViewOutbox.Columns.Cast<DataGridViewColumn>().Select(c => c.Name));
                    Console.WriteLine($"Available columns: {columnNames}");
                }
            }
        }

        /// <summary>
        /// Displays the content of a specific outbox message.
        /// </summary>
        /// <param name="messageID">The ID of the message to view.</param>
        private void ViewOutboxMessage(int messageID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = @"
        SELECT 
            m.Subject,
            m.Body,
            m.SentDate,
            CASE 
                WHEN a.Role = 'Lecturer' THEN l.Name
                WHEN a.Role = 'Student' THEN s.Name
                WHEN a.Role = 'DepartmentHead' THEN dh.Name
                ELSE a.UserName
            END AS RecipientName
        FROM Messages m
        INNER JOIN Auth a ON m.RecipientID = a.AuthID
        LEFT JOIN Lecturers l ON a.AuthID = l.AuthID
        LEFT JOIN Students s ON a.AuthID = s.AuthID
        LEFT JOIN DepartmentHeads dh ON a.AuthID = dh.AuthID
        WHERE m.MessageID = @MessageID
    ";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@MessageID", messageID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string subject = reader["Subject"].ToString();
                                string body = reader["Body"].ToString();
                                string recipientName = reader["RecipientName"].ToString();
                                DateTime sentDate = Convert.ToDateTime(reader["SentDate"]);

                                MessageBox.Show($"אל: {recipientName}\nנושא: {subject}\nתאריך: {sentDate}\n\n{body}", "הודעה שנשלחה");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"אירעה שגיאה בהצגת ההודעה: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// מאתחל את לשונית החיפוש
        /// </summary>
        public void InitializeSearchTab(TabPage tab)
        {
            Label labelSearch = new Label() { Text = "חיפוש:", Location = new Point(20, 20), Width = 100 };
            textBoxSearch = new TextBox() { Location = new Point(150, 20), Width = 150 };

            Label labelSearchType = new Label() { Text = "סוג חיפוש:", Location = new Point(300, 20) };
            comboBoxSearchType = new ComboBox() { Location = new Point(420, 20), Width = 150 };
            comboBoxSearchType.Items.AddRange(new object[] { "הכל", "Student", "Lecturer", "DepartmentHead" });
            comboBoxSearchType.SelectedIndex = 0;

            Button buttonSearch = new Button() { Text = "חפש", Location = new Point(570, 20) };
            buttonSearch.Click += ButtonSearch_Click;

            dataGridViewSearchResults = new DataGridView();
            dataGridViewSearchResults.Location = new Point(20, 60);
            dataGridViewSearchResults.Size = new Size(700, 300);
            dataGridViewSearchResults.AutoGenerateColumns = false;

            // הגדרת העמודות
            dataGridViewSearchResults.Columns.Add(new DataGridViewTextBoxColumn() { Name = "AuthID",DataPropertyName = "AuthID", Visible = false });
            dataGridViewSearchResults.Columns.Add(new DataGridViewTextBoxColumn() { Name = "FullName", HeaderText = "שם מלא", DataPropertyName = "FullName" });
            dataGridViewSearchResults.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Role", HeaderText = "תפקיד", DataPropertyName = "Role" });
            dataGridViewSearchResults.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Email", HeaderText = "אימייל", DataPropertyName = "Email" });

            DataGridViewButtonColumn sendMessageColumn = new DataGridViewButtonColumn();
            sendMessageColumn.Name = "SendMessage";
            sendMessageColumn.HeaderText = "שלח הודעה";
            sendMessageColumn.Text = "שלח";
            sendMessageColumn.UseColumnTextForButtonValue = true;
            dataGridViewSearchResults.Columns.Add(sendMessageColumn);

            dataGridViewSearchResults.CellContentClick += DataGridViewSearchResults_CellContentClick;

            tab.Controls.Add(labelSearch);
            tab.Controls.Add(textBoxSearch);
            tab.Controls.Add(labelSearchType);
            tab.Controls.Add(comboBoxSearchType);
            tab.Controls.Add(buttonSearch);
            tab.Controls.Add(dataGridViewSearchResults);
        }

        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = textBoxSearch.Text;
            string searchType = comboBoxSearchType.SelectedItem.ToString();
            if (searchType == "הכל") searchType = "All";

            DataTable results = DbFunctions.SearchUsers(searchTerm, searchType, currentUserRole, UserID);
            dataGridViewSearchResults.DataSource = results;
            foreach (DataColumn column in results.Columns)
            {
                Console.WriteLine(column.ColumnName);
            }
        }

        private void DataGridViewSearchResults_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn sendMessageColumn = dataGridViewSearchResults.Columns["SendMessage"];
            if (sendMessageColumn != null && e.ColumnIndex == sendMessageColumn.Index && e.RowIndex >= 0)
            {
                string missingInfo = "";
                bool hasAuthID = false;
                bool hasFullName = false;

                if (dataGridViewSearchResults.Columns.Contains("AuthID"))
                {
                    if (dataGridViewSearchResults.Rows[e.RowIndex].Cells["AuthID"].Value != null)
                    {
                        hasAuthID = true;
                    }
                    else
                    {
                        missingInfo += "AuthID חסר. ";
                    }
                }
                else
                {
                    missingInfo += "עמודת AuthID לא קיימת. ";
                }

                if (dataGridViewSearchResults.Columns.Contains("FullName"))
                {
                    if (dataGridViewSearchResults.Rows[e.RowIndex].Cells["FullName"].Value != null)
                    {
                        hasFullName = true;
                    }
                    else
                    {
                        missingInfo += "FullName חסר. ";
                    }
                }
                else
                {
                    missingInfo += "עמודת FullName לא קיימת. ";
                }

                if (hasAuthID && hasFullName)
                {
                    int recipientId = Convert.ToInt32(dataGridViewSearchResults.Rows[e.RowIndex].Cells["AuthID"].Value);
                    string recipientName = dataGridViewSearchResults.Rows[e.RowIndex].Cells["FullName"].Value.ToString();
                    OpenSendMessageForm(recipientId, recipientName);
                }
                else
                {
                    string availableInfo = "מידע זמין:\n";
                    foreach (DataGridViewColumn column in dataGridViewSearchResults.Columns)
                    {
                        availableInfo += $"{column.Name}: {dataGridViewSearchResults.Rows[e.RowIndex].Cells[column.Name].Value}\n";
                    }

                    MessageBox.Show($"לא ניתן לשלוח הודעה למשתמש זה. מידע חסר:\n{missingInfo}\n\n{availableInfo}");
                }
            }
        }

        private void OpenSendMessageForm(int recipientId, string recipientName)
        {
            Form sendMessageForm = new Form();
            sendMessageForm.Text = $"שליחת הודעה ל-{recipientName}";
            sendMessageForm.Size = new Size(400, 300);

            TextBox subjectTextBox = new TextBox() { Location = new Point(20, 20), Width = 350 };
            TextBox messageTextBox = new TextBox() { Location = new Point(20, 50), Width = 350, Height = 150, Multiline = true };
            Button sendButton = new Button() { Text = "שלח", Location = new Point(150, 220) };

            sendButton.Click += (s, e) =>
            {
                SendMessage(recipientId, subjectTextBox.Text, messageTextBox.Text);
                sendMessageForm.Close();
            };

            sendMessageForm.Controls.Add(subjectTextBox);
            sendMessageForm.Controls.Add(messageTextBox);
            sendMessageForm.Controls.Add(sendButton);

            sendMessageForm.ShowDialog();
        }

        /// <summary>
        /// טוען את ההודעות עבור המשתמש הנוכחי
        /// </summary>
        public void LoadMessages()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = @"
                    SELECT 
                        m.MessageID,
                        m.Subject,
                        m.SentDate,
                        m.IsFavorite,
                        m.SenderRole,
                        CASE 
                            WHEN m.SenderRole = 'DepartmentHead' THEN 4
                            WHEN m.SenderRole = 'Lecturer' THEN 3
                            WHEN m.SenderRole = 'Assistant' THEN 2
                            WHEN m.SenderRole = 'Student' THEN 1
                            ELSE 0
                        END AS SenderImportance
                    FROM Messages m
                    WHERE m.RecipientID = @RecipientID
                    ORDER BY m.SentDate DESC";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@RecipientID", UserID);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridViewMessages.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"אירעה שגיאה בטעינת ההודעות: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// ממיין את ההודעות לפי הקריטריון שנבחר
        /// </summary>
        public void SortMessages(string sortBy)
        {
            if (dataGridViewMessages.DataSource is DataTable dt)
            {
                switch (sortBy)
                {
                    case "SentDate":
                        dt.DefaultView.Sort = "SentDate DESC";
                        break;
                    case "IsFavorite":
                        dt.DefaultView.Sort = "IsFavorite DESC, SentDate DESC";
                        break;
                    case "SenderImportance":
                        dt.DefaultView.Sort = "SenderImportance DESC, SentDate DESC";
                        break;
                }
                dataGridViewMessages.DataSource = dt.DefaultView.ToTable();
            }
        }

        /// <summary>
        /// מאתחל את לשונית תיבת הדואר הנכנס
        /// </summary>
        public void InitializeInboxTab(TabPage tabInbox)
        {
            dataGridViewMessages = new DataGridView();
            dataGridViewMessages.Dock = DockStyle.Fill;
            dataGridViewMessages.AutoGenerateColumns = false;

            dataGridViewMessages.Columns.Add(new DataGridViewTextBoxColumn() { Name = "SentDate", HeaderText = "תאריך", DataPropertyName = "SentDate" });
            dataGridViewMessages.Columns.Add(new DataGridViewCheckBoxColumn() { Name = "IsFavorite", HeaderText = "מועדף", DataPropertyName = "IsFavorite" });
            dataGridViewMessages.Columns.Add(new DataGridViewTextBoxColumn() { Name = "SenderRole", HeaderText = "תפקיד השולח", DataPropertyName = "SenderRole" });
            dataGridViewMessages.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Subject", HeaderText = "נושא", DataPropertyName = "Subject" });

            Panel sortPanel = new Panel();
            sortPanel.Dock = DockStyle.Top;
            sortPanel.Height = 40;

            btnSortByDate = new Button() { Text = "מיין לפי תאריך" };
            btnSortByFavorite = new Button() { Text = "מיין לפי מועדף" };
            btnSortBySenderImportance = new Button() { Text = "מיין לפי חשיבות השולח" };

            btnSortByDate.Click += (s, e) => SortMessages("SentDate");
            btnSortByFavorite.Click += (s, e) => SortMessages("IsFavorite");
            btnSortBySenderImportance.Click += (s, e) => SortMessages("SenderImportance");

            sortPanel.Controls.Add(btnSortByDate);
            sortPanel.Controls.Add(btnSortByFavorite);
            sortPanel.Controls.Add(btnSortBySenderImportance);

            tabInbox.Controls.Add(dataGridViewMessages);
            tabInbox.Controls.Add(sortPanel);

            LoadMessages();
        }

        /// <summary>
        /// טוען את ההודעות לתיבת הדואר הנכנס
        /// </summary>
        public void LoadInboxMessages()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = @"
                    SELECT 
                        m.MessageID,
                        CASE 
                            WHEN a.Role = 'Lecturer' THEN l.Name
                            WHEN a.Role = 'Student' THEN s.Name
                            WHEN a.Role = 'DepartmentHead' THEN dh.Name
                            ELSE a.UserName
                        END AS SenderName,
                        m.Subject,
                        m.SentDate,
                        m.IsRead
                    FROM Messages m
                    INNER JOIN Auth a ON m.SenderID = a.AuthID
                    LEFT JOIN Lecturers l ON a.AuthID = l.AuthID
                    LEFT JOIN Students s ON a.AuthID = s.AuthID
                    LEFT JOIN DepartmentHeads dh ON a.AuthID = dh.AuthID
                    WHERE m.RecipientID = @RecipientID
                    ORDER BY m.SentDate DESC
                ";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@RecipientID", UserID);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        dataGridViewInbox.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"אירעה שגיאה בטעינת ההודעות: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// מטפל בלחיצה על תא בטבלת תיבת הדואר הנכנס
        /// </summary>
        public void DataGridViewInbox_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            string columnName = dataGridViewInbox.Columns[e.ColumnIndex].Name;

            if (columnName == "ViewMessage")
            {
                int messageID = Convert.ToInt32(dataGridViewInbox.Rows[e.RowIndex].Cells["MessageID"].Value);
                ViewMessage(messageID);
                LoadInboxMessages();
            }
        }

        /// <summary>
        /// מציג הודעה ספציפית
        /// </summary>
        public void ViewMessage(int messageID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = @"
                    SELECT 
                        m.Subject,
                        m.Body,
                        m.SentDate,
                        CASE 
                            WHEN a.Role = 'Lecturer' THEN l.Name
                            WHEN a.Role = 'Student' THEN s.Name
                            WHEN a.Role = 'DepartmentHead' THEN dh.Name
                            ELSE a.UserName
                        END AS SenderName
                    FROM Messages m
                    INNER JOIN Auth a ON m.SenderID = a.AuthID
                    LEFT JOIN Lecturers l ON a.AuthID = l.AuthID
                    LEFT JOIN Students s ON a.AuthID = s.AuthID
                    LEFT JOIN DepartmentHeads dh ON a.AuthID = dh.AuthID
                    WHERE m.MessageID = @MessageID
                ";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@MessageID", messageID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string subject = reader["Subject"].ToString();
                                string body = reader["Body"].ToString();
                                string senderName = reader["SenderName"].ToString();
                                DateTime sentDate = Convert.ToDateTime(reader["SentDate"]);

                                MessageBox.Show($"מאת: {senderName}\nנושא: {subject}\nתאריך: {sentDate}\n\n{body}", "הודעה");
                            }
                        }
                    }

                    // סימון ההודעה כנקראה
                    string updateQuery = "UPDATE Messages SET IsRead = 1 WHERE MessageID = @MessageID";
                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, con))
                    {
                        updateCmd.Parameters.AddWithValue("@MessageID", messageID);
                        updateCmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"אירעה שגיאה בהצגת ההודעה: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// מאתחל את לשונית שליחת ההודעה
        /// </summary>
        public void InitializeSendMessageTab(TabPage tab)
        {
            Label labelRecipient = new Label() { Text = "נמען:", Location = new Point(20, 20) };
            this.comboBoxRecipient = new ComboBox() { Location = new Point(150, 20), Width = 300 };
            Label labelSubject = new Label() { Text = "נושא:", Location = new Point(20, 60) };
            this.textBoxSubject = new TextBox() { Location = new Point(150, 60), Width = 300 };
            Label labelBody = new Label() { Text = "תוכן:", Location = new Point(20, 100) };
            this.textBoxBody = new TextBox() { Location = new Point(150, 100), Width = 300, Height = 200, Multiline = true };

            Button buttonSend = new Button() { Text = "שלח", Location = new Point(150, 320) };
            buttonSend.Click += ButtonSend_Click;

            LoadRecipients(comboBoxRecipient);

            tab.Controls.Add(labelRecipient);
            tab.Controls.Add(comboBoxRecipient);
            tab.Controls.Add(labelSubject);
            tab.Controls.Add(textBoxSubject);
            tab.Controls.Add(labelBody);
            tab.Controls.Add(textBoxBody);
            tab.Controls.Add(buttonSend);
        }

        public void ButtonSend_Click(object sender, EventArgs e)
        {
            if (comboBoxRecipient.SelectedValue == null)
            {
                MessageBox.Show("יש לבחור נמען.");
                return;
            }

            int recipientId = Convert.ToInt32(comboBoxRecipient.SelectedValue);
            SendMessage(recipientId, textBoxSubject.Text, textBoxBody.Text);
        }

        /// <summary>
        /// טוען את רשימת הנמענים האפשריים
        /// </summary>
        public void LoadRecipients(ComboBox comboBox)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = @"
                    SELECT 
                        a.AuthID, 
                        a.Role,
                        CASE 
                            WHEN a.Role = 'Lecturer' THEN l.Name
                            WHEN a.Role = 'Student' THEN s.Name
                            WHEN a.Role = 'DepartmentHead' THEN dh.Name
                            ELSE a.UserName
                        END AS FullName
                    FROM Auth a
                    LEFT JOIN Lecturers l ON a.AuthID = l.AuthID
                    LEFT JOIN Students s ON a.AuthID = s.AuthID
                    LEFT JOIN DepartmentHeads dh ON a.AuthID = dh.AuthID
                    WHERE a.RegistrationStatus = 'Approved' AND a.AuthID != @AuthID
                ";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@AuthID", UserID);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        comboBox.DataSource = dt;
                        comboBox.DisplayMember = "FullName";
                        comboBox.ValueMember = "AuthID";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"אירעה שגיאה בטעינת רשימת הנמענים: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// שולח הודעה
        /// </summary>
        public void SendMessage(int recipientId, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(body))
            {
                MessageBox.Show("יש להזין נושא ותוכן להודעה.");
                return;
            }

            string recipientRole = GetUserRole(recipientId);
            string senderRole = currentUserRole;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = @"
                        INSERT INTO Messages (SenderID, RecipientID, Subject, Body, SentDate, IsRead, SenderRole, RecipientRole)
                        VALUES (@SenderID, @RecipientID, @Subject, @Body, GETDATE(), 0, @SenderRole, @RecipientRole)
                    ";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@SenderID", UserID);
                        cmd.Parameters.AddWithValue("@RecipientID", recipientId);
                        cmd.Parameters.AddWithValue("@Subject", subject);
                        cmd.Parameters.AddWithValue("@Body", body);
                        cmd.Parameters.AddWithValue("@SenderRole", senderRole);
                        cmd.Parameters.AddWithValue("@RecipientRole", recipientRole);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("ההודעה נשלחה בהצלחה.");

                        // איפוס השדות לאחר השליחה
                        comboBoxRecipient.SelectedIndex = -1;
                        textBoxSubject.Text = "";
                        textBoxBody.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"אירעה שגיאה בשליחת ההודעה: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// מחזיר את התפקיד של המשתמש
        /// </summary>
        public string GetUserRole(int authID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = "SELECT Role FROM Auth WHERE AuthID = @AuthID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@AuthID", authID);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                            return result.ToString();
                        else
                            throw new Exception("לא נמצא תפקיד עבור המשתמש.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"אירעה שגיאה בקבלת תפקיד המשתמש: {ex.Message}");
                    return string.Empty;
                }
            }
        }
    }
}