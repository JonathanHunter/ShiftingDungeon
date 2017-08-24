namespace ShiftingDungeon.Dungeon
{
    using UnityEngine;

    public class Room : MonoBehaviour
    {
        [SerializeField]
        private int row = 0;
        [SerializeField]
        private int col = 0;

        [SerializeField]
        private Room up = null;
        [SerializeField]
        private Room down = null;
        [SerializeField]
        private Room left = null;
        [SerializeField]
        private Room right = null;

        [SerializeField]
        private Door upperDoor = null;
        [SerializeField]
        private Door lowerDoor = null;
        [SerializeField]
        private Door leftDoor = null;
        [SerializeField]
        private Door rightDoor = null;

        /// <summary> The index of this room in the map. </summary>
        public int Index { get; private set; }
        /// <summary> The map row this room is on. </summary>
        public int Row { get { return this.row; } internal set { this.row = value; } }
        /// <summary> The map column this room is on. </summary>
        public int Col { get { return this.col; } internal set { this.col = value; } }
        /// <summary> The room above this one if it exists. </summary>
        public Room Up { get { return this.up; } internal set { this.up = value; } }
        /// <summary> The room below this one if it exists. </summary>
        public Room Down { get { return this.down; } internal set { this.down = value; } }
        /// <summary> The room to the left of this one if it exists. </summary>
        public Room Left { get { return this.left; } internal set { this.left = value; } }
        /// <summary> The room to the right of this one if it exists. </summary>
        public Room Right { get { return this.right; } internal set { this.right = value; } }

        /// <summary> Initializes this Room. </summary>
        internal void Init(int index)
        {
            this.Index = index;

            //if (!CheckDoors())
            //    return;

            //this.upperDoor.Init(this, this.Up);
            //this.lowerDoor.Init(this, this.Down);
            //this.leftDoor.Init(this, this.Left);
            //this.rightDoor.Init(this, this.Right);
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
    }
}
