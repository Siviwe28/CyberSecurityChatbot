using System;
using System.IO;
using System.Media;
using System.Windows;

namespace CyberSecurityChatbotWPF.Classes
{
    public static class AudioPlayer
    {
        public static void PlaySound(string soundFileName)
        {
            try
            {
                string soundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Audio", soundFileName);

                if (File.Exists(soundPath))
                {
                    using (SoundPlayer player = new SoundPlayer(soundPath))
                    {
                        player.Play(); // Play async (doesn't block UI)
                    }
                }
            }
            catch (Exception ex)
            {
                // Silent fail - audio is optional
                System.Diagnostics.Debug.WriteLine($"Audio error: {ex.Message}");
            }
        }

        public static void PlayGreeting()
        {
            PlaySound("Greeting.wav");
        }

        public static void PlayNotification()
        {
            PlaySound("notification.wav");
        }
    }
}