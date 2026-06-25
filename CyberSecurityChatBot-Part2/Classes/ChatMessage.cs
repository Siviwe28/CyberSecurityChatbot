using System;

namespace CyberSecurityChatbotWPF.Classes
{
    public class ChatMessage
    {
        // Properties
        public string Sender { get; set; }
        public string Message { get; set; }
        public bool IsUserMessage { get; set; }
        public DateTime Timestamp { get; set; }

        // Constructor 1: Full constructor
        public ChatMessage(string sender, string message, bool isUserMessage)
        {
            Sender = sender;
            Message = message;
            IsUserMessage = isUserMessage;
            Timestamp = DateTime.Now;
        }

        // Constructor 2: With custom timestamp
        public ChatMessage(string sender, string message, bool isUserMessage, DateTime timestamp)
        {
            Sender = sender;
            Message = message;
            IsUserMessage = isUserMessage;
            Timestamp = timestamp;
        }

        // Constructor 3: Default constructor
        public ChatMessage()
        {
            Sender = "System";
            Message = string.Empty;
            IsUserMessage = false;
            Timestamp = DateTime.Now;
        }

        // Override ToString for debugging
        public override string ToString()
        {
            return $"[{Timestamp:HH:mm:ss}] {Sender}: {Message}";
        }

        // Helper method to get formatted timestamp
        public string FormattedTimestamp => Timestamp.ToString("hh:mm tt");

        // Helper method to get short timestamp
        public string ShortTimestamp => Timestamp.ToString("HH:mm");
    }
}