namespace cybersecuritychatbot
{
    partial class Form1
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

        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.chatTabPage = new System.Windows.Forms.TabPage();
            this.chatHistoryTextBox = new System.Windows.Forms.RichTextBox();
            this.chatInputTextBox = new System.Windows.Forms.TextBox();
            this.chatSendButton = new System.Windows.Forms.Button();
            this.tasksTabPage = new System.Windows.Forms.TabPage();
            this.tasksDataGridView = new System.Windows.Forms.DataGridView();
            this.taskTitleTextBox = new System.Windows.Forms.TextBox();
            this.taskDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.taskDueDatePicker = new System.Windows.Forms.DateTimePicker();
            this.addTaskButton = new System.Windows.Forms.Button();
            this.quizTabPage = new System.Windows.Forms.TabPage();
            this.quizQuestionLabel = new System.Windows.Forms.Label();
            this.quizOption1 = new System.Windows.Forms.RadioButton();
            this.quizOption2 = new System.Windows.Forms.RadioButton();
            this.quizOption3 = new System.Windows.Forms.RadioButton();
            this.quizOption4 = new System.Windows.Forms.RadioButton();
            this.quizNextButton = new System.Windows.Forms.Button();
            this.quizFeedbackLabel = new System.Windows.Forms.Label();
            this.quizQuestionNumberLabel = new System.Windows.Forms.Label();
            this.quizScoreLabel = new System.Windows.Forms.Label();
            this.activityLogTabPage = new System.Windows.Forms.TabPage();
            this.activityLogListBox = new System.Windows.Forms.ListBox();
            this.tabControl1.SuspendLayout();
            this.chatTabPage.SuspendLayout();
            this.tasksTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tasksDataGridView)).BeginInit();
            this.quizTabPage.SuspendLayout();
            this.activityLogTabPage.SuspendLayout();
            this.SuspendLayout();

            // tabControl1
            this.tabControl1.Controls.Add(this.chatTabPage);
            this.tabControl1.Controls.Add(this.tasksTabPage);
            this.tabControl1.Controls.Add(this.quizTabPage);
            this.tabControl1.Controls.Add(this.activityLogTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 600);
            this.tabControl1.TabIndex = 0;

            // chatTabPage
            this.chatTabPage.Controls.Add(this.chatHistoryTextBox);
            this.chatTabPage.Controls.Add(this.chatInputTextBox);
            this.chatTabPage.Controls.Add(this.chatSendButton);
            this.chatTabPage.Location = new System.Drawing.Point(4, 22);
            this.chatTabPage.Name = "chatTabPage";
            this.chatTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.chatTabPage.Size = new System.Drawing.Size(792, 574);
            this.chatTabPage.TabIndex = 0;
            this.chatTabPage.Text = "Chat";
            this.chatTabPage.UseVisualStyleBackColor = true;

            // chatHistoryTextBox
            this.chatHistoryTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.chatHistoryTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.chatHistoryTextBox.ForeColor = System.Drawing.Color.White;
            this.chatHistoryTextBox.Location = new System.Drawing.Point(3, 3);
            this.chatHistoryTextBox.Name = "chatHistoryTextBox";
            this.chatHistoryTextBox.ReadOnly = true;
            this.chatHistoryTextBox.Size = new System.Drawing.Size(786, 500);
            this.chatHistoryTextBox.TabIndex = 0;
            this.chatHistoryTextBox.Text = "";

            // chatInputTextBox
            this.chatInputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chatInputTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.chatInputTextBox.ForeColor = System.Drawing.Color.White;
            this.chatInputTextBox.Location = new System.Drawing.Point(3, 509);
            this.chatInputTextBox.Name = "chatInputTextBox";
            this.chatInputTextBox.Size = new System.Drawing.Size(690, 20);
            this.chatInputTextBox.TabIndex = 1;

            // chatSendButton
            this.chatSendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chatSendButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.chatSendButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chatSendButton.ForeColor = System.Drawing.Color.White;
            this.chatSendButton.Location = new System.Drawing.Point(699, 509);
            this.chatSendButton.Name = "chatSendButton";
            this.chatSendButton.Size = new System.Drawing.Size(90, 20);
            this.chatSendButton.TabIndex = 2;
            this.chatSendButton.Text = "Send";
            this.chatSendButton.UseVisualStyleBackColor = false;

            // tasksTabPage
            this.tasksTabPage.Controls.Add(this.tasksDataGridView);
            this.tasksTabPage.Controls.Add(this.taskTitleTextBox);
            this.tasksTabPage.Controls.Add(this.taskDescriptionTextBox);
            this.tasksTabPage.Controls.Add(this.taskDueDatePicker);
            this.tasksTabPage.Controls.Add(this.addTaskButton);
            this.tasksTabPage.Location = new System.Drawing.Point(4, 22);
            this.tasksTabPage.Name = "tasksTabPage";
            this.tasksTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.tasksTabPage.Size = new System.Drawing.Size(792, 574);
            this.tasksTabPage.TabIndex = 1;
            this.tasksTabPage.Text = "Tasks";
            this.tasksTabPage.UseVisualStyleBackColor = true;

            // tasksDataGridView
            this.tasksDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tasksDataGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.tasksDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tasksDataGridView.Location = new System.Drawing.Point(3, 3);
            this.tasksDataGridView.Name = "tasksDataGridView";
            this.tasksDataGridView.Size = new System.Drawing.Size(786, 400);
            this.tasksDataGridView.TabIndex = 0;

            // taskTitleTextBox
            this.taskTitleTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.taskTitleTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.taskTitleTextBox.ForeColor = System.Drawing.Color.White;
            this.taskTitleTextBox.Location = new System.Drawing.Point(3, 409);
            this.taskTitleTextBox.Name = "taskTitleTextBox";
            this.taskTitleTextBox.Size = new System.Drawing.Size(786, 20);
            this.taskTitleTextBox.TabIndex = 1;
            this.taskTitleTextBox.Text = "Task title";

            // taskDescriptionTextBox
            this.taskDescriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.taskDescriptionTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.taskDescriptionTextBox.ForeColor = System.Drawing.Color.White;
            this.taskDescriptionTextBox.Location = new System.Drawing.Point(3, 435);
            this.taskDescriptionTextBox.Multiline = true;
            this.taskDescriptionTextBox.Name = "taskDescriptionTextBox";
            this.taskDescriptionTextBox.Size = new System.Drawing.Size(786, 80);
            this.taskDescriptionTextBox.TabIndex = 2;
            this.taskDescriptionTextBox.Text = "Task description";

            // taskDueDatePicker
            this.taskDueDatePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.taskDueDatePicker.Location = new System.Drawing.Point(3, 521);
            this.taskDueDatePicker.Name = "taskDueDatePicker";
            this.taskDueDatePicker.Size = new System.Drawing.Size(200, 20);
            this.taskDueDatePicker.TabIndex = 3;

            // addTaskButton
            this.addTaskButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addTaskButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.addTaskButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addTaskButton.ForeColor = System.Drawing.Color.White;
            this.addTaskButton.Location = new System.Drawing.Point(699, 521);
            this.addTaskButton.Name = "addTaskButton";
            this.addTaskButton.Size = new System.Drawing.Size(90, 23);
            this.addTaskButton.TabIndex = 4;
            this.addTaskButton.Text = "Add Task";
            this.addTaskButton.UseVisualStyleBackColor = false;

            // quizTabPage
            this.quizTabPage.Controls.Add(this.quizQuestionLabel);
            this.quizTabPage.Controls.Add(this.quizOption1);
            this.quizTabPage.Controls.Add(this.quizOption2);
            this.quizTabPage.Controls.Add(this.quizOption3);
            this.quizTabPage.Controls.Add(this.quizOption4);
            this.quizTabPage.Controls.Add(this.quizNextButton);
            this.quizTabPage.Controls.Add(this.quizFeedbackLabel);
            this.quizTabPage.Controls.Add(this.quizQuestionNumberLabel);
            this.quizTabPage.Controls.Add(this.quizScoreLabel);
            this.quizTabPage.Location = new System.Drawing.Point(4, 22);
            this.quizTabPage.Name = "quizTabPage";
            this.quizTabPage.Size = new System.Drawing.Size(792, 574);
            this.quizTabPage.TabIndex = 2;
            this.quizTabPage.Text = "Quiz";
            this.quizTabPage.UseVisualStyleBackColor = true;

            // quizQuestionLabel
            this.quizQuestionLabel.AutoSize = true;
            this.quizQuestionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quizQuestionLabel.ForeColor = System.Drawing.Color.White;
            this.quizQuestionLabel.Location = new System.Drawing.Point(20, 20);
            this.quizQuestionLabel.MaximumSize = new System.Drawing.Size(750, 0);
            this.quizQuestionLabel.Name = "quizQuestionLabel";
            this.quizQuestionLabel.Size = new System.Drawing.Size(71, 20);
            this.quizQuestionLabel.TabIndex = 0;
            this.quizQuestionLabel.Text = "Question";

            // quizOption1
            this.quizOption1.AutoSize = true;
            this.quizOption1.ForeColor = System.Drawing.Color.White;
            this.quizOption1.Location = new System.Drawing.Point(40, 60);
            this.quizOption1.Name = "quizOption1";
            this.quizOption1.Size = new System.Drawing.Size(85, 17);
            this.quizOption1.TabIndex = 1;
            this.quizOption1.TabStop = true;
            this.quizOption1.Text = "radioButton1";
            this.quizOption1.UseVisualStyleBackColor = true;

            // quizOption2
            this.quizOption2.AutoSize = true;
            this.quizOption2.ForeColor = System.Drawing.Color.White;
            this.quizOption2.Location = new System.Drawing.Point(40, 90);
            this.quizOption2.Name = "quizOption2";
            this.quizOption2.Size = new System.Drawing.Size(85, 17);
            this.quizOption2.TabIndex = 2;
            this.quizOption2.TabStop = true;
            this.quizOption2.Text = "radioButton2";
            this.quizOption2.UseVisualStyleBackColor = true;

            // quizOption3
            this.quizOption3.AutoSize = true;
            this.quizOption3.ForeColor = System.Drawing.Color.White;
            this.quizOption3.Location = new System.Drawing.Point(40, 120);
            this.quizOption3.Name = "quizOption3";
            this.quizOption3.Size = new System.Drawing.Size(85, 17);
            this.quizOption3.TabIndex = 3;
            this.quizOption3.TabStop = true;
            this.quizOption3.Text = "radioButton3";
            this.quizOption3.UseVisualStyleBackColor = true;

            // quizOption4
            this.quizOption4.AutoSize = true;
            this.quizOption4.ForeColor = System.Drawing.Color.White;
            this.quizOption4.Location = new System.Drawing.Point(40, 150);
            this.quizOption4.Name = "quizOption4";
            this.quizOption4.Size = new System.Drawing.Size(85, 17);
            this.quizOption4.TabIndex = 4;
            this.quizOption4.TabStop = true;
            this.quizOption4.Text = "radioButton4";
            this.quizOption4.UseVisualStyleBackColor = true;

            // quizNextButton
            this.quizNextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.quizNextButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.quizNextButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.quizNextButton.ForeColor = System.Drawing.Color.White;
            this.quizNextButton.Location = new System.Drawing.Point(690, 540);
            this.quizNextButton.Name = "quizNextButton";
            this.quizNextButton.Size = new System.Drawing.Size(90, 23);
            this.quizNextButton.TabIndex = 5;
            this.quizNextButton.Text = "Next";
            this.quizNextButton.UseVisualStyleBackColor = false;

            // quizFeedbackLabel
            this.quizFeedbackLabel.AutoSize = true;
            this.quizFeedbackLabel.ForeColor = System.Drawing.Color.LightGreen;
            this.quizFeedbackLabel.Location = new System.Drawing.Point(20, 190);
            this.quizFeedbackLabel.MaximumSize = new System.Drawing.Size(750, 0);
            this.quizFeedbackLabel.Name = "quizFeedbackLabel";
            this.quizFeedbackLabel.Size = new System.Drawing.Size(55, 13);
            this.quizFeedbackLabel.TabIndex = 6;
            this.quizFeedbackLabel.Text = "Feedback";

            // quizQuestionNumberLabel
            this.quizQuestionNumberLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.quizQuestionNumberLabel.AutoSize = true;
            this.quizQuestionNumberLabel.ForeColor = System.Drawing.Color.White;
            this.quizQuestionNumberLabel.Location = new System.Drawing.Point(20, 550);
            this.quizQuestionNumberLabel.Name = "quizQuestionNumberLabel";
            this.quizQuestionNumberLabel.Size = new System.Drawing.Size(82, 13);
            this.quizQuestionNumberLabel.TabIndex = 7;
            this.quizQuestionNumberLabel.Text = "Question 1 of 10";

            // quizScoreLabel
            this.quizScoreLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.quizScoreLabel.AutoSize = true;
            this.quizScoreLabel.ForeColor = System.Drawing.Color.White;
            this.quizScoreLabel.Location = new System.Drawing.Point(120, 550);
            this.quizScoreLabel.Name = "quizScoreLabel";
            this.quizScoreLabel.Size = new System.Drawing.Size(41, 13);
            this.quizScoreLabel.TabIndex = 8;
            this.quizScoreLabel.Text = "Score: 0";

            // activityLogTabPage
            this.activityLogTabPage.Controls.Add(this.activityLogListBox);
            this.activityLogTabPage.Location = new System.Drawing.Point(4, 22);
            this.activityLogTabPage.Name = "activityLogTabPage";
            this.activityLogTabPage.Size = new System.Drawing.Size(792, 574);
            this.activityLogTabPage.TabIndex = 3;
            this.activityLogTabPage.Text = "Activity Log";
            this.activityLogTabPage.UseVisualStyleBackColor = true;

            // activityLogListBox
            this.activityLogListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.activityLogListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.activityLogListBox.ForeColor = System.Drawing.Color.White;
            this.activityLogListBox.FormattingEnabled = true;
            this.activityLogListBox.Location = new System.Drawing.Point(0, 0);
            this.activityLogListBox.Name = "activityLogListBox";
            this.activityLogListBox.Size = new System.Drawing.Size(792, 574);
            this.activityLogListBox.TabIndex = 0;

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Cybersecurity Awareness Bot";
            this.tabControl1.ResumeLayout(false);
            this.chatTabPage.ResumeLayout(false);
            this.chatTabPage.PerformLayout();
            this.tasksTabPage.ResumeLayout(false);
            this.tasksTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tasksDataGridView)).EndInit();
            this.quizTabPage.ResumeLayout(false);
            this.quizTabPage.PerformLayout();
            this.activityLogTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage chatTabPage;
        private System.Windows.Forms.RichTextBox chatHistoryTextBox;
        private System.Windows.Forms.TextBox chatInputTextBox;
        private System.Windows.Forms.Button chatSendButton;
        private System.Windows.Forms.TabPage tasksTabPage;
        private System.Windows.Forms.DataGridView tasksDataGridView;
        private System.Windows.Forms.TextBox taskTitleTextBox;
        private System.Windows.Forms.TextBox taskDescriptionTextBox;
        private System.Windows.Forms.DateTimePicker taskDueDatePicker;
        private System.Windows.Forms.Button addTaskButton;
        private System.Windows.Forms.TabPage quizTabPage;
        private System.Windows.Forms.Label quizQuestionLabel;
        private System.Windows.Forms.RadioButton quizOption1;
        private System.Windows.Forms.RadioButton quizOption2;
        private System.Windows.Forms.RadioButton quizOption3;
        private System.Windows.Forms.RadioButton quizOption4;
        private System.Windows.Forms.Button quizNextButton;
        private System.Windows.Forms.Label quizFeedbackLabel;
        private System.Windows.Forms.Label quizQuestionNumberLabel;
        private System.Windows.Forms.Label quizScoreLabel;
        private System.Windows.Forms.TabPage activityLogTabPage;
        private System.Windows.Forms.ListBox activityLogListBox;
    }
}


