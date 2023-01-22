using Assignment.ScriptableObjects;

namespace Assignment.Battle.Model
{
    public class BattleAxieInfo
    {
        #region FIELDS

        private readonly float initHealth;
        private readonly float moveSpeed;

        private float currentHealth;

        #endregion

        #region PROPERTIES

        public float InitHealth => initHealth;

        public float MoveSpeed => moveSpeed;

        public float CurrentHealth
        {
            get => currentHealth;
            set => currentHealth = value;
        }

        #endregion

        #region METHODS

        public BattleAxieInfo(AxieConfigInfo axieInfo)
        {
            this.initHealth = axieInfo.initHealth;
            this.moveSpeed = axieInfo.moveSpeed;
        }

        #endregion
    }
}