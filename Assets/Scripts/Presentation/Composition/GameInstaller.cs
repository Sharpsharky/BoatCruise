using SailingBoat.Application.Camera;
using SailingBoat.Application.Services;
using SailingBoat.Domain.Parsing;
using SailingBoat.Domain.Pathfinding;
using SailingBoat.Infrastructure.Input;
using SailingBoat.Infrastructure.Parsing;
using SailingBoat.Infrastructure.Pathfinding;
using SailingBoat.Presentation.Utilities;
using SailingBoat.Presentation.Views;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace SailingBoat.Presentation.Composition
{
    public class GameInstaller : MonoInstaller
    {
        [Header("Map Setup")]
        [SerializeField] private MapView mapView;

        [Header("Visualization")]
        [SerializeField] private LineRendererVisualizer pathVisualizer;

        [Header("Input Service")]
        [SerializeField] private UnityInputService inputService;

        [Header("Controllers")]
        [SerializeField] private BoatController boatController;

        [Header("Camera")]
        [SerializeField] private CameraFollow cameraFollow;
        [SerializeField] private CinemachineCamera cinemachineCamera;

        public override void InstallBindings()
        {
            // Infrastructure
            Container.Bind<IInputService>().FromInstance(inputService).AsSingle();
            Container.Bind<IMapParser>().To<TextAssetMapParser>().AsTransient();
            Container.Bind<IPathfinder>().To<AStarPathfinder>().AsSingle();

            // Application
            Container.Bind<MapService>().AsSingle();
            Container.Bind<PathfindingService>().AsSingle();
            Container.Bind<BoatService>().AsSingle();

            // Presentation
            Container.Bind<MapView>().FromInstance(mapView).AsSingle();
            Container.Bind<BoatController>().FromInstance(boatController).AsSingle();
            Container.Bind<LineRendererVisualizer>().FromInstance(pathVisualizer).AsSingle();
            Container.Bind<CameraFollow>().FromInstance(cameraFollow).AsSingle();
            Container.Bind<CinemachineCamera>().FromInstance(cinemachineCamera).AsSingle();
        }
    }
}