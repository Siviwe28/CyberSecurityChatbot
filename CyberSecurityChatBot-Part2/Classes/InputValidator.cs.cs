using System;

namespace CyberSecurityChatbotWPF.Classes
{
    public class InputValidator
    {
        public static bool IsValid(string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }
    }
}