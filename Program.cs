using System;
using System.Text;

namespace iobloc
{
    class Program
    {
        static void Main(string[] args)
        {
            // rudimentary logging
            StringBuilder errors = new StringBuilder();
            try
            {
                // optional external settings file
                var settingsFilePath = args.Length > 0 ? args[0] : null;
                // initialization of environment and open to menu
                Engine.Start(settingsFilePath);
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
