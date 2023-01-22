using System;
using System.Collections.Generic;
using System.Numerics;
using Assignment.ScriptableObjects;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Assignment.Battle
{
    public class BattleField : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private BattleFieldConfig battleFieldConfig;
        [SerializeField] private BattleFieldUIConfig battleFieldUIConfig;
        [SerializeField] private GameObject prefabAttackAxie;
        [SerializeField] private GameObject prefabDefendAxie;

        private readonly Dictionary<Vector2Int, BattleAxie> coord2Axie = new Dictionary<Vector2Int, BattleAxie>();
        private Vector2 gridSize;

        #endregion

        #region PROPERTIES

        public Vector2 GridSize => gridSize;

        #endregion

        #region UNITY EVENTS

        private void Awake()
        {
            this.ApplyUIConfig(this.battleFieldUIConfig);
            this.ApplySpawnConfig(this.battleFieldConfig, this.prefabAttackAxie, this.prefabDefendAxie);
        }

        #endregion

        #region METHODS

        private void ApplyUIConfig(BattleFieldUIConfig config)
        {
            if (config == null) return;
            switch (config.gridSizeType)
            {
                case GridSizeType.RelativeWithScreenSize:
                    break;
                case GridSizeType.FixedValue:
                    this.gridSize = config.value;
                    break;
            }
        }

        private void ApplySpawnConfig(BattleFieldConfig config, GameObject attackAxie, GameObject defendAxie)
        {
            if (config == null || attackAxie == null || defendAxie == null) return;
            config.attackAxiePoints.ForEach(group => this.SpawnAxieGroup(group, attackAxie));
            config.defendAxiePoints.ForEach(group => this.SpawnAxieGroup(group, defendAxie));
        }

        private void SpawnAxieGroup(BattleGroup battleGroup, GameObject axiePrefab)
        {
            for (int offsetX = 0; offsetX < battleGroup.size.x; offsetX++)
            {
                for (int offsetY = 0; offsetY < battleGroup.size.y; offsetY++)
                {
                    Vector2Int coordinate = new Vector2Int(
                        offsetX + battleGroup.topLeft.x,
                        offsetY + battleGroup.topLeft.y
                    );
                    this.SpawnAxie(coordinate, axiePrefab);
                }
            }
        }

        private void SpawnAxie(Vector2Int coordinate, GameObject axiePrefab)
        {
            GameObject goAxie = Instantiate(axiePrefab);
            BattleAxie battleAxie = goAxie.GetComponent<BattleAxie>();
            if (battleAxie == null)
            {
                DestroyImmediate(goAxie);
                return;
            }

            this.coord2Axie.Add(coordinate, battleAxie);
            goAxie.transform.position = this.GetWorldPosition(coordinate);
        }

        public Vector3 GetWorldPosition(Vector2Int coordinate)
        {
            Vector2Int mapSize = battleFieldConfig.mapSize;
            Vector2 centerPoint = new Vector2(mapSize.x - 1, mapSize.y - 1) / 2;
            float offsetX = coordinate.x - centerPoint.x;
            float offsetY = coordinate.y - centerPoint.y;
            return new Vector3(offsetX * this.GridSize.x, 0, offsetY * this.GridSize.y);
        }

        #endregion
    }
}