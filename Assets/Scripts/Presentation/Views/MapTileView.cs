using SailingBoat.Domain.Grid;
using SailingBoat.Infrastructure.Input;
using UnityEngine;
using Zenject;

namespace SailingBoat.Presentation.Views
{
    [RequireComponent(typeof(Collider))]
    public class MapTileView : MonoBehaviour
    {
        public HexTile Tile { get; set; }

        private IInputService _inputService;

        [Inject]
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        private void OnMouseDown()
        {
            _inputService?.TileClicked(Tile);
        }
    }
}