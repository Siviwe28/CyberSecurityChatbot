using System;

class RespondHandler
{
    public static void Handle(string input)
    {
        if (input.Contains("how are you"))
        {
            Console.WriteLine("I'm functioning perfectly! 😄");
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
            Console.WriteLine("Avoid suspicious emails and links.");
        }
        else if (input.Contains("browsing"))
        {
            Console.WriteLine("Only use secure websites (HTTPS).");
        }
        else
        {
            Console.WriteLine("I didn’t quite understand that. Could you rephrase?");
        }
    }
}