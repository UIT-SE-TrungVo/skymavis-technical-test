using System;
using System.Collections.Generic;
using Assignment.Battle.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Assignment.Battle.UI
{
    public class BattleBalanceUI : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private BattleField battleField;
        [SerializeField] private Slider sliderBalance;

        public float attackerTotalHealth;
        public float defenderTotalHealth;

        #endregion

        #region PROPERTIES

        #endregion

        #region UNITY EVENTS

        private void Update()
        {
            List<BattleAxie> listAxies = this.battleField.GetAllAxies();
            float totalHealthAttacker = 0.0f;
            float totalHealthDefender = 0.0f;
            listAxies.ForEach(axie =>
            {
                float health = axie.CurrentHealth;
                BattleAxieSide side = axie.AxieSide;
                if (side == BattleAxieSide.Attacker) totalHealthAttacker += health;
                else if (side == BattleAxieSide.Defender) totalHealthDefender += health;
            });

            float rate = totalHealthAttacker / (totalHealthDefender + totalHealthAttacker);
            this.sliderBalance.value = rate;

            attackerTotalHealth = totalHealthAttacker;
            defenderTotalHealth = totalHealthDefender;
        }

        #endregion

        #region METHODS

        #endregion
    }
}