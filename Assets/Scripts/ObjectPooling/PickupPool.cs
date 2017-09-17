namespace ShiftingDungeon.ObjectPooling
{
    using UnityEngine;
    using Character.Pickups;

    public class PickupPool : ObjectPool
    {
        [SerializeField]
        private Money goldTemplet = null;
        [SerializeField]
        private int goldPoolSize = 4;

        /// <summary> Singleton instance for this object pool. </summary>
        public static PickupPool Instance { get; private set; }

        protected override void PreInit()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Duplicate Pickup Pool detected: removing " + this.gameObject.name);
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
        }

        protected override IPoolable[] GetTemplets()
        {
            return new IPoolable[] { this.goldTemplet };
        }

        protected override int[] GetPoolSizes()
        {
            return new int[] { this.goldPoolSize };
        }

        public GameObject GetGold()
        {
            return GetPart(this.goldTemplet);
        }

        public void ReturnGold(GameObject gold)
        {
            ReturnPart(this.goldTemplet, gold);
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
