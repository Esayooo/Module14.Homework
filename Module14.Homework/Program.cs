using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module14.Homework
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.StartGame();
        }
    }
    class Game
    {
        private List<Player> players;
        private List<Karta> deck;

        public Game()
        {
            players = new List<Player>();
            deck = GenerateDeck();
            ShuffleDeck();
            InitializePlayers(2);
            DealCards();
        }

        public void StartGame()
        {
            while (!GameIsOver())
            {
                PlayRound();
            }

            Player winner = players.OrderByDescending(p => p.GetScore()).First();
            Console.WriteLine($"Игрок {winner.Name} выиграл игру!");
        }

        private void PlayRound()
        {
            List<Karta> roundCards = players.Select(p => p.PlayCard()).ToList();
            Player roundWinner = GetRoundWinner(roundCards);
            Console.WriteLine($"{roundWinner.Name} выигрывает раунд!");
            roundWinner.AddCardsToDeck(roundCards);
        }

        private Player GetRoundWinner(List<Karta> roundCards)
        {
            return players.OrderByDescending(p => p.GetTopCardValue(roundCards)).First();
        }

        private bool GameIsOver()
        {
            return players.Any(p => p.GetCardCount() == 0);
        }

        private void InitializePlayers(int playerCount)
        {
            for (int i = 1; i <= playerCount; i++)
            {
                players.Add(new Player($"Игрок {i}"));
            }
        }

        private void DealCards()
        {
            int playerIndex = 0;

            foreach (var card in deck)
            {
                players[playerIndex].AddCard(card);
                playerIndex = (playerIndex + 1) % players.Count;
            }
        }

        private List<Karta> GenerateDeck()
        {
            List<Karta> newDeck = new List<Karta>();

            foreach (KartaType type in Enum.GetValues(typeof(KartaType)))
            {
                foreach (KartaSuit suit in Enum.GetValues(typeof(KartaSuit)))
                {
                    newDeck.Add(new Karta(type, suit));
                }
            }

            return newDeck;
        }

        private void ShuffleDeck()
        {
            Random random = new Random();
            deck = deck.OrderBy(card => random.Next()).ToList();
        }
    }

    class Player
    {
        private List<Karta> hand;

        public string Name { get; }

        public Player(string name)
        {
            Name = name;
            hand = new List<Karta>();
        }

        public Karta PlayCard()
        {
            Karta playedCard = hand.First();
            hand.Remove(playedCard);
            return playedCard;
        }

        public void AddCard(Karta card)
        {
            hand.Add(card);
        }

        public void AddCardsToDeck(List<Karta> cards)
        {
            hand.AddRange(cards);
        }

        public int GetCardCount()
        {
            return hand.Count;
        }

        public int GetTopCardValue(List<Karta> roundCards)
        {
            return roundCards.Contains(hand.First()) ? 1 : 0;
        }

        public int GetScore()
        {
            return hand.Count;
        }
    }

    class Karta
    {
        public KartaType Type { get; }
        public KartaSuit Suit { get; }

        public Karta(KartaType type, KartaSuit suit)
        {
            Type = type;
            Suit = suit;
        }
    }

    enum KartaType
    {
        Six = 6,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }

    enum KartaSuit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }
}
