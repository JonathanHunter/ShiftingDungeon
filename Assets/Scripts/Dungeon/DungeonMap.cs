namespace ShiftingDungeon.Dungeon
{
    using UnityEngine;

    public class DungeonMap : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Debug feature to enable drawing the map grid with gizmos.")]
        private bool visializeMap = false;
        [SerializeField]
        private Room[] rooms = null;

        /// <summary> The size of this map. </summary>
        public Vector2 mapSize = Vector2.zero;
        
        /// <summary> The rooms in this map. </summary>
        public Room[] Rooms { get { return this.rooms; } }
        /// <summary> The rows in this map. </summary>
        public int MapRows { get { return (int)this.mapSize.x; } }
        /// <summary> The columns in this map. </summary>
        public int MapCols { get { return (int)this.mapSize.y; } }
        /// <summary> The map for this dungeon floor.  Values refer to index in Rooms array. </summary>
        public int[,] Map { get; private set; }

        private bool inited = false;

        /// <summary> Initializes this Dungeon Map. </summary>
        public void Init()
        {
            if (inited)
                return;

            inited = true;

            // Initialize map
            this.Map = new int[this.MapRows, this.MapCols];
            for(int r = 0; r < this.MapRows; r++)
                for(int c = 0; c < this.MapCols; c++)
                    this.Map[r, c] = -1;

            // Add rooms to map
            for (int i = 0; i < rooms.Length; i++)
            {
                if (!AddRoomToMap(this.Rooms[i], i))
                    return;
            }

            // Initialize room state
            for(int i = 0; i < rooms.Length; i++)
            {
                rooms[i].Init(i);
            }

            rooms[0].Activate();
        }

        /// <summary> 
        /// Sets the room list to the provided list and deletes the rooms that were in the previous list. 
        /// Must call Init() after this method. 
        /// </summary>
        /// <param name="rooms"> The new rooms list. </param>
        public void SetRoomList(Room[] rooms)
        {
            foreach (Room r in this.rooms)
                Destroy(r.gameObject);
            this.rooms = rooms;
            this.inited = false;
        }

        private bool AddRoomToMap(Room room, int index)
        {
            if (room.Row < 0 || room.Row >= this.MapRows)
            {
                Debug.Log("Room " + room.name + " has an out of bounds row number");
                return false;
            }
            if (room.Col < 0 || room.Col >= this.MapCols)
            {
                Debug.Log("Room " + room.name + " has an out of bounds column number");
                return false;
            }
            if(this.Map[room.Row, room.Col] != -1)
            {
                Debug.Log("Room " + room.name + " is trying to be placed on top of another room");
                return false;
            }

            this.Map[room.Row, room.Col] = index;
            ConnectNeighbors(room);
            return true;
        }

        private void ConnectNeighbors(Room room)
        {
            if (room.Col - 1 > 0 && this.Map[room.Row, room.Col - 1] != -1)
            {
                Room up = this.Rooms[this.Map[room.Row, room.Col - 1]];
                room.Up = up;
                up.Down = room;
            }

            if (room.Col + 1 < this.MapCols && this.Map[room.Row, room.Col + 1] != -1)
            {
                Room down = this.Rooms[this.Map[room.Row, room.Col + 1]];
                room.Down = down;
                down.Up = room;
            }

            if (room.Row - 1 > 0 && this.Map[room.Row - 1, room.Col] != -1)
            {
                Room left = this.Rooms[this.Map[room.Row - 1, room.Col]];
                room.Left = left;
                left.Right = room;
            }

            if (room.Row + 1 < this.MapRows && this.Map[room.Row + 1, room.Col] != -1)
            {
                Room right = this.Rooms[this.Map[room.Row + 1, room.Col]];
                room.Right = right;
                right.Left = room;
            }
        }

        private void OnDrawGizmos()
        {
            if(visializeMap)
            {
                foreach(Room r in rooms)
                {
                    Vector2 pos = new Vector2(this.transform.position.x + r.Row, this.transform.position.y - r.Col);
                    Gizmos.color = r.gameObject.activeSelf ? Color.red : Color.gray;
                    Gizmos.DrawCube(pos, new Vector3(.5f, .5f, 1f));
                    Gizmos.color = Color.white;
                    DrawArrow(this.transform.position, r, r.Up);
                    DrawArrow(this.transform.position, r, r.Down);
                    DrawArrow(this.transform.position, r, r.Left);
                    DrawArrow(this.transform.position, r, r.Right);
                }
            }
        }

        private void DrawArrow(Vector3 rootPos, Room startRoom, Room endRoom)
        {
            if (endRoom == null)
                return;
            Vector3 start = new Vector2(rootPos.x + startRoom.Row, rootPos.y - startRoom.Col);
            Vector3 end = new Vector2(rootPos.x + endRoom.Row, rootPos.y - endRoom.Col);
            Vector3 es = Vector3.Normalize(start - end);
            Vector3 left = new Vector3(-es.y, es.x);
            Vector3 leftDiagonal = Vector3.Normalize(es + left / 2f) / 3f;
            Vector3 rightDiagonal = Vector3.Normalize(es - left / 2f) / 3f;
            end += .5f * es;
            Gizmos.DrawLine(start, end);
            Gizmos.DrawLine(end, end + leftDiagonal);
            Gizmos.DrawLine(end, end + rightDiagonal);
        }
    }
}
