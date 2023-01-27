using System;
using Assignment.Scene;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assignment.Battle.UI
{
    public class BattleResultUI : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private RectTransform panel;
        [SerializeField] private Button btnRestart;
        [SerializeField] private Button btnMainMenu;
        [SerializeField] private Text txtGameResult;

        #endregion

        #region PROPERTIES

        #endregion

        #region UNITY EVENTS

        private void Awake()
        {
            this.btnRestart.onClick.AddListener(RestartGame);
            this.btnMainMenu.onClick.AddListener(BackToMainMenu);
        }

        #endregion

        #region METHODS

        private static void RestartGame()
        {
            SceneManager.LoadScene(SceneNames.battleScene, LoadSceneMode.Single);
        }

        private static void BackToMainMenu()
        {
            SceneManager.LoadScene(SceneNames.welcomeScene, LoadSceneMode.Single);
        }

        public void HidePanel()
        {
            this.panel.transform.localScale = Vector3.zero;
        }

        public void ShowPanel(BattleAxieSide? winner)
        {
            switch (winner)
            {
                case BattleAxieSide.Attacker:
                    this.txtGameResult.text = "Attackers Win";
                    break;
                case BattleAxieSide.Defender:
                    this.txtGameResult.text = "Defenders Win";
                    break;
                case null:
                    this.txtGameResult.text = "Game Draw !";
                    break;
            }

            this.panel.transform.DOScale(Vector3.one, 1.0f)
                .SetEase(Ease.OutElastic);
        }

        #endregion
    }
}