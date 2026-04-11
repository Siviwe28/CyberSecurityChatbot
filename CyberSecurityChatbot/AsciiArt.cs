using System;

class AsciiArt
{
    public static void Show()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;

        Console.WriteLine(@"
   ____       _                _   _                 
  / ___| ___ | | ___  _ __ ___| |_(_) ___  _ __  ___ 
 | |    / _ \| |/ _ \| '__/ __| __| |/ _ \| '_ \/ __|
 | |___| (_) | | (_) | |  \__ \ |_| | (_) | | | \__ \
  \____|\___/|_|\___/|_|  |___/\__|_|\___/|_| |_|___/

      CYBERSECURITY AWARENESS BOT
");

        Console.ResetColor();
    }
}