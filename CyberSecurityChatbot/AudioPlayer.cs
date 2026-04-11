using System;
using System.Media;

class AudioPlayer
{
    public static void PlayGreeting()
    {
        if (OperatingSystem.IsWindows())
        {
            try
            {
                SoundPlayer player = new SoundPlayer(CyberSecurityChatbot.Properties.Resources.Greeting);
                player.Play();
            }
            catch
            {
                Console.WriteLine("⚠️ Audio file not found.");
            }
        }
    }
}