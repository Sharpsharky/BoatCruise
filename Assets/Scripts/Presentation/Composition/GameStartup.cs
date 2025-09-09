using SailingBoat.Application.Camera;
using SailingBoat.Application.Services;
using SailingBoat.Infrastructure.Parsing;
using SailingBoat.Infrastructure.Pathfinding;
using SailingBoat.Presentation.Utilities;
using SailingBoat.Presentation.Views;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace SailingBoat.Presentation.Composition
{
    public class GameStartup : MonoBehaviour
    {
        [SerializeField] private TextAsset mapAsset;

        [Inject] private PathfindingService _pathService;
        [Inject] private readonly LineRendererVisualizer _pathVisualizer;
        [Inject] private readonly BoatController _boatController;
        [Inject] private readonly MapView _mapView;
        [Inject] private readonly CameraFollow _cameraFollow;
        [Inject] private readonly CinemachineCamera _cinemachineCamera;

        private void Awake()
        {
            var parser = new TextAssetMapParser();
            var mapService = new MapService(parser);
            _pathService = new PathfindingService(new AStarPathfinder());
            _pathService.OnPathFound += _pathVisualizer.ShowPath;

            mapService.OnMapReady += grid =>
            {
                _mapView.Initialize(grid);
                _boatController.Initialize(_mapView, _pathService, _cameraFollow);

                _mapView.OnMapInitialized += () =>
                {
                    _cameraFollow.Initialize(_cinemachineCamera, _mapView.GetCenterTileTransform());
                };
            };

            _ = mapService.LoadMapAsync(mapAsset);
        }
    }
}
