using System;
using Assignment.Battle.BattleItem;
using Assignment.Battle.BattleItem.Enum;
using UnityEngine;

namespace Assignment.ScriptableObjects
{
    [Serializable]
    public class ItemConfigInfo
    {
        public BattleItemName configName;
        public string name;
        public BattleItemStats stats;
    }
}