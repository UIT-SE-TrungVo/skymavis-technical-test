using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assignment.Battle.UI
{
    public class BattleSlashEffect : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private RectTransform nodeRoot;
        [SerializeField] private Text txtDamage;
        [SerializeField] private Image imgSlash;
        [SerializeField] private Vector2 maxJumpOffset;
        [SerializeField] private float defaultCamSize;

        private Camera gameCamera;
        private bool isCameraNull;
        private Vector3 targetPos;
        private int initialFontSize;

        private float stayTime;
        private float disappearTime;

        #endregion

        #region PROPERTIES

        #endregion

        #region UNITY EVENTS

        private void Awake()
        {
            this.gameCamera = Camera.main;
            this.isCameraNull = this.gameCamera == null;
            this.nodeRoot.localScale = Vector3.one;
            this.initialFontSize = this.txtDamage.fontSize;
        }

        private void Update()
        {
            if (this.isCameraNull || this.targetPos == null) return;
            Vector3 posOnScreen = Camera.current.WorldToScreenPoint(targetPos);
            this.nodeRoot.position = posOnScreen + this.GetOffsetByCamera();
            this.nodeRoot.localScale = Vector3.one * this.GetScaleAdjustedByCamera();

            this.stayTime -= Time.deltaTime;
            if (this.stayTime <= 0)
            {
                this.DisappearEffect(this.disappearTime);
            }
        }

        #endregion

        #region METHODS

        public void AppearEffect(Vector3 posTarget, string damage, float appearTime, float stayTime,
            float disappearTime)
        {
            this.targetPos = posTarget;
            this.txtDamage.text = damage;
            this.stayTime = stayTime;
            this.disappearTime = disappearTime;

            this.nodeRoot.localScale = Vector3.one;
            this.nodeRoot.localPosition = Vector3.zero;
            this.nodeRoot.DOScale(Vector3.one, appearTime)
                .SetEase(Ease.OutBack);

            Vector3 targetJump = this.GetRandomJumpDestination();
            float jumpHeight = targetJump.sqrMagnitude;
            this.txtDamage.GetComponent<RectTransform>().localPosition = Vector3.zero;
            this.txtDamage.GetComponent<RectTransform>().DOLocalJump(targetJump, jumpHeight, 1, appearTime);

            bool isDamageNumber = int.TryParse(damage, out var _);
            Color damageColor = isDamageNumber ? Color.red : Color.black;
            FontStyle damageStyle = isDamageNumber ? FontStyle.Bold : FontStyle.BoldAndItalic;
            int fontSize = isDamageNumber ? this.initialFontSize : this.initialFontSize / 2;
            this.txtDamage.color = damageColor;
            this.txtDamage.fontStyle = damageStyle;
            this.txtDamage.fontSize = fontSize;
        }

        public void DisappearEffect(float disappearTime)
        {
            this.nodeRoot.DOScale(Vector3.zero, disappearTime);
        }

        private Vector3 GetRandomJumpDestination()
        {
            float x = RandomHelper.GetRandomFloat(-this.maxJumpOffset.x, this.maxJumpOffset.x);
            float y = RandomHelper.GetRandomFloat(-this.maxJumpOffset.y, this.maxJumpOffset.y);
            return new Vector3(x, y, 0);
        }

        private float GetScaleAdjustedByCamera()
        {
            float camSize = this.gameCamera.orthographicSize;
            float addPerSize = 0.25f;
            return 1.0f + addPerSize * (this.defaultCamSize - camSize);
        }

        private Vector3 GetOffsetByCamera()
        {
            float camSize = this.gameCamera.orthographicSize;
            float addPerSize = 2.0f;
            float heightAtDefaultCamSize = 20.0f;
            return Vector3.up * (heightAtDefaultCamSize + addPerSize * (this.defaultCamSize - camSize));
        }

        #endregion
    }
}