namespace PokerHands
{
    public class Card
    {
        public CardValue CardValue { get; set; }
        public Suit Suit { get; set; }

        public Card(Suit suit, CardValue cardValue)
        {
            Suit = suit;
            CardValue = cardValue;
        }
    }
}