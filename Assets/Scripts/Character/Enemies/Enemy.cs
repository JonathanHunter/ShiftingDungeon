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
        private Enums.EnemyTypes type;

        /// <summary> The type of this enemy. </summary>
        public Enums.EnemyTypes Type { get { return this.type; } }
        /// <summary> The health of this enemy. </summary>
        public int Health { get; protected set; }

        private void Update()
        {
            if (Managers.GameState.Instance.IsPaused)
                return;

            LocalUpdate();
            if(this.Health <= 0)
            {
                this.Health = this.maxHealth;
                if (Random.Range(0f, 1f) < .5f)
                {
                    int count = Random.Range(1, 4);
                    for (int i = 0; i < count; i++)
                    {
                        GameObject gold = PickupPool.Instance.GetGold();
                        int value = Random.Range(1, 4);
                        gold.transform.position = this.transform.position;
                        gold.transform.localScale = new Vector3(value, value, 1);
                        Vector2 randForce = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
                        gold.GetComponent<Rigidbody2D>().AddForce(randForce, ForceMode2D.Impulse);
                    }
                }

                EnemyPool.Instance.ReturnEnemy(this.type, this.gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if(collider.tag == Enums.Tags.HeroWeapon.ToString())
            {
                if(collider.gameObject.GetComponent<IDamageDealer>() != null)
                {
                    TakeDamage(collider.GetComponent<IDamageDealer>().GetDamage());
                }
            }

            LocalCollision(collider);
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
        protected abstract void LocalCollision(Collider2D collision);
        /// <summary> Take Damage for subclasses. </summary>
        protected abstract void TakeDamage(int damage);
    }
}
