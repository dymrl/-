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
                Console.WriteLine(Players[j].Cards.Count);
            }

            PokerGroup CardsA= new PokerGroup();
            for(int i=0; i < 3; i++)
            {
                Card tempCard = new Card((Color)i, 9);
                CardsA.Cards.Add(tempCard);
            }
            CardsA.Cards.Add(new Card((Color)2, 5));
            foreach (var tempCard in CardsA.Cards)
            {
                Console.WriteLine(tempCard.ToString());
            }
            Console.WriteLine(IsRules(CardsA));
            Console.WriteLine(CardsA.type);

            PokerGroup CardsB = new PokerGroup();
            for (int i = 0; i < 3; i++)
            {
                Card tempCard = new Card((Color)i, 11);
                CardsB.Cards.Add(tempCard);
            }
            CardsB.Cards.Add(new Card((Color)2, 5));
            foreach (var tempCard in CardsB.Cards)
            {
                Console.WriteLine(tempCard.ToString());
            }
            Console.WriteLine(IsRules(CardsB));
            Console.WriteLine(CardsB.type);

            Console.WriteLine(CardsA > CardsB);

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
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 17; j++)
                {
                    //Console.WriteLine("{0} {1} {2}", i, j, k);
                    Players[i].Cards.Add(Cards[CardIndex[k]]);
                    k++;
                }
                if (Players[i].isLord)
                {
                    for (int j = 17; j < 20; j++)
                    {
                        Players[i].Cards.Add(Cards[CardIndex[k]]);
                        k++;
                    }
                }
            }
        }

        public static bool IsRules(PokerGroup leadPokers) //判断所出牌组类型以及其是否符合规则
        {
            bool isRule = false;
            leadPokers.Cards.Sort();
            switch (leadPokers.Cards.Count)
            {
                case 0:
                    isRule = false;
                    break;
                case 1:
                    isRule = true;
                    leadPokers.type = PokerGroupType.单张;
                    break;
                case 2:
                    if (IsSame(leadPokers, 2))
                    {
                        isRule = true;
                        leadPokers.type = PokerGroupType.对子;
                    }
                    else
                    {
                        if (leadPokers.Cards[0].Num == 53 && leadPokers.Cards[1].Num == 54)
                        {
                            leadPokers.type = PokerGroupType.双王;
                            isRule = true;
                        }
                        else
                        {
                            isRule = false;
                        }
                    }
                    break;
                case 3:
                    if (IsSame(leadPokers, 3))
                    {
                        leadPokers.type = PokerGroupType.三张相同;
                        isRule = true;
                    }
                    else
                    {
                        isRule = false;
                    }
                    break;
                case 4:
                    if (IsSame(leadPokers, 4))
                    {
                        leadPokers.type = PokerGroupType.炸弹;
                        isRule = true;
                    }
                    else
                    {
                        if (IsThreeLinkPokers(leadPokers))
                        {
                            leadPokers.type = PokerGroupType.三带一;
                            isRule = true;
                        }
                        else
                        {
                            isRule = false;
                        }
                    }
                    break;
                case 5:
                    if (IsStraight(leadPokers))
                    {
                        leadPokers.type = PokerGroupType.五张顺子;
                        isRule = true;
                    }
                    else
                    {
                        isRule = false;
                    }
                    break;
                case 6:
                    if (IsStraight(leadPokers))
                    {
                        leadPokers.type = PokerGroupType.六张顺子;
                        isRule = true;
                    }
                    else
                    {
                        if (IsLinkPair(leadPokers))
                        {
                            leadPokers.type = PokerGroupType.三连对;
                            isRule = true;
                        }
                        else
                        {
                            if (IsSame(leadPokers, 4))
                            {
                                leadPokers.type = PokerGroupType.四带二;
                                isRule = true;
                            }
                            else
                            {
                                if (IsThreeLinkPokers(leadPokers))
                                {
                                    leadPokers.type = PokerGroupType.二连飞机;
                                    isRule = true;
                                }
                                else
                                {
                                    isRule = false;
                                }
                            }
                        }
                    }
                    break;
                case 7:
                    if (IsStraight(leadPokers))
                    {
                        leadPokers.type = PokerGroupType.七张顺子;
                        isRule = true;
                    }
                    else
                    {
                        isRule = false;
                    }
                    break;
                case 8:
                    if (IsStraight(leadPokers))
                    {
                        leadPokers.type = PokerGroupType.八张顺子;
                        isRule = true;
                    }
                    else
                    {
                        if (IsLinkPair(leadPokers))
                        {
                            leadPokers.type = PokerGroupType.四连对;
                            isRule = true;
                        }
                        else
                        {
                            if (IsThreeLinkPokers(leadPokers))
                            {
                                leadPokers.type = PokerGroupType.飞机带翅膀;
                                isRule = true;
                            }
                            else
                            {
                                isRule = false;
                            }
                        }
                    }
                    break;
                case 9:
                    if (IsStraight(leadPokers))
                    {
                        leadPokers.type = PokerGroupType.九张顺子;
                        isRule = true;
                    }
                    else
                    {
                        if (IsThreeLinkPokers(leadPokers))
                        {
                            leadPokers.type = PokerGroupType.三连飞机;
                        }
                        else
                        {
                            isRule = false;
                        }
                    }
                    break;
                case 10:
                    if (IsStraight(leadPokers))
                    {
                        leadPokers.type = PokerGroupType.十张顺子;
                        isRule = true;
                    }
                    else
                    {
                        if (IsLinkPair(leadPokers))
                        {
                            leadPokers.type = PokerGroupType.五连对;
                            isRule = true;
                        }
                        else
                        {
                            isRule = false;
                        }
                    }
                    break;
                case 11:
                    if (IsStraight(leadPokers))
                    {
                        leadPokers.type = PokerGroupType.十一张顺子;
                        isRule = true;
                    }
                    else
                    {
                        isRule = false;
                    }
                    break;
                case 12:
                    if (IsStraight(leadPokers))
                    {
                        leadPokers.type = PokerGroupType.十二张顺子;
                        isRule = true;
                    }
                    else
                    {
                        if (IsLinkPair(leadPokers))
                        {
                            leadPokers.type = PokerGroupType.六连对;
                            isRule = true;
                        }
                        else
                        {
                            if (IsThreeLinkPokers(leadPokers))
                            {
                                //12有三连飞机带翅膀和四连飞机两种情况,所以在IsThreeLinkPokers中做了特殊处理,此处不用给type赋值.
                                isRule = true;
                            }
                            else
                            {
                                isRule = false;
                            }
                        }
                    }
                    break;
                case 13:
                    isRule = false;
                    break;
                case 14:
                    if (IsLinkPair(leadPokers))
                    {
                        leadPokers.type = PokerGroupType.七连对;
                        isRule = true;
                    }
                    else
                    {
                        isRule = false;
                    }
                    break;
                case 15:
                    if (IsThreeLinkPokers(leadPokers))
                    {
                        leadPokers.type = PokerGroupType.五连飞机;
                        isRule = true;
                    }
                    else
                    {
                        isRule = false;
                    }
                    break;
                case 16:
                    if (IsLinkPair(leadPokers))
                    {
                        leadPokers.type = PokerGroupType.八连对;
                        isRule = true;
                    }
                    else
                    {
                        if (IsThreeLinkPokers(leadPokers))
                        {
                            leadPokers.type = PokerGroupType.四连飞机带翅膀;
                            isRule = true;
                        }
                        else
                        {
                            isRule = false;
                        }
                    }
                    break;
                case 17:
                    isRule = false;
                    break;
                case 18:
                    if (IsLinkPair(leadPokers))
                    {
                        leadPokers.type = PokerGroupType.六连对;
                        isRule = true;
                    }
                    else
                    {
                        if (IsThreeLinkPokers(leadPokers))
                        {
                            leadPokers.type = PokerGroupType.六连飞机;
                            isRule = true;
                        }
                        else
                        {
                            isRule = false;
                        }
                    }
                    break;
                case 19:
                    isRule = false;
                    break;
                case 20:
                    if (IsLinkPair(leadPokers))
                    {
                        leadPokers.type = PokerGroupType.十连对;
                        isRule = true;
                    }
                    else
                    {
                        if (IsThreeLinkPokers(leadPokers))
                        {
                            leadPokers.type = PokerGroupType.五连飞机带翅膀;
                            isRule = true;
                        }
                        else
                        {
                            isRule = false;
                        }
                    }
                    break;
            }
            return isRule;
        }
        /// <summary>
        /// 判断一个牌组指定数量相邻的牌是否两两相同
        /// </summary>
        /// <param name="PG">牌组对象</param>
        /// <param name="amount">指定数量的相邻牌组</param>
        /// <returns>指定数量的相邻牌是否两两相同</returns>
        public static bool IsSame(PokerGroup PG, int amount)
        {
            bool IsSame1 = false;
            bool IsSame2 = false;
            for (int i = 0; i < amount - 1; i++) //从大到小比较相邻牌是否相同
            {
                if (PG.Cards[i].Num == PG.Cards[i + 1].Num)
                {
                    IsSame1 = true;
                }
                else
                {
                    IsSame1 = false;
                    break;
                }
            }
            for (int i = PG.Cards.Count - 1; i > PG.Cards.Count - amount; i--)  //从小到大比较相邻牌是否相同
            {
                if (PG.Cards[i].Num == PG.Cards[i - 1].Num)
                {
                    IsSame2 = true;
                }
                else
                {
                    IsSame2 = false;
                    break;
                }
            }
            if (IsSame1 || IsSame2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 判断牌组是否为顺子
        /// </summary>
        /// <param name="PG">牌组</param>
        /// <returns>是否为顺子</returns>
        public static bool IsStraight(PokerGroup PG)
        {
            bool IsStraight = false;
            foreach (Card poker in PG.Cards)//不能包含2、小王、大王
            {
                if (poker.Num == 2 || poker.Num == 53 || poker.Num == 54)
                {
                    IsStraight = false;
                    return IsStraight;
                }
            }
            for (int i = 0; i < PG.Cards.Count - 1; i++)
            {
                if (PG.Cards[i].Num - 1 == PG.Cards[i + 1].Num)
                {
                    IsStraight = true;
                }
                else
                {
                    IsStraight = false;
                    break;
                }
            }
            return IsStraight;
        }
        /// <summary>
        /// 判断牌组是否为连对
        /// </summary>
        /// <param name="PG">牌组</param>
        /// <returns>是否为连对</returns>
        public static bool IsLinkPair(PokerGroup PG)
        {
            bool IsLinkPair = false;
            foreach (Card poker in PG.Cards) //不能包含2、小王、大王
            {
                if (poker.Num == 2 || poker.Num == 53 || poker.Num == 54)
                {
                    IsLinkPair = false;
                    return IsLinkPair;
                }
            }
            for (int i = 0; i < PG.Cards.Count - 2; i += 2)  //首先比较是否都为对子，再比较第一个对子的点数-1是否等于第二个对子，最后检察最小的两个是否为对子（这里的for循环无法检测到最小的两个，所以需要拿出来单独检查）
            {
                if (PG.Cards[i].Num == PG.Cards[i + 1].Num && PG.Cards[i].Num - 1 == PG.Cards[i + 2].Num && PG.Cards[i + 2].Num == PG.Cards[i + 3].Num)
                {
                    IsLinkPair = true;
                }
                else
                {
                    IsLinkPair = false;
                    break;
                }
            }
            return IsLinkPair;
        }
        /// <summary>
        /// 判断牌组是否为连续三张牌,飞机,飞机带翅膀
        /// </summary>
        /// <param name="PG">牌组</param>
        /// <returns>是否为连续三张牌</returns>
        public static bool IsThreeLinkPokers(PokerGroup PG) //判断三张牌方法为判断两两相邻的牌,如果两两相邻的牌相同,则count自加1.最后根据count的值判断牌的类型为多少个连续三张
        {
            bool IsThreeLinkPokers = false;
            int HowMuchLinkThree = 0;  //飞机的数量
            PG = SameThreeSort(PG); //排序,把飞机放在前面
            for (int i = 2; i < PG.Cards.Count; i++)  //得到牌组中有几个飞机
            {
                if (PG.Cards[i].Num == PG.Cards[i - 1].Num && PG.Cards[i].Num == PG.Cards[i - 2].Num)
                {
                    HowMuchLinkThree++;
                }
            }
            if (HowMuchLinkThree > 0)  //当牌组里面有三个时
            {
                if (HowMuchLinkThree > 1)  //当牌组为飞机时
                {
                    for (int i = 0; i < HowMuchLinkThree * 3 - 3; i += 3) //判断飞机之间的点数是否相差1
                    {
                        if (PG.Cards[i].Num != 2 && PG.Cards[i].Num - 1 == PG.Cards[i + 3].Num) //2点不能当飞机出
                        {
                            IsThreeLinkPokers = true;
                        }
                        else
                        {
                            IsThreeLinkPokers = false;
                            break;
                        }
                    }
                }
                else
                {
                    IsThreeLinkPokers = true; //牌组为普通三个,直接返回true
                }
            }
            else
            {
                IsThreeLinkPokers = false;
            }
            if (HowMuchLinkThree == 4)
            {
                PG.type = PokerGroupType.四连飞机;
            }
            if (HowMuchLinkThree == 3 && PG.Cards.Count == 12)
            {
                PG.type = PokerGroupType.三连飞机带翅膀;
            }
            return IsThreeLinkPokers;

        }
        /// <summary>
        /// 对飞机和飞机带翅膀进行排序,把飞机放在前面,翅膀放在后面.
        /// </summary>
        /// <param name="PG">牌组</param>
        /// <returns>是否为连续三张牌</returns>
        public static PokerGroup SameThreeSort(PokerGroup PG)
        {
            Card FourPoker = new Card();  //如果把4张当三张出并且带4张的另外一张,就需要特殊处理,这里记录出现这种情况的牌的点数.
            bool FindedThree = false;  //已找到三张相同的牌
            PokerGroup tempPokerGroup = new PokerGroup();  //记录三张相同的牌
            int count = 0; //记录在连续三张牌前面的翅膀的张数
            int Four = 0; // 记录是否连续出现三三相同,如果出现这种情况则表明出现把4张牌(炸弹)当中的三张和其他牌配成飞机带翅膀,并且翅膀中有炸弹牌的点数.
            // 比如有如下牌组: 998887777666 玩家要出的牌实际上应该为 888777666带997,但是经过从大到小的排序后变成了998887777666 一不美观,二不容易比较.
            for (int i = 2; i < PG.Cards.Count; i++)  //直接从2开始循环,因为PG[0],PG[1]的引用已经存储在其他变量中,直接比较即可
            {
                if (PG.Cards[i].Num == PG.Cards[i - 2].Num && PG.Cards[i].Num == PG.Cards[i - 1].Num)// 比较PG[i]与PG[i-1],PG[i]与PG[i-2]是否同时相等,如果相等则说明这是三张相同牌
                {
                    if (Four >= 1) //默认的Four为0,所以第一次运行时这里为false,直接执行else
                                   //一旦连续出现两个三三相等,就会执行这里的if
                    {
                        FourPoker = PG.Cards[i]; //当找到四张牌时,记录下4张牌的点数
                        Card changePoker;
                        for (int k = i; k > 0; k--) //把四张牌中的一张移动到最前面.
                        {
                            changePoker = PG.Cards[k];
                            PG.Cards[k] = PG.Cards[k - 1];
                            PG.Cards[k - 1] = changePoker;
                        }
                        count++; //由于此时已经找到三张牌,下面为count赋值的程序不会执行,所以这里要手动+1
                    }
                    else
                    {
                        Four++; //记录本次循环,因为本次循环找到了三三相等的牌,如果连续两次找到三三相等的牌则说明找到四张牌(炸弹)
                        tempPokerGroup.Cards.Add(PG.Cards[i]); //把本次循环的PG[i]记录下来,即记录下三张牌的点数
                    }
                    FindedThree = true; //标记已找到三张牌
                }
                else
                {
                    Four = 0; //没有找到时,连续找到三张牌的标志Four归零
                    if (!FindedThree) //只有没有找到三张牌时才让count增加.如果已经找到三张牌,则不再为count赋值.
                    {
                        count = i - 1;
                    }
                }
            }
            foreach (Card tempPoker in tempPokerGroup.Cards)  //迭代所有的三张牌点数
            {
                Card changePoker;  //临时交换Poker
                for (int i = 0; i < PG.Cards.Count; i++)  //把所有的三张牌往前移动
                {
                    if (PG.Cards[i].Num == tempPoker.Num)  //当PG[i]等于三张牌的点数时
                    {
                        if (PG.Cards[i].Num == FourPoker.Num) //由于上面已经把4张牌中的一张放到的最前面,这张牌也会与tempPoker相匹配所以这里进行处理
                                                // 当第一次遇到四张牌的点数时,把记录四张牌的FourPoker赋值为null,并中断本次循环.由于FourPoker已经为Null,所以下次再次遇到四张牌的点数时会按照正常情况执行.
                        {
                            FourPoker = null;
                            continue;
                        }
                        changePoker = PG.Cards[i - count];
                        PG.Cards[i - count] = PG.Cards[i];
                        PG.Cards[i] = changePoker;
                    }
                }
            }
            return PG;
        }

    }

    public class Player
    {
        private string Name
        { set; get; }

        public bool isLord;
        public List<Card> Cards;

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

    public class Card : IComparable<Card>
    {
        public Color Color;
        public int Num;
        //public int Index;
        public Card(Color Color, int Num)
        {
            this.Color = Color;
            this.Num = Num;
        }
        
        public Card()
        { }

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

        public static bool operator ==(Card A, Card B)
        {
            return A.Num == B.Num;

        }

        public static bool operator !=(Card A, Card B)
        {
            return A.Num != B.Num;

        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return this == (Card)obj;
        }

        public override int GetHashCode()
        {
            return Num.GetHashCode()^Color.GetHashCode();
        }

        public static bool operator <(Card A, Card B)
        {
            return A.Num < B.Num;
        }

        public static bool operator >(Card A, Card B)
        {
            return A.Num > B.Num;
        }

    }

    public class PokerGroup
    {
        public List<Card> Cards = new List<Card>();
        public PokerGroupType type;
        public void Sort()
        {
            Cards.Sort();
        }
        public PokerGroup()
        {

        }
        public static bool operator >(PokerGroup LP, PokerGroup RP)
        {
            bool IsGreater = false;
            if (LP.type != RP.type && LP.type != PokerGroupType.炸弹 && LP.type != PokerGroupType.双王)
            {
                IsGreater = false;
            }
            else
            {
                if (LP.type == PokerGroupType.炸弹 && RP.type == PokerGroupType.炸弹) //LPRP都为炸弹
                {
                    if (LP.Cards[0] > RP.Cards[0]) //比较大小
                    {
                        IsGreater = true;
                    }
                    else
                    {
                        IsGreater = false;
                    }
                }
                else
                {
                    if (LP.type == PokerGroupType.炸弹) //只有LP为炸弹
                    {
                        IsGreater = true;
                    }
                    else
                    {
                        if (LP.type == PokerGroupType.双王) //LP为双王
                        {
                            IsGreater = true;
                        }
                        else
                        {
                            if (LP.Cards[0] > RP.Cards[0]) //LP为普通牌组
                            {
                                IsGreater = true;
                            }
                            else
                            {
                                IsGreater = false;
                            }
                        }
                    }
                }
            }
            return IsGreater;
        }

        public static bool operator <(PokerGroup LP, PokerGroup RP)
        {
            bool IsGreater = false;
            if (LP.type != RP.type && LP.type != PokerGroupType.炸弹 && LP.type != PokerGroupType.双王)
            {
                IsGreater = false;
            }
            else
            {
                if (LP.type == PokerGroupType.炸弹 && RP.type == PokerGroupType.炸弹) //LPRP都为炸弹
                {
                    if (LP.Cards[0] > RP.Cards[0]) //比较大小
                    {
                        IsGreater = true;
                    }
                    else
                    {
                        IsGreater = false;
                    }
                }
                else
                {
                    if (LP.type == PokerGroupType.炸弹) //只有LP为炸弹
                    {
                        IsGreater = true;
                    }
                    else
                    {
                        if (LP.type == PokerGroupType.双王) //LP为双王
                        {
                            IsGreater = true;
                        }
                        else
                        {
                            if (LP.Cards[0] > RP.Cards[0]) //LP为普通牌组
                            {
                                IsGreater = true;
                            }
                            else
                            {
                                IsGreater = false;
                            }
                        }
                    }
                }
            }
            return !IsGreater;
        }

    }

    public enum PokerGroupType
    {
        单张 = 1,
        对子 = 2,
        双王 = 3,
        三张相同 = 4,
        三带一 = 5,
        炸弹 = 6,
        五张顺子 = 7,
        六张顺子 = 8,
        三连对 = 9,
        四带二 = 10,
        二连飞机 = 11,
        七张顺子 = 12,
        四连对 = 13,
        八张顺子 = 14,
        飞机带翅膀 = 15,
        九张顺子 = 16,
        三连飞机 = 17,
        五连对 = 18,
        十张顺子 = 19,
        十一张顺子 = 20,
        十二张顺子 = 21,
        四连飞机 = 22,
        三连飞机带翅膀 = 23,
        六连对 = 24,
        //没有13
        七连对 = 25,
        五连飞机 = 26,
        八连对 = 27,
        四连飞机带翅膀 = 28,
        //没有17
        九连对 = 29,
        六连飞机 = 30,
        //没有19
        十连对 = 31,
        五连飞机带翅膀 = 32
    }


}


