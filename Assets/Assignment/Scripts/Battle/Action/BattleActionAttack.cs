using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Assignment.Battle.Action
{
    public class BattleActionAttack : BattleAction
    {
        #region FIELDS

        private BattleAxie target;

        #endregion

        #region PROPETIES

        public BattleAxie Target => target;

        #endregion

        #region METHODS

        public BattleActionAttack(BattleAxie target)
        {
            this.target = target;
        }

        public override bool CanPerform()
        {
            throw new System.NotImplementedException();
        }

        public override void PerformSuccess()
        {
            throw new System.NotImplementedException();
        }

        public override void PerformFailed()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}