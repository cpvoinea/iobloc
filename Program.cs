using System;

namespace iobloc
{
    class Program
    {
        /// <summary>
        /// List of games. Add a new case for each game in this list
        /// </summary>
        static readonly string[] _list = { "tetris", "runner", "helicopter", "breakout" };

        static void Main(string[] args)
        {
            // Show games options
            for (int i = 0; i < _list.Length; i++)
                Console.WriteLine("{0}: {1}", i, _list[i]);
            // Choose a game
            int key = Console.ReadKey().KeyChar - '0';
            if (key < 0 || key >= _list.Length)
                return;

            Game game = null;
            try
            {
                IBoard board = null;
                switch (key)
                {
                    case 0:
                        board = new TetrisBoard();
                        break;
                    case 1:
                        board = new RunnerBoard();
                        break;
                    case 2:
                        board = new HelicopterBoard();
                        break;
                    case 3:
                        board = new BreakoutBoard();
                        break;
                }

                game = new Game(board);
                game.Ended += GameEnded;
                game.Start();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex);
                Console.ReadKey(true);
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

        static void GameEnded(object sender, EventArgs ea)
        {
            Console.WriteLine((ea as Game.EndedArgs).Message);
            Console.ReadKey(true);
            Console.Clear();
        }
    }
}
