namespace Assignment.Battle.Model
{
    public class BattleFieldTurnMgr
    {
        #region FIELDS

        private float secondAdded;
        private float secondPerTurn;

        #endregion

        #region PROPERTIES

        public float SecondPerTurn
        {
            get => secondPerTurn;
            set => secondPerTurn = value;
        }

        #endregion

        #region METHODS

        public BattleFieldTurnMgr(float secondPerTurn)
        {
            this.secondPerTurn = secondPerTurn;
            this.secondAdded = 0;
        }

        public bool GetUpdate(float deltaTime)
        {
            secondAdded += deltaTime;
            if (secondAdded >= secondPerTurn)
            {
                secondAdded -= secondPerTurn;
                return true;
            }

            return false;
        }

        #endregion
    }
}