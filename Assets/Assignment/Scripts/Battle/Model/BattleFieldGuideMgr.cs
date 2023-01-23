using System;
using System.Collections.Generic;
using Assignment.Battle.Model;
using UnityEngine;

namespace Assignment.Battle.GuideMap
{
    public class BattleFieldGuideMgr
    {
        #region FIELDS

        //in some cases which attackers form a line that blocks some coords from defenders way -> assign it with high value
        private const int VALUE_VALID_BUT_UNREACHED_COORD = int.MaxValue;

        private static readonly Vector2Int[] Directions =
        {
            new Vector2Int(0, -1),
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
        };

        private readonly BattleFieldPositionMgr positionMgr;
        private readonly Dictionary<Vector2Int, int> coord2DistanceToEnemy = new Dictionary<Vector2Int, int>();

        #endregion

        #region METHODS

        public BattleFieldGuideMgr(BattleFieldPositionMgr positionMgr)
        {
            this.positionMgr = positionMgr;
        }

        public void UpdateMap()
        {
            coord2DistanceToEnemy.Clear();
            Queue<Vector2Int> queueVisit = new Queue<Vector2Int>();
            foreach (KeyValuePair<Vector2Int, BattleAxie> coordAndAxie in positionMgr.Coord2Axie)
            {
                if (coordAndAxie.Value.AxieSide == BattleAxieSide.Defender)
                {
                    coord2DistanceToEnemy.Add(coordAndAxie.Key, 0);
                    queueVisit.Enqueue(coordAndAxie.Key);
                    continue;
                }

                coord2DistanceToEnemy.Add(coordAndAxie.Key, -1);
            }

            while (queueVisit.Count > 0)
            {
                Vector2Int fromCoord = queueVisit.Dequeue();
                foreach (Vector2Int direction in Directions)
                {
                    Vector2Int toCoord = fromCoord + direction;
                    if (!this.positionMgr.IsCoordInMap(toCoord)) continue;

                    int fromValue = this.coord2DistanceToEnemy[fromCoord];
                    if (!this.coord2DistanceToEnemy.ContainsKey(toCoord))
                    {
                        this.coord2DistanceToEnemy.Add(toCoord, fromValue + 1);
                        queueVisit.Enqueue(toCoord);
                    }
                    else if (this.coord2DistanceToEnemy[toCoord] > fromValue + 1)
                    {
                        this.coord2DistanceToEnemy[toCoord] = fromValue + 1;
                    }
                }
            }
        }

        public Vector2Int? GetNextMoveCoord(Vector2Int currentCoord)
        {
            if (!this.positionMgr.IsCoordInMap(currentCoord)) return null;
            List<Vector2Int> listPoint = new List<Vector2Int>();
            int bestValue = -1;
            foreach (Vector2Int direction in Directions)
            {
                Vector2Int coord = currentCoord + direction;
                if (!this.positionMgr.IsCoordInMap(coord)) continue;
                int distanceFromCoord = this.coord2DistanceToEnemy.ContainsKey(coord)
                    ? this.coord2DistanceToEnemy[coord]
                    : VALUE_VALID_BUT_UNREACHED_COORD;
                //avoid 0 and -1 (occupied by defenders / attacker)
                if (distanceFromCoord > 0) 
                {
                    if (bestValue == -1 || bestValue > distanceFromCoord)
                    {
                        bestValue = distanceFromCoord;
                        listPoint.Clear();
                    }
                    else if (distanceFromCoord > bestValue) continue;

                    listPoint.Add(coord);
                }

                Debug.LogFormat("Check coord {0} {1} {2} {3}",
                    currentCoord,
                    coord,
                    this.coord2DistanceToEnemy.ContainsKey(coord) ? this.coord2DistanceToEnemy[coord] : -999,
                    this.positionMgr.IsCoordInMap(coord)
                );
            }

            return listPoint.Count > 0 ? RandomHelper.GetRandomElementFromList(listPoint) : (Vector2Int?)null;
        }

        public Vector2Int? GetNearbyEnemyCoord(Vector2Int currentCoord, BattleAxieSide side)
        {
            int enemyValue = side == BattleAxieSide.Attacker ? 0 : -1;
            List<Vector2Int> listNearbyEnemies = new List<Vector2Int>();
            foreach (Vector2Int direction in Directions)
            {
                Vector2Int coord = currentCoord + direction;
                Debug.LogFormat("Check {0} {1}", coord, this.coord2DistanceToEnemy.ContainsKey(coord));
                if (!this.positionMgr.IsCoordInMap(coord)) continue;
                int value = this.coord2DistanceToEnemy.ContainsKey(coord)
                    ? this.coord2DistanceToEnemy[coord]
                    : VALUE_VALID_BUT_UNREACHED_COORD;
                if (value == enemyValue)
                {
                    listNearbyEnemies.Add(coord);
                }
            }

            return listNearbyEnemies.Count > 0
                ? RandomHelper.GetRandomElementFromList(listNearbyEnemies)
                : (Vector2Int?)null;
        }

        #endregion
    }
}