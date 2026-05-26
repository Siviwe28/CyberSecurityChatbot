using System;

class Program
{
    static void Main()
    {
        AudioPlayer.PlayGreeting();
        AsciiArt.Show();

        string name = Display.GetUserName();
        Chatbot.StartChat(name);
    }
}