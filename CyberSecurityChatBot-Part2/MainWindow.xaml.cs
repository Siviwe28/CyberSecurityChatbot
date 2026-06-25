using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CyberSecurityChatbotWPF.Classes;
using CyberSecurityChatbotWPF.Services;
using CyberSecurityChatbotWPF.Utilities;
using CyberSecurityChatbotWPF.Models;
using System.Linq;
using System.Text.RegularExpressions;

namespace CyberSecurityChatbot_Part2
{
    public partial class MainWindow : Window
    {
        private EnhancedRespondHandler respondHandler;
        private string userName = "Guest";
        private MediaPlayer mediaPlayer;
        private TaskManager taskManager;
        private QuizManager quizManager;
        private List<TaskItem> currentTasks;
        private bool quizAnswered = false;
        private bool showFullLog = false;
        private string lastTaskAdded = "";
        private string waitingForReminderResponse = "";

        private Dictionary<string, List<string>> sentimentResponses;

        public MainWindow()
        {
            InitializeComponent();
            respondHandler = new EnhancedRespondHandler();
            taskManager = new TaskManager();
            quizManager = new QuizManager();

            InitializeSentimentResponses();
            PlayStartupSound();
            this.Loaded += MainWindow_Loaded;
            WireUpEventHandlers();
            LoadTasks();
            UpdateLogDisplay();
        }

        private void InitializeSentimentResponses()
        {
            sentimentResponses = new Dictionary<string, List<string>>()
            {
                ["worried"] = new List<string>
                {
                    "It's completely understandable to feel that way. Cybersecurity threats can be concerning. Let me share some tips to help you stay safe.",
                    "I understand your concern. Online safety is important. Here are some practical steps you can take to protect yourself.",
                    "Don't worry, you're not alone in feeling this way. Let me give you some guidance on how to stay secure online."
                },
                ["scared"] = new List<string>
                {
                    "I understand this can be scary. The good news is there are simple steps you can take to protect yourself. Let me help you.",
                    "Feeling scared is natural when it comes to cybersecurity. Let me show you how to build your defenses step by step.",
                    "Take a deep breath. Cybersecurity can seem intimidating, but I'm here to guide you through it. Here's what you can do."
                },
                ["curious"] = new List<string>
                {
                    "It's great that you're curious about cybersecurity! That's the first step to staying safe online. Here's what you should know.",
                    "I love your curiosity! Cybersecurity is fascinating. Let me share some interesting and important information with you.",
                    "Curiosity is a superpower in cybersecurity! Let me give you some valuable insights to satisfy your interest."
                },
                ["frustrated"] = new List<string>
                {
                    "I know cybersecurity can be frustrating at times. Let me break this down in a simpler way for you.",
                    "I understand the frustration. Cybersecurity can be complex. Let me explain this in plain language so it's easier to understand.",
                    "Don't let the frustration get to you. Let me simplify this for you so you can feel more confident about your security."
                },
                ["happy"] = new List<string>
                {
                    "I'm glad to hear that! Cybersecurity awareness is something to be proud of. You're on the right track!",
                    "That's wonderful to hear! Staying positive about security is a great mindset. Keep up the good work!"
                },
                ["confused"] = new List<string>
                {
                    "I can see this might be confusing. Let me explain it more clearly for you.",
                    "Cybersecurity can be confusing at first. Let me break this down step by step so it makes more sense."
                },
                ["angry"] = new List<string>
                {
                    "I understand your frustration. Cybersecurity issues can be upsetting. Let me help you address this calmly.",
                    "I can sense your frustration. Let me help you understand this better so you can take control of the situation."
                }
            };
        }

        private void WireUpEventHandlers()
        {
            SendButton.Click += SendButton_Click;
            MessageTextBox.KeyDown += UserInputTextBox_KeyDown;

            AddTaskButton.Click += AddTaskButton_Click;
            MarkCompleteButton.Click += MarkCompleteButton_Click;
            DeleteTaskButton.Click += DeleteTaskButton_Click;

            StartQuizButton.Click += StartQuizButton_Click;
            ResetQuizButton.Click += ResetQuizButton_Click;
            NextQuestionButton.Click += NextQuestionButton_Click;
            FinalResultsButton.Click += FinalResultsButton_Click;

            RefreshLogButton.Click += RefreshLogButton_Click;
            ShowMoreLogButton.Click += ShowMoreLogButton_Click;
        }

        private void PlayStartupSound()
        {
            try
            {
                mediaPlayer = new MediaPlayer();
                string[] possiblePaths = {
                    "Audio/Greeting.wav",
                    System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Audio", "Greeting.wav")
                };

                bool soundFound = false;
                foreach (string soundPath in possiblePaths)
                {
                    if (System.IO.File.Exists(soundPath))
                    {
                        mediaPlayer.Open(new Uri(soundPath, UriKind.RelativeOrAbsolute));
                        mediaPlayer.Play();
                        soundFound = true;
                        break;
                    }
                }

                if (!soundFound)
                {
                    System.Media.SystemSounds.Beep.Play();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Could not play startup sound: {ex.Message}");
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AskForUserName();
        }

        private void AskForUserName()
        {
            var nameDialog = new Window
            {
                Title = "Welcome to Cybersecurity Bot!",
                Width = 400,
                Height = 200,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                ResizeMode = ResizeMode.NoResize,
                Background = new SolidColorBrush(Color.FromRgb(26, 26, 46))
            };

            var mainPanel = new StackPanel { Margin = new Thickness(20) };

            var titleText = new TextBlock
            {
                Text = "Welcome to Cybersecurity Awareness Bot",
                Foreground = Brushes.White,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 15)
            };

            var instructionText = new TextBlock
            {
                Text = "Please enter your name:",
                Foreground = Brushes.White,
                FontSize = 14,
                Margin = new Thickness(0, 0, 0, 10)
            };

            var textBox = new TextBox
            {
                Margin = new Thickness(0, 0, 0, 15),
                Padding = new Thickness(10),
                FontSize = 14,
                Height = 35,
                Background = Brushes.White,
                Foreground = Brushes.Black
            };

            var button = new Button
            {
                Content = "Start Chatting",
                Background = new SolidColorBrush(Color.FromRgb(52, 152, 219)),
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(15, 8, 15, 8),
                FontSize = 14,
                Cursor = Cursors.Hand,
                Height = 35
            };

            button.Click += (s, ev) =>
            {
                if (!string.IsNullOrWhiteSpace(textBox.Text))
                {
                    userName = textBox.Text.Trim();
                    nameDialog.Close();
                    StartPersonalizedChat();
                }
                else
                {
                    MessageBox.Show("Please enter your name!", "Name Required",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            };

            textBox.KeyDown += (s, ev) =>
            {
                if (ev.Key == Key.Enter)
                    button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            };

            textBox.Focus();
            mainPanel.Children.Add(titleText);
            mainPanel.Children.Add(instructionText);
            mainPanel.Children.Add(textBox);
            mainPanel.Children.Add(button);
            nameDialog.Content = mainPanel;
            nameDialog.ShowDialog();
        }

        private void StartPersonalizedChat()
        {
            AddBotMessage($"Welcome to the Cybersecurity Awareness Bot, {userName}!");
            AddBotMessage("Ask me about passwords, phishing, privacy, or scams!");
            AddBotMessage("You can also add tasks, play the quiz, or view activity logs!");
            AddBotMessage("I can detect your feelings too - just tell me how you're feeling about cybersecurity!");

            if (StatusTextBlock != null)
                StatusTextBlock.Text = $"Ready, {userName}!";
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void UserInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }

        private void SendMessage()
        {
            string userInput = MessageTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                if (StatusTextBlock != null)
                    StatusTextBlock.Text = "Please enter a message";
                return;
            }

            AddUserMessage(userInput);
            MessageTextBox.Clear();

            // FIRST: Check if we're waiting for a reminder response
            if (!string.IsNullOrEmpty(waitingForReminderResponse) && !string.IsNullOrEmpty(lastTaskAdded))
            {
                string lower = userInput.ToLower().Trim();

                if (waitingForReminderResponse == "yesno")
                {
                    if (lower == "yes" || lower == "yeah" || lower == "yep" || lower == "sure" ||
                        lower == "ok" || lower == "okay" || lower == "y" || lower == "ye" ||
                        lower.Contains("yes") || lower.Contains("sure") || lower.Contains("okay"))
                    {
                        waitingForReminderResponse = "days";
                        AddBotMessage($"Great! How many days from now would you like to be reminded about '{lastTaskAdded}'?");
                        if (StatusTextBlock != null)
                            StatusTextBlock.Text = "Waiting for number of days";
                        return;
                    }
                    else if (lower == "no" || lower == "nah" || lower == "nope" || lower == "n" ||
                             lower.Contains("no") || lower.Contains("nope"))
                    {
                        lastTaskAdded = "";
                        waitingForReminderResponse = "";
                        AddBotMessage("Okay, no reminder set for this task.");
                        if (StatusTextBlock != null)
                            StatusTextBlock.Text = "No reminder set";
                        return;
                    }
                    else
                    {
                        waitingForReminderResponse = "";
                    }
                }
                else if (waitingForReminderResponse == "days")
                {
                    int days = ExtractNumberOfDays(userInput);
                    if (days > 0)
                    {
                        string reminderText = days == 1 ? "1 day" : $"{days} days";
                        string result = taskManager.AddTask(lastTaskAdded, lastTaskAdded, reminderText);
                        ActivityLogger.Log($"Reminder set via NLP for: '{lastTaskAdded}' on {reminderText}");
                        LoadTasks();
                        string temp = lastTaskAdded;
                        lastTaskAdded = "";
                        waitingForReminderResponse = "";
                        AddBotMessage($"Got it! I'll remind you about '{temp}' in {reminderText}.");
                        if (StatusTextBlock != null)
                            StatusTextBlock.Text = $"Reminder set for '{temp}' in {reminderText}";
                        return;
                    }
                    else
                    {
                        AddBotMessage("Please enter a valid number of days (e.g., 3, 5, 7). How many days from now?");
                        return;
                    }
                }
            }

            // SECOND: Check for sentiment detection
            string sentimentResponse = ProcessSentimentDetection(userInput);
            if (sentimentResponse != null)
            {
                AddBotMessage(sentimentResponse);
                string tip = GetCybersecurityTipForSentiment(userInput);
                if (!string.IsNullOrEmpty(tip))
                {
                    AddBotMessage(tip);
                }
                if (StatusTextBlock != null)
                    StatusTextBlock.Text = $"Sentiment detected in message";
                return;
            }

            // THIRD: Check for NLP commands
            string response = ProcessNLPCommand(userInput);
            if (response != null)
            {
                AddBotMessage(response);
                if (userInput.ToLower().Contains("add task") || userInput.ToLower().Contains("remind me") ||
                    userInput.ToLower().Contains("task") || userInput.ToLower().Contains("enable") ||
                    userInput.ToLower().Contains("set up"))
                {
                    LoadTasks();
                }
                if (StatusTextBlock != null)
                    StatusTextBlock.Text = $"Ready, {userName}!";
                return;
            }

            // FOURTH: Fall back to existing handler
            EnhancedRespondHandler.Sentiment sentiment;
            response = respondHandler.GetResponse(userInput, out sentiment);

            string empathyResponse = GetEmpathyResponse(sentiment);
            if (!string.IsNullOrEmpty(empathyResponse))
            {
                AddBotMessage(empathyResponse);
            }

            AddBotMessage(response);
            UpdateSentimentStatus(sentiment);

            if (StatusTextBlock != null)
                StatusTextBlock.Text = $"Ready, {userName}!";
        }

        private int ExtractNumberOfDays(string input)
        {
            string lower = input.ToLower().Trim();

            if (int.TryParse(lower, out int days))
            {
                return days > 0 ? days : 0;
            }

            Regex dayRegex = new Regex(@"(\d+)\s*days?");
            Match match = dayRegex.Match(lower);
            if (match.Success)
            {
                if (int.TryParse(match.Groups[1].Value, out int result))
                {
                    return result > 0 ? result : 0;
                }
            }

            if (lower.Contains("day") || lower.Contains("days"))
            {
                string[] parts = lower.Split(' ');
                for (int i = 0; i < parts.Length; i++)
                {
                    if ((parts[i].ToLower().Contains("day") || parts[i].ToLower().Contains("days")) && i > 0)
                    {
                        if (int.TryParse(parts[i - 1], out int result))
                        {
                            return result > 0 ? result : 0;
                        }
                    }
                }
            }

            if (lower.Contains("tomorrow"))
                return 1;

            if (lower.Contains("week"))
                return 7;

            if (lower.Contains("month"))
                return 30;

            return 0;
        }

        private string ProcessSentimentDetection(string input)
        {
            string lower = input.ToLower();

            if (lower.Contains("worried") || lower.Contains("concerned") || lower.Contains("nervous") ||
                lower.Contains("anxious") || lower.Contains("stress") || lower.Contains("worried about"))
            {
                ActivityLogger.Log($"Sentiment detected: Worried from '{input}'");
                return GetRandomSentimentResponse("worried");
            }

            if (lower.Contains("scared") || lower.Contains("afraid") || lower.Contains("terrified") ||
                lower.Contains("fear") || lower.Contains("frightened") || lower.Contains("panic"))
            {
                ActivityLogger.Log($"Sentiment detected: Scared from '{input}'");
                return GetRandomSentimentResponse("scared");
            }

            if (lower.Contains("curious") || lower.Contains("wonder") || lower.Contains("interested") ||
                lower.Contains("learn") || lower.Contains("want to know") || lower.Contains("tell me more"))
            {
                ActivityLogger.Log($"Sentiment detected: Curious from '{input}'");
                return GetRandomSentimentResponse("curious");
            }

            if (lower.Contains("frustrated") || lower.Contains("annoyed") || lower.Contains("irritated") ||
                lower.Contains("fed up") || lower.Contains("tired") || lower.Contains("confused") ||
                lower.Contains("don't understand") || lower.Contains("doesn't make sense"))
            {
                ActivityLogger.Log($"Sentiment detected: Frustrated from '{input}'");
                return GetRandomSentimentResponse("frustrated");
            }

            if (lower.Contains("happy") || lower.Contains("great") || lower.Contains("good") ||
                lower.Contains("excited") || lower.Contains("awesome") || lower.Contains("fantastic") ||
                lower.Contains("wonderful") || lower.Contains("love") || lower.Contains("enjoy"))
            {
                ActivityLogger.Log($"Sentiment detected: Happy from '{input}'");
                return GetRandomSentimentResponse("happy");
            }

            if (lower.Contains("angry") || lower.Contains("mad") || lower.Contains("upset") ||
                lower.Contains("furious") || lower.Contains("rage") || lower.Contains("annoying"))
            {
                ActivityLogger.Log($"Sentiment detected: Angry from '{input}'");
                return GetRandomSentimentResponse("angry");
            }

            return null;
        }

        private string GetRandomSentimentResponse(string sentimentType)
        {
            if (sentimentResponses.ContainsKey(sentimentType))
            {
                var responses = sentimentResponses[sentimentType];
                Random random = new Random();
                int index = random.Next(responses.Count);
                return responses[index];
            }
            return null;
        }

        private string GetCybersecurityTipForSentiment(string input)
        {
            string lower = input.ToLower();
            string[] tips = {
                "Always use strong passwords with at least 12 characters, including numbers, symbols, and both cases.",
                "Never click on suspicious links in emails. Always verify the sender's address.",
                "Enable two-factor authentication on all your important accounts for extra security.",
                "Regularly update your software and operating system to protect against vulnerabilities.",
                "Be cautious of unsolicited phone calls asking for personal information. Hang up and call back using official numbers.",
                "Use a VPN when connecting to public Wi-Fi to encrypt your internet traffic.",
                "Back up your important files regularly to protect against ransomware attacks.",
                "Review your social media privacy settings to control who can see your information.",
                "Never share your passwords with anyone, even if they claim to be from tech support.",
                "Look for HTTPS and the padlock icon in the address bar before entering sensitive information online."
            };

            Random random = new Random();
            int index = random.Next(tips.Length);

            if (lower.Contains("scam") || lower.Contains("phishing"))
            {
                return "Tip: Always verify email sender addresses before clicking links. Scammers spoof legitimate domains. Never share personal information via email.";
            }
            else if (lower.Contains("password"))
            {
                return "Tip: Use unique passwords for each account and consider using a password manager to generate and store complex passwords securely.";
            }
            else if (lower.Contains("privacy"))
            {
                return "Tip: Check app permissions on your phone. Some apps request unnecessary access to your data. Limit what you share on social media.";
            }
            else if (lower.Contains("safe") || lower.Contains("browsing"))
            {
                return "Tip: Look for HTTPS and the padlock icon in the address bar before entering sensitive information. Avoid using public computers for banking.";
            }
            else if (lower.Contains("2fa") || lower.Contains("two factor") || lower.Contains("authentication"))
            {
                return "Tip: Enable two-factor authentication wherever possible. It adds an extra layer of security beyond just your password.";
            }
            else
            {
                return tips[index];
            }
        }

        private string GetEmpathyResponse(EnhancedRespondHandler.Sentiment sentiment)
        {
            switch (sentiment)
            {
                case EnhancedRespondHandler.Sentiment.Worried:
                    return "I understand your concern. Let me help you with this.";
                case EnhancedRespondHandler.Sentiment.Curious:
                    return "That's a great question to ask! Let me share some useful information.";
                case EnhancedRespondHandler.Sentiment.Frustrated:
                    return "I know this can be frustrating. Let me simplify this for you.";
                case EnhancedRespondHandler.Sentiment.Happy:
                    return "That's wonderful to hear! Keep up the good security practices!";
                default:
                    return "";
            }
        }

        private string ProcessNLPCommand(string input)
        {
            string lower = input.ToLower().Trim();

            if (lower.Contains("add task") || lower.Contains("add a task") || lower.Contains("create task") ||
                lower.Contains("create a task") || lower.Contains("new task") || lower.Contains("task:") ||
                lower.Contains("task -"))
            {
                ActivityLogger.Log($"NLP recognised task intent from: '{input}'");
                string taskName = ExtractTaskNameUniversal(input);
                if (!string.IsNullOrEmpty(taskName) && taskName.Length > 0)
                {
                    string result = taskManager.AddTask(taskName, taskName, "");
                    lastTaskAdded = taskName;
                    waitingForReminderResponse = "yesno";
                    ActivityLogger.Log($"Task added via NLP: '{taskName}'");
                    LoadTasks();
                    return result + " Would you like to set a reminder? (Yes/No)";
                }
                return "I couldn't understand the task. Please specify what task to add. Example: 'Add task - Review privacy settings'";
            }

            if (lower.StartsWith("enable ") || lower.StartsWith("set up ") ||
                lower.Contains(" enable ") || lower.Contains(" set up "))
            {
                ActivityLogger.Log($"NLP recognised enable/setup intent from: '{input}'");
                string taskName = ExtractTaskNameUniversal(input);
                if (!string.IsNullOrEmpty(taskName) && taskName.Length > 0)
                {
                    string result = taskManager.AddTask(taskName, taskName, "");
                    lastTaskAdded = taskName;
                    waitingForReminderResponse = "yesno";
                    ActivityLogger.Log($"Task added via NLP: '{taskName}'");
                    LoadTasks();
                    return result + " Would you like to set a reminder? (Yes/No)";
                }
            }

            if (lower.Contains("remind me") || lower.Contains("reminder") || lower.Contains("set a reminder") ||
                lower.Contains("remind me to") || lower.Contains("remind me about") || lower.Contains("don't forget"))
            {
                ActivityLogger.Log($"NLP recognised reminder intent from: '{input}'");
                string reminderText = ExtractReminderTextUniversal(input);
                string reminderTime = ExtractReminderTimeUniversal(input);
                if (!string.IsNullOrEmpty(reminderText))
                {
                    string result = taskManager.AddTask(reminderText, reminderText, reminderTime);
                    ActivityLogger.Log($"Reminder set via NLP: '{reminderText}' on {reminderTime}");
                    LoadTasks();
                    return result + $" Reminder set for {reminderTime}.";
                }
                return "I couldn't understand the reminder. Please specify what to be reminded about.";
            }

            if (lower.Contains("start quiz") || lower.Contains("take quiz") || lower.Contains("test my knowledge") ||
                lower.Contains("quiz me") || lower.Contains("play the game") || lower.Contains("start the quiz") ||
                lower.Contains("do the quiz") || lower.Contains("begin quiz"))
            {
                ActivityLogger.Log("NLP recognised quiz intent");
                StartQuiz();
                return "Starting the cybersecurity quiz! Answer the questions one at a time.";
            }

            if (lower.Contains("show activity log") || lower.Contains("what have you done") ||
                lower.Contains("what did you do") || lower.Contains("show log") || lower.Contains("recent actions") ||
                lower.Contains("what have you done for me") || lower.Contains("show me the log") ||
                lower.Contains("activity log") || lower.Contains("what actions"))
            {
                ActivityLogger.Log("NLP recognised log intent");
                return GetLogDisplay();
            }

            return null;
        }

        private string ExtractTaskNameUniversal(string input)
        {
            if (input.Contains(" - "))
            {
                int index = input.IndexOf(" - ") + 3;
                if (index < input.Length)
                {
                    string task = input.Substring(index).Trim();
                    if (!string.IsNullOrEmpty(task))
                        return task;
                }
            }

            if (input.Contains(": "))
            {
                int index = input.IndexOf(": ") + 2;
                if (index < input.Length)
                {
                    string task = input.Substring(index).Trim();
                    if (!string.IsNullOrEmpty(task))
                        return task;
                }
            }

            string[] keywords = { "add task", "add a task", "create task", "create a task", "new task", "enable", "set up" };
            foreach (string keyword in keywords)
            {
                if (input.ToLower().Contains(keyword))
                {
                    int index = input.ToLower().IndexOf(keyword) + keyword.Length;
                    if (index < input.Length)
                    {
                        string task = input.Substring(index).Trim();
                        string[] removeWords = { "to ", "for ", "a ", "an " };
                        foreach (string word in removeWords)
                        {
                            if (task.ToLower().StartsWith(word))
                            {
                                task = task.Substring(word.Length).Trim();
                                break;
                            }
                        }
                        if (!string.IsNullOrEmpty(task))
                            return task;
                    }
                }
            }

            if (input.ToLower().Contains("task:"))
            {
                int index = input.ToLower().IndexOf("task:") + 5;
                if (index < input.Length)
                {
                    string task = input.Substring(index).Trim();
                    if (!string.IsNullOrEmpty(task))
                        return task;
                }
            }
            if (input.ToLower().Contains("task -"))
            {
                int index = input.ToLower().IndexOf("task -") + 6;
                if (index < input.Length)
                {
                    string task = input.Substring(index).Trim();
                    if (!string.IsNullOrEmpty(task))
                        return task;
                }
            }

            if (input.ToLower().StartsWith("enable "))
            {
                return input.Substring(7).Trim();
            }
            if (input.ToLower().StartsWith("set up "))
            {
                return input.Substring(7).Trim();
            }

            if (input.Contains(" -"))
            {
                int index = input.IndexOf(" -") + 2;
                if (index < input.Length)
                {
                    string task = input.Substring(index).Trim();
                    if (!string.IsNullOrEmpty(task))
                        return task;
                }
            }

            string[] allKeywords = { "add task", "add a task", "create task", "create a task", "new task" };
            foreach (string keyword in allKeywords)
            {
                if (input.ToLower().Contains(keyword))
                {
                    int index = input.ToLower().IndexOf(keyword) + keyword.Length;
                    if (index < input.Length)
                    {
                        string task = input.Substring(index).Trim();
                        if (!string.IsNullOrEmpty(task))
                            return task;
                    }
                }
            }

            if (input.Length < 30 && !input.Contains("?") && !input.Contains("how") && !input.Contains("what"))
            {
                return input.Trim();
            }

            return "";
        }

        private string ExtractReminderTextUniversal(string input)
        {
            string lower = input.ToLower();
            string cleaned = input;

            string[] timeWords = { " tomorrow", " today", " in ", " days", " day", " week", " weeks", " month", " months" };
            foreach (string word in timeWords)
            {
                if (lower.Contains(word))
                {
                    int index = lower.IndexOf(word);
                    if (index > 0)
                    {
                        cleaned = input.Substring(0, index).Trim();
                        break;
                    }
                }
            }

            string[] keywords = { "remind me to", "remind me", "set a reminder for", "reminder for",
                                  "remind me about", "don't forget to", "don't forget" };
            foreach (string keyword in keywords)
            {
                if (lower.Contains(keyword))
                {
                    int index = lower.IndexOf(keyword) + keyword.Length;
                    if (index < cleaned.Length)
                    {
                        string reminder = cleaned.Substring(index).Trim();
                        if (!string.IsNullOrEmpty(reminder))
                            return reminder;
                    }
                }
            }

            if (!string.IsNullOrEmpty(cleaned) && cleaned.Length > 5)
                return cleaned;

            return "Reminder Task";
        }

        private string ExtractReminderTimeUniversal(string input)
        {
            string lower = input.ToLower();

            if (lower.Contains("tomorrow"))
                return "tomorrow";
            if (lower.Contains("today"))
                return "today";
            if (lower.Contains("next week") || lower.Contains("in a week"))
                return "in 1 week";
            if (lower.Contains("month"))
                return "in 1 month";

            Regex dayRegex = new Regex(@"(\d+)\s*days?");
            Match match = dayRegex.Match(lower);
            if (match.Success)
            {
                return match.Groups[1].Value + " days";
            }

            Regex weekRegex = new Regex(@"(\d+)\s*weeks?");
            Match weekMatch = weekRegex.Match(lower);
            if (weekMatch.Success)
            {
                return weekMatch.Groups[1].Value + " weeks";
            }

            if (lower.Contains("day"))
            {
                string[] parts = input.Split(' ');
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i].ToLower().Contains("day") && i > 0)
                    {
                        return parts[i - 1] + " days";
                    }
                }
                return "in 1 day";
            }

            return "in 3 days";
        }

        private void AddUserMessage(string message)
        {
            var chatMessage = new ChatMessage(userName, message, true);
            ChatMessagesListBox.Items.Add(chatMessage);
            ScrollToBottom();
        }

        private void AddBotMessage(string message)
        {
            var chatMessage = new ChatMessage("Bot", message, false);
            ChatMessagesListBox.Items.Add(chatMessage);
            ScrollToBottom();
        }

        private void ScrollToBottom()
        {
            if (ChatMessagesListBox.Items.Count > 0)
            {
                var lastItem = ChatMessagesListBox.Items[ChatMessagesListBox.Items.Count - 1];
                ChatMessagesListBox.ScrollIntoView(lastItem);
            }
        }

        private void UpdateSentimentStatus(EnhancedRespondHandler.Sentiment sentiment)
        {
            switch (sentiment)
            {
                case EnhancedRespondHandler.Sentiment.Worried:
                    if (StatusTextBlock != null)
                    {
                        StatusTextBlock.Text = $"Sentiment: Worried - {userName}, stay safe!";
                        StatusTextBlock.Foreground = new SolidColorBrush(Color.FromRgb(255, 193, 7));
                    }
                    break;
                case EnhancedRespondHandler.Sentiment.Curious:
                    if (StatusTextBlock != null)
                    {
                        StatusTextBlock.Text = $"Sentiment: Curious - Great! Keep learning about security!";
                        StatusTextBlock.Foreground = new SolidColorBrush(Color.FromRgb(78, 205, 196));
                    }
                    break;
                case EnhancedRespondHandler.Sentiment.Frustrated:
                    if (StatusTextBlock != null)
                    {
                        StatusTextBlock.Text = $"Sentiment: Frustrated - Let me help simplify this for you";
                        StatusTextBlock.Foreground = new SolidColorBrush(Color.FromRgb(255, 107, 107));
                    }
                    break;
                case EnhancedRespondHandler.Sentiment.Happy:
                    if (StatusTextBlock != null)
                    {
                        StatusTextBlock.Text = $"Sentiment: Happy - Great to see you enjoying cybersecurity!";
                        StatusTextBlock.Foreground = new SolidColorBrush(Color.FromRgb(76, 209, 55));
                    }
                    break;
                default:
                    if (StatusTextBlock != null)
                    {
                        StatusTextBlock.Text = $"Ready, {userName}!";
                        StatusTextBlock.Foreground = new SolidColorBrush(Color.FromRgb(176, 196, 222));
                    }
                    break;
            }
        }

        private void LoadTasks()
        {
            try
            {
                currentTasks = taskManager.GetAllTasks();
                TasksListBox.Items.Clear();

                if (currentTasks != null && currentTasks.Count > 0)
                {
                    foreach (var task in currentTasks)
                    {
                        string displayText = task.Title;
                        if (task.IsComplete)
                        {
                            displayText = "[COMPLETE] " + task.Title;
                        }
                        if (!string.IsNullOrEmpty(task.Reminder) && task.Reminder != task.Title)
                        {
                            displayText += " (Reminder: " + task.Reminder + ")";
                        }
                        TasksListBox.Items.Add(displayText);
                    }
                    StatusTextBlock.Text = $"Loaded {currentTasks.Count} tasks from database";
                }
                else
                {
                    TasksListBox.Items.Add("No tasks available. Add a new task above.");
                    StatusTextBlock.Text = "No tasks found";
                }
            }
            catch (Exception ex)
            {
                TasksListBox.Items.Clear();
                TasksListBox.Items.Add("Error loading tasks: " + ex.Message);
                StatusTextBlock.Text = $"Error loading tasks: {ex.Message}";
            }
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TaskTitleTextBox.Text.Trim();
            string description = TaskDescriptionTextBox.Text.Trim();
            string reminder = TaskReminderTextBox.Text.Trim();

            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Please enter a task title.", "Task Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(description))
                description = title;

            string result = taskManager.AddTask(title, description, reminder);

            AddBotMessage($"Task added: {title}");
            LoadTasks();
            ClearTaskInputs();
            StatusTextBlock.Text = result;

            MessageBox.Show($"Task '{title}' has been added successfully!", "Task Added", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ClearTaskInputs()
        {
            TaskTitleTextBox.Clear();
            TaskDescriptionTextBox.Clear();
            TaskReminderTextBox.Clear();
        }

        private void MarkCompleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (TasksListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a task to mark as complete.", "No Task Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (currentTasks != null && TasksListBox.SelectedIndex < currentTasks.Count)
            {
                var selectedTask = currentTasks[TasksListBox.SelectedIndex];
                if (selectedTask != null)
                {
                    string result = taskManager.MarkAsComplete(selectedTask.Id);
                    AddBotMessage($"Task marked complete: {selectedTask.Title}");
                    LoadTasks();
                    StatusTextBlock.Text = result;
                }
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TasksListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a task to delete.", "No Task Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this task?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (currentTasks != null && TasksListBox.SelectedIndex < currentTasks.Count)
                {
                    var selectedTask = currentTasks[TasksListBox.SelectedIndex];
                    if (selectedTask != null)
                    {
                        string message = taskManager.DeleteTask(selectedTask.Id);
                        AddBotMessage($"Task deleted: {selectedTask.Title}");
                        LoadTasks();
                        StatusTextBlock.Text = message;
                    }
                }
            }
        }

        private void StartQuiz()
        {
            quizManager.StartQuiz();
            quizAnswered = false;
            FinalResultsBorder.Visibility = Visibility.Collapsed;
            FinalResultsButton.Visibility = Visibility.Collapsed;
            NextQuestionButton.Visibility = Visibility.Collapsed;
            FeedbackTextBlock.Text = "";
            OptionsStackPanel.Visibility = Visibility.Visible;
            StatusTextBlock.Text = "Quiz started! Answer the questions.";

            OptionA.IsChecked = false;
            OptionB.IsChecked = false;
            OptionC.IsChecked = false;
            OptionD.IsChecked = false;

            ShowQuestion();
        }

        private void StartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            StartQuiz();
            AddBotMessage("Starting the cybersecurity quiz! Answer the questions one at a time.");
        }

        private void ResetQuizButton_Click(object sender, RoutedEventArgs e)
        {
            quizManager.ResetQuiz();
            quizAnswered = false;
            QuestionTextBlock.Text = "Quiz reset. Click 'Start Quiz' to begin again.";
            QuestionNumberText.Text = "Question 0 of 0";
            OptionsStackPanel.Visibility = Visibility.Visible;
            FeedbackTextBlock.Text = "";
            ScoreTextBlock.Text = "Score: 0/0";
            FinalResultsBorder.Visibility = Visibility.Collapsed;
            FinalResultsButton.Visibility = Visibility.Collapsed;
            NextQuestionButton.Visibility = Visibility.Collapsed;
            StatusTextBlock.Text = "Quiz reset";

            OptionA.IsChecked = false;
            OptionB.IsChecked = false;
            OptionC.IsChecked = false;
            OptionD.IsChecked = false;

            OptionA.IsEnabled = true;
            OptionB.IsEnabled = true;
            OptionC.IsEnabled = true;
            OptionD.IsEnabled = true;
        }

        private void ShowQuestion()
        {
            var question = quizManager.GetCurrentQuestion();
            if (question == null)
            {
                if (quizManager.IsFinished())
                {
                    ShowFinalResults();
                }
                return;
            }

            QuestionNumberText.Text = $"Question {quizManager.GetCurrentIndex() + 1} of {quizManager.GetTotalQuestions()}";
            QuestionTextBlock.Text = question.Question;

            if (question.Options.Count >= 2)
            {
                OptionA.Content = $"A) {question.Options[0]}";
                OptionB.Content = $"B) {question.Options[1]}";
                OptionC.Content = question.Options.Count > 2 ? $"C) {question.Options[2]}" : "";
                OptionD.Content = question.Options.Count > 3 ? $"D) {question.Options[3]}" : "";

                OptionA.Visibility = Visibility.Visible;
                OptionB.Visibility = Visibility.Visible;
                OptionC.Visibility = question.Options.Count > 2 ? Visibility.Visible : Visibility.Collapsed;
                OptionD.Visibility = question.Options.Count > 3 ? Visibility.Visible : Visibility.Collapsed;

                OptionA.IsChecked = false;
                OptionB.IsChecked = false;
                OptionC.IsChecked = false;
                OptionD.IsChecked = false;

                OptionA.IsEnabled = true;
                OptionB.IsEnabled = true;
                OptionC.IsEnabled = true;
                OptionD.IsEnabled = true;
            }

            OptionsStackPanel.Visibility = Visibility.Visible;
            FeedbackTextBlock.Text = "";
            quizAnswered = false;
            NextQuestionButton.Visibility = Visibility.Collapsed;

            ScoreTextBlock.Text = $"Score: {quizManager.GetCurrentIndex()}/{quizManager.GetTotalQuestions()}";
        }

        private void OptionSelected(object sender, RoutedEventArgs e)
        {
            if (!quizAnswered && quizManager.IsQuizActive())
            {
                SubmitQuizAnswer();
            }
        }

        private void SubmitQuizAnswer()
        {
            if (quizAnswered)
                return;

            string selectedAnswer = "";
            if (OptionA.IsChecked == true)
                selectedAnswer = "A";
            else if (OptionB.IsChecked == true)
                selectedAnswer = "B";
            else if (OptionC.IsChecked == true)
                selectedAnswer = "C";
            else if (OptionD.IsChecked == true)
                selectedAnswer = "D";

            if (string.IsNullOrEmpty(selectedAnswer))
            {
                MessageBox.Show("Please select an answer.", "Answer Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            OptionA.IsEnabled = false;
            OptionB.IsEnabled = false;
            OptionC.IsEnabled = false;
            OptionD.IsEnabled = false;

            bool correct = quizManager.SubmitAnswer(selectedAnswer);
            string feedback = quizManager.GetFeedback(correct);

            if (correct)
                FeedbackTextBlock.Text = $"Correct! {feedback}";
            else
                FeedbackTextBlock.Text = $"Incorrect. {feedback}";

            quizAnswered = true;

            ScoreTextBlock.Text = $"Score: {quizManager.GetCurrentIndex()}/{quizManager.GetTotalQuestions()}";

            if (quizManager.IsFinished())
            {
                NextQuestionButton.Visibility = Visibility.Collapsed;
                FinalResultsButton.Visibility = Visibility.Visible;
                StatusTextBlock.Text = "Quiz complete! Click 'Show Results' to see your final score.";
            }
            else
            {
                NextQuestionButton.Visibility = Visibility.Visible;
                StatusTextBlock.Text = "Click 'Next Question' to continue.";
            }
        }

        private void NextQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            if (!quizManager.IsFinished())
            {
                ShowQuestion();
                NextQuestionButton.Visibility = Visibility.Collapsed;
            }
        }

        private void FinalResultsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowFinalResults();
        }

        private void ShowFinalResults()
        {
            string score = quizManager.GetFinalScore();
            string message = quizManager.GetFinalMessage();

            FinalScoreText.Text = $"Final Score: {score}";
            FinalMessageText.Text = message;
            FinalResultsBorder.Visibility = Visibility.Visible;
            FinalResultsButton.Visibility = Visibility.Collapsed;
            OptionsStackPanel.Visibility = Visibility.Collapsed;
            QuestionTextBlock.Text = "Quiz completed!";
            FeedbackTextBlock.Text = "";
            ScoreTextBlock.Text = $"Score: {score}";
            StatusTextBlock.Text = message;
        }

        private void UpdateLogDisplay()
        {
            try
            {
                var logs = ActivityLogger.GetRecentLog(10);
                LogListBox.Items.Clear();

                if (logs.Count > 0)
                {
                    foreach (var log in logs)
                    {
                        LogListBox.Items.Add(log);
                    }
                }
                else
                {
                    LogListBox.Items.Add("No activity logged yet.");
                }

                if (ActivityLogger.GetCount() > 10)
                {
                    ShowMoreLogButton.Visibility = Visibility.Visible;
                }
                else
                {
                    ShowMoreLogButton.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                LogListBox.Items.Clear();
                LogListBox.Items.Add("Error loading logs: " + ex.Message);
            }
        }

        private void RefreshLogButton_Click(object sender, RoutedEventArgs e)
        {
            showFullLog = false;
            UpdateLogDisplay();
            StatusTextBlock.Text = "Log refreshed";
        }

        private void ShowMoreLogButton_Click(object sender, RoutedEventArgs e)
        {
            if (!showFullLog)
            {
                var logs = ActivityLogger.GetFullLog();
                LogListBox.Items.Clear();
                foreach (var log in logs)
                {
                    LogListBox.Items.Add(log);
                }
                showFullLog = true;
                ShowMoreLogButton.Content = "Show Less";
                StatusTextBlock.Text = "Showing full log history";
            }
            else
            {
                showFullLog = false;
                UpdateLogDisplay();
                ShowMoreLogButton.Content = "Show More";
                StatusTextBlock.Text = "Showing recent log entries";
            }
        }

        private string GetLogDisplay()
        {
            var logs = ActivityLogger.GetRecentLog(10);
            if (logs.Count == 0)
                return "No activity logged yet.";

            string result = "Here's a summary of recent actions:\n";
            foreach (string log in logs)
            {
                result += log + "\n";
            }

            if (ActivityLogger.GetCount() > 10)
            {
                result += "\nThere are more entries. Use 'show more' to see them all.";
            }

            return result;
        }
    }
}