using System.Collections.Generic;
using UnityEngine;

namespace Assignment.Battle.UI
{
    public class BattleEffect : MonoBehaviour
    {
        #region FIELDS

        private static BattleEffect instance;

        [SerializeField] private List<BattleSlashEffect> listSlashEffect;
        private int nextSlashEffectIdx = 0;

        #endregion

        #region PROPERTIES

        public static BattleEffect Instance
        {
            get => instance;
            private set => instance = value;
        }

        #endregion

        #region UNITY EVENTS

        private void Awake()
        {
            Instance = this;
        }

        #endregion

        #region METHODS

        public static void DisplayDamagedEffect(GameObject target, string damage)
        {
            if (Instance == null || Camera.current == null || target == null) return;
            Instance.PrivateDisplayDamagedEffect(target, damage);
        }

        private void PrivateDisplayDamagedEffect(GameObject target, string damage)
        {
            if (Camera.main == null) return;

            BattleSlashEffect eff = this.listSlashEffect[this.nextSlashEffectIdx];
            eff.AppearEffect(
                target.transform.position,
                damage,
                0.3f,
                5.0f,
                0.3f
            );
            
            this.nextSlashEffectIdx = (this.nextSlashEffectIdx + 1) % this.listSlashEffect.Count;
        }

        #endregion
    }
}