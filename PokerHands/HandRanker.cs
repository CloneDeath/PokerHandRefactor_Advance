using System.Collections.Generic;
using System.Linq;

namespace PokerHands
{
    public class HandRanker
    {
        private class CardValueGroup
        {
            public CardValue Value { get; set; }
            public int Count { get; set; }
        }

        public int RankHands(IList<Card> hand1, IList<Card> hand2)
        {
            var hand1CardValueGroups = GetCardValueGroups(hand1);

            var hand2CardValueGroups = GetCardValueGroups(hand2);

            var oVl = hand1CardValueGroups.OrderBy(i => (int)i.Value).Select(i => (int)i.Value).FirstOrDefault();

            var oCon = hand1CardValueGroups.Count == 5;

            if (hand1CardValueGroups.Count == 5)
            {
                if (hand1CardValueGroups.Any(i => i.Value == CardValue.Ace))
                {
                    for (var i = 0; i < 4; i++)
                    {
                        if (hand1CardValueGroups.Any(v => (int)v.Value == (i))) ;
                        else
                        {
                            oCon = false;

                            break;
                        }
                    }
                }
                else
                {
                    for (var i = 0; i < hand1CardValueGroups.Count; i++)
                    {
                        if (hand1CardValueGroups.Any(v => (int)v.Value == (oVl))) oVl = oVl + 1;
                        else
                        {
                            oCon = false;

                            break;
                        }
                    }
                }
            }

            var tVl = hand2CardValueGroups.OrderBy(i => (int)i.Value).Select(i => (int)i.Value).FirstOrDefault();

            var tCon = hand2CardValueGroups.Count == 5;

            if (hand2CardValueGroups.All(i => i.Count > 0 && i.Count < 2))
            {
                if (hand2CardValueGroups.Any(i => i.Value == CardValue.Ace))
                {
                    for (var i = 0; i < 4; i++)
                    {
                        if (hand2CardValueGroups.Any(v => (int)v.Value == (i))) ;
                        else
                        {
                            tCon = false;

                            break;
                        }
                    }
                }
                else
                {
                    foreach (var value in hand2CardValueGroups)
                    {
                        if (hand2CardValueGroups.Any(v => (int)v.Value == (tVl))) tVl = tVl + 1;
                        else
                        {
                            tCon = false;

                            break;
                        }
                    }
                }
            }

            var oF = hand1.All(i => i.Suit == hand1.Select(j => j.Suit).FirstOrDefault());
            var tF = hand2.All(i => i.Suit == hand2.Select(j => j.Suit).FirstOrDefault());

            if ((oF && oCon) || (tF && tCon))
            {
                if (!(oF && oCon)) return 2;

                if (!(tF && tCon)) return 1;

                var o_m_v_12 = hand1CardValueGroups.OrderByDescending(i => (int)i.Value).Select(i => (int)i.Value).FirstOrDefault();
                var tMVM = hand2CardValueGroups.OrderByDescending(i => (int)i.Value).Select(i => (int)i.Value).FirstOrDefault();

                if (o_m_v_12 == 12)
                {
                    var lowCard = hand1CardValueGroups.OrderBy(i => i.Value).Select(i => (int)i.Value).FirstOrDefault();

                    if (lowCard == 0) o_m_v_12 = 3;
                }

                if (tMVM == 12)
                {
                    var lowCard = hand2CardValueGroups.OrderBy(i => i.Value).Select(i => (int)i.Value).FirstOrDefault();

                    if (lowCard == 0) tMVM = 3;
                }

                if (o_m_v_12 > tMVM) return 1;

                if (o_m_v_12 < tMVM) return 2;
            }

            if (hand1CardValueGroups.Any(i => i.Count == 4) || hand2CardValueGroups.Any(i => i.Count == 4))
            {
                if (hand1CardValueGroups.Any(i => i.Count == 4) && hand2CardValueGroups.All(i => i.Count != 4)) return 1;

                if (hand2CardValueGroups.Any(i => i.Count == 4) && hand1CardValueGroups.All(i => i.Count != 4)) return 2;

                var oFKIOD = hand1CardValueGroups.Where(i => i.Count == 4).Select(i => (int)i.Value).FirstOrDefault();
                var T_why_Not = hand2CardValueGroups.Where(i => i.Count == 4).Select(i => (int)i.Value).FirstOrDefault();

                if (oFKIOD > T_why_Not) return 1;

                if (T_why_Not > oFKIOD) return 2;
            }

            if ((hand1CardValueGroups.Any(i => i.Count == 3) && hand1CardValueGroups.Any(i => i.Count == 2)) || (hand2CardValueGroups.Any(i => i.Count == 3) && hand2CardValueGroups.Any(i => i.Count == 2)))
            {
                if ((hand1CardValueGroups.Any(i => i.Count == 3) && hand1CardValueGroups.Any(i => i.Count == 2)) && !(hand2CardValueGroups.Any(i => i.Count == 3) && hand2CardValueGroups.Any(i => i.Count == 2))) return 1;

                if (!(hand1CardValueGroups.Any(i => i.Count == 3) && hand1CardValueGroups.Any(i => i.Count == 2))) return 2;
            }

            if (oF || tF)
            {
                if (oF && !tF) return 1;

                if (!oF) return 2;
            }

            if (oCon || tCon)
            {
                if (oCon && !tCon) return 1;

                if (!oCon) return 2;

                var o9 = hand1CardValueGroups.OrderByDescending(i => i.Value).Select(i => (int)i.Value).FirstOrDefault();
                var T2 = hand2CardValueGroups.OrderByDescending(i => i.Value).Select(i => (int)i.Value).FirstOrDefault();

                if (o9 == 12)
                {
                    var lowCard = hand1CardValueGroups.OrderBy(i => i.Value).Select(i => (int)i.Value).FirstOrDefault();

                    if (lowCard == 0) o9 = 3;
                }

                if (T2 == 12)
                {
                    var lowCard = hand2CardValueGroups.OrderBy(i => i.Value).Select(i => (int)i.Value).FirstOrDefault();

                    if (lowCard == 0) T2 = 3;
                }

                if (o9 > T2) return 1;

                if (o9 < T2) return 2;

                return -1;
            }

            if (hand1CardValueGroups.Any(i => i.Count == 3) || hand2CardValueGroups.Any(i => i.Count == 3))
            {
                var o90 = hand1CardValueGroups.Where(i => i.Count == 3).Select(i => i.Value).FirstOrDefault();
                var T3Fv = hand2CardValueGroups.Where(i => i.Count == 3).Select(i => i.Value).FirstOrDefault();

                if (o90 > T3Fv) return 1;

                if (T3Fv > o90) return 2;
            }

            if (hand1CardValueGroups.Count(i => i.Count == 2) == 2 || hand2CardValueGroups.Count(i => i.Count == 2) == 2)
            {
                if (hand1CardValueGroups.Count(i => i.Count == 2) == 2 && hand2CardValueGroups.Count(i => i.Count == 2) != 2) return 1;

                if (hand2CardValueGroups.Count(i => i.Count == 2) == 2 && hand1CardValueGroups.Count(i => i.Count == 2) != 2) return 2;

                var o78658 = hand1CardValueGroups.Where(i => i.Count == 2).OrderByDescending(i => i.Value).Select(i => i.Value).FirstOrDefault();
                var sIsAfterT = hand2CardValueGroups.Where(i => i.Count == 2).OrderByDescending(i => i.Value).Select(i => i.Value).FirstOrDefault();

                if (o78658 > sIsAfterT) return 1;

                if (o78658 < sIsAfterT) return 2;

                hand1CardValueGroups = hand1CardValueGroups.Where(i => i.Value != o78658).ToList();
                hand2CardValueGroups = hand2CardValueGroups.Where(i => i.Value != sIsAfterT).ToList();
            }

            if (hand1CardValueGroups.Any(i => i.Count == 2) || hand2CardValueGroups.Any(i => i.Count == 2))
            {
                if (hand1CardValueGroups.Any(i => i.Count == 2) && hand2CardValueGroups.All(i => i.Count != 2)) return 1;

                if (hand1CardValueGroups.All(i => i.Count != 2) && hand2CardValueGroups.Any(i => i.Count == 2)) return 2;

                var o123894_Griffins = hand1CardValueGroups.Where(i => i.Count == 2).Select(i => i.Value).FirstOrDefault();
                var t_24_This_IsWrong = hand2CardValueGroups.Where(i => i.Count == 2).Select(i => i.Value).FirstOrDefault();

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