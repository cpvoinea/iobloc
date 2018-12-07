using System;
using System.Text;
using System.Windows.Forms;

namespace iobloc
{
    class Program
    {
        private static bool USE_FORMS = true;

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // rudimentary logging
            StringBuilder errors = new StringBuilder();
            Console.Clear();
            try
            {
                // optional external settings file
                string settingsFilePath = args.Length > 0 ? args[0] : null;
                using (Engine engine = new Engine(settingsFilePath, USE_FORMS))
                    engine.Start();
            }
            catch (Exception ex)
            {
                errors.AppendLine(ex.ToString());
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
