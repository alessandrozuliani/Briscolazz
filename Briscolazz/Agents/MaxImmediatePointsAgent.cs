using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Briscolazz.Program;

namespace Briscolazz
{
    public class MaxImmediatePointsAgent : Agent
    {
        public MaxImmediatePointsAgent(Game game) : base(game) { }



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

            var first = Game.Cards.FirstOrDefault(x => x.Location == EnLocation.tableFirst);
            if(first == null)
            {
                return AvailableCards[Game.Rng.Next(0, AvailableCards.Count)];
            }
            else
            {
                return AvailableCards.MaxBy(x => EstimateChoiceOutcome(first, x, Game.Briscola));
            }



        }
        public static int EstimateChoiceOutcome(Card first, Card second, EnSuit currentBriscola)
        {
            if (first.Suit == second.Suit)
            {
                if (second.Value > first.Value)
                {
                    return second.Score() + first.Score();
                }
                else
                {
                    return -(second.Score() + first.Score());
                }
            }
            else
            {
                if (second.Suit == currentBriscola)
                {
                    return second.Score() + first.Score();
                }
                else
                {
                    return -(second.Score() + first.Score());
                }
            }
        }
    }

}
