using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Briscolazz.Program;

namespace Briscolazz
{
    public enum EnPlayerNumber
    {
        one,
        two
    }
    public abstract class Agent
    {
        
        public Agent(Game game)
        {
            this.Game = game;
        }

        public Game Game;

        public EnPlayerNumber PlayerNumber;

        public abstract Card PickCard();
    }

}
