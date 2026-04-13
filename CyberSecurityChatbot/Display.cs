using System;

class Display
{
    public static string GetUserName()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Enter your name: ");
        string name = Console.ReadLine();

        if (!InputValidator.IsValid(name))
        {
            name = "User";
        }

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Welcome, {name}!");
        Console.ResetColor();

        TypeEffect("I'm here to help you stay safe online.\n");

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Bot: How are you today?");
        Console.WriteLine();

        ShowMenu();

        return name;
    }

    public static void ShowMenu()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("You can ask me about:");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("Password safety");
        Console.WriteLine("Phishing attacks");
        Console.WriteLine("Safe browsing");
        Console.WriteLine("Or type 'exit' to quit");
        Console.WriteLine();
        Console.ResetColor();
    }

    public static void AskQuestion()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("You: ");
        Console.ResetColor();
    }

    public static void TypeEffect(string message)
    {
        foreach (char c in message)
        {
            Console.Write(c);
            System.Threading.Thread.Sleep(15);
        }
    }
}
