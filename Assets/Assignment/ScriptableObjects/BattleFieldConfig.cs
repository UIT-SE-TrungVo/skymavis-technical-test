using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assignment.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BattleField", menuName = "GameConfiguration/BattleField", order = 0)]
    public class BattleFieldConfig : ScriptableObject
    {
        public Vector2Int mapSize;
        public List<BattleGroup> attackAxiePoints;
        public List<BattleGroup> defendAxiePoints;

        public float secondPerTurn;

        public BattleSpeedScale battleSpeedScale;
    }

    [Serializable]
    public struct BattleGroup
    {
        public Vector2Int topLeft;
        public Vector2Int size;
    }

    [Serializable]
    public struct BattleSpeedScale
    {
        public List<float> listScale;
        public int speedIndexAtStart;
    }
}