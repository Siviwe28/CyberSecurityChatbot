using System;
using System.Media;

class AudioPlayer
{
    public static void PlayGreeting()
    {
        try
        {
            SoundPlayer player = new SoundPlayer("PlayGreeting.wav");
            player.PlaySync();
        }
        catch
        {
            Console.WriteLine("⚠️ Audio file not found or cannot be played.");
        }
    }
}
