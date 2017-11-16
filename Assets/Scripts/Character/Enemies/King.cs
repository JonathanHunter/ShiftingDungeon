namespace ShiftingDungeon.Character.Enemies
{
    using System.Collections.Generic;
    using UnityEngine;

    public class King : Enemy
    {
        [SerializeField]
        private GameObject summoningPointsPrefab;
        [SerializeField]
        private SpriteRenderer[] sprites;
        [SerializeField]
        private Collider2D weaponCollider;
        [SerializeField]
        private GameObject shield;
        [SerializeField]
        private Weapons.Gun gun;
        [SerializeField]
        private float invulnerabilityTime = .5f;
        [SerializeField]
        private float flashInterval = .002f;
        [SerializeField]
        private float stunTime = 2f;
        [SerializeField]
        private float idleTime = .5f;
        [SerializeField]
        private float meleeRange = .5f;
        [SerializeField]
        private float moveSpeed = 2.5f;
        [SerializeField]
        private Util.SoundPlayer sfx;
        [SerializeField]
        private Animator anim;

        private enum KingState { Idle, Move, Attack, Shield, Stun, Shoot }

        private Rigidbody2D rgbdy;
        private KingState currentState = KingState.Idle;
        private SummoningField[] summoningPoints;
        private Transform hero;
        private List<GameObject> spawnedEnemies;
        private Vector3 movementLocation;
        private bool doOnce = false;
        private bool isInvulnerable = false;
        private bool isVisible = true;
        private float invunTimer = 0;
        private float flashTimer = 0;
        private float stunTimer = 0;
        private float idleTimer = 0;
        private float animationTime = 0f;
        private int meleeHash = 0;
        private int meleeOverHash = 0;
        private int summonHash = 0;
        private int summonOverHash = 0;

        protected override void LocalInitialize()
        {
            if (this.anim == null)
                this.anim = this.gameObject.GetComponent<Animator>();

            this.rgbdy = GetComponent<Rigidbody2D>();
            this.hero = Managers.DungeonManager.GetHero().transform;
            this.spawnedEnemies = new List<GameObject>();
            this.doOnce = false;
            this.isInvulnerable = false;
            this.isVisible = true;
            this.invunTimer = 0;
            this.flashTimer = 0;
            this.meleeHash = Animator.StringToHash("Melee");
            this.meleeOverHash = Animator.StringToHash("MeleeOver");
            this.summonHash = Animator.StringToHash("Summon");
            this.summonOverHash = Animator.StringToHash("SummonOver");
            this.gun.Init(8);
        }

        protected override void LocalReInitialize()
        {
            this.shield.SetActive(false);
            GameObject points = Instantiate(this.summoningPointsPrefab);
            points.transform.position = Vector3.zero;
            points.transform.rotation = Quaternion.identity;
            this.summoningPoints = points.GetComponentsInChildren<SummoningField>();
            this.doOnce = false;
            this.isInvulnerable = false;
            this.isVisible = true;
            this.invunTimer = 0;
            this.flashTimer = 0;
            foreach (SpriteRenderer b in this.sprites)
                b.enabled = isVisible;
        }

        protected override void LocalUpdate()
        {
            if (invunTimer > 0)
            {
                if (flashTimer > flashInterval)
                {
                    isVisible = !isVisible;
                    flashTimer = 0;
                    foreach (SpriteRenderer b in this.sprites)
                        b.enabled = isVisible;
                }
                flashTimer += Time.deltaTime;
                invunTimer -= Time.deltaTime;
            }
            else if (!isVisible || isInvulnerable)
            {
                isVisible = true;
                foreach (SpriteRenderer b in this.sprites)
                    b.enabled = true;
                isInvulnerable = false;
            }

            KingState temp = this.currentState;
            this.animationTime = this.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
            switch (this.currentState)
            {
                case KingState.Idle: Idle(); break;
                case KingState.Move: Move(); break;
                case KingState.Attack: Attack(); break;
                case KingState.Shield: Shield(); break;
                case KingState.Stun: Stun(); break;
                case KingState.Shoot: Shoot(); break;
            }

            if (temp != currentState)
            {
                this.doOnce = false;
            }
        }

        protected override void LocalDeallocate()
        {
            foreach (GameObject g in this.spawnedEnemies)
                ObjectPooling.EnemyPool.Instance.ReturnEnemy(g.GetComponent<Enemy>().Type, g);

            this.spawnedEnemies.Clear();
            if (this.summoningPoints != null)
            {
                Destroy(this.summoningPoints[0].transform.parent.gameObject);
                this.summoningPoints = null;
            }
        }

        protected override void LocalDelete()
        {
        }

        protected override void TakeDamage(int damage)
        {
            if (!this.isInvulnerable && this.currentState != KingState.Shield)
            {
                this.isInvulnerable = true;
                this.Health -= damage;
                this.invunTimer = this.invulnerabilityTime;
                this.sfx.PlaySong(1);
            }
        }

        protected override void LocalCollision(Collider2D collision)
        {
        }

        private void Idle()
        {
            if (!this.doOnce)
            {
                this.idleTimer = idleTime;
                this.doOnce = true;
            }

            if (Vector2.Distance(this.hero.position, this.transform.position) < this.meleeRange)
            {
                this.anim.SetTrigger(this.meleeHash);
                this.currentState = KingState.Attack;
            }

            if ((this.idleTimer -= Time.deltaTime) < 0)
            {
                float val = Random.Range(0f, 1f);
                if (val < .10f)
                {
                    this.anim.SetTrigger(this.summonHash);
                    this.currentState = KingState.Shield;
                }
                else if (val < .5f)
                {
                    this.currentState = KingState.Move;
                }
                else if (val < .75f)
                {
                    this.anim.SetTrigger(this.summonHash);
                    this.currentState = KingState.Shoot;
                }
                else
                    this.doOnce = false;
            }
        }

        private void Move()
        {
            if (!this.doOnce)
            {
                int val = Random.Range(0, this.summoningPoints.Length + 1);
                if (val >= this.summoningPoints.Length)
                    this.movementLocation = this.hero.position;
                else
                    this.movementLocation = this.summoningPoints[val].transform.position;

                LookAt(this.movementLocation);
                this.doOnce = true;
            }

            if (Vector2.Distance(this.movementLocation, this.transform.position) < .1f)
            {
                LookAt(this.hero.position);
                this.rgbdy.velocity = Vector2.zero;
                this.currentState = KingState.Idle;
            }
            else
            {
                LookAt(this.movementLocation);
                this.rgbdy.velocity = -this.transform.up * this.moveSpeed;
            }
        }

        private void Attack()
        {
            if (!this.doOnce)
            {
                LookAt(this.hero.position);
                this.weaponCollider.enabled = true;
                this.doOnce = true;
            }

            if(this.animationTime > .9f)
            {
                this.weaponCollider.enabled = false;
                this.anim.SetTrigger(meleeOverHash);
                this.currentState = KingState.Idle;
            }
        }

        private void Shield()
        {
            if (!this.doOnce)
            {
                this.sfx.PlaySong(0);
                LookAt(this.hero.position);
                foreach (SummoningField s in this.summoningPoints)
                {
                    GameObject e = s.Spawn();
                    if (e != null)
                        this.spawnedEnemies.Add(e);
                }

                this.shield.SetActive(true);
                this.doOnce = true;
            }

            bool allDead = true;
            foreach (GameObject e in this.spawnedEnemies)
                if (e.gameObject.activeInHierarchy)
                    allDead = false;

            if (allDead)
            {
                this.shield.SetActive(false);
                this.spawnedEnemies.Clear();
                this.anim.SetTrigger(this.summonOverHash);
                this.currentState = KingState.Stun;
            }
        }

        private void Stun()
        {
            if (!this.doOnce)
            {
                this.stunTimer = this.stunTime;
                this.weaponCollider.enabled = false;
                this.doOnce = true;
            }

            if ((this.stunTimer -= Time.deltaTime) < 0)
            {
                this.currentState = KingState.Idle;
            }
        }

        private void Shoot()
        {
            if(!this.doOnce)
            {
                LookAt(this.hero.position);
                this.gun.ReInit();
                this.doOnce = true;
            }

            if(this.gun.WeaponUpdate())
            {
                this.currentState = KingState.Idle;
                this.gun.CleanUp();
                this.anim.SetTrigger(this.summonOverHash);
                this.currentState = KingState.Idle;
            }
        }

        private void LookAt(Vector3 pos)
        {
            float angle = Vector2.SignedAngle(Vector2.down, (pos - this.transform.position).normalized);
            this.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
