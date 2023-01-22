using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assignment.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AxieConfig", menuName = "GameConfiguration/Axie", order = 0)]
    public class AxieConfig : ScriptableObject
    {
        public AxieConfigInfo attackerInfo;
        public AxieConfigInfo defenderInfo;
    }

    [Serializable]
    public struct AxieConfigInfo
    {
        public string axieId;
        public float initHealth;
        public float moveSpeed;
    }
}