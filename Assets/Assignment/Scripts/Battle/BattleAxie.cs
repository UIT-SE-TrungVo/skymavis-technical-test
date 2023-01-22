using Assignment.Battle.Model;
using Assignment.ScriptableObjects;
using UnityEngine;

namespace Assignment.Battle
{
    public class BattleAxie : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private AxieConfig config;
        [SerializeField] private BattleAxieType axieType;
        
        private BattleAxieInfo stats;
        private float currentHealth;
        private BattleAxieView axieView;

        #endregion

        #region PROPERTIES

        public BattleAxieType AxieType => axieType;

        public BattleAxieInfo Stats => stats;

        public float CurrentHealth  
        {
            get => currentHealth;
            private set => currentHealth = value;
        }

        #endregion

        #region UNITY EVENTS

        private void Awake()
        {
            this.axieView = this.GetComponentInChildren<BattleAxieView>();
            
            this.ApplyConfig();
        }

        #endregion

        #region METHODS

        public bool CanMove()
        {
            return this.stats.MoveSpeed > 0;
        }

        public bool IsDead()
        {
            return this.stats.CurrentHealth <= 0;
        }

        private void ApplyConfig()
        {
            AxieConfigInfo info;
            switch (this.AxieType)
            {
                case BattleAxieType.Attacker:
                    info = config.attackerInfo;
                    break;
                case BattleAxieType.Defender:
                    info = config.defenderInfo;
                    break;
                default:
                    return;
            }

            this.stats = new BattleAxieInfo(info);
            if (this.axieView != null)
            {
                this.axieView.ApplyViewConfig(info);
            }
        }
        #endregion
    }
}