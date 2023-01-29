using System;
using Assignment.Battle.BattleItem;
using Assignment.Battle.BattleItem.Enum;
using UnityEngine;

namespace Assignment.ScriptableObjects
{
    [Serializable]
    public class ItemUIConfigInfo
    {
        public BattleItemName configName;
        public Sprite image;
    }
}