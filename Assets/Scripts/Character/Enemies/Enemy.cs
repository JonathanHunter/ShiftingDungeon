namespace ShiftingDungeon.Character.Enemies
{
    using UnityEngine;
    using ObjectPooling;
    using Util;
    using Weapons;

    public abstract class Enemy : MonoBehaviour, IPoolable, IDamageDealer
    {
        [SerializeField]
        private int damage = 1;
        [SerializeField]
        private int referenceIndex = 0;
        [SerializeField]
        private int maxHealth = 3;
        [SerializeField]
        private Enums.EnemyTypes type = Enums.EnemyTypes.Basic;

        /// <summary> The type of this enemy. </summary>
        public Enums.EnemyTypes Type { get { return this.type; } }
        /// <summary> The health of this enemy. </summary>
        public int Health { get; private set; }

        private void Update()
        {
            if (Managers.GameState.Instance.IsPaused)
                return;

            LocalUpdate();
            if(this.Health <= 0)
            {
                EnemyPool.Instance.ReturnEnemy(this.type, this.gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.collider.tag == Enums.Tags.HeroWeapon.ToString())
            {
                if(collision.collider.gameObject.GetComponent<IDamageDealer>() != null)
                {
                    this.Health -= collision.collider.gameObject.GetComponent<IDamageDealer>().GetDamage();
                }
            }

            LocalCollision(collision);
        }

        public IPoolable SpawnCopy(int referenceIndex)
        {
            Enemy enemy = Instantiate<Enemy>(this);
            enemy.referenceIndex = referenceIndex;
            return enemy;
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
            this.Health = maxHealth;
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
        /// <summary> Local Collision for subclasses. </summary>
        protected abstract void LocalCollision(Collision2D collision);
    }
}
