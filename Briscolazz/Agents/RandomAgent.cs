using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Briscolazz.Program;

namespace Briscolazz
{
    public class RandomAgent : Agent
    {
        public RandomAgent(Game game) : base(game) { }



        public override Card PickCard()
        {
            var AvailableCards = new List<Card>();
            if (PlayerNumber == EnPlayerNumber.one)
            {
                AvailableCards = Game.Cards.Where(x => x.Location == EnLocation.player1hand).ToList();
            }
            else
            {

                AvailableCards = Game.Cards.Where(x => x.Location == EnLocation.player2hand).ToList();
            }
            var choosenCard = AvailableCards[Game.Rng.Next(0, AvailableCards.Count)];

            //Console.WriteLine("Player n°" + EnPlayerNumber.one.ToString() + " played " + choosenCard.ToString());

            return choosenCard;
        }
    }

}
