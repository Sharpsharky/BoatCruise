using SailingBoat.Domain.Grid;
using SailingBoat.Domain.Pathfinding;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SailingBoat.Application.Services
{
    public class PathfindingService
    {
        private readonly IPathfinder _pathfinder;
        public event Action<IList<HexTile>> OnPathFound;

        public PathfindingService(IPathfinder pathfinder)
        {
            _pathfinder = pathfinder;
        }

        public async Task<IList<HexTile>> ComputePathAsync(HexGrid grid, HexTile start, HexTile end)
        {
            var path = await _pathfinder.FindPathAsync(grid, start, end);
            OnPathFound?.Invoke(path);

            return path;
        }
    }
}
