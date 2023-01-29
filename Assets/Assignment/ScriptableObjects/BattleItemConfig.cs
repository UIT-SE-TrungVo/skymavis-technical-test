using System.Collections.Generic;
using Assignment.Battle.BattleItem.Enum;
using UnityEngine;

namespace Assignment.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "GameConfiguration/Item", order = 0)]
    public class BattleItemConfig : ScriptableObject
    {
        public float criticalDamagePercent;
        
        public ItemConfigInfo itemA;
        public ItemConfigInfo itemB;
        public ItemConfigInfo itemC;
        public ItemConfigInfo itemD;
        
        public ItemAAInfo itemAA;
        public ItemABInfo itemAB;
        public ItemACInfo itemAC;
        public ItemADInfo itemAD;
        public ItemBBInfo itemBB;
        public ItemBCInfo itemBC;
        public ItemBDInfo itemBD;
        public ItemCCInfo itemCC;
        public ItemCDInfo itemCD;
        public ItemDDInfo itemDD;

        public Sprite defaultItemImage;
        public List<ItemUIConfigInfo> listItemImage;
    }
}