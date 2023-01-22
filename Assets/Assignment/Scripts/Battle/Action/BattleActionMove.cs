using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Assignment.Battle.Action
{
    public class BattleActionMove : BattleAction
    {
        #region FIELDS

        private Vector2Int destination;

        #endregion

        #region PROPETIES

        public Vector2Int Destination => destination;

        #endregion

        #region METHODS

        public BattleActionMove(Vector2Int destination)
        {
            this.destination = destination;
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