using SailingBoat.Domain.Grid;
using SailingBoat.Domain.Pathfinding;
using SailingBoat.Infrastructure.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SailingBoat.Infrastructure.Pathfinding
{
    public class AStarPathfinder : IPathfinder
    {
        private readonly Dictionary<HexTile, HexTile> cameFrom = new();
        private readonly Dictionary<HexTile, int> costSoFar = new ();

        public async Task<IList<HexTile>> FindPathAsync(HexGrid grid, HexTile start, HexTile end)
        {
            await Task.Yield();

            PriorityQueue<HexTile> frontier = new();
            cameFrom.Clear();
            costSoFar.Clear();

            frontier.Enqueue(start, 0);
            cameFrom[start] = null;
            costSoFar[start] = 0;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();
                if (current.Equals(end))
                    break;

                foreach (var next in grid.GetNeighbors(current))
                {
                    int newCost = costSoFar[current] + next.GetMovementCost();
                    if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        int priority = newCost + grid.Distance(next, end);
                        frontier.Enqueue(next, priority);
                        cameFrom[next] = current;
                    }
                }
            }

            var path = new List<HexTile>();
            if (!cameFrom.ContainsKey(end))
                return path;

            var node = end;
            while (node != null)
            {
                path.Add(node);
                cameFrom.TryGetValue(node, out node);
            }
            path.Reverse();
            return path;
        }
    }
}