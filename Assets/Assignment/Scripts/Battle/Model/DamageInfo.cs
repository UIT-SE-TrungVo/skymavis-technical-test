namespace Assignment.Battle.BattleItem
{
    public class DamageInfo
    {
        #region FIELDS

        private float initialDamage;
        private BattleAxie attacker;
        private BattleAxie target;

        private float critRate;
        private float percentCritDamage;
        private float dodgeChance;

        private float outputDamage;
        private bool isCrit = false;
        private bool isDodged = false;

        #endregion

        #region PROPERTIES

        public float OutputDamage => outputDamage;

        public float CritRate => critRate;

        public float PercentCritDamage => percentCritDamage;

        public float DodgeChance => dodgeChance;

        public bool IsCrit => isCrit;

        public bool IsDodged => isDodged;

        #endregion

        #region METHODS

        public DamageInfo(float initialDamage, BattleAxie attacker, BattleAxie target)
        {
            this.initialDamage = initialDamage;
            this.attacker = attacker;
            this.target = target;

            this.ApplyAttackerBasicStats();
            this.ApplyDefenderBasicStats();
            this.outputDamage = this.initialDamage;
        }

        private void ApplyAttackerBasicStats()
        {
            BattleItem item = this.attacker.Item;
            if (item == null)
            {
                this.critRate = 0;
                this.percentCritDamage = 0;
                return;
            }

            this.percentCritDamage = BattleItemInfo.Config.criticalDamagePercent;
            this.critRate = item.BasicStats.critRate;
            this.initialDamage += item.BasicStats.damage;
        }

        private void ApplyDefenderBasicStats()
        {
            BattleItem item = this.target.Item;
            if (item == null)
            {
                this.dodgeChance = 0;
                return;
            }

            this.dodgeChance = item.BasicStats.dodgeChance;
        }

        public DamageInfo SetDamage(float damage)
        {
            this.outputDamage = damage;
            return this;
        }

        public DamageInfo SetCritRate(float critRate)
        {
            this.critRate = critRate;
            return this;
        }

        public DamageInfo SetDodgeChance(float dodgeChance)
        {
            this.dodgeChance = dodgeChance;
            return this;
        }

        public DamageInfo SetPercentCritDamage(float percentCritDamage)
        {
            this.percentCritDamage = percentCritDamage;
            return this;
        }

        public DamageInfo ConductDamageFactors()
        {
            this.attacker.Item?.OnAttack(this);
            this.target.Item?.OnAttacked(this);

            this.isDodged = RandomHelper.CanPerformPossibility(this.dodgeChance);
            this.isCrit = !this.isDodged && RandomHelper.CanPerformPossibility(this.critRate);
            return this;
        }

        public float GetFinalDamage()
        {
            if (this.IsDodged) return 0;

            float critDamage = 0;
            if (this.isCrit) critDamage = this.OutputDamage * this.PercentCritDamage / 100;
            return this.outputDamage + critDamage;
        }

        #endregion
    }
}