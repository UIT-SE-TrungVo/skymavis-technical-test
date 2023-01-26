using System.Collections.Generic;
using UnityEngine;

namespace Assignment.Battle.Model
{
    public class BattleFieldPositionMgr
    {
        #region FIELDS

        private readonly Dictionary<Vector2Int, BattleAxie> coord2Axie = new Dictionary<Vector2Int, BattleAxie>();
        private readonly Dictionary<BattleAxie, Vector2Int> axie2Coord = new Dictionary<BattleAxie, Vector2Int>();
        private readonly Vector2Int mapSize;

        #endregion

        #region PROPERTIES

        public Dictionary<Vector2Int, BattleAxie> Coord2Axie => coord2Axie;

        #endregion

        #region METHODS

        public BattleFieldPositionMgr(Vector2Int mapSize)
        {
            this.mapSize = mapSize;
        }

        public bool PutAxieAtCoord(Vector2Int coord, BattleAxie axie)
        {
            if (!this.IsCoordInMap(coord)) return false;
            if (coord2Axie.ContainsKey(coord) && coord2Axie[coord] != axie) return false;

            this.RemoveAxie(axie);
            this.RemoveAtCoord(coord);

            coord2Axie.Add(coord, axie);
            axie2Coord.Add(axie, coord);
            return true;
        }

        public bool RemoveAxie(BattleAxie axie)
        {
            if (!axie2Coord.ContainsKey(axie)) return false;
            coord2Axie.Remove(axie2Coord[axie]);
            axie2Coord.Remove(axie);
            return true;
        }

        public bool RemoveAtCoord(Vector2Int coord)
        {
            if (!this.IsCoordInMap(coord)) return false;
            if (!coord2Axie.ContainsKey(coord)) return false;
            axie2Coord.Remove(coord2Axie[coord]);
            coord2Axie.Remove(coord);
            return true;
        }

        public BattleAxie GetAxieAtCoord(Vector2Int coord)
        {
            return coord2Axie.ContainsKey(coord) ? coord2Axie[coord] : null;
        }

        public Vector2Int? GetCoordOfAxie(BattleAxie axie)
        {
            return axie2Coord.ContainsKey(axie) ? axie2Coord[axie] : (Vector2Int?)null;
        }

        public bool IsCoordInMap(Vector2Int coord)
        {
            if (coord.x < 0 || coord.x >= this.mapSize.x) return false;
            return coord.y >= 0 && coord.y < this.mapSize.y;
        }

        #endregion
    }
}