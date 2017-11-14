namespace ShiftingDungeon.Dungeon
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Tilemaps;
    using Character.Enemies;
    using Character.Traps;
    using ProcGen.RoomGen;
    using ObjectPooling;
    using RoomParts;
    using Util;

    public class Room : MonoBehaviour
    {
        [SerializeField]
        private int row = 0;
        [SerializeField]
        private int col = 0;
        [SerializeField]
        private bool isGenerated = false;

        [SerializeField]
        private Room up = null;
        [SerializeField]
        private Room down = null;
        [SerializeField]
        private Room left = null;
        [SerializeField]
        private Room right = null;

        [SerializeField]
        internal Door upperDoor = null;
        [SerializeField]
        internal Door lowerDoor = null;
        [SerializeField]
        internal Door leftDoor = null;
        [SerializeField]
        internal Door rightDoor = null;

        [SerializeField]
        private Spawners.Spawner[] entitiesToSpawn;

        /// <summary> The index of this room in the map. </summary>
        public int Index { get; private set; }
        /// <summary> The map row this room is on. </summary>
        public int Row { get { return this.row; } internal set { this.row = value; } }
        /// <summary> The map column this room is on. </summary>
        public int Col { get { return this.col; } internal set { this.col = value; } }
        /// <summary> True if this room is being proceedurally generated. </summary>
        public bool IsGenerated { get { return this.isGenerated; } internal set { this.isGenerated = value; } }
        /// <summary> The room above this one if it exists. </summary>
        public Room Up { get { return this.up; } internal set { this.up = value; } }
        /// <summary> The room below this one if it exists. </summary>
        public Room Down { get { return this.down; } internal set { this.down = value; } }
        /// <summary> The room to the left of this one if it exists. </summary>
        public Room Left { get { return this.left; } internal set { this.left = value; } }
        /// <summary> The room to the right of this one if it exists. </summary>
        public Room Right { get { return this.right; } internal set { this.right = value; } }
        /// <summary> The procedurally generated room. </summary>
        public int[,] Grid { get; private set; }

        private List<GameObject> doors;
        private List<GameObject> walls;
        private List<GameObject> stairs;
        private List<Enemy> enemies;
        private List<Trap> traps;
        private bool fullCleared;

        /// <summary> Initializes this Room. </summary>
        internal void Init(int index, bool hasStairs)
        {
            this.Index = index;

            if (!this.IsGenerated)
            {
                if (!CheckDoors())
                    return;

                this.upperDoor.Init(this, this.Up);
                this.lowerDoor.Init(this, this.Down);
                this.leftDoor.Init(this, this.Left);
                this.rightDoor.Init(this, this.Right);
                this.Grid = null;
            }
            else
            {
                CellularAutomata ca = new CellularAutomata(15);
                ca.GeneratePattern(30);
                this.Grid = ca.GetPattern();
                if (hasStairs)
                    this.Grid[7, 7] = 2;
            }

            this.doors = new List<GameObject>();
            this.walls = new List<GameObject>();
            this.stairs = new List<GameObject>();
            this.enemies = new List<Enemy>();
            this.traps = new List<Trap>();
            Deactivate();
            this.fullCleared = false;
        }

        public void Activate(Tilemap floorMap)
        {
            if (this.IsGenerated)
            {
                floorMap.GetComponent<TilemapCollider2D>().enabled = false;
                AllocateObjects(floorMap);
                floorMap.GetComponent<TilemapCollider2D>().enabled = true;
                floorMap.transform.parent.gameObject.SetActive(true);
            }
            else
            {
                foreach (Spawners.Spawner spawner in this.entitiesToSpawn)
                {
                    if (this.fullCleared)
                    {
                        if (!(spawner is Spawners.EnemySpawner))
                            spawner.Spawn();
                    }
                    else
                        spawner.Spawn();
                }
            }

            this.gameObject.SetActive(true);
            this.upperDoor.Init(this, this.Up);
            this.lowerDoor.Init(this, this.Down);
            this.leftDoor.Init(this, this.Left);
            this.rightDoor.Init(this, this.Right);
        }

        public void Deactivate()
        {
            foreach (GameObject door in this.doors)
                RoomPool.Instance.ReturnDoor(door);
            this.doors.Clear();

            foreach (GameObject wall in this.walls)
                RoomPool.Instance.ReturnWall(wall);
            this.walls.Clear();

            foreach (GameObject stair in this.stairs)
                RoomPool.Instance.ReturnStair(stair);
            this.stairs.Clear();

            this.fullCleared = true;
            foreach (Enemy e in this.enemies)
            {
                if (e.gameObject.activeInHierarchy)
                    this.fullCleared = false;
                EnemyPool.Instance.ReturnEnemy(e.Type, e.gameObject);
            }

            this.enemies.Clear();

            foreach (Trap t in this.traps)
                TrapPool.Instance.ReturnTrap(t.Type, t.gameObject);
            this.traps.Clear();
            
            foreach (Spawners.Spawner spawner in this.entitiesToSpawn)
                spawner.Return();

            this.gameObject.SetActive(false);
        }
        
        private bool CheckDoors()
        {
            if (this.upperDoor == null)
            {
                Debug.LogError("Room " + this.name + " missing upper door");
                return false;
            }
            if (this.lowerDoor == null)
            {
                Debug.LogError("Room " + this.name + " missing lower door");
                return false;
            }
            if (this.leftDoor == null)
            {
                Debug.LogError("Room " + this.name + " missing left door");
                return false;
            }
            if (this.rightDoor == null)
            {
                Debug.LogError("Room " + this.name + " missing right door");
                return false;
            }

            return true;
        }

        private void AllocateObjects(Tilemap floorMap)
        {
            AllocateDoors();
            AllocateWalls();
            AllocateFloorsAndHoles(floorMap);
            if((!fullCleared && this.Index != 0) || Managers.DungeonManager.Instance.isTitleScreen)
                AllocateEnemies();
            AllocateTraps();
        }

        private void AllocateDoors()
        {
            this.upperDoor = RoomPool.Instance.GetDoor().GetComponent<Door>();
            this.lowerDoor = RoomPool.Instance.GetDoor().GetComponent<Door>();
            this.leftDoor = RoomPool.Instance.GetDoor().GetComponent<Door>();
            this.rightDoor = RoomPool.Instance.GetDoor().GetComponent<Door>();

            this.upperDoor.transform.position = new Vector3(0, 8, Constants.ROOM_PART_Z_DEPTH);
            this.upperDoor.transform.rotation = Quaternion.identity;
            this.upperDoor.transform.localScale = Vector3.one;

            this.lowerDoor.transform.position = new Vector3(0, -8, Constants.ROOM_PART_Z_DEPTH);
            this.lowerDoor.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            this.lowerDoor.transform.localScale = Vector3.one;

            this.leftDoor.transform.position = new Vector3(-8, 0, Constants.ROOM_PART_Z_DEPTH);
            this.leftDoor.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            this.leftDoor.transform.localScale = Vector3.one;

            this.rightDoor.transform.position = new Vector3(8, 0, Constants.ROOM_PART_Z_DEPTH);
            this.rightDoor.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
            this.rightDoor.transform.localScale = Vector3.one;

            this.doors.Add(this.upperDoor.gameObject);
            this.doors.Add(this.lowerDoor.gameObject);
            this.doors.Add(this.leftDoor.gameObject);
            this.doors.Add(this.rightDoor.gameObject);
        }

        private void AllocateWalls()
        {
            GameObject wall = RoomPool.Instance.GetWall();
            wall.transform.position = new Vector3(0, 3, Constants.ROOM_PART_Z_DEPTH);
            wall.transform.rotation = Quaternion.identity;
            wall.transform.localScale = Vector3.one;
            this.walls.Add(wall);
        }

        private void AllocateFloorsAndHoles(Tilemap floorMap)
        {
            for(int r = 0; r < this.Grid.GetLength(0); r++)
            {
                for (int c = 0; c < this.Grid.GetLength(1); c++)
                {
                    if (this.Grid[r, c] == 1)
                    {
                        floorMap.SetTile(new Vector3Int(r, -c, 0), GetFloorTile(r,c));
                    }
                    else if(this.Grid[r, c] == 2)
                    {
                        floorMap.SetTile(new Vector3Int(r, -c, 0), GetFloorTile(r, c));
                        GameObject stair = RoomPool.Instance.GetStair();
                        stair.transform.position = new Vector3(-7 + r, 7 - c, Constants.ROOM_PART_Z_DEPTH);
                        stair.transform.rotation = Quaternion.identity;
                        stair.transform.localScale = Vector3.one;
                        this.stairs.Add(stair);
                    }
                    else
                    {
                        floorMap.SetTile(new Vector3Int(r, -c, 0), GetHoleTile(r, c));
                    }
                }
            }
        }

        private void AllocateEnemies()
        {
            List<Vector2> positions = new List<Vector2>();
            int length = System.Enum.GetNames(typeof(Enums.EnemyTypes)).Length - 1;
            for (int i = 0; i < 4; i++)
            {
                bool placed = false;
                int tries = 0;
                while (!placed && tries < 100)
                {
                    int r = Random.Range(1, 15);
                    int c = Random.Range(1, 15);
                    Vector2 position = new Vector2(-7 + r, 7 - c);
                    if (this.Grid[r, c] == 1 && !positions.Contains(position))
                    {
                        GameObject enemy = EnemyPool.Instance.GetEnemy((Enums.EnemyTypes)Random.Range(0, length));
                        if (enemy != null)
                        {
                            enemy.transform.position = new Vector3(position.x, position.y, 9);
                            enemy.transform.rotation = Quaternion.identity;
                            this.enemies.Add(enemy.GetComponent<Enemy>());
                            positions.Add(position);
                            placed = true;
                        }
                    }

                    tries++;
                }
            }
        }

        private void AllocateTraps()
        {
            List<Vector2> positions = new List<Vector2>();
            for (int i = 0; i < System.Enum.GetNames(typeof(Enums.Traps)).Length; i++)
            {
                if (Random.Range(0f, 1f) < .4f)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (Random.Range(0f, 1f) < .6f)
                        {
                            bool placed = false;
                            int tries = 0;
                            while (!placed && tries < 100)
                            {
                                int r = Random.Range(1, 15);
                                int c = Random.Range(1, 15);
                                Vector3 position = new Vector3(-7 + r, 7 - c, Constants.ROOM_PART_Z_DEPTH - 1);
                                if (this.Grid[r, c] == 1 && !positions.Contains(position))
                                {
                                    GameObject trap = TrapPool.Instance.GetTrap((Enums.Traps)i);
                                    trap.transform.position = new Vector3(position.x, position.y, 9);
                                    trap.transform.rotation = Quaternion.identity;
                                    this.traps.Add(trap.GetComponent<Trap>());
                                    positions.Add(position);
                                    placed = true;
                                }

                                tries++;
                            }
                        }
                    }
                }
            }
        }

        private Tile GetFloorTile(int r, int c)
        {
            bool up = c == 0 || this.Grid[r, c - 1] != 1;
            bool down = c == this.Grid.GetLength(1) - 1 || this.Grid[r, c + 1] != 1;
            bool left = r == 0 || this.Grid[r - 1, c] != 1;
            bool right = r == this.Grid.GetLength(0) - 1 || this.Grid[r + 1, c] != 1;
            int sprite = 0;
            if (up)
                sprite |= 0x8;
            if (down)
                sprite |= 0x4;
            if (left)
                sprite |= 0x2;
            if (right)
                sprite |= 0x1;

            return RoomPool.Instance.floorTiles[sprite];
        }

        private Tile GetHoleTile(int r, int c)
        {
            bool up = c == 0 || this.Grid[r, c - 1] != 0;
            bool down = c == this.Grid.GetLength(1) - 1 || this.Grid[r, c + 1] != 0;
            bool left = r == 0 || this.Grid[r - 1, c] != 0;
            bool right = r == this.Grid.GetLength(0) - 1 || this.Grid[r + 1, c] != 0;
            int sprite = 0;
            if (up)
                sprite |= 0x8;
            if (down)
                sprite |= 0x4;
            if (left)
                sprite |= 0x2;
            if (right)
                sprite |= 0x1;

            return RoomPool.Instance.holeTiles[sprite];
        }

        /// <summary>
        /// Switches the sprites of doors in the room.
        /// </summary>
        /// <param name="newDoor">The reference door object to get new sprites from.</param>
        public void SwitchDoorSprites(GameObject newDoor)
        {
            Sprite newSprite = newDoor.GetComponent<SpriteRenderer>().sprite;
            RuntimeAnimatorController newAnimator = newDoor.GetComponent<Animator>().runtimeAnimatorController;
            foreach (GameObject door in this.doors)
            {
                door.GetComponent<SpriteRenderer>().sprite = newSprite;
                bool open = door.GetComponent<Door>().IsOpen;
                Animator doorAnimator = door.GetComponent<Animator>();
                doorAnimator.runtimeAnimatorController = newAnimator;
                doorAnimator.SetBool("open", open);
            }
        }
    }
}
