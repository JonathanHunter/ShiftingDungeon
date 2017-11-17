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
        [SerializeField]
        private UI.EnemyHealth healthBar;
        [SerializeField]
        private Effects.Shake spriteShake;
        /// <summary> The number of particles emitted by the enemy upon death. </summary>
        [SerializeField]
        private int numDeathParticles = 100;

        /// <summary> The particle system to trigger when the enemy dies. </summary>
        private EnemyDeathParticles deathParticles = null;

        /// <summary> The type of this enemy. </summary>
        public Enums.EnemyTypes Type { get { return this.type; } }
        /// <summary> The health of this enemy. </summary>
        public int Health { get; protected set; }

        private void Start()
        {
            this.deathParticles = GetComponentInChildren<EnemyDeathParticles>();
        }

        private void Update()
        {
            if (Managers.GameState.Instance.IsPaused)
                return;

            LocalUpdate();
            if(this.Health <= 0)
            {
                this.Health = this.maxHealth;
                if (Random.Range(0f, 1f) < .6f)
                {
                    int count = Random.Range(1, 4);
                    for (int i = 0; i < count; i++)
                    {
                        GameObject gold = PickupPool.Instance.GetGold();
                        if (gold != null)
                        {
                            float valueChance = Random.Range(0f, 1f);
                            float value = 0;
                            if (valueChance < .25f)
                                value = .75f;
                            else if (valueChance < .50f)
                                value = .5f;
                            else
                                value = .25f;

                            gold.transform.position = this.transform.position;
                            gold.transform.localScale = new Vector3(value, value, 1);
                            Vector2 randForce = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
                            gold.GetComponent<Rigidbody2D>().AddForce(randForce, ForceMode2D.Impulse);
                        }
                    }
                }
                if (deathParticles)
                {
                    deathParticles.Emit(numDeathParticles);
                }

                Managers.DeathSoundPlayer.Instance.PlayDeathSound();
                EnemyPool.Instance.ReturnEnemy(this.type, this.gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if(collider.tag == Enums.Tags.HeroWeapon.ToString())
            {
                if(collider.gameObject.GetComponent<IDamageDealer>() != null)
                {
                    int temp = this.Health;
                    TakeDamage(collider.GetComponent<IDamageDealer>().GetDamage());
                    if (temp != this.Health)
                    {
                        this.healthBar.Percent = (this.Health / (float)this.maxHealth);
                        this.spriteShake.StartShake(2);
                    }
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
            this.healthBar.Percent = 1f;
            if (deathParticles)
            {
                deathParticles.ReInitialize();
            }
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
