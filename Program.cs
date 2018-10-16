namespace iobloc
{
    class Program
    {
        static void Main(string[] args)
        {
            var configFilePath = args.Length > 0 ? args[0] : null;
            using (Engine engine = new Engine(configFilePath))
            {
                engine.ShowMenu();
            }
        }
    }
}
