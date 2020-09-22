using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BlackJack.Classes
{
    class Dealer
    {
        public List<Card> deck = new List<Card>();
        public List<Card> hand = new List<Card>();

        /*public Dealer() //Contructor
        {
            Init();
        }*/

        public void Init() //Dar dos cartas al Dealer
        {
            Generate();
            Randomize();
            //AddCard(Deal());
            //AddCard(Deal());
        }

        public void Generate() //Generar
        {
            deck = new List<Card>();
            deck.Add(new Card(0, 1)); //fix
            for (int j = 0; j <= 3; j++)
            {
                for (int i = 1; i <= 13; i++)
                {
                    deck.Add(new Card(j, i));
                    //deck[j * 13 + i] = new Card(j, i+1);
                    //MessageBox.Show((j + 1).ToString() + ":   Value:" + (i + 1).ToString());
                }
            }

            //MessageBox.Show(deck.Count.ToString());
        }

        public void Randomize() //Revolver
        {
            List<Card> deck_copy = this.deck.ToList(); 

            //MessageBox.Show(deck_copy.Count.ToString() + " 1: " + this.deck.Count.ToString());

            this.deck = new List<Card>(); //clear

            Random _random = new Random();
            int n;

            int _max = deck_copy.Count;
            //vaciar
            for (int i = 0; i < _max; i++)
            {
                n = _random.Next(0, deck_copy.Count - 1); //sel
                this.deck.Add(deck_copy[n]); //assign
                deck_copy.Remove(deck_copy[n]); //remove
            }

            //MessageBox.Show(deck_copy.Count.ToString()+ " 4: " + this.deck.Count.ToString());
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

        public void RemoveCard(Card newCard)
        {
            hand.Remove(newCard);
        }

        public int getCardCount()
        {
            return (hand.Count);
        }
        public List<Card> Hand { get => hand; set => hand = value; }
    }
}
