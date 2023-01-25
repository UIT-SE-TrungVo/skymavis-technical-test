using System;
using Assignment.Battle.Model;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Assignment.Battle.UI
{
    public class BattleAxieOverheadUI : MonoBehaviour
    {
        #region FIELDS

        private BattleAxie owner;
        [SerializeField] private RectTransform rectCenter;
        [SerializeField] private Slider sliderHealth;
        [SerializeField] private float sliderLerpSpeed;
        private bool canUpdate;

        #endregion

        #region PROPERTIES

        #endregion

        #region UNITY EVENTS

        private void Start()
        {
            this.owner = this.GetComponentInParent<BattleAxie>();
            this.canUpdate = this.owner != null && this.sliderHealth != null && this.rectCenter != null;
        }

        private void Update()
        {
            if (!this.canUpdate || this.owner == null) return;
            this.UpdateHealthBar();
            this.KeepOnOwnerHead();
        }

        #endregion

        #region METHODS

        private void UpdateHealthBar()
        {
            BattleAxieInfo info = this.owner.Stats;
            float currentHealth = this.owner.CurrentHealth;
            float maxHealth = Math.Max(currentHealth, info.InitHealth);

            float targetRatio = currentHealth / maxHealth;
            float limitLerp = this.sliderLerpSpeed * Time.deltaTime;
            this.sliderHealth.value = Mathf.Lerp(sliderHealth.value, targetRatio, limitLerp);
        }

        private void KeepOnOwnerHead()
        {
            Vector3 axiePosition = owner.transform.position;
            Vector3 position = Camera.main.WorldToScreenPoint(axiePosition);
            this.rectCenter.transform.position = position;
            this.rectCenter.localScale = owner.transform.localScale;
        }

        #endregion
    }
}