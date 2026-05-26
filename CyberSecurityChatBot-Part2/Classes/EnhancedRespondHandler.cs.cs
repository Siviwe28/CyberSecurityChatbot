using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityChatbotWPF.Classes
{
    public class EnhancedRespondHandler
    {
        private Dictionary<string, List<string>> keywordResponses;
        private Random random = new Random();
        private string currentTopic = "";

        public enum Sentiment { Neutral, Worried, Curious, Frustrated, Happy }

        public EnhancedRespondHandler()
        {
            InitializeResponses();
        }

        private void InitializeResponses()
        {
            keywordResponses = new Dictionary<string, List<string>>()
            {
                ["password"] = new List<string>
                {
                    "Use strong passwords with at least 12 characters, including numbers, symbols, and both cases.",
                    "Never reuse passwords across different accounts. Each account needs its own unique password.",
                    "Consider using a password manager to generate and store complex passwords securely."
                },
                ["phishing"] = new List<string>
                {
                    "Always verify email sender addresses before clicking links. Scammers spoof legitimate domains.",
                    "Never share personal information via email. Legitimate companies won't ask for passwords via email.",
                    "Hover over links before clicking to see the actual URL. Look for misspellings in domain names."
                },
                ["privacy"] = new List<string>
                {
                    "Review your social media privacy settings regularly. Limit who can see your posts.",
                    "Check app permissions on your phone. Some apps request unnecessary access to your data.",
                    "Use VPN on public WiFi to encrypt your internet traffic and protect your privacy."
                },
                ["scam"] = new List<string>
                {
                    "If something sounds too good to be true, it probably is. Scammers create urgent situations.",
                    "Hang up on unsolicited calls asking for personal information. Call back using official numbers.",
                    "Never send money to someone you haven't met in person, especially via gift cards or wire transfer."
                },
                ["safe browsing"] = new List<string>
                {
                    "Look for HTTPS and the padlock icon in the address bar before entering sensitive information.",
                    "Keep your browser and extensions updated to protect against known vulnerabilities.",
                    "Avoid using public computers for banking or entering passwords."
                }
            };
        }

        public string GetResponse(string userInput, out Sentiment detectedSentiment)
        {
            string lowerInput = userInput.ToLower();
            detectedSentiment = Sentiment.Neutral;

            foreach (var keyword in keywordResponses.Keys)
            {
                if (lowerInput.Contains(keyword))
                {
                    currentTopic = keyword;
                    return GetRandomResponse(keyword);
                }
            }

            return GetDefaultResponse();
        }

        private string GetRandomResponse(string keyword)
        {
            if (keywordResponses.ContainsKey(keyword) && keywordResponses[keyword].Count > 0)
            {
                int index = random.Next(keywordResponses[keyword].Count);
                return keywordResponses[keyword][index];
            }
            return GetDefaultResponse();
        }

        private string GetDefaultResponse()
        {
            string[] defaultResponses = {
                "I can help with cybersecurity topics like passwords, phishing, privacy, scams, and safe browsing. What would you like to know?",
                "Not sure about that topic. Would you like tips about password safety, recognizing scams, or protecting your privacy?",
                "I specialize in cybersecurity awareness. Feel free to ask me about specific security topics."
            };
            return defaultResponses[random.Next(defaultResponses.Length)];
        }
    }
}