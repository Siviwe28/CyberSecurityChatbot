using System;

class RespondHandler
{
    public static void Handle(string input)
    {
        Console.Write("Bot: ");

        if (input.Contains("how are you"))
        {
            Console.WriteLine("I'm functioning perfectly.");
        }
        else if (input.Contains("purpose"))
        {
            Console.WriteLine("I help users stay safe online.");
        }
        else if (input.Contains("what can i ask"))
        {
            Console.WriteLine("Ask about passwords, phishing, or safe browsing.");
        }
        else if (input.Contains("password"))
        {
            Console.WriteLine("Use strong passwords with letters, numbers, and symbols.");
        }
        else if (input.Contains("phishing"))
{
    Console.WriteLine("Avoid suspicious emails and never click unknown links.");
}
        else if (input.Contains("browsing"))
        {
            Console.WriteLine("Use HTTPS websites and avoid public WiFi for sensitive transactions.");
        }
        else
        {
            Console.WriteLine("I can help with passwords, phishing, or safe browsing. Type 'help' for options.");
        }
    }
}
