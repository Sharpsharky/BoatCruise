using SailingBoat.Application.Camera;
using SailingBoat.Domain.Grid;
using SailingBoat.Infrastructure.Addressables;
using SailingBoat.Infrastructure.Input;
using SailingBoat.Presentation.Config;
using SailingBoat.Presentation.UI.Label;
using SailingBoat.Presentation.Views;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace SailingBoat.Application.Services
{
    public class BoatController : MonoBehaviour
    {
        [Header("Boat Addressable Key")]
        [SerializeField] private string boatLabel = "Boat";

        [Header("Boat Configuration")]
        [SerializeField] private BoatViewConfig config;

        [SerializeField] private StartPromptUI startPrompt;

        private readonly IAsyncLoader _loader = new AddressablesLoader();
        private BoatView _boatView;
        private BoatService _boatService;
        private HexGrid _grid;
        private CancellationTokenSource _movementCts;
        private CameraFollow _cameraFollow;
        private IInputService _inputService;

        [Inject]
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        public void Initialize(MapView mapView, PathfindingService pathService, CameraFollow cameraFollow)
        {
            _grid = mapView.CurrentGrid;
            _cameraFollow = cameraFollow;
            startPrompt.Show();
            _inputService.OnTileClicked += async clicked => await HandleClickAsync(clicked, pathService);
        }

        private async Task HandleClickAsync(HexTile clicked, PathfindingService pathService)
        {
            if (clicked == null)
                return;

            if (_boatView == null)
            {
                var worldPos = clicked.GetMiddle();
                var prefab = await _loader.LoadAsync<GameObject>(boatLabel);
                var boatGO = Instantiate(prefab, worldPos, Quaternion.identity);
                _boatView = boatGO.GetComponent<BoatView>();
                _boatService = new BoatService();
                _boatView.Initialize(_boatService);
                _cameraFollow.ChangeTarget(boatGO.transform);
                startPrompt.HideAnimated();

                return;
            }

            _movementCts?.Cancel();
            _movementCts = new CancellationTokenSource();

            _boatView.transform.GetPositionAndRotation(out var boatPos, out var boatRot);
            var startTile = _grid.GetClosestTile(boatPos);
            var path = await pathService.ComputePathAsync(_grid, startTile, clicked);

            if (path != null && path.Count > 1)
            {
                await _boatService.MoveBoatAlongPathAsync(path, config.moveSpeed, config.rotationSpeed, _movementCts.Token, boatPos, boatRot);
            }
        }
    }
}