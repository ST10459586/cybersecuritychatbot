using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Dapper;
using System.Text.RegularExpressions;

namespace cybersecuritychatbot
{
    public partial class Form1 : Form
    {
        //database
        private DatabaseHelper dbHelper;
        private int currentUserId;
        private string currentUsername;

        // User profile and memory
        private UserProfile userProfile;
        private readonly List<string> conversationHistory = new List<string>();
        private readonly List<string> favoriteTopics = new List<string>();
        ///private string securityConcern = "";

        // Task management
        private readonly List<CybersecurityTask> tasks = new List<CybersecurityTask>();

        // Quiz management
        private List<QuizQuestion> quizQuestions = new List<QuizQuestion>();
        private int currentQuizQuestion = 0;
        private int quizScore = 0;

        // Activity log
        private readonly List<ActivityLogEntry> activityLog = new List<ActivityLogEntry>();

        public Form1()
        {
            InitializeComponent();
            InitializeComponents();
            dbHelper = new DatabaseHelper();
            //InitializeDatabaseConnection();
            InitializeQuiz();
            PlayWelcomeAudio();
            ShowLoginDialog();
        }
        private void AddNewTask(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(taskTitleTextBox.Text)) return;

            var task = new CybersecurityTask
            {
                Title = taskTitleTextBox.Text,
                Description = taskDescriptionTextBox.Text,
                DueDate = taskDueDatePicker.Value,
                Status = "Pending",
                ReminderDate = ParseReminder(taskDescriptionTextBox.Text) // New
            };

            tasks.Add(task);
            UpdateTasksGrid();
            AddToActivityLog($"Added task: {task.Title}");
        }

        private DateTime? ParseReminder(string description)
        {
            if (description.Contains("remind me in") || description.Contains("reminder in"))
            {
                var parts = description.Split(' ');
                if (int.TryParse(parts[parts.Length - 2], out int days))
                {
                    return DateTime.Now.AddDays(days);
                }
            }
            return null;
        }

        public class DatabaseHelper : IDisposable
        {
            private readonly SqlConnection _connection;

            public DatabaseHelper()
            {
                _connection = new SqlConnection("Data Source=labS5HDZL;Initial Catalog=BuzzByteDB;Integrated Security=True;");
                _connection.Open();
            }
            public int CreateUser(string username)
            {
                const string query = @"
            INSERT INTO Users (Username, DateCreated)
            OUTPUT INSERTED.UserId
            VALUES (@username, @dateCreated)";

                return _connection.ExecuteScalar<int>(query, new
                {
                    username,
                    dateCreated = DateTime.Now
                });
            }

            public bool UserExists(string username)
            {
                const string query = "SELECT 1 FROM Users WHERE Username = @username";
                return _connection.ExecuteScalar<bool>(query, new { username });
            }
            public int? GetUserId(string username)
            {
                const string query = "SELECT UserId FROM Users WHERE Username = @username";
                return _connection.QueryFirstOrDefault<int?>(query, new { username });
            }
            // USER PREFERENCES OPERATIONS
            public void SetUserPreference(int userId, string preferenceName, string preferenceValue)
            {
                const string query = @"
            MERGE INTO UserPreferences AS target
            USING (SELECT @userId AS UserId, @preferenceName AS PreferenceName) AS source
            ON target.UserId = source.UserId AND target.PreferenceName = source.PreferenceName
            WHEN MATCHED THEN
                UPDATE SET PreferenceValue = @preferenceValue
            WHEN NOT MATCHED THEN
                INSERT (UserId, PreferenceName, PreferenceValue)
                VALUES (@userId, @preferenceName, @preferenceValue);";  // Added semicolon here

                _connection.Execute(query, new
                {
                    userId,
                    preferenceName,
                    preferenceValue
                });
            }

            public string GetUserPreference(int userId, string preferenceName)
            {
                const string query = @"
            SELECT PreferenceValue 
            FROM UserPreferences 
            WHERE UserId = @userId AND PreferenceName = @preferenceName";

                return _connection.QueryFirstOrDefault<string>(query, new
                {
                    userId,
                    preferenceName
                });
            }

            // CONVERSATION LOGGING
            public void LogConversation(int userId, string message, bool isUserMessage)
            {
                const string query = @"
            INSERT INTO ConversationLog 
            (UserId, Message, IsUserMessage, Timestamp)
            VALUES (@userId, @message, @isUserMessage, @timestamp)";

                _connection.Execute(query, new
                {
                    userId,
                    message,
                    isUserMessage,
                    timestamp = DateTime.Now
                });
            }

            public IEnumerable<ConversationRecord> GetConversationHistory(int userId, int limit = 50)
            {
                const string query = @"
            SELECT TOP (@limit) *
            FROM ConversationLog
            WHERE UserId = @userId
            ORDER BY Timestamp DESC";

                return _connection.Query<ConversationRecord>(query, new
                {
                    userId,
                    limit
                });
            }

            // KNOWLEDGE BASE OPERATIONS
            public string GetBotResponse(string input)
            {
                const string query = @"
            SELECT TOP 1 Response
            FROM KnowledgeBase
            WHERE @input LIKE '%' + Keyword + '%'
            ORDER BY Confidence DESC, LastUsed DESC";

                return _connection.QueryFirstOrDefault<string>(query, new { input });
            }
            public void LearnResponse(string keyword, string response)
            {
                const string query = @"
            MERGE INTO KnowledgeBase AS target
            USING (SELECT @keyword AS Keyword, @response AS Response) AS source
            ON target.Keyword = source.Keyword AND target.Response = source.Response
            WHEN MATCHED THEN
                UPDATE SET Confidence = Confidence + 5, LastUsed = @now
            WHEN NOT MATCHED THEN
                INSERT (Keyword, Response, Confidence, LastUsed)
                VALUES (@keyword, @response, 80, @now);";  // Added semicolon here

                _connection.Execute(query, new
                {
                    keyword,
                    response,
                    now = DateTime.Now
                });
            }
            public void Dispose()
            {
                _connection?.Close();
                _connection?.Dispose();
            }
            //Task Assistance
            public int AddTask(CybersecurityTask task)
            {
                const string query = @"
            INSERT INTO Tasks (Title, Description, DueDate, ReminderDate, Status, UserId)
            OUTPUT INSERTED.TaskId
            VALUES (@Title, @Description, @DueDate, @ReminderDate, @Status, @UserId);";

                return _connection.ExecuteScalar<int>(query, task);
            }

            public void UpdateTask(CybersecurityTask task)
            {
                const string query = @"
            UPDATE Tasks 
            SET Title = @Title, 
                Description = @Description, 
                DueDate = @DueDate, 
                ReminderDate = @ReminderDate, 
                Status = @Status
            WHERE TaskId = @TaskId;";

                _connection.Execute(query, task);
            }

            public void DeleteTask(int taskId)
            {
                const string query = "DELETE FROM Tasks WHERE TaskId = @taskId;";
                _connection.Execute(query, new { taskId });
            }

            public List<CybersecurityTask> GetUserTasks(int userId)
            {
                const string query = "SELECT * FROM Tasks WHERE UserId = @userId ORDER BY DueDate;";
                return _connection.Query<CybersecurityTask>(query, new { userId }).ToList();
            }

            public List<CybersecurityTask> GetPendingReminders(int userId)
            {
                const string query = @"
            SELECT * FROM Tasks 
            WHERE UserId = @userId 
            AND ReminderDate IS NOT NULL 
            AND ReminderDate <= GETDATE()
            AND Status = 'Pending'
            ORDER BY ReminderDate;";

                return _connection.Query<CybersecurityTask>(query, new { userId }).ToList();
            }
        }
        private void InitializeComponents()
        {
            // Set up tab control (hidden tabs)
            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.ItemSize = new Size(0, 1);
            tabControl1.SizeMode = TabSizeMode.Fixed;

            // Set default user
            userProfile = new UserProfile("User");

            // Initialize chat tab
            chatSendButton.Click += async (sender, e) => await ProcessChatInput();
            chatInputTextBox.KeyDown += async (sender, e) =>
            {
                if (e.KeyCode == Keys.Enter) await ProcessChatInput();
            };

            // Initialize tasks tab
            tasksDataGridView.AutoGenerateColumns = false;
            tasksDataGridView.Columns.Add("Title", "Title");
            tasksDataGridView.Columns.Add("Description", "Description");
            tasksDataGridView.Columns.Add("DueDate", "Due Date");
            tasksDataGridView.Columns.Add("Status", "Status");
            addTaskButton.Click += AddNewTask;

            var completeButton = new DataGridViewButtonColumn
            {
                Text = "Complete",
                UseColumnTextForButtonValue = true
            };
            tasksDataGridView.Columns.Add(completeButton);

            // Initialize quiz tab
            quizNextButton.Click += ProcessQuizAnswer;
            tasksDataGridView.CellClick += (s, e) =>
            {
                if (e.ColumnIndex == completeButton.Index && e.RowIndex >= 0)
                {
                    tasks[e.RowIndex].Status = "Completed";
                    UpdateTasksGrid();
                }
            };

            // Initialize activity log
            UpdateActivityLog();
        }
        private void ShowLoginDialog()
        {
            using (var loginForm = new Form())
            {
                loginForm.Text = "Welcome to Cybersecurity Bot";
                loginForm.Size = new Size(300, 180);
                loginForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                loginForm.StartPosition = FormStartPosition.CenterScreen;

                var lblUsername = new Label { Text = "Enter your name:", Left = 20, Top = 20, Width = 100 };
                var txtUsername = new TextBox { Left = 120, Top = 20, Width = 150 };
                var btnLogin = new Button { Text = "Continue", Left = 120, Top = 60, Width = 80 };

                btnLogin.Click += (sender, e) =>
                {
                    if (!string.IsNullOrWhiteSpace(txtUsername.Text))
                    {
                        currentUsername = txtUsername.Text.Trim();
                        try
                        {
                            currentUserId = dbHelper.CreateUser(currentUsername);
                            loginForm.DialogResult = DialogResult.OK;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Login failed: {ex.Message}", "Error",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter your name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };

                loginForm.Controls.Add(lblUsername);
                loginForm.Controls.Add(txtUsername);
                loginForm.Controls.Add(btnLogin);

                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    WelcomeUser();
                }
                else
                {
                    Application.Exit();
                }
            }
        }
        private void WelcomeUser()
        {
            DisplayAsciiArt();
            PlayWelcomeAudio();
            // Check if this is a returning user
            bool isNewUser = Convert.ToDateTime(dbHelper.GetUserPreference(currentUserId, "FirstLogin")) == DateTime.MinValue;

            if (isNewUser)
            {
                dbHelper.SetUserPreference(currentUserId, "FirstLogin", DateTime.Now.ToString());
                AddChatMessage("BuzzByte", $"Welcome, {currentUsername}! I'm your Cybersecurity Assistant. " +
                    "I'm here to help you stay safe online. You can ask me about passwords, phishing, malware, and more!", Color.LightGreen);
            }
            else
            {
                AddChatMessage("BuzzByte", $"Welcome back, {currentUsername}! Good to see you again. " +
                    "How can I help with your cybersecurity questions today?", Color.LightGreen);
            }
        }
        private async Task ProcessChatInput()
        {
            string input = chatInputTextBox.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;

            AddChatMessage(currentUsername, input, Color.Yellow);
            dbHelper.LogConversation(currentUserId, input, true);
            chatInputTextBox.Clear();

            string response = await GetChatbotResponse(input);
            AddChatMessage("BuzzByte", response, Color.LightGreen);
            dbHelper.LogConversation(currentUserId, response, false);

            if (ShouldLearnResponse(response))
            {
                string keyword = ExtractKeyword(input);
                dbHelper.LearnResponse(keyword, response);
            }
            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                string summary = GetSessionSummary();
                AddChatMessage("BuzzByte", summary, Color.LightGreen);
                await Task.Delay(2000);
                Application.Exit();
            }
        }
        private void DisplayAsciiArt()
        {
            string asciiArt = @"
    _________________________________________________________
   |                                                         |
   |    ____                    ____        _     _          |
   |   |  _ \                  |  _ \      | |   | |         |
   |   | |_) |_   _ _____ ___  | |_) |_   _| |__ | |_   ____ |
   |   |  _ <| | | | __ /|__ / |  _ <| | | | '_ \| __| /  > \|
   |   | |_) | |_| | / /_ / /_ | |_) | |_| | |_) | |_  | |__ |
   |   |____/ \__,_|/___|/___| |____/ \__, |_.__/ \__| \____||
   |                                   __/ |                 |
   |                                  |___/                  |
   |                CYBERSECURITY AWARENESS BOT              |
   |_________________________________________________________|";

            AddChatMessage("", asciiArt, Color.BlueViolet);
        }
        private void PlayWelcomeAudio()
        {
            try
            {
                var player = new SoundPlayer("Audio.wav");
                player.Play();
                AddToActivityLog("Played welcome audio");
            }
            catch { /* Ignore audio errors */ }
        }

        
        private async Task<string> GetChatbotResponse(string input)
        {
            await Task.Delay(100); // Simulate processing

            // Log user message
            dbHelper.LogConversation(currentUserId, input, true);

            // Check database knowledge first
            string knowledgeResponse = dbHelper.GetBotResponse(input);
            if (!string.IsNullOrEmpty(knowledgeResponse))
            {
                dbHelper.LogConversation(currentUserId, knowledgeResponse, false);
                return knowledgeResponse;
            }

            // Existing response logic (from previous examples)
            string response;
            if (string.IsNullOrEmpty(knowledgeResponse))
            {
                response = await GenerateResponseAsync(input);
            }
            else
            {
                response = knowledgeResponse;
            }

            // Learn from this interaction if it's new knowledge
            if (ShouldLearnResponse(response))
            {
                string keyword = ExtractKeyword(input);
                dbHelper.LearnResponse(keyword, response);
            }

            // Log bot response
            dbHelper.LogConversation(currentUserId, response, false);

            return response;
        }

        private async Task<string> GenerateResponseAsync(string input)
        {
            await Task.Delay(100); // Simulate processing

            input = input.ToLower();
            conversationHistory.Add(input);

            // Add this at the beginning of GetChatbotResponse method
            string sentimentResponse = AnalyzeSentiment(input);
            if (!string.IsNullOrEmpty(sentimentResponse))
            {
                return sentimentResponse;
            }

            //Greetings
            if (input.Contains("hello") || input.Contains("hi") || input.Contains("hey"))
            {
                return $"Hello {userProfile.Name}! I'm your Cybersecurity Assistant. How can I help you today?";
            }

            // Check for specific commands
            if (input.Contains("task") || input.Contains("remind"))
            {
                tabControl1.SelectedTab = tasksTabPage;
                return "I've opened the task assistant for you. What would you like to do?";
            }
            if (Regex.IsMatch(input, @"\b(add|create|new|set)\b.*\b(task|reminder|todo)\b", RegexOptions.IgnoreCase))
            {
                tabControl1.SelectedTab = tasksTabPage;
                return "I’ve opened the Task Assistant. What would you like to add?";
            }

            if (input.Contains("quiz") || input.Contains("test"))
            {
                tabControl1.SelectedTab = quizTabPage;
                return "I've opened the cybersecurity quiz for you. Good luck!";
            }

            if (input.Contains("log") || input.Contains("activity"))
            {
                tabControl1.SelectedTab = activityLogTabPage;
                return "Here's your activity log.";
            }

            //Goodbyes
            if (input.Contains("bye") || input.Contains("goodbye") || input.Contains("exit"))
            {
                return "Goodbye! Remember to stay safe online. Type 'exit' to close the application";
            }

            // Bot identity
            if (input.Contains("who are you") || input.Contains("what are you"))
            {
                return "I'm your Cybersecurity Awareness Bot. My purpose is to help you learn about online safety and security best practices.";
            }

            // Bot capabilities
            if (input.Contains("what can you do") || input.Contains("help"))
            {
                return "I can help with:\n" +
                       "- Answering cybersecurity questions\n" +
                       "- Managing security tasks (type 'task')\n" +
                       "- Testing your knowledge with quizzes (type 'quiz')\n" +
                       "- Showing activity history (type 'log')\n\n" +
                       "Try asking about: passwords, phishing, malware, or privacy.";
            }

            // Mood/status questions
            if (input.Contains("how are you") || input.Contains("how's it going"))
            {
                return "I'm functioning optimally, thank you! Ready to help with any cybersecurity questions you have.";
            }

            // Check for cybersecurity topics
            if (input.Contains("password"))
            {
                favoriteTopics.Add("passwords");
                return "Password security tips:\n" +
                       "1. Use at least 12 characters\n" +
                       "2. Include numbers, symbols, and mixed case\n" +
                       "3. Don't reuse passwords across sites\n" +
                       "4. Consider using a password manager";
            }

            if (input.Contains("phishing"))
            {
                favoriteTopics.Add("phishing");
                return "Phishing protection:\n" + 
                       "1. Don't click links in suspicious emails\n" +
                       "2. Check sender email addresses carefully\n" +
                       "3. Look for spelling/grammar mistakes\n" +
                       "4. When in doubt, contact the company directly";
            }

            if (input.Contains("malware") || input.Contains("virus"))
            {
                favoriteTopics.Add("malware");
                return "Malware prevention:\n" +
                       "1. Keep your operating system and apps updated\n" +
                       "2. Use reputable antivirus software\n" +
                       "3. Don't download files from untrusted sources\n" +
                       "4. Be cautious with email attachments\n" +
                       "5. Regularly back up your important files";
            }

            if (input.Contains("privacy") || input.Contains("data protection"))
            {
                favoriteTopics.Add("privacy");
                return "Privacy protection:\n" +
                       "1. Review privacy settings on social media\n" +
                       "2. Use encrypted messaging apps\n" +
                       "3. Be careful what personal info you share online\n" +
                       "4. Use a VPN on public WiFi\n" +
                       "5. Regularly check app permissions on your devices";
            }

            if (input.Contains("social media") || input.Contains("facebook") || input.Contains("twitter"))
            {
                favoriteTopics.Add("social media");
                return "Social media safety:\n" +
                       "1. Adjust privacy settings to limit visibility\n" +
                       "2. Be cautious about sharing location/life updates\n" +
                       "3. Don't accept friend requests from strangers\n" +
                       "4. Watch out for fake accounts and scams\n" +
                       "5. Think before you post - the internet never forgets!";
            }

            if (input.Contains("public wifi") || input.Contains("coffee shop wifi"))
            {
                favoriteTopics.Add("public wifi");
                return "Public WiFi safety:\n" +
                       "1. Avoid accessing sensitive accounts\n" +
                       "2. Use a VPN if possible\n" +
                       "3. Look for 'https' in website addresses\n" +
                       "4. Turn off file sharing\n" +
                       "5. Consider using your mobile hotspot instead";
            }

            // Feature navigation
            if (input.Contains("task") || input.Contains("remind") || input.Contains("todo"))
            {
                tabControl1.SelectedTab = tasksTabPage;
                return "I've opened the Task Assistant. You can add cybersecurity tasks like:\n" +
                       "- 'Update all passwords'\n" +
                       "- 'Enable two-factor authentication'\n" +
                       "- 'Check privacy settings'\n" +
                       "- 'Backup important files'";
            }

            if (input.Contains("quiz") || input.Contains("test") || input.Contains("game"))
            {
                tabControl1.SelectedTab = quizTabPage;
                return "I've opened the Cybersecurity Quiz. Test your knowledge on:\n" +
                       "- Password security\n" +
                       "- Phishing attacks\n" +
                       "- Malware protection\n" +
                       "- Online privacy\n\n" +
                       "Good luck!";
            }

            if (input.Contains("log") || input.Contains("history") || input.Contains("activity"))
            {
                tabControl1.SelectedTab = activityLogTabPage;
                return "Here's your activity log showing recent actions:\n" +
                       "- Questions asked\n" +
                       "- Tasks created\n" +
                       "- Quiz attempts\n" +
                       "- Feature usage";
            }

            // Add these additional conditionals to the GetChatbotResponse method

            if (input.Contains("two factor") || input.Contains("2fa") || input.Contains("multi factor"))
            {
                favoriteTopics.Add("2fa");
                return "Two-Factor Authentication (2FA):\n" +
                       "1. Adds an extra layer of security beyond passwords\n" +
                       "2. Typically requires:\n" +
                       "   - Something you know (password)\n" +
                       "   - Something you have (phone, security key)\n" +
                       "3. Types of 2FA:\n" +
                       "   - SMS codes (least secure)\n" +
                       "   - Authenticator apps (more secure)\n" +
                       "   - Hardware tokens (most secure)\n" +
                       "4. Enable 2FA on:\n" +
                       "   - Email accounts\n" +
                       "   - Banking/financial sites\n" +
                       "   - Social media accounts";
            }

            if (input.Contains("ransomware"))
            {
                favoriteTopics.Add("ransomware");
                return "Ransomware Protection:\n" +
                       "1. What it is: Malware that encrypts your files and demands payment\n" +
                       "2. Prevention:\n" +
                       "   - Regular backups (follow 3-2-1 rule)\n" +
                       "   - Keep software updated\n" +
                       "   - Be cautious with email attachments\n" +
                       "3. If infected:\n" +
                       "   - Disconnect from networks immediately\n" +
                       "   - Don't pay the ransom\n" +
                       "   - Contact IT professionals\n" +
                       "   - Restore from clean backups";
            }

            if (input.Contains("backup") || input.Contains("back up"))
            {
                favoriteTopics.Add("backups");
                return "Backup Best Practices:\n" +
                       "1. Follow the 3-2-1 rule:\n" +
                       "   - 3 copies of important data\n" +
                       "   - 2 different storage types\n" +
                       "   - 1 offsite copy\n" +
                       "2. Backup options:\n" +
                       "   - External hard drives\n" +
                       "   - Cloud storage\n" +
                       "   - Network-attached storage (NAS)\n" +
                       "3. Test restores periodically\n" +
                       "4. Automate backups when possible\n" +
                       "5. Include important documents, photos, and system images";
            }

            if (input.Contains("update") || input.Contains("patch") || input.Contains("upgrade"))
            {
                favoriteTopics.Add("updates");
                return "Software Updates:\n" +
                       "1. Why they matter:\n" +
                       "   - Fix security vulnerabilities\n" +
                       "   - Patch known exploits\n" +
                       "   - Improve stability\n" +
                       "2. What to update:\n" +
                       "   - Operating system\n" +
                       "   - Web browsers\n" +
                       "   - Office software\n" +
                       "   - Security software\n" +
                       "   - Router firmware\n" +
                       "3. Enable automatic updates when possible";
            }

            if (input.Contains("scam") || input.Contains("fraud") || input.Contains("hoax"))
            {
                favoriteTopics.Add("scams");
                return "Common Online Scams:\n" +
                       "1. Phishing emails/texts\n" +
                       "2. Tech support scams\n" +
                       "3. Fake shopping websites\n" +
                       "4. Romance scams\n" +
                       "5. Investment scams\n\n" +
                       "Red flags:\n" +
                       "- Urgent requests for money\n" +
                       "- Requests for gift cards\n" +
                       "- Too-good-to-be-true offers\n" +
                       "- Pressure to act quickly";
            }
            if (input.Contains("joke") || input.Contains("funny") || input.Contains("laugh"))
            {
                var jokes = new[]
                {
        "Why don't hackers go on vacation? They're afraid of getting phished!",
        "What's a hacker's favorite type of music? Malware-iachi!",
        "Why did the password go to therapy? It had too many security issues!",
        "What do you call a security expert who's always cold? A firewall!",
        "Why was the computer cold? It left its Windows open!"
    };

                Random rand = new Random();
                return jokes[rand.Next(jokes.Length)] + "\n\nNow, back to security!";
            }

            if (input.Contains("fun fact") || input.Contains("interesting fact"))
            {
                var facts = new[]
                {
        "The first computer virus was created in 1971 and was called 'Creeper'.",
        "About 90% of security breaches start with a phishing email.",
        "The most common password is still '123456' - don't use it!",
        "It takes the average person about 3 seconds to spot a phishing email.",
        "Multi-factor authentication blocks 99.9% of automated attacks."
    };

                Random rand = new Random();
                return "Did you know?\n" + facts[rand.Next(facts.Length)];
            }
            // Default response
            return "I'm not sure I understand. You can ask about:\n" + "- Password security\n" + "- Phishing protection\n" + "- Or type 'task', 'quiz', or 'log' for other features";
        }
        private bool ShouldLearnResponse(string response)
        {
            // Don't learn greetings, jokes, etc.
            if (response.StartsWith("Hello") || response.StartsWith("Welcome") ||
                response.Contains("joke") || response.Contains("fact"))
            {
                return false;
            }
            // Don't learn if it's just listing options
            if (response.Contains("you can ask about:") || response.Contains("type '"))
            {
                return false;
            }
            // Don't learn error messages
            if (response.Contains("I'm not sure") || response.Contains("Please provide"))
            {
                return false;
            }
            // Only learn responses that contain actual cybersecurity information
            var cybersecurityKeywords = new[] { "password", "phishing", "malware", "privacy", "wifi", "scam", "virus", "hack" };
            return cybersecurityKeywords.Any(keyword => response.ToLower().Contains(keyword));
        }
        
        private string ExtractKeyword(string input)
        {
            // Simple keyword extraction - can be enhanced
            var keywords = new[] { "password", "phishing", "malware", "privacy", "wifi", "scam", "virus", "hack" };
            foreach (var keyword in keywords)
            {
                if (input.Contains(keyword))
                    return keyword;
            }

            // Fallback - first meaningful word
            var words = input.Split(new[] { ' ', '?', '!' }, StringSplitOptions.RemoveEmptyEntries);
            return words.Length > 1 ? words[1] : "general";
        }
        private string AnalyzeSentiment(string input)
        {
            if (input.Contains("?") && (input.Contains("why") || input.Contains("how")))
            {
                return "That's a great question! Let me explain...";
            }

            if (input.Contains("thank") || input.Contains("thanks") || input.Contains("appreciate"))
            {
                return "You're welcome! I'm happy to help with your cybersecurity questions.";
            }

            if (input.Contains("frustrat") || input.Contains("annoy") || input.Contains("angry"))
            {
                return "I understand cybersecurity can be frustrating. Let's work through this together.";
            }

            if (input.Contains("worri") || input.Contains("concern") || input.Contains("scare") || input.Contains("afraid"))
            {
                return "It's completely normal to have concerns about online security. The good news is there are many ways to protect yourself.";
            }

            if (input.Contains("confus") || input.Contains("don't understand") || input.Contains("not sure"))
            {
                return "Let me try to explain that differently. What specific part is confusing?";
            }

            return string.Empty;
        }

        private void AddChatMessage(string sender, string message, Color color)
        {
            chatHistoryTextBox.SelectionColor = color;
            chatHistoryTextBox.AppendText($"{sender}: {message}\n\n");
            chatHistoryTextBox.ScrollToCaret();
            AddToActivityLog($"Chat message: {message}");
        }
        private void UpdateTasksGrid()
        {
            tasksDataGridView.Rows.Clear();
            foreach (var task in tasks)
            {
                tasksDataGridView.Rows.Add(
                    task.Title,
                    task.Description,
                    task.DueDate.ToShortDateString(),
                    task.Status);
            }
        }
        private void InitializeQuiz()
        {
            quizQuestions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "What should you do if you receive an email asking for your password?",
                    Options = new List<string>
                    {
                        "Reply with your password",
                        "Delete the email",
                        "Report the email as phishing",
                        "Ignore it"
                    },
                    CorrectAnswerIndex = 2,
                    Explanation = "Correct! Reporting phishing emails helps prevent scams."
                },
                new QuizQuestion
                {
                    Question = "What is the recommended minimum length for a strong password?",
                    Options = new List<string>
                    {
                        "6 characters",
                        "8 characters",
                        "12 characters",
                        "16 characters"
                    },
                    CorrectAnswerIndex = 2,
                    Explanation = "Correct! 12 characters is the current recommended minimum length for strong passwords."
                },
                new QuizQuestion
                {
                    Question = "What does VPN stand for in cybersecurity?",
                    Options = new List<string> { "Virtual Private Network", "Verified Protected Node", "Virtual Protection Node", "Verified Private Network" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Correct! VPN stands for Virtual Private Network."
                },
                new QuizQuestion
                {
                    Question = "Which of these is NOT a common type of malware?",
                    Options = new List<string> { "Virus", "Worm", "Firewall", "Trojan" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Correct! A firewall is a security system, not malware."
                },
                new QuizQuestion
                {
                    Question = "What is the purpose of two-factor authentication?",
                    Options = new List<string> { "To make logging in faster", "To provide backup login methods", "To add an extra layer of security", "To reduce password complexity requirements" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Correct! 2FA adds an extra security layer beyond just passwords."
                },
                new QuizQuestion
                {
                    Question = "What should you do before connecting to public WiFi?",
                    Options = new List<string> { "Disable your firewall", "Enable file sharing", "Use a VPN", "Nothing special is needed" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Correct! A VPN encrypts your connection on public networks."
                },
                new QuizQuestion
                {
                    Question = "What is the 3-2-1 backup rule?",
                    Options = new List<string> { "3 backups, 2 locations, 1 must be offline", "3 days between backups, 2 copies, 1 must be encrypted", "3 types of backups, 2 must be automated, 1 manual", "3 backup methods, 2 must be cloud-based, 1 local" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Correct! 3 copies, 2 different media types, with 1 offsite."
                },
                new QuizQuestion
                {
                    Question = "What is the most secure way to handle sensitive documents?",
                    Options = new List<string> { "Email them to yourself", "Store them in cloud storage", "Encrypt them before storage", "Print and store physically" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Correct! Encryption protects data even if storage is compromised."
                },
                new QuizQuestion
{
    Question = "What is the primary goal of ransomware?",
    Options = new List<string>
    {
        "To encrypt files and demand payment",
        "To steal passwords",
        "To display ads",
        "To slow down your computer"
    },
    CorrectAnswerIndex = 0,
    Explanation = "Correct! Ransomware encrypts files and demands payment for decryption."
},
new QuizQuestion
{
    Question = "True or False: You should pay the ransom if infected with ransomware.",
    Options = new List<string> { "True", "False" },
    CorrectAnswerIndex = 1,
    Explanation = "Correct! Paying the ransom funds criminals and doesn’t guarantee file recovery."
}
            };

            LoadQuizQuestion(0);
        }

        private void LoadQuizQuestion(int index)
        {
            if (index < 0 || index >= quizQuestions.Count) return;

            var question = quizQuestions[index];
            quizQuestionLabel.Text = question.Question;
            quizOption1.Text = question.Options[0];
            quizOption2.Text = question.Options[1];
            quizOption3.Text = question.Options[2];
            quizOption4.Text = question.Options[3];
            quizOption1.Checked = false;
            quizOption2.Checked = false;
            quizOption3.Checked = false;
            quizOption4.Checked = false;
            quizFeedbackLabel.Text = "";
            quizQuestionNumberLabel.Text = $"Question {index + 1} of {quizQuestions.Count}";
        }

        private void ProcessQuizAnswer(object sender, EventArgs e)
        {
            if (!quizOption1.Checked && !quizOption2.Checked &&
                !quizOption3.Checked && !quizOption4.Checked) return;

            int selectedIndex = quizOption1.Checked ? 0 :
                               quizOption2.Checked ? 1 :
                               quizOption3.Checked ? 2 : 3;

            bool isCorrect = selectedIndex == quizQuestions[currentQuizQuestion].CorrectAnswerIndex;

            if (isCorrect) quizScore++;

            quizFeedbackLabel.Text = quizQuestions[currentQuizQuestion].Explanation;

            if (currentQuizQuestion < quizQuestions.Count - 1)
            {
                currentQuizQuestion++;
                LoadQuizQuestion(currentQuizQuestion);
            }
            else
            {
                ShowQuizResults();
            }

            AddToActivityLog($"Answered quiz question {(isCorrect ? "correctly" : "incorrectly")}");
        }

        private void ShowQuizResults()
        {
            string result = $"Quiz completed! Your score: {quizScore}/{quizQuestions.Count}\n\n";
            result += quizScore == quizQuestions.Count ?
                "Perfect! You're a cybersecurity pro!" :
                quizScore >= quizQuestions.Count / 2 ?
                "Good job! Keep learning to stay safe online." :
                "Keep practicing! Cybersecurity is important for everyone.";

            MessageBox.Show(result, "Quiz Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            AddToActivityLog($"Completed quiz with score {quizScore}/{quizQuestions.Count}");

            // Reset quiz
            currentQuizQuestion = 0;
            quizScore = 0;
            LoadQuizQuestion(0);
        }

        private void AddToActivityLog(string action)
        {
            activityLog.Add(new ActivityLogEntry
            {
                Timestamp = DateTime.Now,
                Action = action
            });

            // Keep only last 10 entries
            if (activityLog.Count > 10)
                activityLog.RemoveAt(0);

            UpdateActivityLog();
        }

        private void UpdateActivityLog()
        {
            activityLogListBox.Items.Clear();
            foreach (var entry in activityLog.OrderByDescending(e => e.Timestamp))
            {
                activityLogListBox.Items.Add($"{entry.Timestamp:HH:mm:ss} - {entry.Action}");
            }
        }

        private string GetSessionSummary()
        {
            return $"Session Summary:\n" +
                   $"Topics discussed: {favoriteTopics.Count}\n" +
                   $"Tasks created: {tasks.Count}\n" +
                   $"Quiz score: {quizScore}/{quizQuestions.Count}";
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            dbHelper?.Dispose();
            base.OnFormClosing(e);
        }
        public class ConversationRecord
        {
            public int LogId { get; set; }
            public int UserId { get; set; }
            public string Message { get; set; }
            public bool IsUserMessage { get; set; }
            public DateTime Timestamp { get; set; }
        }

        public class KnowledgeRecord
        {
            public int KnowledgeId { get; set; }
            public string Keyword { get; set; }
            public string Response { get; set; }
            public int Confidence { get; set; }
            public DateTime LastUsed { get; set; }
        }

        public class UserPreferenceRecord
        {
            public int PreferenceId { get; set; }
            public int UserId { get; set; }
            public string PreferenceName { get; set; }
            public string PreferenceValue { get; set; }
        }
        public class UserInfo
        {
            public int UserID { get; set; }
            public string Username { get; set; }
            public DateTime FirstSeen { get; set; }
            public DateTime LastSeen { get; set; }
            public bool IsActive { get; set; }
        }
        // Model classes defined here since we're not using separate files
        public class UserProfile
        {
            public string Name { get; }
            public UserProfile(string name) => Name = name;
        }
        public class CybersecurityTask
        {
            public int TaskId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime DueDate { get; set; }
            public DateTime? ReminderDate { get; set; }
            public string Status { get; set; } = "Pending";
            public int UserId { get; set; }
        }

        public class TaskReminder
        {
            public int ReminderId { get; set; }
            public int TaskId { get; set; }
            public DateTime ReminderTime { get; set; }
            public bool IsTriggered { get; set; }
        }

        public class QuizQuestion
        {
            public string Question { get; set; }
            public List<string> Options { get; set; }
            public int CorrectAnswerIndex { get; set; }
            public string Explanation { get; set; }
        }

        public class ActivityLogEntry
        {
            public DateTime Timestamp { get; set; }
            public string Action { get; set; }
        }
    }
}
