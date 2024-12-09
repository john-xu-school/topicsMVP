
# Technical Specification

## 1. PlayerController Class

### Variables
- `public Tilemap playerMap`: Tilemap for player rendering
- `public Tilemap tm`: Main game tilemap
- `public TileBase playerTile`: Tile representing the player
- `public Vector2Int playerPos`: Current player position
- `public tileBehaviorManager tileManager`: Reference to tile behavior management script
- `public ProceduralGenerator pg`: Reference to procedural level generation script
- `public int orientation`: Player's current facing direction
  - `-1`: Facing left
  - `1`: Facing right
  - `0`: Facing down
- `public HealthController hc`: Reference to health management script
- `public LevelHandler lh`: Reference to level progression script
- `public GameObject playerEffects`: Player visual effects
- `private GameObject playerEffectsInstance`: Instantiated player effects
- `float playerFallTimer`: Timer for controlling player falling
- `[SerializeField] float playerFallDelay`: Delay between fall steps
- `[SerializeField] private int frameRate`: Locked frame rate (set to 30)

### Methods
- `Start()`: 
  - Initializes player position
  - Spawns player effects
  - Places player tile on map
- `Update()`: 
  - Handles player input
  - Manages player movement
  - Controls falling mechanics
  - Checks for health pickups
  - Manages game state (winning/losing)
- `checkFall()`: 
  - Implements gravity
  - Makes player fall if no tile is underneath
- `heal()`: 
  - Increases health when player touches health tile
  - Removes health tile after pickup

## 2. ProceduralGenerator Class

### Variables
- `public int width`: Level width
- `public int height`: Level height
- `public Tilemap tm`: Tilemap for level generation
- `public TileBase[] allTileType`: Array of possible tile types
- `public TileBase healthTile`: Tile type for health pickups
- `public TileBase emptyTile`: Placeholder empty tile
- `public Dictionary<Vector3, Vector3> healthEffectPos`: Tracks health effect positions
- `public GameObject healEffect`: Visual effect for health pickups
- `private int[,] mapOfType`: 2D array tracking tile types
- `public float spawnRate`: Probability of spawning health tiles

### Methods
- `Start()`: 
  - Generates random tile map
  - Randomly spawns health tiles and effects

## 3. tileBehaviorManager Class

### Variables
- `public Tilemap tm`: Main game tilemap
- `public ProceduralGenerator pg`: Reference to procedural generator
- `public bool[,] visited`: Tracks visited tiles during cascade
- `private int rows`: Level width
- `private int cols`: Level height
- `public Vector3Int startingPos`: Starting position for tile cascade
- `public TileBase test`: Test tile (unused)
- `public PlayerController pc`: Reference to player controller
- `public float tileFallDelay`: Delay between tile fall steps
- `public float blockShakeDelay`: Delay for block shaking effect
- `public List<Vector3Int> shouldFallBlocks`: Blocks scheduled to fall
- `public Tilemap shakingMap`: Tilemap for block shake animation
- `public GameObject tileDestroyEffect`: Visual effect for tile destruction

### Methods
- `Start()`: Initializes visited tile tracking
- `cascade(Vector3Int pos, TileBase tileBase)`: 
  - Triggers tile destruction
  - Finds and removes connected tiles
  - Manages falling blocks
- `findConnected(Vector3Int pos, TileBase tileBase, bool[,] visited)`: 
  - Recursive method to find connected tiles of same type
- `checkBlockFall(List<(int r, int c)> toDelete)`: 
  - Identifies blocks that should fall after tile destruction
- `fallBehavior()` (Coroutine): 
  - Manages block falling animation
  - Shakes blocks before falling
- `fallAnim(Vector3Int pos, TileBase tb)` (Coroutine): 
  - Animates individual block falling

## Game Mechanics Overview

1. **Level Generation**: Procedurally generated using random tile placement
2. **Player Movement**: 
   - Move left/right with A/D
   - Jump up with W
   - Destroy adjacent tiles with left mouse click
3. **Tile Mechanics**: 
   - Connected tiles can be destroyed together
   - Blocks fall after tile destruction
   - Health tiles can be picked up to restore health
4. **Win/Lose Conditions**:
   - Win by reaching bottom of the level
   - Lose by touching non-health/non-empty tiles
   - Health system manages player survival
