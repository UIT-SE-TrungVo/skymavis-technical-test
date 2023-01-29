using System;
using Assignment.Battle.BattleItem.Enum;

namespace Assignment.ScriptableObjects
{
    [Serializable]
    public class ItemAAInfo : ItemConfigInfo
    {
        public int countEmpoweredAttack;
        public float percentDamage;
    }

    [Serializable]
    public class ItemABInfo : ItemConfigInfo
    {
        public float rateConvertHealthToDamage;
    }

    [Serializable]
    public class ItemACInfo : ItemConfigInfo
    {
        public float multiplyCritDamage;
    }

    [Serializable]
    public class ItemADInfo : ItemConfigInfo
    {
        public float percentLifeSteal;
    }

    [Serializable]
    public class ItemBBInfo : ItemConfigInfo
    {
        public float percentReduceDamage;
    }

    [Serializable]
    public class ItemBCInfo : ItemConfigInfo
    {
        //no additional info
    }

    [Serializable]
    public class ItemBDInfo : ItemConfigInfo
    {
        public float percentHealthRecoverFromDodge;
    }

    [Serializable]
    public class ItemCCInfo : ItemConfigInfo
    {
        //no additional info
    }

    [Serializable]
    public class ItemCDInfo : ItemConfigInfo
    {
        //no additional info
    }

    [Serializable]
    public class ItemDDInfo : ItemConfigInfo
    {
        public float dodgeGainEachTime;
        public float maxDodge;
    }
}