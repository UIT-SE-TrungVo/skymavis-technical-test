using System;
using Assignment.Battle.BattleItem;
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
        [SerializeField] private RectTransform nodeRoot;
        [SerializeField] private Slider sliderHealth;
        [SerializeField] private Image imageItem;
        [SerializeField] private float sliderLerpSpeed;
        [SerializeField] private float healthBarOpacity;

        private float defaultCamSize;
        private bool canUpdate;
        private RectTransform rectSliderHealth;
        private Image imgSliderHealthFill;
        private Camera curCamera;

        #endregion

        #region PROPERTIES

        #endregion

        #region UNITY EVENTS

        private void Start()
        {
            this.curCamera = Camera.main;
            this.owner = this.GetComponentInParent<BattleAxie>();
            this.canUpdate = this.owner != null && this.sliderHealth != null;

            this.rectSliderHealth = this.sliderHealth.GetComponent<RectTransform>();
            this.imgSliderHealthFill = this.sliderHealth.fillRect.GetComponent<Image>();
            if (this.curCamera != null)
            {
                this.defaultCamSize = this.curCamera.orthographicSize;
            }
        }

        private void Update()
        {
            if (!this.canUpdate || this.owner == null) return;
            this.UpdateHealthBar();
            this.KeepOnOwnerHead();
            this.UpdateItem();
        }

        #endregion

        #region METHODS

        private void UpdateHealthBar()
        {
            BattleAxieInfo info = this.owner.Stats;
            float currentHealth = this.owner.CurrentHealth;
            float maxHealth = Math.Max(currentHealth, info.InitHealth);

            float targetRatio = currentHealth / maxHealth;
            float limitLerp = this.sliderLerpSpeed * Time.unscaledDeltaTime;
            this.sliderHealth.value = Mathf.Lerp(sliderHealth.value, targetRatio, limitLerp);

            Color healthColor = this.GetHealthColor(1.0f, this.sliderHealth.value);
            this.imgSliderHealthFill.color = healthColor;
        }

        private void KeepOnOwnerHead()
        {
            Vector3 axiePosition = owner.transform.position;
            Vector3 position = this.curCamera.WorldToScreenPoint(axiePosition);
            this.nodeRoot.position = position + this.GetAddPositionAdjustedByCamera();
            this.nodeRoot.localScale = Vector3.one * this.GetScaleAdjustedByCamera();
        }

        private Vector3 GetAddPositionAdjustedByCamera()
        {
            float camSize = this.curCamera.orthographicSize;
            float addPerSize = 5.0f;
            float heightAtDefaultCamSize = 40.0f;
            return Vector3.up * (heightAtDefaultCamSize + addPerSize * (this.defaultCamSize - camSize));
        }

        private float GetScaleAdjustedByCamera()
        {
            float camSize = this.curCamera.orthographicSize;
            float addPerSize = 0.25f;
            return 1.0f + addPerSize * (this.defaultCamSize - camSize);
        }

        private Color GetHealthColor(float maxValue, float currentValue)
        {
            const float maxHue = 0.27f;
            const float minHue = 0.0f;

            float hue = minHue + (maxHue - minHue) * (currentValue / maxValue);
            Color color = Color.HSVToRGB(hue, 1.0f, 0.8f);
            color.a = this.healthBarOpacity;
            return color;
        }

        private void UpdateItem()
        {
            if (this.owner.Item == null)
            {
                this.imageItem.transform.localScale = Vector3.zero;
                return;
            }

            this.imageItem.transform.localScale = Vector3.one;

            Sprite spriteItem = BattleItemInfo.GetSpriteByItemName(this.owner.Item.ItemName);
            this.imageItem.sprite = spriteItem;
        }

        #endregion
    }
}