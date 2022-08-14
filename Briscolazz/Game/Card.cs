using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Briscolazz.Program;

namespace Briscolazz
{
    public enum EnLocation
    {
        deck,
        briscola,
        player1hand,
        player2hand,
        //player3hand,
        //player4hand,
        team1stack,
        team2stack,
        tableFirst,
        tableSecond,
        //tableThird,
        //tableFourth
    }

    public enum EnSuit
    {
        bastoni,
        coppe,
        denari,
        spade
    }

    public enum EnValue
    {
        due,
        quattro,
        cinque,
        sei,
        sette,
        fante,
        cavallo,
        re, 
        tre,
        asso
    }
    public class Card
    {
        public EnSuit Suit;
        public EnValue Value;
        public EnLocation Location;
        public override string ToString()
        {
            return Value.ToString() + " di " + Suit.ToString();
        }

        public int Score()
        {
            switch (Value)
            {
                case EnValue.asso:
                    return 11;
                case EnValue.tre:
                    return 10;
                case EnValue.re:
                    return 4;
                case EnValue.cavallo:
                    return 3;
                case EnValue.fante:
                    return 2;
                default:
                    return 0;
            }
        }
    }

}
