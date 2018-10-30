using System;

namespace iobloc
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var settingsFilePath = args.Length > 0 ? args[0] : null;
                Serializer.LoadSettings(settingsFilePath);
                Serializer.LoadHighscores();
                UIPainter.Initialize();

                BoardRunner.Run(new MenuBoard());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Serializer.SaveSettings();
                Serializer.SaveHighscores();
                UIPainter.Exit();
            }
        }
    }
}
