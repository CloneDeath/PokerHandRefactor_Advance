using System.Collections.Generic;
using System.Linq;

namespace PokerHands
{
    public class HandState
    {
        public List<CardValueGroup> _hand1CardValueGroups;
        public List<CardValueGroup> _hand2CardValueGroups;
        public int _hand1LowestValue;
        public bool _hand1HasFiveCardGroups;
        public int _hand2lowestValue;
        public bool _hand2HasFiveCardGroups;

        public void ProcessBothHands()
        {
            _hand1LowestValue = _hand1CardValueGroups.OrderBy(i => (int) i.Value).Select(i => (int) i.Value).FirstOrDefault();

            _hand1HasFiveCardGroups = _hand1CardValueGroups.Count == 5;

            if (_hand1CardValueGroups.Count == 5)
            {
                if (_hand1CardValueGroups.Any(i => i.Value == CardValue.Ace))
                {
                    for (var i = 0; i < 4; i++)
                    {
                        if (_hand1CardValueGroups.Any(v => (int) v.Value == (i))) ;
                        else
                        {
                            _hand1HasFiveCardGroups = false;

                            break;
                        }
                    }
                }
                else
                {
                    for (var i = 0; i < _hand1CardValueGroups.Count; i++)
                    {
                        if (_hand1CardValueGroups.Any(v => (int) v.Value == (_hand1LowestValue))) _hand1LowestValue = _hand1LowestValue + 1;
                        else
                        {
                            _hand1HasFiveCardGroups = false;

                            break;
                        }
                    }
                }
            }

            _hand2lowestValue = _hand2CardValueGroups.OrderBy(i => (int) i.Value).Select(i => (int) i.Value).FirstOrDefault();

            _hand2HasFiveCardGroups = _hand2CardValueGroups.Count == 5;

            if (_hand2CardValueGroups.All(i => i.Count > 0 && i.Count < 2))
            {
                if (_hand2CardValueGroups.Any(i => i.Value == CardValue.Ace))
                {
                    for (var i = 0; i < 4; i++)
                    {
                        if (_hand2CardValueGroups.Any(v => (int) v.Value == (i))) ;
                        else
                        {
                            _hand2HasFiveCardGroups = false;

                            break;
                        }
                    }
                }
                else
                {
                    foreach (var value in _hand2CardValueGroups)
                    {
                        if (_hand2CardValueGroups.Any(v => (int) v.Value == (_hand2lowestValue))) _hand2lowestValue = _hand2lowestValue + 1;
                        else
                        {
                            _hand2HasFiveCardGroups = false;

                            break;
                        }
                    }
                }
            }
        }

        public void GetCardHandGroups(IList<Card> hand1, IList<Card> hand2)
        {
            _hand1CardValueGroups = GetCardValueGroups(hand1);
            _hand2CardValueGroups = GetCardValueGroups(hand2);
        }

        private static List<CardValueGroup> GetCardValueGroups(IList<Card> hand)
        {
            return hand.GroupBy(i => i.CardValue).Select(g => new CardValueGroup
            {
                Value = g.Key,
                Count = g.Select(v => (int)v.CardValue).Count()
            }).ToList();
        }
    }
}