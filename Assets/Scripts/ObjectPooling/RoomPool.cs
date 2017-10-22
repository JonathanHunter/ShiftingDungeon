namespace ShiftingDungeon.ObjectPooling
{
    using UnityEngine;
    using UnityEngine.Tilemaps;
    using Dungeon.RoomParts;

    public class RoomPool : ObjectPool
    {
        [SerializeField]
        private Door doorTemplet = null;
        [SerializeField]
        private int doorPoolSize = 4;
        [SerializeField]
        private StaticRoomPiece wallTemplet = null;
        [SerializeField]
        private int wallPoolSize = 1;
        [SerializeField]
        private Stairs stairTemplet = null;
        [SerializeField]
        private int stairPoolSize = 1;
        
        /// <summary> The sprites for holes of various edges. </summary>
        public Tile[] holeTiles;
        /// <summary> The sprites for floors of various edges. </summary>
        public Tile[] floorTiles;

        /// <summary> Singleton instance for this object pool. </summary>
        public static RoomPool Instance { get; private set; }

        protected override void PreInit()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Duplicate Room Pool detected: removing " + this.gameObject.name);
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
        }

        protected override IPoolable[] GetTemplets()
        {
            return new IPoolable[] { this.doorTemplet, this.wallTemplet, this.stairTemplet };
        }

        protected override int[] GetPoolSizes()
        {
            return new int[] { this.doorPoolSize, this.wallPoolSize, this.stairPoolSize };
        }

        public GameObject GetDoor()
        {
            return GetPart(this.doorTemplet);
        }

        public GameObject GetWall()
        {
            return GetPart(this.wallTemplet);
        }

        public GameObject GetStair()
        {
            return GetPart(this.stairTemplet);
        }

        public void ReturnDoor(GameObject door)
        {
            ReturnPart(this.doorTemplet, door);
        }

        public void ReturnWall(GameObject wall)
        {
            ReturnPart(this.wallTemplet, wall);
        }

        public void ReturnStair(GameObject stair)
        {
            ReturnPart(this.stairTemplet, stair);
        }

        private GameObject GetPart(IPoolable templet)
        {
            IPoolable entity = AllocateEntity(templet);
            if (entity == null)
                return null;

            return entity.GetGameObject();
        }

        private void ReturnPart(IPoolable templet, GameObject part)
        {
            IPoolable entity = part.GetComponent<IPoolable>();
            DeallocateEntity(templet, entity);
        }
    }
}
