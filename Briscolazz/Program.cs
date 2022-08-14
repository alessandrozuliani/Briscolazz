namespace Briscolazz
{
    public class Program
    {
        static void Main(string[] args)
        {
            //const int seed = 1;
            //var game = new Game(seed);

            var p1Won = 0;
            var p2Won = 0;
            var draws = 0;

            for (int i = 0; i < 10000; i++)
            {
                var game = new Game();
                game.Player1 = new MaxImmediatePointsAgent(game);
                game.Player1.PlayerNumber = EnPlayerNumber.one;
                game.Player2 = new MaxImmediatePointsAgent(game);
                game.Player2.PlayerNumber = EnPlayerNumber.two;

                do
                {
                    game.GameLoop();
                }
                while (game.GameState != EnGameState.gameover);

                switch (game.Winner) 
                {
                    case EnPlayerNumber.one:
                        p1Won++;
                        break;
                    case EnPlayerNumber.two:
                        p2Won++;
                        break;
                    case null:
                        draws++;
                        break;

                }

                //game.DisplayFinishedGameSummary();
            }

            Console.WriteLine("10000 games been played.");
            Console.WriteLine($"p1 won {p1Won} times.");
            Console.WriteLine($"p2 won {p2Won} times.");
            Console.WriteLine($"{draws} draws.");





            Console.WriteLine("game over, press any key to quit");
            Console.ReadLine();

        }

    }

}