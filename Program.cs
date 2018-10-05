using System;

namespace iobloc
{
    class Program
    {
        /// <summary>
        /// List of games. Add a new case for each game in this list
        /// </summary>
        static readonly string[] _list = { ">>level<<", "tetris", "runner", "helicopter", "breakout", "invaders", "snake" };

        static string _log = string.Empty;

        static void Main(string[] args)
        {
            // Choose a game
            var key = ShowOptions();
            while (key.Key != ConsoleKey.Escape)
            {
                int option = key.KeyChar - '0';
                if (option >= 0 && option < _list.Length)
                {
                    IBoard board = null;
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
                    }

                    RunGame(board);
                }

                key = ShowOptions();
            }
        }

        static ConsoleKeyInfo ShowOptions()
        {
            Console.Clear();
            Console.Write(_log);
            // Show games options
            for (int i = 0; i < _list.Length; i++)
                Console.WriteLine("{0}: {1}", i, _list[i]);
            Console.Write("Option (ESC to exit): ");
            return Console.ReadKey();
        }

        static void RunGame(IBoard board)
        {
            Game game = null;
            try
            {
                Log("Starting " + board);
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
            Log(((Game.EndedArgs)ea).Message);
        }
    }
}
