using System;
using Assignment.Battle.Model;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Assignment.Battle.UI
{
    public class BattleAxieStatsUI : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private Text textHealth;
        [SerializeField] private Text textNumber;
        [SerializeField] private Text textDamage;
        [SerializeField] private RectTransform panel;
        [SerializeField] private Image imgIconTarget;

        private BattleAxie selectedAxie;
        private bool isAxieNull = true;

        private Vector3 vetPanelShowPos;
        private Vector3 vetPanelHidePos;

        #endregion

        #region PROPERTIES

        #endregion

        #region UNITY EVENTS

        private void Awake()
        {
            this.vetPanelShowPos = panel.transform.position;
            this.vetPanelHidePos = this.vetPanelShowPos + Vector3.right * this.panel.rect.width;
        }

        private void Start()
        {
            this.HidePanel();
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (!Physics.Raycast(ray, out hit, 100.0f)) return;
                if (hit.transform == null) return;

                BattleAxie axie = hit.transform.GetComponent<BattleAxie>();
                if (axie != null && axie == this.selectedAxie)
                {
                    this.selectedAxie = null;
                    this.HidePanel();
                }
                else
                {
                    this.selectedAxie = axie;
                }

                this.isAxieNull = (this.selectedAxie == null);

                this.DoAnimShowPanel();
                this.DoAnimShowTargetIcon();
            }

            this.SnapTargetIconOnAxie();
        }

        private void LateUpdate()
        {
            if (this.isAxieNull) return;
            this.UpdateAxieStats(this.selectedAxie);
        }

        #endregion

        #region METHODS

        private void UpdateAxieStats(BattleAxie axie)
        {
            BattleAxieInfo info = axie.Stats;
            float currentHealth = axie.CurrentHealth;
            int battleNumber = axie.LuckyBattleNumber;
            float latestDamage = axie.LatestDamage;

            this.textHealth.text = "Health: @current@ / @max@"
                .Replace("@current@", Mathf.RoundToInt(currentHealth).ToString())
                .Replace("@max@", Mathf.RoundToInt(info.InitHealth).ToString());

            this.textNumber.text = "Battle Number: @number@"
                .Replace("@number@", battleNumber.ToString());

            this.textDamage.text = "Damage: @damage@"
                .Replace("@damage@", Mathf.RoundToInt(latestDamage).ToString());
        }

        private void DoAnimShowPanel()
        {
            this.HidePanel();
            this.panel.transform.DOMove(this.vetPanelShowPos, 0.5f)
                .SetEase(Ease.OutBack);
        }

        private void HidePanel()
        {
            this.panel.position = this.vetPanelHidePos;
        }

        private void DoAnimShowTargetIcon()
        {
            this.imgIconTarget.transform.localScale = Vector3.zero;
            this.imgIconTarget.transform.DOScale(Vector3.one, 0.3f)
                .SetEase(Ease.OutBack);
        }

        private void SnapTargetIconOnAxie()
        {
            if (this.isAxieNull)
            {
                this.imgIconTarget.transform.localScale = Vector3.zero;
            }
            else
            {
                const float offsetYToCenterOnAxie = 20.0f;
                Vector3 targetPosition = Camera.main.WorldToScreenPoint(this.selectedAxie.transform.position);
                targetPosition.y += offsetYToCenterOnAxie;
                targetPosition.z = 0;
                this.imgIconTarget.transform.position = targetPosition;
            }
        }

        #endregion
    }
}