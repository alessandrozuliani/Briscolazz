using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Briscolazz.Program;

namespace Briscolazz
{
    public enum EnGameState
    {
        initializing,
        firstCardToBePlayed,
        secondCardToBePlayed,
        fromTableToStack,
        draw,
        gameover
    }


    public class Game
    {
        public Game(int? seed = null)
        {
            this.Cards = new List<Card>();

            this.Player1 = new RandomAgent(this);
            Player1.PlayerNumber = EnPlayerNumber.one;

            this.Player2 = new RandomAgent(this);
            Player2.PlayerNumber = EnPlayerNumber.two;

            if (seed.HasValue)
            {
                Rng = new Random(seed.Value);
            }
            else
            {
                Rng = new Random();
            }

        }       

        public Random Rng;
        public List<Card> Cards;
        public EnSuit Briscola;
        //player 1 goes first
        public EnPlayerNumber LastHandWinner = EnPlayerNumber.one;
        public EnPlayerNumber? Winner = null;
        public EnGameState GameState = EnGameState.initializing;
        public Agent Player1;
        public Agent Player2;
        //public Agent Player3;
        //public Agent Player4;

        public void InitCardsForNewGame()
        {
            foreach (var suit in (EnSuit[])Enum.GetValues(typeof(EnSuit)))
            {
                foreach (var value in (EnValue[])Enum.GetValues(typeof(EnValue)))
                {
                    Cards.Add(new Card()
                    {
                        Suit = suit,
                        Value = value,
                        Location = EnLocation.deck
                    });
                }
            }

            Cards.Shuffle(this.Rng);

            //distribuisco la prima mano
            Cards[0].Location = EnLocation.player1hand;
            Cards[1].Location = EnLocation.player1hand;
            Cards[2].Location = EnLocation.player1hand;

            //distribuisco la prima mano
            Cards[3].Location = EnLocation.player2hand;
            Cards[4].Location = EnLocation.player2hand;
            Cards[5].Location = EnLocation.player2hand;

            //metto la briscola sotto il mazzo
            Cards.Last().Location = EnLocation.briscola;

            Briscola = Cards.Last().Suit;

        }

        public void GameLoop()
        {
            switch (GameState)
            {
                case EnGameState.initializing:
                    InitCardsForNewGame();
                    GameState = EnGameState.firstCardToBePlayed;
                    break;

                case EnGameState.firstCardToBePlayed:

                    if (Cards.Where(x => x.Location == EnLocation.player1hand).Count() == 0)
                    {

                        var p1Points = Cards.Where(x => x.Location == EnLocation.team1stack).Sum(x => x.Score());
                        var p2Points = Cards.Where(x => x.Location == EnLocation.team2stack).Sum(x => x.Score());
                        Winner = p1Points > p2Points ? EnPlayerNumber.one : EnPlayerNumber.two;
                        if (p1Points == p2Points) Winner = null;

                        GameState = EnGameState.gameover;
                        break;
                    }
                    if (LastHandWinner == EnPlayerNumber.one)
                    {
                        Player1.PickCard().Location = EnLocation.tableFirst;
                    }
                    else
                    {
                        Player2.PickCard().Location = EnLocation.tableFirst;
                    }

                    GameState = EnGameState.secondCardToBePlayed;
                    break;

                case EnGameState.secondCardToBePlayed:

                    if (LastHandWinner == EnPlayerNumber.one)
                    {
                        Player2.PickCard().Location = EnLocation.tableSecond;
                    }
                    else
                    {
                        Player1.PickCard().Location = EnLocation.tableSecond;
                    }

                    GameState = EnGameState.fromTableToStack;
                    break;

                case EnGameState.fromTableToStack:

                    var first = Cards.Single(x => x.Location == EnLocation.tableFirst);
                    var second = Cards.Single(x => x.Location == EnLocation.tableSecond);

                    if (first.Suit == second.Suit)
                    {
                        if (second.Value > first.Value)
                        {
                            if (LastHandWinner == EnPlayerNumber.one)
                            {
                                first.Location = EnLocation.team2stack;
                                second.Location = EnLocation.team2stack;
                                LastHandWinner = EnPlayerNumber.two;
                            }
                            else
                            {
                                first.Location = EnLocation.team1stack;
                                second.Location = EnLocation.team1stack;
                                LastHandWinner = EnPlayerNumber.one;
                            }
                        }
                        else
                        {
                            if (LastHandWinner == EnPlayerNumber.one)
                            {
                                first.Location = EnLocation.team1stack;
                                second.Location = EnLocation.team1stack;
                            }
                            else
                            {
                                first.Location = EnLocation.team2stack;
                                second.Location = EnLocation.team2stack;
                            }
                        }
                    }
                    else
                    {
                        if (second.Suit == Briscola)
                        {
                            if (LastHandWinner == EnPlayerNumber.one)
                            {
                                first.Location = EnLocation.team2stack;
                                second.Location = EnLocation.team2stack;
                                LastHandWinner = EnPlayerNumber.two;
                            }
                            else
                            {
                                first.Location = EnLocation.team1stack;
                                second.Location = EnLocation.team1stack;
                                LastHandWinner = EnPlayerNumber.one;
                            }
                        }
                        else
                        {
                            if (LastHandWinner == EnPlayerNumber.one)
                            {
                                first.Location = EnLocation.team1stack;
                                second.Location = EnLocation.team1stack;
                            }
                            else
                            {
                                first.Location = EnLocation.team2stack;
                                second.Location = EnLocation.team2stack;
                            }
                        }
                    }

                    GameState = EnGameState.draw;
                    break;

                case EnGameState.draw:
                    var cardsLeft = Cards.Where(x => x.Location == EnLocation.deck).Count();
                    switch (cardsLeft)
                    {
                        // last card before briscola, assign it to winner, assing briscola, proceed to empty deck phase.
                        case 1:
                            if (LastHandWinner == EnPlayerNumber.one)
                            {
                                Cards.Single(x => x.Location == EnLocation.deck).Location = EnLocation.player1hand;
                                Cards.Single(x => x.Location == EnLocation.briscola).Location = EnLocation.player2hand;
                            }
                            else
                            {
                                Cards.Single(x => x.Location == EnLocation.deck).Location = EnLocation.player2hand;
                                Cards.Single(x => x.Location == EnLocation.briscola).Location = EnLocation.player1hand;
                            }
                            break;

                        // empty deck phase, do nothing.
                        case 0:
                            break;
                        // full deck, draw cards
                        default:
                            if (LastHandWinner == EnPlayerNumber.one)
                            {
                                Cards.First(x => x.Location == EnLocation.deck).Location = EnLocation.player1hand;
                                Cards.First(x => x.Location == EnLocation.deck).Location = EnLocation.player2hand;
                            }
                            if (LastHandWinner == EnPlayerNumber.two)
                            {
                                Cards.First(x => x.Location == EnLocation.deck).Location = EnLocation.player2hand;
                                Cards.First(x => x.Location == EnLocation.deck).Location = EnLocation.player1hand;
                            }
                            break;
                    }
                    GameState = EnGameState.firstCardToBePlayed;
                    break;               

            }
        }

       

        public void DisplayGameStateInConsole()
        {
            Console.Clear();
            Console.WriteLine("Player 1 cards : ");
            foreach (var (card, index) in Cards.Where(x => x.Location == EnLocation.player1hand).Select((v, i) => (v, i)))
            {
                Console.WriteLine($"({index + 1}) - " + card);
            }
            Console.WriteLine();
            Console.WriteLine("Player 1 score : " + Cards.Where(x => x.Location == EnLocation.team1stack).Sum(x => x.Score()));
            Console.WriteLine();

            Console.WriteLine("Player 2 cards : ");
            foreach (var (card, index) in Cards.Where(x => x.Location == EnLocation.player2hand).Select((v, i) => (v, i)))
            {
                Console.WriteLine($"({index + 1}) - " + card);
            }
            Console.WriteLine();
            Console.WriteLine("Player 2 score : " + Cards.Where(x => x.Location == EnLocation.team2stack).Sum(x => x.Score()));
            Console.WriteLine();
            if (Cards.Any(x => x.Location == EnLocation.deck))
            {
                Console.WriteLine();
                Console.WriteLine("Cards in the deck : " + Cards.Where(x => x.Location == EnLocation.deck).Count());
            }
            if (Cards.Any(x => x.Location == EnLocation.briscola))
            {
                Console.WriteLine();
                Console.WriteLine("briscola : " + Cards.FirstOrDefault(x => x.Location == EnLocation.briscola));
            }

            var first = Cards.FirstOrDefault(x => x.Location == EnLocation.tableFirst);
            if (first != null)
            {
                Console.WriteLine();
                Console.WriteLine("Cards on the table: ");
                Console.WriteLine("first: " + first);
            }
            var second = Cards.FirstOrDefault(x => x.Location == EnLocation.tableSecond);
            if (first != null)
            {
                Console.WriteLine("second: " + second);
            }
            //var third = cards.FirstOrDefault(x => x.Location == EnLocation.tableThird);
            //if (first != null)
            //{
            //    Console.WriteLine("third: " + first);
            //}
            //var fourth = cards.FirstOrDefault(x => x.Location == EnLocation.tableFourth);
            //if (first != null)
            //{
            //    Console.WriteLine("fourth: " + first);
            //}

        }

        public void DisplayFinishedGameSummary()
        {
            var p1Points = Cards.Where(x => x.Location == EnLocation.team1stack).Sum(x => x.Score());
            var p2Points = Cards.Where(x => x.Location == EnLocation.team2stack).Sum(x => x.Score());
            Console.Clear();
            Console.WriteLine("Game Over!");
            switch (Winner)
            {
                case null:
                    Console.WriteLine("DRAW!!!!");
                    break;
                case EnPlayerNumber.one:
                    Console.WriteLine("Player 1 won!!!!");
                    break;
                case EnPlayerNumber.two:
                    Console.WriteLine("Player 2 won!!!!");
                    break;

            }
            Console.WriteLine();
            Console.WriteLine("Cards in player 1 stack:  " + Cards.Where(x => x.Location == EnLocation.team1stack).Count());
            Console.WriteLine("Player 1 final points: " + p1Points);
            Console.WriteLine();
            Console.WriteLine("Cards in player 2 stack:  " + Cards.Where(x => x.Location == EnLocation.team2stack).Count());
            Console.WriteLine("Player 2 final points: " + p2Points);
        }
    }



}
