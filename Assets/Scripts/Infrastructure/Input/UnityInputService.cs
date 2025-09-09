using SailingBoat.Domain.Grid;
using System;
using UnityEngine;

namespace SailingBoat.Infrastructure.Input
{
    public class UnityInputService : MonoBehaviour, IInputService
    {
        public event Action<HexTile> OnTileClicked;

        public void TileClicked(HexTile tile)
        {
            OnTileClicked?.Invoke(tile);
        }
    }
}