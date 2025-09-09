using System.Collections.Generic;
using System.Threading.Tasks;

namespace SailingBoat.Domain.Pathfinding
{
    public interface IPathfinder
    {
        Task<IList<Grid.HexTile>> FindPathAsync(
            Grid.HexGrid grid,
            Grid.HexTile start,
            Grid.HexTile end);
    }
}