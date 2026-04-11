using CyberSecurityChatbot;
using System;

class Display
{
    public static string GetUserName()
    {
        Console.Write("Enter your name: ");
        string name = Console.ReadLine();

        if (!InputValidator.IsValid(name))
        {
            name = "User";
        }

        Console.WriteLine($"\nWelcome, {name}! 👋");
        TypeEffect("I’m here to help you stay safe online.\n");

        return name;
    }

    public static void TypeEffect(string message)
    {
        foreach (char c in message)
        {
            Console.Write(c);
            System.Threading.Thread.Sleep(20);
        }
    }

    public static void AskQuestion()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\nAsk me something: ");
        Console.ResetColor();
    }
}