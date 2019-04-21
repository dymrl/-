using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doudizhu
{
    class Program
    {
        static void Main(string[] args)
        {
            Player[] Players = { new Player("Bill", false), new Player("Tom", false), new Player("Jack", true) };
            DealCard(ref Players);
            for (int j = 0; j < 3; j++)
            {
                Console.WriteLine(Players[j].ToString());
                Players[j].Cards.Sort();
                if (Players[j].isLord)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        Console.WriteLine(i + "\t" + Players[j].Cards[i].ToString());
                    }
                }
                else
                {
                    for (int i = 0; i < 17; i++)
                    {
                        Console.WriteLine(i + "\t" + Players[j].Cards[i].ToString());
                    }
                }


            }
            Console.ReadLine();
        }

        static private int[] ShuffleCards()
        {
            int[] CardIndex = new int[54];
            bool[] isCreate = new bool[54];
            for (int i = 0; i < 54; i++)
            {
                isCreate[i] = false;
            }
            Random rnd = new Random();
            for (int i = 0; i < 54; i++)
            {
                int j;
                do
                {
                    j = rnd.Next(0, 54);
                    if (!isCreate[j])
                    {
                        CardIndex[i] = j;
                    }
                } while (isCreate[j]);
                isCreate[j] = true;
            }
            return CardIndex;
        }

        static private Card[] MakeCards()
        {
            Card[] Cards = new Card[54];
            int i = 0;
            int j = 0;
            int k = 0;
            while (i < 44)  //生成3-K
            {
                while (j < 11)
                {
                    while (k < 4)
                    {
                        Cards[i] = new Card((Color)k, j + 3);
                        k++;
                        i++;
                    }
                    k = 0;
                    j++;
                }
            }
            j = 0;
            k = 0;
            while (i < 52)  //生成A\2
            {
                while (j < 2)
                {
                    while (k < 4)
                    {
                        Cards[i] = new Card((Color)k, j + 1);
                        k++;
                        i++;
                    }
                    k = 0;
                    j++;
                }
            }
            Cards[52] = new Card(Color.BlackJoker, 53);
            Cards[53] = new Card(Color.ColorJoker, 54);
            return Cards;
        }

        static private void DealCard(ref Player[] Players)
        {
            Card[] Cards = new Card[54];
            int[] CardIndex = ShuffleCards();
            Cards = MakeCards();
            /*for(int i =0; i < 54; i++)
            {
                Console.WriteLine(CardIndex[i]);
            }*/
            int k = 0;
            for(int i=0; i < 3; i++)
            {
                for(int j = 0; j < 17; j++)
                {
                    //Console.WriteLine("{0} {1} {2}", i, j, k);
                    Players[i].Cards.Add(Cards[CardIndex[k]]);
                    k++;
                }
                if (Players[i].isLord)
                {
                    for(int j= 17; j < 20; j++)
                    {
                        Players[i].Cards.Add(Cards[CardIndex[k]]);
                        k++;
                    }
                }
            }
        }

    }

    public class Player
    {
        private string Name
        { set; get; }

        public bool isLord;
        public List<Card> Cards;
        public int CardCount = 17;


        public string GetName()
        {
            return Name;
        }

        public void SetName(string Value)
        {
            Name = Value;
        }

        public Player(string Name, bool isLord)
        {
            SetName(Name);
            this.isLord = isLord;
            if (isLord)
                CardCount = 20;
            Cards = new List<Card>();
        }

        public Player()
        {

        }

        public override string ToString()
        {
            if (isLord)
                return "Lord " + Name;
            else
                return "Farmer " + Name;
        }
    }

    public enum Color
    {
        Spade = 0,
        Heart = 1,
        Diamond = 2,
        Club = 3,
        BlackJoker = 4,
        ColorJoker = 5
    }

    public class Card:IComparable<Card>
    {
        public Color Color;
        public int Num;
        //public int Index;
        public Card(Color Color, int Num)
        {
            this.Color = Color;
            this.Num = Num;
        }
        public override string ToString()
        {
            switch (Num)
            {
                case 1:
                    return "A " + Color;
                case 11:
                    return "J " + Color;
                case 12:
                    return "Q " + Color;
                case 13:
                    return "K " + Color;
                case 53:
                    return Color.ToString();
                case 54:
                    return Color.ToString();
                default:
                    return Num + " " + Color;
            }
        }
        public int CompareTo(Card other)
        {
            if (this.Num == other.Num)
                return this.Color.CompareTo(other.Color);
            else
                return this.Num.CompareTo(other.Num);
        }
    }
}

