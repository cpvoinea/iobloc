using System;

namespace iobloc
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game();
            game.Ended += GameEnded;
            try
            {
                game.Start();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex);
            }
            finally
            {
                game.Ended -= GameEnded;
                game.Close();
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
