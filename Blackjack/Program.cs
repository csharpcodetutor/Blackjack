using Blackjack.Reources;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BlackjackWCards
{
    class Program
    {
        static void Main(string[] args)
        {
            Deck deck = new Deck();
            deck.Init();
            deck.TripleShuffle();
            Console.WriteLine("Enter your name");

            Player dealer = new Player();
            Player player1 = new Player { Name = Console.ReadLine() };

            Console.Clear();

            dealer.hand.CardsInHand.Add(deck.DealCard());
            dealer.hand.CardsInHand.Add(deck.DealCard(false));

            player1.hand.CardsInHand.Add(deck.DealCard(false));
            player1.hand.CardsInHand.Add(deck.DealCard(false));

            Console.OutputEncoding = Encoding.UTF8;

            DisplayTable(dealer, player1);

            int hitorstand = 0;

            while (hitorstand != 2)
            {

                if (player1.Score() > 21)
                {
                    DisplayTable(dealer, player1, true);
                    Console.CursorTop += 10;
                    Console.CursorLeft = 1;
                    Console.WriteLine($"{player1.Name} your busted, you lose!");

                    Console.WriteLine("Press any key to exit");
                    Console.ReadKey();
                    return;
                }
                else if (player1.Score() == 21)
                {
                    DisplayTable(dealer, player1, true);
                    Console.CursorTop += 10;
                    Console.CursorLeft = 1;
                    Console.WriteLine($"{player1.Name} scores 21, you win!");

                    Console.WriteLine("Press any key to exit");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("");
                Console.WriteLine("1. Hit");
                Console.WriteLine("2. Stand");

                Console.Write("Choice? ");

                var answer = Console.ReadKey().KeyChar.ToString();
                if (!int.TryParse(answer, out hitorstand))
                {
                    Console.WriteLine("");
                    Console.WriteLine("Invalid selection");
                }

                if (hitorstand != 1 && hitorstand != 2)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Invalid selection");
                }
                else if (hitorstand == 1)
                {
                    // Code to hit the player
                    player1.hand.CardsInHand.Add(deck.DealCard(false));

                    DisplayTable(dealer, player1);
                }

            }

            while (dealer.Score() < 17)
            {
                if (dealer.Score() > 21)
                {
                    DisplayTable(dealer, player1, true);
                    Console.CursorTop += 10;
                    Console.CursorLeft = 1;
                    Console.WriteLine("Dealer is busted, you win!");

                    Console.WriteLine("Press any key to exit");
                    Console.ReadKey();
                    return;
                }
                else if (dealer.Score() == 21)
                {
                    DisplayTable(dealer, player1, true);
                    Console.CursorTop += 10;
                    Console.CursorLeft = 1;
                    Console.WriteLine("Dealer scores 21, you lose!");

                    Console.WriteLine("Press any key to exit");
                    Console.ReadKey();
                    return;
                }

                dealer.hand.CardsInHand.Add(deck.DealCard(false));

                DisplayTable(dealer, player1);
            }

            if (dealer.Score() > 21)
            {
                DisplayTable(dealer, player1, true);
                Console.CursorTop += 10;
                Console.CursorLeft = 1;
                Console.WriteLine("Dealer is busted, you win!");

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
                return;
            }
            else if (dealer.Score() == player1.Score())
            {
                DisplayTable(dealer, player1, true);
                Console.CursorTop += 10;
                Console.CursorLeft = 1;
                Console.WriteLine("Dealer and Joseph tied, its a draw!");

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
                return;
            }
            else if (dealer.Score() > player1.Score())
            {
                DisplayTable(dealer, player1, true);
                Console.CursorTop += 10;
                Console.CursorLeft = 1;
                Console.WriteLine("Dealer has high hand, you lose!");

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
                return;
            }
            else
            {
                DisplayTable(dealer, player1, true);
                Console.CursorTop += 10;
                Console.CursorLeft = 1;
                Console.WriteLine($"{player1.Name} has the high hand, you win!");

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
                return;
            }


        }

        public static void DisplayTable(Player dealer, Player player, bool flip = false)
        {
            Console.Clear();

            Console.CursorLeft = 4;

            if (flip)
                Console.WriteLine($"DEALER ({dealer.Score()}) {ShowHand(dealer)}");
            else
                Console.WriteLine($"DEALER (XX) XX {dealer.hand.CardsInHand[1]}");

            Console.CursorTop = 1;
            Console.CursorLeft = 32;

            if (flip)
                DisplayCardGraphic(dealer, true);
            else
                DisplayCardGraphic(dealer);


            Console.CursorLeft = 4;
            Console.CursorTop += 10;

            Console.WriteLine($"{player.Name}  ({player.Score()}) {ShowHand(player)}");

            Console.CursorLeft = 32;

            if (flip)
                DisplayCardGraphic(player, true);
            else
                DisplayCardGraphic(player);
        }

        public static string ShowHand(Player player)
        {
            string hand = "";
            foreach (Card card in player.hand.CardsInHand)
            {
                hand += $"{card} ";
            }

            return hand;
        }


        public static void DisplayCardGraphic(Player player, bool flip = false)
        {
            int top = Console.CursorTop;
            Console.CursorLeft = 32;
            foreach (Card card in player.hand.CardsInHand)
            {
                if (flip)
                    card.Hidden = false;

                card.PrintCard();

                Console.CursorTop = top;
                Console.CursorLeft += 4;
            }

            Console.CursorTop += 1;
        }
    }



    public class Deck
    {
        List<Card> cards = new List<Card>();
        public List<Card> Cards
        {
            get
            {
                if (cards == null || cards.Count == 0)
                    LoadDeck();

                return cards;
            }
        }

        public void Init()
        {
            LoadDeck();
        }

        private void LoadDeck()
        {
            int i = 1;
            while (i < 14)
            {
                cards.Add(new Card { FaceName = CardSuite.Hearts, FaceValue = i, Hidden = true });
                cards.Add(new Card { FaceName = CardSuite.Diamonds, FaceValue = i, Hidden = true });
                cards.Add(new Card { FaceName = CardSuite.Clubs, FaceValue = i, Hidden = true });
                cards.Add(new Card { FaceName = CardSuite.Spades, FaceValue = i, Hidden = true });
                i++;
            }
        }

        public void TripleShuffle()
        {
            SingleShuffle();
            SingleShuffle();
            SingleShuffle();
        }

        private void SingleShuffle()
        {
            //RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            //var bytes = new byte[4];
            //provider.GetBytes(bytes);
            //int seed = BitConverter.ToInt32(bytes, 0);

            Random rand = new Random();

            for (int i = 0; i < cards.Count; i++)
            {
                var card1 = cards[i];
                int index = i + rand.Next(cards.Count - i);
                var card2 = cards[index];

                cards[index] = card1;
                cards[i] = card2;
            }
        }

        public Card DealCard(bool hide = true)
        {
            var card = cards[0];
            cards.Remove(card);

            card.Hidden = hide;

            return card;
        }
    }

    public class Player
    {
        public string Name { get; set; }
        public Hand hand = new Hand();

        public int Score()
        {
            int total = 0;
            foreach (Card card in hand.CardsInHand)
                total += card.FaceValue;

            return total;
        }
    }

    public class Hand
    {
        public List<Card> CardsInHand = new List<Card>();
    }

   


}
