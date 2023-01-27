using Assignment.Scene;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assignment.Scripts.Welcome
{
    public class WelcomeScene : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private Button btnEnter;

        #endregion

        #region PROPERTIES

        #endregion

        #region UNITY EVENTS

        private void Awake()
        {
            this.btnEnter.onClick.AddListener(this.EnterGame);
        }

        #endregion

        #region METHODS

        private void EnterGame()
        {
            SceneManager.LoadScene(SceneNames.battleScene, LoadSceneMode.Single);
        }

        #endregion
    }
}