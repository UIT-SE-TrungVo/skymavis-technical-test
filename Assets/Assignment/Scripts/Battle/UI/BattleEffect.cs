using System.Collections.Generic;
using UnityEngine;

namespace Assignment.Battle.UI
{
    public class BattleEffect : MonoBehaviour
    {
        #region FIELDS

        private static BattleEffect instance;

        [SerializeField] private GameObject prefabSlashEffect;
        private Queue<BattleSlashEffect> queueIdleSlashEffect = new Queue<BattleSlashEffect>();

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

            BattleSlashEffect eff;
            if (this.queueIdleSlashEffect.Count <= 0)
            {
                eff = Instantiate(prefabSlashEffect, this.transform)
                    .GetComponent<BattleSlashEffect>();
            }
            else
            {
                eff = this.queueIdleSlashEffect.Dequeue();
            }

            if (eff == null) return;
            eff.Mgr = this;
            eff.gameObject.SetActive(true);
            eff.AppearEffect(
                target.transform.position,
                damage,
                0.3f,
                5.0f,
                0.3f
            );
        }

        public void RetrieveAttackEffect(BattleSlashEffect effect)
        {
            this.queueIdleSlashEffect.Enqueue(effect);
            effect.gameObject.SetActive(false);
        }

        #endregion
    }
}