# Sailing Boat Hex Map Prototype - Architecture Overview

## Design Philosophy & Structure
I followed Clean Architecture principles with clear separation of concerns across the following layers:

- Domain - Core logic and data structures (e.g. HexTile, HexGrid, TileType),
- Application - Services coordinating use cases (e.g. MapService, PathfindingService, BoatService),
- Infrastructure - IO-related and 3rd-party integrations (e.g. Addressables, input),
- Presentation - Visual components and MonoBehaviours (e.g. MapView, BoatView, StartPromptUI).

Each layer lives in a dedicated assembly definition (.asmdef), allowing modularity and compile-time dependency control.

## Map Generation
Two maps are provided as TextAsset files, where 0 = Water, 1 = Terrain.
TextAssetMapParser parses the map into a domain-level HexGrid.
MapService loads and constructs the grid using data from the parser.
MapView visualizes the grid with tiles loaded via Unity Addressables (decorated with labels).
Decorations like rocks or vegetation are randomly placed on terrain tiles. Only one decoration per tile is allowed.
The water background plane is dynamically scaled based on the top-left and bottom-right corners of the map, calculated in HexGrid.

## Addressables System
All tile prefabs, boat prefab, decorations, and the background water plane are loaded asynchronously via Addressables.LoadAssetAsync<T>.
Labels (Decoration, Boat, WaterBackground) are used for flexible runtime filtering and grouping.
Groups are manually configured (e.g. one for decorations, one for boats, one for terrain).

## Pathfinding Algorithm
Implemented an A* pathfinding algorithm in AStarPathfinder.
Internally uses a priority queue for the frontier (min-heap implementation).

Time complexity:
A* on hex grid: O(E log V), where:
V = number of walkable hex tiles
E = edges (up to 6 per tile)

Heuristic: hexagonal distance ( (abs(dx) + abs(dy) + abs(dx+dy)) / 2 )

Path is visualized using LineRendererVisualizer.

## Input and Interactivity
Custom IInputService abstraction and UnityInputService MonoBehaviour implementation.

Click on a tile to:
If the boat hasn't been spawned yet -> spawn it.
Else -> compute path to that tile.
MapTileView detects clicks using Unity's OnMouseDown() and invokes the event with the associated HexTile.

## Boat Movement
BoatService animates the boat using MoveToAsync() and RotateTowardsAsync().
The boat stops at each corner to rotate smoothly based on angle and rotationSpeed.
BoatView handles rotation and movement over time using await Task.Yield() for non-blocking async execution.
The camera follows the boat during movement using Cinemachine.

## Config System for Designers
To allow designers and non-programmers to easily tweak gameplay and visual parameters, I introduced a ScriptableObject-based config system, used for:
- Boat movement settings, such as speed and rotation speed - configurable in BoatViewConfig, and injected into the boat's movement logic.
- Map generation settings.

## UI & DOTween
A StartPromptUI Canvas element displays a label: "Click a water tile to start".
When the boat is spawned, the prompt animates upward and fades out using DOTween and CanvasGroup.DOFade() + RectTransform.DOAnchorPosY(), then disables itself.