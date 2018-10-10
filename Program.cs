using System;
using System.Collections.Generic;

namespace iobloc
{
    class Program
    {
        static Dictionary<string, int> HS = Settings.Game.Highscore;

        /// <summary>
        /// List of games. Add a new case for each game in this list
        /// </summary>
        static readonly string[] _list = { ">>level<<", "tetris", "runner", "helicopter", "breakout", "invaders", "snake", "sokoban", "table (WIP)" };

        static string _log = string.Empty;

        static void Main(string[] args)
        {
            // Choose a game
            var key = ShowOptions();
            while (key.Key != ConsoleKey.Escape)
            {
                int option = key.KeyChar - '0';
                if (option >= 0 && option < _list.Length || option == 9)
                {
                    IBoard board = null;
                    bool table = false;
                    switch (option)
                    {
                        case 0:
                            board = new LevelBoard();
                            break;
                        case 1:
                            board = new TetrisBoard();
                            break;
                        case 2:
                            board = new RunnerBoard();
                            break;
                        case 3:
                            board = new HelicopterBoard();
                            break;
                        case 4:
                            board = new BreakoutBoard();
                            break;
                        case 5:
                            board = new InvadersBoard();
                            break;
                        case 6:
                            board = new SnakeBoard();
                            break;
                        case 7:
                            board = new SokobanBoard();
                            break;
                        case 8:
                            table = true;
                            break;
                        case 9:
                            ShowLog();
                            break;
                    }

                    if (board != null)
                        RunGame(board);
                    else if (table)
                        RunTable();
                }

                key = ShowOptions();
            }
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
            for (int i = 0; i < _list.Length; i++)
            {
                Console.ForegroundColor = (ConsoleColor)(15 - i);
                Console.WriteLine("{0}: {1}", i, _list[i]);
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

        static void RunGame(IBoard board)
        {
            Game game = null;
            try
            {
                Log("Start " + board);
                game = new Game(board);
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
