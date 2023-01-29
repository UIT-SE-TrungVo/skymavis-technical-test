using System;
using Assignment.Battle.BattleItem.Enum;

namespace Assignment.Battle.BattleItem
{
    public abstract class BattleItem
    {
        #region FIELDS

        private BattleItemName itemName;
        private BattleAxie owner;
        private BattleItemStats basicStats;

        #endregion

        #region PROPERTIES

        public BattleAxie Owner
        {
            get => owner;
            private set => owner = value;
        }

        public BattleItemStats BasicStats
        {
            get => basicStats;
            private set => basicStats = value;
        }

        public BattleItemName ItemName
        {
            get => itemName;
            protected set => itemName = value;
        }

        #endregion

        #region METHODS

        public BattleItem(BattleAxie owner, BattleItemStats itemInfo)
        {
            this.owner = owner;
            this.BasicStats = itemInfo;
        }

        public abstract void OnEquip();

        public abstract void OnAttack(DamageInfo damageInfo);

        public abstract void OnAttacked(DamageInfo damageInfo);

        public abstract void OnSuccessfulAttack(DamageInfo damageInfo, float totalDamage);

        public abstract void OnDodgedAttack();

        #endregion
    }
}