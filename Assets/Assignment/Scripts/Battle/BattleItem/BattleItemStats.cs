using System;
using Assignment.Battle.BattleItem.Enum;

namespace Assignment.Battle.BattleItem
{
    [Serializable]
    public struct BattleItemStats
    {
        public BattleItemClass itemClass;
        public float damage;
        public float health;
        public float critRate;
        public float dodgeChance;
    }
}