using System.Collections.Generic;
using Assignment.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Assignment.Battle.UI
{
    public class BattleTimeUI : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private BattleFieldConfig config;
        [SerializeField] private Button btnPlay;
        [SerializeField] private Button btnPause;
        [SerializeField] private Button btnIncSpeed;
        [SerializeField] private Button btnDecSpeed;
        [SerializeField] private Text txtCurSpeed;
        private List<float> listSpeedScale;
        private int curSpeedScaleIndex;
        private bool isPaused;

        private float currentTimeScale;

        #endregion

        #region PROPERTIES

        public bool IsPaused
        {
            get => isPaused;
            private set
            {
                isPaused = value;
                this.UpdateTimeScale();
            }
        }

        public int CurSpeedScaleIndex
        {
            get => curSpeedScaleIndex;
            private set
            {
                curSpeedScaleIndex = value;
                this.UpdateTimeScale();
            }
        }

        #endregion

        #region UNITY EVENTS

        private void Awake()
        {
            this.listSpeedScale = config.battleSpeedScale.listScale;
            this.curSpeedScaleIndex = config.battleSpeedScale.speedIndexAtStart;

            this.btnPlay.onClick.AddListener(this.DoPlay);
            this.btnPause.onClick.AddListener(this.DoPause);
            this.btnIncSpeed.onClick.AddListener(this.IncTimeScale);
            this.btnDecSpeed.onClick.AddListener(this.DecTimeScale);

            this.UpdateTimeScale();
        }

        private void LateUpdate()
        {
            Time.timeScale = currentTimeScale;
        }

        #endregion

        #region METHODS

        private void UpdateTimeScale()
        {
            this.btnPlay.gameObject.SetActive(this.IsPaused);
            this.btnPause.gameObject.SetActive(!this.IsPaused);
            int index = Mathf.Clamp(this.CurSpeedScaleIndex, 0, this.listSpeedScale.Count - 1);

            if (this.CurSpeedScaleIndex <= 0)
            {
                this.currentTimeScale = this.IsPaused ? 0 : this.listSpeedScale[index];
                this.LateUpdate();
            }
            else
            {
                this.currentTimeScale = this.IsPaused ? 0 : this.listSpeedScale[index];
            }

            txtCurSpeed.text = "x" + currentTimeScale.ToString("n1");
        }

        private void IncTimeScale()
        {
            if (this.CurSpeedScaleIndex >= this.listSpeedScale.Count - 1) return;
            this.CurSpeedScaleIndex += 1;
        }

        private void DecTimeScale()
        {
            if (this.CurSpeedScaleIndex <= 0) return;
            this.CurSpeedScaleIndex -= 1;
        }

        private void DoPlay()
        {
            if (!this.IsPaused) return;
            this.IsPaused = false;
        }

        private void DoPause()
        {
            if (this.IsPaused) return;
            this.IsPaused = true;
        }

        #endregion
    }
}