using System.Collections.Generic;
using UnityEngine;

namespace SailingBoat.Domain.Grid
{
    public enum TileType { Water, Terrain }

    public class HexGrid
    {
        private readonly HexTile[,] _tiles;
        public int Width { get; }
        public int Height { get; }

        // Offsets for even-r (r % 2 == 0)
        private static readonly (int dq, int dr)[] evenOffsets =
        {
            (+1, 0), (0, -1), (-1, -1), (-1, 0), (-1, +1), (0, +1)
        };

        // Offsets for odd-r (r % 2 != 0)
        private static readonly (int dq, int dr)[] oddOffsets =
        {
            (+1, 0), (+1, -1), (0, -1), (-1, 0), (0, +1), (+1, +1)
        };

        public HexGrid(int width, int height)
        {
            Width = width;
            Height = height;
            _tiles = new HexTile[width, height];
        }

        public void SetTile(int q, int r, HexTile tile)
        {
            _tiles[q, r] = tile;
        }

        public HexTile GetTile(int q, int r) => _tiles[q, r];

        public IEnumerable<HexTile> GetNeighbors(HexTile tile)
        {
            var offsets = (tile.R % 2 == 0) ? evenOffsets : oddOffsets;
            foreach (var (dq, dr) in offsets)
            {
                int nq = tile.Q + dq;
                int nr = tile.R + dr;
                if (nq >= 0 && nq < Width && nr >= 0 && nr < Height)
                {
                    var neighbor = _tiles[nq, nr];
                    if (neighbor != null && neighbor.IsWalkable())
                        yield return neighbor;
                }
            }
        }
        public HexTile GetClosestTile(Vector3 worldPos)
        {
            HexTile best = null;
            float bestDist = float.MaxValue;

            int width = _tiles.GetLength(0);
            int height = _tiles.GetLength(1);

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    var t = _tiles[x, y];
                    var d = Vector3.Distance(t.GetMiddle(), worldPos);
                    if (d < bestDist)
                    {
                        bestDist = d;
                        best = t;
                    }
                }

            return best;
        }

        public void GetWorldCorners(out Vector3 topLeft, out Vector3 bottomRight)
        {
            float minX = float.MaxValue;
            float minZ = float.MaxValue;
            float maxX = float.MinValue;
            float maxZ = float.MinValue;

            for (int q = 0; q < Width; q++)
            {
                for (int r = 0; r < Height; r++)
                {
                    var tile = _tiles[q, r];
                    if (tile == null)
                        continue;

                    var pos = tile.GetMiddle();
                    minX = Mathf.Min(minX, pos.x);
                    minZ = Mathf.Min(minZ, pos.z);
                    maxX = Mathf.Max(maxX, pos.x);
                    maxZ = Mathf.Max(maxZ, pos.z);
                }
            }

            topLeft = new Vector3(minX, 0f, maxZ);
            bottomRight = new Vector3(maxX, 0f, minZ);
        }

        public int Distance(HexTile a, HexTile b)
        {
            int dq = a.Q - b.Q;
            int dr = a.R - b.R;
            return (Mathf.Abs(dq) + Mathf.Abs(dr) + Mathf.Abs(dq + dr)) / 2;
        }
    }
}
