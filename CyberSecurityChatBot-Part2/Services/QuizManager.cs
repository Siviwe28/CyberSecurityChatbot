using System;
using System.Collections.Generic;
using CyberSecurityChatbotWPF.Utilities;

namespace CyberSecurityChatbotWPF.Services
{
    public class QuizManager
    {
        private List<QuizQuestion> questions;
        private int currentIndex = 0;
        private int score = 0;
        private bool quizActive = false;

        public QuizManager()
        {
            InitializeQuestions();
        }

        private void InitializeQuestions()
        {
            questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> { "Reply with your password", "Delete the email", "Report the email as phishing", "Ignore it" },
                    CorrectAnswer = "C",
                    Explanation = "Reporting phishing emails helps prevent scams.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Question = "A strong password should contain at least 12 characters with a mix of letters, numbers, and symbols.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "A",
                    Explanation = "Strong passwords should be long and complex with various character types.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Question = "What does HTTPS indicate on a website?",
                    Options = new List<string> { "The site is secure", "The site is slow", "The site is fake", "The site is old" },
                    CorrectAnswer = "A",
                    Explanation = "HTTPS means the connection is encrypted and secure.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Question = "Social engineering attacks rely on manipulating people rather than hacking systems.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "A",
                    Explanation = "Social engineering targets human psychology to gain information.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Question = "What is two-factor authentication?",
                    Options = new List<string> { "Using two passwords", "Requiring a second verification method", "Having two accounts", "Using two devices" },
                    CorrectAnswer = "B",
                    Explanation = "2FA requires a second verification method like a code or biometric.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Question = "It is safe to use the same password for multiple accounts.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "B",
                    Explanation = "Using the same password across accounts increases security risk.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Question = "What should you do on public Wi-Fi?",
                    Options = new List<string> { "Use a VPN", "Enter passwords freely", "Download files", "Share personal information" },
                    CorrectAnswer = "A",
                    Explanation = "VPN encrypts your connection on public networks.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Question = "Malware can only infect computers through email attachments.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "B",
                    Explanation = "Malware can spread through various methods including downloads and websites.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Question = "What is the best way to protect your privacy on social media?",
                    Options = new List<string> { "Share everything", "Review privacy settings", "Post personal details", "Accept all friend requests" },
                    CorrectAnswer = "B",
                    Explanation = "Review and adjust privacy settings to control what you share.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Question = "Regular data backups help protect against ransomware attacks.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "A",
                    Explanation = "Backups ensure you can recover data if encrypted by ransomware.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Question = "What is a common sign of a phishing attempt?",
                    Options = new List<string> { "Personalized greeting", "Urgent action required", "No attachments", "Known sender" },
                    CorrectAnswer = "B",
                    Explanation = "Phishing emails often create urgency to trick users.",
                    IsTrueFalse = false
                }
            };
        }

        public void StartQuiz()
        {
            currentIndex = 0;
            score = 0;
            quizActive = true;
            ActivityLogger.Log("Quiz started");
        }

        public QuizQuestion GetCurrentQuestion()
        {
            if (currentIndex < questions.Count)
                return questions[currentIndex];
            return null;
        }

        public bool SubmitAnswer(string answer)
        {
            if (!quizActive || currentIndex >= questions.Count)
                return false;

            var question = questions[currentIndex];
            bool correct = question.CorrectAnswer.ToUpper() == answer.ToUpper();
            if (correct)
                score++;

            ActivityLogger.Log($"Quiz - Question {currentIndex + 1}: {(correct ? "Correct" : "Incorrect")}");

            currentIndex++;
            return correct;
        }

        public string GetFeedback(bool correct)
        {
            if (currentIndex - 1 >= 0 && currentIndex - 1 < questions.Count)
                return questions[currentIndex - 1].Explanation;
            return "";
        }

        public bool IsFinished()
        {
            return currentIndex >= questions.Count;
        }

        public string GetFinalScore()
        {
            quizActive = false;
            string message = $"{score} out of {questions.Count}";
            ActivityLogger.Log($"Quiz completed - Score: {message}");
            return message;
        }

        public string GetFinalMessage()
        {
            int percentage = (score * 100) / questions.Count;
            if (percentage >= 80)
                return "Great job! You're a cybersecurity pro!";
            else if (percentage >= 60)
                return "Good effort! Keep learning to stay safe online!";
            else
                return "Keep studying cybersecurity to protect yourself online!";
        }

        public void ResetQuiz()
        {
            currentIndex = 0;
            score = 0;
            quizActive = false;
        }

        public int GetTotalQuestions()
        {
            return questions.Count;
        }

        public int GetCurrentIndex()
        {
            return currentIndex;
        }

        public bool IsQuizActive()
        {
            return quizActive;
        }
    }
}