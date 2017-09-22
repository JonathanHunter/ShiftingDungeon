namespace ShiftingDungeon.ObjectPooling
{
    using UnityEngine;
    using Dungeon.RoomParts;

    public class RoomPool : ObjectPool
    {
        [SerializeField]
        private Door doorTemplet = null;
        [SerializeField]
        private int doorPoolSize = 4;
        [SerializeField]
        private StaticRoomPiece floorTemplet = null;
        [SerializeField]
        private int floorPoolSize = 225;
        [SerializeField]
        private StaticRoomPiece wallTemplet = null;
        [SerializeField]
        private int wallPoolSize = 1;
        [SerializeField]
        private StaticRoomPiece holeTemplet = null;
        [SerializeField]
        private int holePoolSize = 225;
        [SerializeField]
        private Stairs stairTemplet = null;
        [SerializeField]
        private int stairPoolSize = 1;

        /// <summary> The sprites for holes of various edges. </summary>
        public Sprite[] holeSprites;

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
            return new IPoolable[] { this.doorTemplet, this.floorTemplet, this.wallTemplet, this.holeTemplet, this.stairTemplet };
        }

        protected override int[] GetPoolSizes()
        {
            return new int[] { this.doorPoolSize, this.floorPoolSize, this.wallPoolSize, this.holePoolSize, this.stairPoolSize };
        }

        public GameObject GetDoor()
        {
            return GetPart(this.doorTemplet);
        }

        public GameObject GetFloor()
        {
            return GetPart(this.floorTemplet);
        }

        public GameObject GetWall()
        {
            return GetPart(this.wallTemplet);
        }

        public GameObject GetHole()
        {
            return GetPart(this.holeTemplet);
        }

        public GameObject GetStair()
        {
            return GetPart(this.stairTemplet);
        }

        public void ReturnDoor(GameObject door)
        {
            ReturnPart(this.doorTemplet, door);
        }

        public void ReturnFloor(GameObject floor)
        {
            ReturnPart(this.floorTemplet, floor);
        }

        public void ReturnWall(GameObject wall)
        {
            ReturnPart(this.wallTemplet, wall);
        }

        public void ReturnHole(GameObject hole)
        {
            ReturnPart(this.holeTemplet, hole);
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
