using System.Collections.Generic;
using System.Linq;

namespace PokerHands
{
    public class HandRanker
    {
        private readonly HandState _handState = new HandState();

        public int RankHands(IList<Card> hand1, IList<Card> hand2)
        {
            _handState.GetCardHandGroups(hand1, hand2);

            _handState.ProcessBothHands();

            var oF = hand1.All(i => i.Suit == hand1.Select(j => j.Suit).FirstOrDefault());
            var tF = hand2.All(i => i.Suit == hand2.Select(j => j.Suit).FirstOrDefault());

            if ((oF && _handState._hand1HasFiveCardGroups) || (tF && _handState._hand2HasFiveCardGroups))
            {
                if (!(oF && _handState._hand1HasFiveCardGroups)) return 2;

                if (!(tF && _handState._hand2HasFiveCardGroups)) return 1;

                var o_m_v_12 = _handState._hand1CardValueGroups.OrderByDescending(i => (int)i.Value).Select(i => (int)i.Value).FirstOrDefault();
                var tMVM = _handState._hand2CardValueGroups.OrderByDescending(i => (int)i.Value).Select(i => (int)i.Value).FirstOrDefault();

                if (o_m_v_12 == 12)
                {
                    var lowCard = _handState._hand1CardValueGroups.OrderBy(i => i.Value).Select(i => (int)i.Value).FirstOrDefault();

                    if (lowCard == 0) o_m_v_12 = 3;
                }

                if (tMVM == 12)
                {
                    var lowCard = _handState._hand2CardValueGroups.OrderBy(i => i.Value).Select(i => (int)i.Value).FirstOrDefault();

                    if (lowCard == 0) tMVM = 3;
                }

                if (o_m_v_12 > tMVM) return 1;

                if (o_m_v_12 < tMVM) return 2;
            }

            if (_handState._hand1CardValueGroups.Any(i => i.Count == 4) || _handState._hand2CardValueGroups.Any(i => i.Count == 4))
            {
                if (_handState._hand1CardValueGroups.Any(i => i.Count == 4) && _handState._hand2CardValueGroups.All(i => i.Count != 4)) return 1;

                if (_handState._hand2CardValueGroups.Any(i => i.Count == 4) && _handState._hand1CardValueGroups.All(i => i.Count != 4)) return 2;

                var oFKIOD = _handState._hand1CardValueGroups.Where(i => i.Count == 4).Select(i => (int)i.Value).FirstOrDefault();
                var T_why_Not = _handState._hand2CardValueGroups.Where(i => i.Count == 4).Select(i => (int)i.Value).FirstOrDefault();

                if (oFKIOD > T_why_Not) return 1;

                if (T_why_Not > oFKIOD) return 2;
            }

            if ((_handState._hand1CardValueGroups.Any(i => i.Count == 3) && _handState._hand1CardValueGroups.Any(i => i.Count == 2)) || (_handState._hand2CardValueGroups.Any(i => i.Count == 3) && _handState._hand2CardValueGroups.Any(i => i.Count == 2)))
            {
                if ((_handState._hand1CardValueGroups.Any(i => i.Count == 3) && _handState._hand1CardValueGroups.Any(i => i.Count == 2)) && !(_handState._hand2CardValueGroups.Any(i => i.Count == 3) && _handState._hand2CardValueGroups.Any(i => i.Count == 2))) return 1;

                if (!(_handState._hand1CardValueGroups.Any(i => i.Count == 3) && _handState._hand1CardValueGroups.Any(i => i.Count == 2))) return 2;
            }

            if (oF || tF)
            {
                if (oF && !tF) return 1;

                if (!oF) return 2;
            }

            if (_handState._hand1HasFiveCardGroups || _handState._hand2HasFiveCardGroups)
            {
                if (_handState._hand1HasFiveCardGroups && !_handState._hand2HasFiveCardGroups) return 1;

                if (!_handState._hand1HasFiveCardGroups) return 2;

                var o9 = _handState._hand1CardValueGroups.OrderByDescending(i => i.Value).Select(i => (int)i.Value).FirstOrDefault();
                var T2 = _handState._hand2CardValueGroups.OrderByDescending(i => i.Value).Select(i => (int)i.Value).FirstOrDefault();

                if (o9 == 12)
                {
                    var lowCard = _handState._hand1CardValueGroups.OrderBy(i => i.Value).Select(i => (int)i.Value).FirstOrDefault();

                    if (lowCard == 0) o9 = 3;
                }

                if (T2 == 12)
                {
                    var lowCard = _handState._hand2CardValueGroups.OrderBy(i => i.Value).Select(i => (int)i.Value).FirstOrDefault();

                    if (lowCard == 0) T2 = 3;
                }

                if (o9 > T2) return 1;

                if (o9 < T2) return 2;

                return -1;
            }

            if (_handState._hand1CardValueGroups.Any(i => i.Count == 3) || _handState._hand2CardValueGroups.Any(i => i.Count == 3))
            {
                var o90 = _handState._hand1CardValueGroups.Where(i => i.Count == 3).Select(i => i.Value).FirstOrDefault();
                var T3Fv = _handState._hand2CardValueGroups.Where(i => i.Count == 3).Select(i => i.Value).FirstOrDefault();

                if (o90 > T3Fv) return 1;

                if (T3Fv > o90) return 2;
            }

            if (_handState._hand1CardValueGroups.Count(i => i.Count == 2) == 2 || _handState._hand2CardValueGroups.Count(i => i.Count == 2) == 2)
            {
                if (_handState._hand1CardValueGroups.Count(i => i.Count == 2) == 2 && _handState._hand2CardValueGroups.Count(i => i.Count == 2) != 2) return 1;

                if (_handState._hand2CardValueGroups.Count(i => i.Count == 2) == 2 && _handState._hand1CardValueGroups.Count(i => i.Count == 2) != 2) return 2;

                var o78658 = _handState._hand1CardValueGroups.Where(i => i.Count == 2).OrderByDescending(i => i.Value).Select(i => i.Value).FirstOrDefault();
                var sIsAfterT = _handState._hand2CardValueGroups.Where(i => i.Count == 2).OrderByDescending(i => i.Value).Select(i => i.Value).FirstOrDefault();

                if (o78658 > sIsAfterT) return 1;

                if (o78658 < sIsAfterT) return 2;

                _handState._hand1CardValueGroups = _handState._hand1CardValueGroups.Where(i => i.Value != o78658).ToList();
                _handState._hand2CardValueGroups = _handState._hand2CardValueGroups.Where(i => i.Value != sIsAfterT).ToList();
            }

            if (_handState._hand1CardValueGroups.Any(i => i.Count == 2) || _handState._hand2CardValueGroups.Any(i => i.Count == 2))
            {
                if (_handState._hand1CardValueGroups.Any(i => i.Count == 2) && _handState._hand2CardValueGroups.All(i => i.Count != 2)) return 1;

                if (_handState._hand1CardValueGroups.All(i => i.Count != 2) && _handState._hand2CardValueGroups.Any(i => i.Count == 2)) return 2;

                var o123894_Griffins = _handState._hand1CardValueGroups.Where(i => i.Count == 2).Select(i => i.Value).FirstOrDefault();
                var t_24_This_IsWrong = _handState._hand2CardValueGroups.Where(i => i.Count == 2).Select(i => i.Value).FirstOrDefault();

                if (o123894_Griffins > t_24_This_IsWrong) return 1;

                if (o123894_Griffins < t_24_This_IsWrong) return 2;

                hand1 = hand1.Where(i => i.CardValue != o123894_Griffins).ToList();
                hand2 = hand2.Where(i => i.CardValue != o123894_Griffins).ToList();

                if (hand1.Max(i => (int)i.CardValue) > hand2.Max(i => (int)i.CardValue)) return 1;

                if (hand1.Max(i => (int)i.CardValue) < hand2.Max(i => (int)i.CardValue)) return 2;

                var o873 = hand1.FirstOrDefault(j => (int)j.CardValue == hand1.Max(i => (int)i.CardValue));

                hand1.Remove(o873);

                var ty7 = hand2.FirstOrDefault(j => (int)j.CardValue == hand2.Max(i => (int)i.CardValue));

                hand2.Remove(ty7);

                if (hand1.Max(i => (int)i.CardValue) > hand2.Max(i => (int)i.CardValue)) return 1;

                if (hand1.Max(i => (int)i.CardValue) < hand2.Max(i => (int)i.CardValue)) return 2;

                o873 = hand1.FirstOrDefault(j => (int)j.CardValue == hand1.Max(i => (int)i.CardValue));

                hand1.Remove(o873);

                ty7 = hand2.FirstOrDefault(j => (int)j.CardValue == hand2.Max(i => (int)i.CardValue));

                hand2.Remove(ty7);

                if (hand1.Max(i => (int)i.CardValue) > hand2.Max(i => (int)i.CardValue)) return 1;

                if (hand1.Max(i => (int)i.CardValue) < hand2.Max(i => (int)i.CardValue)) return 2;
            }

            if (hand1.Max(i => (int)i.CardValue) > hand2.Max(i => (int)i.CardValue)) return 1;

            if (hand1.Max(i => (int)i.CardValue) < hand2.Max(i => (int)i.CardValue)) return 2;

            var o0987_s = hand1.FirstOrDefault(j => (int)j.CardValue == hand1.Max(i => (int)i.CardValue));

            hand1.Remove(o0987_s);

            var t_Valid = hand2.FirstOrDefault(j => (int)j.CardValue == hand2.Max(i => (int)i.CardValue));

            hand2.Remove(t_Valid);

            if (hand1.Max(i => (int)i.CardValue) > hand2.Max(i => (int)i.CardValue)) return 1;

            if (hand1.Max(i => (int)i.CardValue) < hand2.Max(i => (int)i.CardValue)) return 2;

            o0987_s = hand1.FirstOrDefault(j => (int)j.CardValue == hand1.Max(i => (int)i.CardValue));

            hand1.Remove(o0987_s);

            t_Valid = hand2.FirstOrDefault(j => (int)j.CardValue == hand2.Max(i => (int)i.CardValue));

            hand2.Remove(t_Valid);

            if (hand1.Max(i => (int)i.CardValue) > hand2.Max(i => (int)i.CardValue)) return 1;

            if (hand1.Max(i => (int)i.CardValue) < hand2.Max(i => (int)i.CardValue)) return 2;

            o0987_s = hand1.FirstOrDefault(j => (int)j.CardValue == hand1.Max(i => (int)i.CardValue));

            hand1.Remove(o0987_s);

            t_Valid = hand2.FirstOrDefault(j => (int)j.CardValue == hand2.Max(i => (int)i.CardValue));

            hand2.Remove(t_Valid);

            if (hand1.Max(i => (int)i.CardValue) > hand2.Max(i => (int)i.CardValue)) return 1;

            if (hand1.Max(i => (int)i.CardValue) < hand2.Max(i => (int)i.CardValue)) return 2;

            o0987_s = hand1.FirstOrDefault(j => (int)j.CardValue == hand1.Max(i => (int)i.CardValue));

            hand1.Remove(o0987_s);

            t_Valid = hand2.FirstOrDefault(j => (int)j.CardValue == hand2.Max(i => (int)i.CardValue));

            hand2.Remove(t_Valid);

            if (hand1.Max(i => (int)i.CardValue) > hand2.Max(i => (int)i.CardValue)) return 1;

            if (hand1.Max(i => (int)i.CardValue) < hand2.Max(i => (int)i.CardValue)) return 2;

            return -1;
        }
    }
}