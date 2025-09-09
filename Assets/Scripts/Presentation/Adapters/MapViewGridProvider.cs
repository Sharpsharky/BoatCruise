using SailingBoat.Application.Abstractions;
using SailingBoat.Domain.Grid;
using SailingBoat.Presentation.Views;
using UnityEngine;

namespace SailingBoat.Application.Camera
{
    public class MapViewGridProvider : MonoBehaviour, IHexGridProvider
    {
        [SerializeField] private MapView mapView;
        public HexGrid CurrentGrid => mapView.CurrentGrid;
    }
}