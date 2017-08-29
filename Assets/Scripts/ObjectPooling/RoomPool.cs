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

        /// <summary> Singleton instance for this object pool. </summary>
        public static RoomPool Instance { get; private set; }

        protected new void Start()
        {
            if(Instance != null && Instance != this)
            {
                Debug.LogError("Duplicate Room Pool detected: removing " + this.gameObject.name);
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            base.Start();
        }

        protected override IPoolable[] GetTemplets()
        {
            return new IPoolable[] { this.doorTemplet, this.floorTemplet, this.wallTemplet, this.holeTemplet };
        }

        protected override int[] GetPoolSizes()
        {
            return new int[] { this.doorPoolSize, this.floorPoolSize, this.wallPoolSize, this.holePoolSize };
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
