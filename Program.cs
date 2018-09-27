using System;

namespace iobloc
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1:tetris");
            Console.WriteLine("2:runner");
            Console.WriteLine("3:helicopter");
            var key = Console.ReadKey().KeyChar;
            if (key != '1' && key != '2' && key != '3')
                return;

            Game game = null;
            try
            {
                IBoard board = null;
                switch (key)
                {
                    case '1':
                        board = new TetrisBoard();
                        break;
                    case '2':
                        board = new RunnerBoard();
                        break;
                    case '3':
                        board = new HelicopterBoard();
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
