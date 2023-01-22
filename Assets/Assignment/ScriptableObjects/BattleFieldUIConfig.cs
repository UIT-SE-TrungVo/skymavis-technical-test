using UnityEngine;

namespace Assignment.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BattleFieldUIConfig", menuName = "GameConfiguration/BattleFieldUI", order = 0)]
    public class BattleFieldUIConfig : ScriptableObject
    {
        public GridSizeType gridSizeType;
        public Vector2 value;
    }

    public enum GridSizeType
    {
        RelativeWithScreenSize,
        FixedValue,
    }
}