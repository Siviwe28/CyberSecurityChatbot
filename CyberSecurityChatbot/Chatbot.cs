using System;

class Chatbot
{
    public static void StartChat(string userName)
    {
        while (true)
        {
            Display.AskQuestion();
            string input = Console.ReadLine().ToLower();

            if (!InputValidator.IsValid(input))
            {
                Console.WriteLine("⚠️ Please enter something.");
                continue;
            }

            if (input.Contains("exit"))
            {
                Console.WriteLine($"Goodbye, {userName}! Stay safe online 👋");
                break;
            }

            RespondHandler.Handle(input);
        }
    }
}