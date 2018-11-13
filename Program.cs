using System;
using System.Text;

namespace iobloc
{
    class Program
    {
        static void Main(string[] args)
        {
            // new DemoBoard().Run();
            // rudimentary logging
            StringBuilder errors = new StringBuilder();
            try
            {
                // optional external settings file
                var settingsFilePath = args.Length > 0 ? args[0] : null;
                // initialization of environment
                Engine.Initialize(settingsFilePath);
                // open to menu
                Engine.Start();
            }
            catch (Exception ex)
            {
                errors.AppendLine(ex.ToString());
            }
            finally
            {
                // restore environment
                Engine.Stop();
            }

            // show eventual errors
            if (errors.Length > 0)
            {
                Console.Clear();
                Console.WriteLine(errors);
                Console.Read();
            }
        }
    }
}
