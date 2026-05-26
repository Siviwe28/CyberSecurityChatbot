using System;

class RespondHandler
{
    public static void Handle(string input)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Bot: ");

        if (input.Contains("how are you"))
        {
            Console.WriteLine("I'm functioning perfectly!");
        }
        else if (input.Contains("purpose"))
        {
            Console.WriteLine("I help users stay safe online.");
        }
        else if (input.Contains("password"))
        {
            Console.WriteLine("Use strong passwords with letters, numbers, and symbols.");
        }
        else if (input.Contains("phishing"))
        {
            Console.WriteLine("Avoid suspicious emails and never click unknown links.");
        }
        else if (input.Contains("safe browsing"))
        {
            Console.WriteLine("Use trusted websites and check for HTTPS.");
        }
        else
        {
            Console.WriteLine("I can help with passwords, phishing, and safe browsing.");
        }

        Console.ResetColor();
    }
}
