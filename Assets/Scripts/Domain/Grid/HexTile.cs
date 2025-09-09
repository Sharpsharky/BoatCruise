using UnityEngine;

namespace SailingBoat.Domain.Grid
{
    public class HexTile
    {
        public int Q { get; }
        public int R { get; }
        public TileType Type { get; }
        public bool HasObstacle { get; set; }

        public HexTile(int q, int r, TileType type)
        {
            Q = q;
            R = r;
            Type = type;
        }

        public bool IsWalkable() => Type == TileType.Water;
        public int GetMovementCost() => IsWalkable() ? 1 : int.MaxValue;
        public Vector3 GetMiddle()
        {
            return new(Q + R * 0.5f - Mathf.Floor(R / 2f), 0f, R * Mathf.Sqrt(3f) / 2f);
        }
    }
}
