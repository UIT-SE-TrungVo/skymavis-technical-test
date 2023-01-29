using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Assignment.Battle.BattleItem.Enum;
using Assignment.ScriptableObjects;
using UnityEngine;

namespace Assignment.Battle.BattleItem
{
    public class BattleItemInfo
    {
        #region FIELDS

        private static BattleItemConfig config;
        private static Sprite defaultItemImage;
        private static Dictionary<BattleItemName, Sprite> nameToSprite = new Dictionary<BattleItemName, Sprite>();

        #endregion

        #region PROPERTIES

        public static BattleItemConfig Config
        {
            get => config;
            set
            {
                config = value;
                ImportMapItemSprite();
            }
        }

        private static void ImportMapItemSprite()
        {
            defaultItemImage = config.defaultItemImage;
            nameToSprite.Clear();
            foreach (ItemUIConfigInfo itemUIConfigInfo in config.listItemImage)
            {
                nameToSprite.Add(itemUIConfigInfo.configName, itemUIConfigInfo.image);
            }
        }

        public static Sprite GetSpriteByItemName(BattleItemName itemName)
        {
            if (!nameToSprite.ContainsKey(itemName)) return defaultItemImage;
            return nameToSprite[itemName];
        }

        #endregion
    }
}