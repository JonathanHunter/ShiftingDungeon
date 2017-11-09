namespace ShiftingDungeon.UI
{

    using UnityEngine;
    using UnityEngine.Tilemaps;
    using System.Collections.Generic;
    using Dungeon;
    using Dungeon.RoomParts;
    using ObjectPooling;
    using Util;

    /// <summary>
    /// Controls the demo of the game on the title screen.
    /// </summary>
    public class TitleScreenDemo : MonoBehaviour
    {

        /// <summary> The amount of time to display a room before changing rooms. </summary>
        [SerializeField]
        private float roomSwitchTime = 30;
        /// <summary> Timer for switching between rooms in the demo. </summary>
        private float roomSwitchTimer = 0;
        /// <summary> Whether the demo is about to be reset. </summary>
        public bool IsResetting {
            get; private set;
        }

        /// <summary> The tile set to use for the currently generated floor. </summary>
        private Enums.TileSets currentTileSet;
        /// <summary> The sprites for holes of various edges in the cavern floors. </summary>
        [SerializeField]
        private Tile[] cavernHoleTiles;
        /// <summary> The sprites for floors of various edges in the cavern floors. </summary>
        [SerializeField]
        private Tile[] cavernFloorTiles;
        /// <summary> The sprites for holes of various edges in the dungeon floors. </summary>
        [SerializeField]
        private Tile[] dungeonHoleTiles;
        /// <summary> The sprites for floors of various edges in the dungeon floors. </summary>
        [SerializeField]
        private Tile[] dungeonFloorTiles;
        /// <summary> The sprites for holes of various edges in the royal court floors. </summary>
        [SerializeField]
        private Tile[] royalCourtHoleTiles;
        /// <summary> The sprites for floors of various edges in the royal court floors. </summary>
        [SerializeField]
        private Tile[] royalCourtFloorTiles;
        /// <summary> All types of hole tiles. </summary>
        private List<Tile[]> holeTiles;
        /// <summary> All types of floor tiles. </summary>
        private List<Tile[]> floorTiles;
        /// <summary> Door prefabs for the three door types. </summary>
        [SerializeField]
        private Door[] doors;
        /// <summary> Wall prefabs for the three wall types. </summary>
        [SerializeField]
        private StaticRoomPiece[] wallPrefabs;
        private GameObject[] wallObjects;

        [SerializeField]
        private GameObject[] arrows;

        /// <summary>
        /// Instantiates all wall types.
        /// </summary>
        private void Start()
        {
            holeTiles = new List<Tile[]>();
            holeTiles.Add(cavernHoleTiles);
            holeTiles.Add(dungeonHoleTiles);
            holeTiles.Add(royalCourtHoleTiles);
            floorTiles = new List<Tile[]>();
            floorTiles.Add(cavernFloorTiles);
            floorTiles.Add(dungeonFloorTiles);
            floorTiles.Add(royalCourtFloorTiles);

            wallObjects = new GameObject[wallPrefabs.Length];
            int currentTileSetIndex = (int) currentTileSet;
            int i = 0;
            foreach (StaticRoomPiece wall in wallPrefabs)
            {
                if (wall)
                {
                    GameObject newWall = Instantiate(wall).gameObject;
                    Vector3 wallPos = newWall.transform.position;
                    wallPos = new Vector3(wallPos.x, 3, wallPos.z - 1);
                    newWall.transform.position = wallPos;
                    newWall.SetActive(i == currentTileSetIndex);
                    wallObjects[i] = newWall;
                }
                i++;
            }
        }

        /// <summary>
        /// Updates the object.
        /// </summary>
        private void Update() {
            roomSwitchTimer += Time.deltaTime;
            if (roomSwitchTimer > roomSwitchTime)
            {
                Managers.DungeonManager.StartResetTitleScreen();
            }
        }

        /// <summary> Prepares for the demo to change rooms. </summary>
        public void StartReset()
        {
            IsResetting = true;
        }

        /// <summary>
        /// Switches the tile set between types before generating a new map.
        /// </summary>
        public void SwitchTileSet()
        {
            int prevTileSetIndex = (int) currentTileSet;
            wallObjects[prevTileSetIndex].SetActive(false);

            SwitchTileSetType();

            RoomPool roomPool = GetComponent<RoomPool>();
            int currentTileSetIndex = (int) currentTileSet;
            roomPool.holeTiles = holeTiles[currentTileSetIndex];
            roomPool.floorTiles = floorTiles[currentTileSetIndex];
            wallObjects[currentTileSetIndex].SetActive(true);

            SwitchedTo((int) currentTileSet);
        }

        /// <summary>
        /// Chooses which tile set type to use for the next generated room.
        /// </summary>
        public void SwitchTileSetType()
        {
            currentTileSet = currentTileSet == Enums.TileSets.Cavern ? Enums.TileSets.RoyalCourt : Enums.TileSets.Cavern;
        }

        /// <summary>
        /// Switches the sprites on doors in a room.
        /// </summary>
        /// <param name="room">The room to switch door sprites for.</param>
        public void SwitchDoors(Room room)
        {
            room.SwitchDoorSprites(doors[(int) currentTileSet].gameObject);
        }

        /// <summary> Resets the demo when the map changes. </summary>
        public void ResetTitleScreen()
        {
            roomSwitchTimer = 0;
            IsResetting = false;
        }

        /// <summary>
        /// Modifies the UI when a tile set switch occurs.
        /// </summary>
        /// <param name="set">The index of the new tile set type.</param>
        public void SwitchedTo(int set) {
            foreach (GameObject g in this.arrows)
                g.SetActive(false);

            this.arrows[set].SetActive(true);
        }
    }
}