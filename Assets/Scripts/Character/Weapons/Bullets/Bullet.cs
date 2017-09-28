namespace ShiftingDungeon.Character.Weapons.Bullets
{
    using UnityEngine;
    using ObjectPooling;
    using Util;

    public abstract class Bullet : MonoBehaviour, IPoolable, IDamageDealer
    {
        [SerializeField]
        private int damage = 1;
        [SerializeField]
        private float lifeTime = 1;
        [SerializeField]
        private Enums.BulletTypes type = Enums.BulletTypes.HeroBasic;
        
        private int referenceIndex = 0;
        private float currentLifeTime = 0;

        private void Update()
        {
            LocalUpdate();
            if ((this.currentLifeTime -= Time.deltaTime) <= 0)
            {
                ReturnBullet();
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            this.currentLifeTime = 0;
        }

        public IPoolable SpawnCopy(int referenceIndex)
        {
            Bullet bullet = Instantiate<Bullet>(this);
            bullet.referenceIndex = referenceIndex;
            return bullet;
        }

        public GameObject GetGameObject()
        {
            return this.gameObject;
        }

        public int GetReferenceIndex()
        {
            return this.referenceIndex;
        }

        public void Initialize()
        {
            LocalInitialize();
        }

        public void ReInitialize()
        {
            LocalReInitialize();
            this.currentLifeTime = this.lifeTime;
            this.gameObject.SetActive(true);
        }

        public void Deallocate()
        {
            LocalDeallocate();
            this.gameObject.SetActive(false);
        }

        public void Delete()
        {
            LocalDelete();
            Destroy(this.gameObject);
        }

        public int GetDamage()
        {
            return this.damage;
        }

        protected void ReturnBullet()
        {
            BulletPool.Instance.ReturnBullet(this.type, this.gameObject);
        }

        /// <summary> Local Update for subclasses. </summary>
        protected abstract void LocalUpdate();
        /// <summary> Local Initialize for subclasses. </summary>
        protected abstract void LocalInitialize();
        /// <summary> Local ReInitialize for subclasses. </summary>
        protected abstract void LocalReInitialize();
        /// <summary> Local Deallocate for subclasses. </summary>
        protected abstract void LocalDeallocate();
        /// <summary> Local Delete for subclasses. </summary>
        protected abstract void LocalDelete();
    }
}
