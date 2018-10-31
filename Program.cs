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
                Engine.Start(settingsFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey(true);
            }
            finally
            {
                Engine.Stop();
            }
        }
    }
}
