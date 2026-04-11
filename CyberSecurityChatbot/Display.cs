using System;
using System.Threading;

class Display
{
    public static void ShowHeader()
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Cyan;
        AsciiArt.Show();

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("==============================================");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("  Welcome to the Cybersecurity Awareness Bot");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("==============================================\n");
        Console.ResetColor();
    }

    public static string GetUserName()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Enter your name: ");
        Console.ResetColor();

        string name = Console.ReadLine() ?? "";

        if (!InputValidator.IsValid(name))
        {
            ShowError("Invalid name entered. Defaulting to 'User'.");
            name = "User";
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\nWelcome, {name}!");
        Console.ResetColor();

        TypeEffect("I'm here to help you stay safe online.\n");

        // ✅ Ask FIRST
        Console.ForegroundColor = ConsoleColor.Yellow;
        TypeEffect("Bot: How are you today?\n");
        Console.ResetColor();

        ShowDivider();

        // ✅ THEN show options
        ShowHelp();

        return name;
    }

    public static void AskQuestion()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("\nYou: ");
        Console.ResetColor();
    }

    public static void ShowDivider()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\n----------------------------------------------");
        Console.ResetColor();
    }

    public static void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"⚠ {message}");
        Console.ResetColor();
    }

    public static void ShowDefaultResponse()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Bot: I didn’t quite understand that. Could you rephrase?");
        Console.ResetColor();
    }

    public static void ShowHelp()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\nYou can ask me about:");
        Console.ResetColor();

        Console.WriteLine(" • Password safety");
        Console.WriteLine(" • Phishing attacks");
        Console.WriteLine(" • Safe browsing");
        Console.WriteLine(" • Or type 'exit' to quit\n");
    }

    public static void TypeEffect(string message)
    {
        foreach (char c in message)
        {
            Console.Write(c);
            Thread.Sleep(25);
        }
    }
}