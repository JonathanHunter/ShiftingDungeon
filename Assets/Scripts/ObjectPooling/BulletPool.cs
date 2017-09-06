namespace ShiftingDungeon.ObjectPooling
{
    using UnityEngine;
    using Character.Weapons.Bullets;
    using Util;

    public class BulletPool : ObjectPool
    {
        [SerializeField]
        private Bullet[] playerBulletPools;
        [SerializeField]
        private int[] playerBulletPoolSizes;

        /// <summary> Singleton instance for this object pool. </summary>
        public static BulletPool Instance { get; private set; }

        protected override void PreInit()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Duplicate Bullet Pool detected: removing " + this.gameObject.name);
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
        }

        protected override IPoolable[] GetTemplets()
        {
            return this.playerBulletPools;
        }

        protected override int[] GetPoolSizes()
        {
            return this.playerBulletPoolSizes;
        }

        public GameObject GetBullet(Enums.BulletTypes type)
        {
            IPoolable entity = AllocateEntity(playerBulletPools[(int)type]);
            if (entity == null)
                return null;

            return entity.GetGameObject();
        }

        public void ReturnBullet(Enums.BulletTypes type, GameObject bullet)
        {
            IPoolable entity = bullet.GetComponent<IPoolable>();
            DeallocateEntity(playerBulletPools[(int)type], entity);
        }
    }
}
