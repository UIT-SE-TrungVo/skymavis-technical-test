using System;
using AxieMixer.Unity;
using UnityEngine;

namespace Assignment.SceneControl
{
    public class SceneController : MonoBehaviour
    {
        #region FIELDS

        #endregion

        #region PROPERTIES

        #endregion

        #region UNITY EVENTS

        private void Awake()
        {
            Mixer.Init();
        }

        #endregion

        #region METHODS

        #endregion
    }
}