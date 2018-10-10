using System;
using System.Collections.Generic;
using System.IO;

namespace iobloc
{
    class Program
    {
        static Dictionary<string, int> HS = Settings.Game.Highscore;

        static string _log = string.Empty;

        static void Main(string[] args)
        {
            string settingsFile = Settings.SettingsFile;
            if (args.Length > 0)
                settingsFile = args[0];
            if (File.Exists(settingsFile))
                Settings.FromFile(settingsFile);

            // Choose a game
            var key = ShowOptions();
            while (key.Key != ConsoleKey.Escape)
            {
                int option = key.KeyChar - '0';
                if (Enum.IsDefined(typeof(GameOption), option))
                    RunGame((GameOption)option);
                else if (option == 9)
                    ShowLog();

                key = ShowOptions();
            }

            Settings.Save();
        }

        static void RunTable()
        {
            new TableBoard();
        }

        static ConsoleKeyInfo ShowOptions()
        {
            Console.Clear();
            ShowHighscores();
            // Show games options
            var options = Enum.GetNames(typeof(GameOption));
            for (int i = 0; i < options.Length; i++)
            {
                Console.ForegroundColor = (ConsoleColor)(15 - i);
                Console.WriteLine("{0}: {1}", i, options[i]);
            }
            Console.ResetColor();
            Console.Write("Option (ESC to exit): ");
            return Console.ReadKey();
        }

        static void ShowHighscores()
        {
            if (HS.Count > 0)
            {
                Console.WriteLine("||Highscore||");
                Console.WriteLine("=============");
                foreach (var g in HS.Keys)
                    Console.WriteLine("{0,-10}{1,3}", g, HS[g]);
                Console.WriteLine("=============");
            }
        }

        static void ShowLog()
        {
            Console.Clear();
            Console.Write(_log);
            Console.ReadKey();
        }

        static void RunGame(GameOption option)
        {
            Game game = null;
            try
            {
                Log("Start " + option);
                game = new Game(option);
                game.Ended += GameEnded;
                game.Start();
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }
            finally
            {
                if (game != null)
                {
                    game.Ended -= GameEnded;
                    game.Close();
                }
            }
        }

        static void Log(string line)
        {
            _log += line + Environment.NewLine;
        }

        static void GameEnded(object sender, EventArgs ea)
        {
            var args = (Game.EndedArgs)ea;
            Log(args.Message);
            if (!HS.ContainsKey(args.GameName) || HS[args.GameName] < args.Score)
                HS[args.GameName] = args.Score;
        }
    }
}
