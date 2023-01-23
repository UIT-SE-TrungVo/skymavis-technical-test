using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assignment.Battle.Model
{
    public class BattleFieldActionMgr
    {
        #region FIELDS

        private BattleFieldPositionMgr positionMgr;

        private Dictionary<BattleAxieSide, Queue<BattleAxie>> side2AxieQueue =
            new Dictionary<BattleAxieSide, Queue<BattleAxie>>();

        #endregion

        #region PROPERTIES

        #endregion

        #region METHODS

        public BattleFieldActionMgr(BattleFieldPositionMgr positionMgr)
        {
            this.positionMgr = positionMgr;
            this.UpdateMapActionAxie();
        }

        public List<System.Action> GetListNextActions()
        {
            List<System.Action> actions = new List<System.Action>();
            foreach (BattleAxieSide axieSide in side2AxieQueue.Keys)
            {
                System.Action sideAction = this.GetNextActionFromQueue(this.side2AxieQueue[axieSide]);
                if (sideAction != null) actions.Add(sideAction);
            }

            return actions;
        }

        private System.Action GetNextActionFromQueue(Queue<BattleAxie> queueAxie)
        {
            System.Action nextAction = null;
            int numLoop = queueAxie.Count;
            for (int i = 0; i < numLoop; i++)
            {
                BattleAxie peakAxie = queueAxie.Dequeue();
                if (peakAxie.IsDead()) continue;

                System.Action axieAction = peakAxie.GetNextAction();

                queueAxie.Enqueue(peakAxie);
                if (axieAction != null)
                {
                    nextAction = axieAction;
                    break;
                }
            }

            return nextAction;
        }

        public void UpdateMapActionAxie()
        {
            this.side2AxieQueue.Clear();
            foreach (BattleAxie axie in this.positionMgr.Coord2Axie.Values)
            {
                BattleAxieSide side = axie.AxieSide;
                if (!this.side2AxieQueue.ContainsKey(side)) this.side2AxieQueue.Add(side, new Queue<BattleAxie>());
                this.side2AxieQueue[side].Enqueue(axie);
            }
        }

        #endregion
    }
}