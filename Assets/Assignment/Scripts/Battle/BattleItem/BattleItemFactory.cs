using System;
using System.Collections.Generic;
using Assignment.Battle.BattleItem.Detail;
using Assignment.Battle.BattleItem.Enum;

namespace Assignment.Battle.BattleItem
{
    public class BattleItemFactory
    {
        public static List<BattleItemName> RandomItemName = new List<BattleItemName>()
        {
            BattleItemName.A,
            BattleItemName.B,
            BattleItemName.C,
            BattleItemName.D
        };

        public static BattleItem GetRandomItemName(BattleAxie owner)
        {
            BattleItemName random = RandomHelper.GetRandomElementFromList(RandomItemName);
            return Get(owner, random);
        }

        public static BattleItem Get(BattleAxie owner, BattleItemName name)
        {
            switch (name)
            {
                case BattleItemName.A:
                    return new PrimaryBattleItem(owner, BattleItemInfo.Config.itemA.stats, name);
                case BattleItemName.B:
                    return new PrimaryBattleItem(owner, BattleItemInfo.Config.itemB.stats, name);
                case BattleItemName.C:
                    return new PrimaryBattleItem(owner, BattleItemInfo.Config.itemC.stats, name);
                case BattleItemName.D:
                    return new PrimaryBattleItem(owner, BattleItemInfo.Config.itemD.stats, name);
                case BattleItemName.AA:
                    break;
                case BattleItemName.AB:
                    break;
                case BattleItemName.AC:
                    break;
                case BattleItemName.AD:
                    break;
                case BattleItemName.BB:
                    break;
                case BattleItemName.BC:
                    break;
                case BattleItemName.BD:
                    break;
                case BattleItemName.CC:
                    break;
                case BattleItemName.CD:
                    break;
                case BattleItemName.DD:
                    break;
            }

            return null;
        }
    }
}