using System;

namespace iobloc
{
    class Program
    {
        static void Main(string[] args)
        {
            var settingsFilePath = args.Length > 0 ? args[0] : null;
            Engine engine = null;
            try
            {
                engine = new Engine(settingsFilePath);
                engine.ShowMenu();
                engine.ShowLog();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (engine != null)
                    engine.ShowLog();
                Console.ReadKey(true);
            }
            finally
            {
                if (engine != null)
                    engine.Dispose();
            }
        }
    }
}
