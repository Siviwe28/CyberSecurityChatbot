using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CyberSecurityChatbotWPF.Classes;
using System.Windows.Media; // Already present, MediaPlayer uses this

namespace CyberSecurityChatbot_Part2
{
    public partial class MainWindow : Window
    {
        private EnhancedRespondHandler respondHandler;
        private string userName = "Guest";
        private MediaPlayer mediaPlayer; // Add this for sound playback

        public MainWindow()
        {
            InitializeComponent();
            respondHandler = new EnhancedRespondHandler();

            // Play startup sound when application starts
            PlayStartupSound();

            // Show name dialog after window loads
            this.Loaded += MainWindow_Loaded;

            // Wire up event handlers
            SendButton.Click += SendButton_Click;
            MessageTextBox.KeyDown += UserInputTextBox_KeyDown;
        }

        private void PlayStartupSound()
        {
            try
            {
                mediaPlayer = new MediaPlayer();

                // Try multiple paths in case the file location varies
                string[] possiblePaths = {

                    "Audio/Greeting.wav",     // For WAV files
                  
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
                    // Optional: Play Windows default beep if custom sound not found
                    System.Media.SystemSounds.Beep.Play();
                }
            }
            catch (Exception ex)
            {
                // Silent fail - don't crash the app if sound can't play
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
            AddBotMessage($"Ask me about passwords, phishing, privacy, or scams!");

            // Update status bar (now using StatusTextBlock instead of StatusText)
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

            EnhancedRespondHandler.Sentiment sentiment;
            string response = respondHandler.GetResponse(userInput, out sentiment);

            AddBotMessage(response);
            UpdateSentimentStatus(sentiment);

            if (StatusTextBlock != null)
                StatusTextBlock.Text = $"Ready, {userName}!";
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
            // Scroll the ListBox to the bottom
            if (ChatMessagesListBox.Items.Count > 0)
            {
                var lastItem = ChatMessagesListBox.Items[ChatMessagesListBox.Items.Count - 1];
                ChatMessagesListBox.ScrollIntoView(lastItem);
            }
        }

        private void UpdateSentimentStatus(EnhancedRespondHandler.Sentiment sentiment)
        {
            // If you have a sentiment status text block, add it to your XAML
            // For now, we'll update the status text block with sentiment info
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
    }
}