using SailingBoat.Domain.Grid;
using System;

namespace SailingBoat.Infrastructure.Input
{
    public interface IInputService
    {
        event Action<HexTile> OnTileClicked;
        void TileClicked(HexTile tile);
    }
}
