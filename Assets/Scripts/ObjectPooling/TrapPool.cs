namespace ShiftingDungeon.ObjectPooling
{
    using Character.Traps;
    using Util;
    using UnityEngine;

    public class TrapPool : ObjectPool
    {
        [SerializeField]
        private Trap[] trapPools;
        [SerializeField]
        private int[] trapPoolSizes;

        public static TrapPool Instance { get; private set; }

        protected override void PreInit()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Duplicate Trap Pool detected: removing " + this.gameObject.name);
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
        }

        protected override IPoolable[] GetTemplets()
        {
            return this.trapPools;
        }

        protected override int[] GetPoolSizes()
        {
            return this.trapPoolSizes;
        }

        public GameObject GetTrap(Enums.Traps type)
        {
            IPoolable entity = AllocateEntity(trapPools[(int)type]);
            if (entity == null)
                return null;

            return entity.GetGameObject();
        }

        public void ReturnTrap(Enums.Traps type, GameObject trap)
        {
            IPoolable entity = trap.GetComponent<IPoolable>();
            DeallocateEntity(trapPools[(int)type], entity);
        }

    }

}
