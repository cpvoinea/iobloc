using System;

namespace iobloc
{
    class Program
    {
        static void Main(string[] args)
        {
            var configFilePath = args.Length > 0 ? args[0] : null;
            Engine engine = null;
            try
            {
                using (engine = new Engine(configFilePath))
                {
                    engine.ShowMenu();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (engine != null)
                    Console.WriteLine(engine.GetLog());
                Console.ReadKey();
            }
        }
    }
}
