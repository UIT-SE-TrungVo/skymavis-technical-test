using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assignment.Battle.UI
{
    public class BattleSlashEffect : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private Text txtDamage;
        [SerializeField] private Image imgSlash;
        [SerializeField] private Vector2 maxJumpOffset;

        private Canvas canvas;
        private BattleEffect mgr;
        private Vector3 targetPos;

        #endregion

        #region PROPERTIES

        public BattleEffect Mgr
        {
            get => mgr;
            set => mgr = value;
        }

        #endregion

        #region UNITY EVENTS

        private void Awake()
        {
            this.canvas = this.GetComponentInChildren<Canvas>();
            this.canvas.worldCamera = Camera.current;
        }

        private void Update()
        {
            if (targetPos == null || Camera.current == null) return;
            Vector3 posOnScreen = Camera.current.WorldToScreenPoint(targetPos);
            this.transform.position = posOnScreen;
        }

        #endregion

        #region METHODS

        public void AppearEffect(Vector3 posTarget, int damage, float appearTime, float stayTime, float disappearTime)
        {
            this.targetPos = posTarget;
            this.txtDamage.text = damage.ToString();

            this.imgSlash.material.color = new Color(0, 0, 0, 0);
            this.imgSlash.material.DOFade(1, appearTime)
                .SetEase(Ease.OutQuint);

            this.txtDamage.transform.localScale = Vector3.one;
            this.txtDamage.transform.localPosition = Vector3.zero;
            Vector3 targetJump = this.GetRandomJumpDestination();
            float jumpHeight = targetJump.sqrMagnitude;
            this.txtDamage.transform.DOLocalJump(targetJump, jumpHeight, 1, appearTime);
        }

        public void DisappearEffect(float disappearTime)
        {
        }

        private Vector3 GetRandomJumpDestination()
        {
            float x = RandomHelper.GetRandomFloat(-this.maxJumpOffset.x, this.maxJumpOffset.x);
            float y = RandomHelper.GetRandomFloat(-this.maxJumpOffset.y, this.maxJumpOffset.y);
            return new Vector3(x, y, 0);
        }

        #endregion
    }
}