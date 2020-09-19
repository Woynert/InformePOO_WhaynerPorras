using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Classes
{
    class Dealer
    {
        List<Card> deck = new List<Card>();
        List<Card> hand = new List<Card>();

        public void Init() //Dar dos cartas al Dealer
        {
            AddCard(Deal());
            AddCard(Deal());
        }

        public void Generate() //Generar
        {
            for (int j = 0; j <= 3; j++)
            {
                for (int i = 0; i <= 12; i++)
                {
                    deck.Add(new Card(j, i + 1));
                    //deck[j * 13 + i] = new Card(j, i+1);
                    //MessageBox.Show((j + 1).ToString() + ":   Value:" + (i + 1).ToString());
                }
            }
        }

        public void Randomize() //Revolver
        {
            List<Card> deck_copy = deck;
            deck = new List<Card>(); //clear

            Random _random = new Random();
            int n;

            //vaciar
            for (int i = 0; i < deck_copy.Count; i++)
            {
                n = _random.Next(deck_copy.Count - 1); //sel
                deck.Add(deck_copy[n]); //assign
                deck_copy.Remove(deck_copy[n]); //remove

            }

        }

        public Card Deal() //Seleccionar una carta
        {
            //Random _random = new Random();
            //int n = _random.Next(deck.Count - 1); //sel

            Card devolver = deck[0]; //save
            deck.Remove(devolver); //remove
            return (devolver); //return
        }

        public void AddCard(Card newCard)
        {
            hand.Add(newCard);
        }
    }
}
