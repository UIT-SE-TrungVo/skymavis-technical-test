using Assignment.Battle.BattleItem.Enum;

namespace Assignment.Battle.BattleItem.Detail
{
    public class PrimaryBattleItem : BattleItem
    {
        public PrimaryBattleItem(BattleAxie owner, BattleItemStats itemInfo, BattleItemName name) : base(owner, itemInfo)
        {
            this.ItemName = name;
        }

        public override void OnEquip()
        {
        }

        public override void OnAttack(DamageInfo damageInfo)
        {
        }

        public override void OnAttacked(DamageInfo damageInfo)
        {
        }

        public override void OnSuccessfulAttack(DamageInfo damageInfo, float totalDamage)
        {
        }

        public override void OnDodgedAttack()
        {
        }
    }
}