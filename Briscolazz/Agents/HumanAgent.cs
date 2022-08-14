using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Briscolazz.Program;

namespace Briscolazz
{
    public class HumanAgent : Agent
    {
        public HumanAgent(Game game) : base(game) { }

        public override Card PickCard()
        {
            Game.DisplayGameStateInConsole();
            Console.WriteLine();
            var AvailableCards = new List<Card>();
            if (PlayerNumber == EnPlayerNumber.one)
            {
                AvailableCards = Game.Cards.Where(x => x.Location == EnLocation.player1hand).ToList();
            }
            else
            {

                AvailableCards = Game.Cards.Where(x => x.Location == EnLocation.player2hand).ToList();
            }
            
            int cardToPlay;
            do
            {
                Console.WriteLine("Pick the card to play for player n°" + PlayerNumber.ToString());
            }
            while (!(int.TryParse(Console.ReadLine(), out cardToPlay) && AvailableCards.Count() >= cardToPlay));

            //Console.WriteLine("Player n°" + PlayerNumber.ToString() + " played " + AvailableCards[cardToPlay-1].ToString());

            return AvailableCards[cardToPlay-1];

        }
    }

}
