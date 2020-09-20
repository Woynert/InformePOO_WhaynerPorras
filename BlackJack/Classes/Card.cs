using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    class Card
    {
        public int suit; //0Corazon 1Diamante 2Trevor 3Pica
        public int symbol; //11J 12Q 13K

        int score; //1->11
        int color; //0Negro 1Rojo

        public Card(int suit, int symbol)
        {
            this.suit   = suit;
            this.symbol = symbol;

            //valor
            if (this.symbol > 10)
            {
                this.score = 10;
            }
            else this.score = symbol;

            //color
            switch (this.suit)
            {
                case 0: this.color = 1; break;
                case 1: this.color = 1; break;
                case 2: this.color = 0; break;
                case 3: this.color = 0; break;
            }
        }

        public int Suit { get => suit; set => suit = value; }
        public int Symbol { get => symbol; set => symbol = value; }
        public int Score { get => score; set => score = value; }
    }
}
