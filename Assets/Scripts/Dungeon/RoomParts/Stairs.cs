namespace ShiftingDungeon.Dungeon.RoomParts
{
    using UnityEngine;
    using ObjectPooling;

    public class Stairs : MonoBehaviour, IPoolable
    {
        private int referenceIndex;

        public IPoolable SpawnCopy(int referenceIndex)
        {
            Stairs stairs = Instantiate<Stairs>(this);
            stairs.referenceIndex = referenceIndex;
            return stairs;
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == Util.Enums.Tags.Hero.ToString())
            {
                Managers.DungeonManager.TransitionMaps();
            }
        }
    }
}
