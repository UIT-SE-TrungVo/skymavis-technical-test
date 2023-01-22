namespace Assignment.Battle.Action
{
    public abstract class BattleAction
    {
        #region FIELDS

        private BattleField battleField;
        private BattleAxie battleAxie;
        private BattleActionType battleActionType;

        #endregion

        #region PROPETIES

        public BattleField BattleField => battleField;
        public BattleAxie BattleAxie => battleAxie;
        public BattleActionType BattleActionType => battleActionType;

        #endregion

        #region METHODS

        public abstract bool CanPerform();

        public abstract void PerformSuccess();

        public abstract void PerformFailed();

        #endregion
    }
}