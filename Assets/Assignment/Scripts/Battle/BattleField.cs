using System;
using System.Collections.Generic;
using System.Linq;
using Assignment.Battle.GuideMap;
using Assignment.Battle.Model;
using Assignment.Battle.UI;
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
        [SerializeField] private GameObject nodeGroupAxie;
        [SerializeField] private BattleResultUI uiResult;

        private BattleFieldPositionMgr positionMgr;
        private BattleFieldGuideMgr guideMgr;
        private BattleFieldTurnMgr turnMgr;
        private BattleFieldActionMgr actionMgr;

        private Vector2 gridSize;
        private bool flagCheckWinCondition = false;

        #endregion

        #region PROPERTIES

        public Vector2 GridSize => gridSize;

        public BattleFieldPositionMgr PositionMgr => positionMgr;

        public BattleFieldGuideMgr GuideMgr => guideMgr;

        public BattleFieldTurnMgr TurnMgr => turnMgr;

        public BattleFieldActionMgr ActionMgr => actionMgr;

        #endregion

        #region UNITY EVENTS

        private void Awake()
        {
            this.ApplyUIConfig(this.battleFieldUIConfig);

            this.positionMgr = new BattleFieldPositionMgr(this.battleFieldConfig.mapSize);
            this.ApplySpawnConfig(this.battleFieldConfig, this.prefabAttackAxie, this.prefabDefendAxie);

            this.guideMgr = new BattleFieldGuideMgr(this.positionMgr);
            this.turnMgr = new BattleFieldTurnMgr(this.battleFieldConfig.secondPerTurn);
            this.actionMgr = new BattleFieldActionMgr(this.positionMgr);

            this.uiResult.HidePanel();
        }

        private void FixedUpdate()
        {
            bool canExecuteATurn = this.turnMgr.GetUpdate(Time.fixedDeltaTime);
            if (canExecuteATurn)
            {
                this.ExecuteATurn();
            }
        }

        private void LateUpdate()
        {
            if (!this.flagCheckWinCondition) return;
            this.flagCheckWinCondition = true;

            BattleAxieSide? surviveSide = null;
            bool isOnlyOneSideSurvive = this.positionMgr.Coord2Axie.Values.ToList().TrueForAll(axie =>
            {
                if (surviveSide.HasValue) return axie.AxieSide == surviveSide.Value;
                surviveSide = axie.AxieSide;
                return true;
            });

            if (!isOnlyOneSideSurvive) return;
            this.uiResult.ShowPanel(surviveSide);
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
            GameObject goAxie = Instantiate(axiePrefab, this.nodeGroupAxie.transform);
            BattleAxie battleAxie = goAxie.GetComponent<BattleAxie>();
            if (battleAxie == null)
            {
                DestroyImmediate(goAxie);
                return;
            }

            battleAxie.BattleField = this;
            this.positionMgr.PutAxieAtCoord(coordinate, battleAxie);
            goAxie.transform.position = this.GetWorldPosition(coordinate);
        }

        public void RemoveAxie(BattleAxie axie)
        {
            this.positionMgr.RemoveAxie(axie);
        }

        public Vector3 GetWorldPosition(Vector2Int coordinate)
        {
            Vector2Int mapSize = battleFieldConfig.mapSize;
            Vector2 centerPoint = new Vector2(mapSize.x - 1, mapSize.y - 1) / 2;
            float offsetX = coordinate.x - centerPoint.x;
            float offsetY = coordinate.y - centerPoint.y;
            return new Vector3(offsetX * this.GridSize.x, 0, offsetY * this.GridSize.y);
        }

        private void ExecuteATurn()
        {
            this.guideMgr.UpdateMap();
            List<System.Action> listAction = this.actionMgr.GetListNextActions();
            listAction.ForEach(action => action?.Invoke());
        }

        public List<BattleAxie> GetAllAxies()
        {
            return this.positionMgr.Coord2Axie.Values.ToList();
        }

        public void OnAxieDie()
        {
            this.flagCheckWinCondition = true;
        }

        #endregion
    }
}