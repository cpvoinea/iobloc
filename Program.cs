using System;
using System.Text;

namespace iobloc
{
    class Program
    {
        static void Main(string[] args)
        {
            StringBuilder errors = new StringBuilder();
            try
            {
                var settingsFilePath = args.Length > 0 ? args[0] : null;
                Engine.Start(settingsFilePath);
            }
            catch (Exception ex)
            {
                errors.AppendLine(ex.ToString());
            }
            finally
            {
                Engine.Stop();
            }

            if (errors.Length > 0)
            {
                Console.Clear();
                Console.WriteLine(errors);
                Console.Read();
            }
        }
    }
}
