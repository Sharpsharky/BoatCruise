using SailingBoat.Domain.Grid;
using SailingBoat.Presentation.Config;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace SailingBoat.Presentation.Views
{
    public class MapView : MonoBehaviour
    {
        [Header("Labels")]
        [SerializeField] private string waterLabel = "WaterTiles";
        [SerializeField] private string terrainLabel = "TerrainTiles";
        [SerializeField] private string decorationLabel = "Decorations";
        [SerializeField] private string backgroundLabel = "Background";

        [Header("Designer Config")]
        [SerializeField] private MapViewConfig config;

        [Inject] private DiContainer _container;

        public HexGrid CurrentGrid { get; private set; }

        public event System.Action OnMapInitialized;

        private readonly Dictionary<HexTile, Transform> tileTransforms = new();

        public async void Initialize(HexGrid grid)
        {
            CurrentGrid = grid;

            var waterHandle = Addressables.LoadAssetsAsync<GameObject>(waterLabel, null);
            var terrainHandle = Addressables.LoadAssetsAsync<GameObject>(terrainLabel, null);
            var decoHandle = Addressables.LoadAssetsAsync<GameObject>(decorationLabel, null);
            var backgroundHandle = Addressables.LoadAssetAsync<GameObject>(backgroundLabel);

            var waterPrefabs = await waterHandle.Task;
            var terrainPrefabs = await terrainHandle.Task;
            var decorationPrefabs = await decoHandle.Task;
            var backgroundPrefab = await backgroundHandle.Task;

            for (int r = 0; r < grid.Height; r++)
            {
                for (int q = 0; q < grid.Width; q++)
                {
                    var tile = grid.GetTile(q, r);
                    var worldPos = HexToWorldPosition(q, r);
                    var prefabList = tile.Type == TileType.Water ? waterPrefabs : terrainPrefabs;
                    var prefab = prefabList[Random.Range(0, prefabList.Count)];

                    var tileInstance = _container.InstantiatePrefab(prefab, worldPos, Quaternion.identity, transform);
                    tileInstance.name = $"Tile_{q}_{r}";

                    if (tileInstance.TryGetComponent<MapTileView>(out var tileView))
                        tileView.Tile = tile;

                    tileTransforms[tile] = tileInstance.transform;

                    if (tile.Type == TileType.Terrain && decorationPrefabs.Count > 0 && Random.value < config.decorationChance)
                    {
                        var decoPrefab = decorationPrefabs[Random.Range(0, decorationPrefabs.Count)];
                        var decoInstance = Instantiate(decoPrefab, transform);
                        float surfaceY = worldPos.y;
                        if (tileInstance.TryGetComponent(out MeshRenderer mr))
                            surfaceY = mr.bounds.max.y;
                        decoInstance.transform.position = new Vector3(worldPos.x, surfaceY, worldPos.z);
                    }
                }
            }

            var waterGO = Instantiate(backgroundPrefab, transform);
            SetupBackground(waterGO, grid);

            OnMapInitialized?.Invoke();
        }

        public Transform GetCenterTileTransform()
        {
            if (CurrentGrid == null)
                return null;

            int centerQ = CurrentGrid.Width / 2;
            int centerR = CurrentGrid.Height / 2;
            return GetTileTransform(CurrentGrid.GetTile(centerQ, centerR));
        }

        private Transform GetTileTransform(HexTile tile)
        {
            if (tile == null)
                return null;

            if (tileTransforms.TryGetValue(tile, out var t))
                return t;

            return null;
        }

        private void SetupBackground(GameObject backgroundPrefab, HexGrid grid)
        {
            grid.GetWorldCorners(out Vector3 topLeft, out Vector3 bottomRight);

            float width = bottomRight.x - topLeft.x;
            float height = topLeft.z - bottomRight.z;

            backgroundPrefab.transform.localScale =
                new Vector3(width + 0.5f + config.backgroundSizeOffset, height + 0.5f + config.backgroundSizeOffset, 1f);

            float centerX = (topLeft.x + bottomRight.x) / 2f;
            float centerZ = (topLeft.z + bottomRight.z) / 2f;
            backgroundPrefab.transform.localPosition = new Vector3(centerX, 0f, centerZ);
        }

        private Vector3 HexToWorldPosition(int q, int r)
        {
            float x = (q + r * 0.5f - Mathf.Floor(r / 2f)) * 1f;
            float z = r * (Mathf.Sqrt(3f) / 2f) * 1f;
            return new Vector3(x, 0, z);
        }
    }
}
