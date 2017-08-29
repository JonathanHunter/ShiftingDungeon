namespace ShiftingDungeon.Dungeon.RoomParts
{
    using UnityEngine;
    using ObjectPooling;

    public class StaticRoomPiece : MonoBehaviour, IPoolable
    {
        private int referenceIndex;

        public IPoolable SpawnCopy(int referenceIndex)
        {
            StaticRoomPiece wall = Instantiate<StaticRoomPiece>(this);
            wall.referenceIndex = referenceIndex;
            return wall;
        }

        public int GetReferenceIndex()
        {
            return this.referenceIndex;
        }

        public GameObject GetGameObject()
        {
            return this.gameObject;
        }

        public void Initialize()
        {
        }

        public void ReInitialize()
        {
            this.gameObject.SetActive(true);
        }

        public void Deallocate()
        {
            this.gameObject.SetActive(false);
        }

        public void Delete()
        {
            Destroy(this.gameObject);
        }
    }
}
