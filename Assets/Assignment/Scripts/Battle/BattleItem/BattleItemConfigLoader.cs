using System;
using Assignment.ScriptableObjects;
using UnityEngine;

namespace Assignment.Battle.BattleItem
{
    public class BattleItemConfigLoader : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private BattleItemConfig config;

        #endregion

        #region PROPERTIES

        #endregion

        #region UNITY EVENTS

        private void Awake()
        {
            BattleItemInfo.Config = config;
        }

        #endregion

        #region METHODS

        #endregion
    }
}