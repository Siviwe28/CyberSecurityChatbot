using CyberSecurityChatbot;
using CyberSecurityChatbot.Core;
using CyberSecurityChatbot.Services;
using CyberSecurityChatbot.UI;

class Program
{
    static void Main(string[] args)
    {
        // Play voice greeting
        AudioPlayer.PlayGreeting();

        // Display ASCII art
        Display.ShowHeader();

        // Start chatbot
        Chatbot bot = new Chatbot();
        bot.Start();
    }
}// test commit