using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Classes
{
    class Player
    {

        List<Card> hand = new List<Card>();

        public void Init(Card newCard1, Card newCard2) //Dar dos cartas al Player
        {
            //AddCard(newCard1);
            //AddCard(newCard2);
        }

        public void AddCard(Card newCard)
        {
            hand.Add(newCard);
        }

        public int getCardCount()
        {
            return (hand.Count);
        }

        public List<Card> Hand { get => hand; }

    }
}
