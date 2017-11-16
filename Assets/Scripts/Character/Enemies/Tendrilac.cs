namespace ShiftingDungeon.Character.Enemies
{
    using System.Collections.Generic;
    using UnityEngine;
    public class Tendrilac : Enemy
    {
        [SerializeField]
        private GameObject summoningPointsPrefab;
        [SerializeField]
        private float attackRange = .5f;
        [SerializeField]
        private float attackTime = 1.5f;
        [SerializeField]
        private float coolDownTime = 3f;
        [SerializeField]
        private float idleTime = 5f;
        [SerializeField]
        private float moveTime = 3f;
        [SerializeField]
        private Util.SoundPlayer sfx;
        [SerializeField]
        private Animator anim;
        [SerializeField]
        private GameObject sprites;

        private enum TendrilacState { Idle, Move, Attack, Stun }

        private Rigidbody2D rgbdy;
        private TendrilacState currentState = TendrilacState.Idle;
        private Hero.StateMap<TendrilacState> stateMap;
        private Transform hero;
        private Vector3 movementLocation;
        private SummoningField[] spawnPoints;
        private List<GameObject> spawnedEnemies;
        private float idleTimer = 0;
        private float moveTimer = 0;
        private float attackTimer = 0;
        private float cooldownTimer = 0;
        private int moveHash;
        private int attackHash;
        private int recentDamage;
        private bool doOnce = false;


        protected override void LocalInitialize()
        {
            if (this.anim == null)
                this.anim = this.gameObject.GetComponent<Animator>();

            this.rgbdy = GetComponent<Rigidbody2D>();
            this.hero = Managers.DungeonManager.GetHero().transform;
            this.stateMap = new Hero.StateMap<TendrilacState>();
            this.spawnedEnemies = new List<GameObject>();
            this.idleTimer = idleTime;
            this.moveHash = Animator.StringToHash("Moving");
            this.attackHash = Animator.StringToHash("Attack");
        }

        protected override void LocalReInitialize()
        {
            this.idleTimer = idleTime;
            this.moveTimer = 0f;
            this.cooldownTimer = 0f;
            this.attackTimer = 0f;
            this.doOnce = false;
            GameObject points = Instantiate(this.summoningPointsPrefab);
            points.transform.position = Vector3.zero;
            points.transform.rotation = Quaternion.identity;
            this.spawnPoints = points.GetComponentsInChildren<SummoningField>();
        }

        protected override void LocalUpdate()
        {
            TendrilacState temp = this.currentState;
            this.currentState = this.stateMap.GetState(this.anim.GetCurrentAnimatorStateInfo(0).fullPathHash);
            if (temp != currentState)
            {
                this.doOnce = false;
            }
            
            print(currentState.ToString());
            switch (this.currentState)
            {
                case TendrilacState.Idle: Idle(); break;
                case TendrilacState.Move: Move(); break;
                case TendrilacState.Attack: Attack(); break;
            }

            if ((this.cooldownTimer -= Time.deltaTime) < 0
                && Vector2.Distance(this.hero.position, this.transform.position) < this.attackRange)
            {
                if (this.currentState == TendrilacState.Idle)
                {
                    this.anim.SetTrigger(this.attackHash);
                }
            }

            bool allDead = true;
            foreach (GameObject e in this.spawnedEnemies)
                if (e.gameObject.activeInHierarchy)
                    allDead = false;

            if(allDead)
            {
                this.spawnedEnemies.Clear();
            }
        }

        protected override void LocalDeallocate()
        {
            foreach (GameObject g in this.spawnedEnemies)
                ObjectPooling.EnemyPool.Instance.ReturnEnemy(g.GetComponent<Enemy>().Type, g);
            
            this.spawnedEnemies.Clear();
            if (this.spawnPoints != null)
            {
                Destroy(this.spawnPoints[0].transform.parent.gameObject);
                this.spawnPoints = null;
            }
        }

        protected override void LocalDelete()
        {
        }

        protected override void TakeDamage(int damage)
        {
            this.Health -= damage;
            if ((recentDamage += damage) >= 3)
                this.anim.SetBool(this.moveHash, true);
            // this.sfx.PlaySong(1);
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

            if ((this.idleTimer -= Time.deltaTime) < 0)
            {
                float val = Random.Range(0f, 1f);
                if (val <= .85f) 
                    this.anim.SetBool(this.moveHash, true);
                else 
                    this.idleTimer = idleTime;
            }
        }

        private void Move()
        {
            if (!this.doOnce)
            {
                int val = Random.Range(this.spawnPoints.Length, this.spawnPoints.Length + 4);
                if (val >= this.spawnPoints.Length) 
                    this.movementLocation = this.hero.position;
                else 
                    this.movementLocation = this.spawnPoints[val].transform.position;
                LookAt(this.movementLocation);
                foreach (SpriteRenderer s in sprites.GetComponentsInChildren<SpriteRenderer>())
                    s.enabled = false;
                this.GetComponent<Collider2D>().enabled = false;
                this.doOnce = true;
                this.moveTimer = this.moveTime;
            }

            if ((this.moveTimer -= Time.deltaTime) < 0
                && Vector2.Distance(this.movementLocation, this.transform.position) < .1f) {
                LookAt(this.hero.position);
                foreach (SpriteRenderer s in sprites.GetComponentsInChildren<SpriteRenderer>())
                    s.enabled = true;
                this.anim.SetBool(this.moveHash, false);
                this.GetComponent<Collider2D>().enabled = true;
            } else
                this.transform.position = movementLocation;
        }

        private void Attack()
        {
            if (!this.doOnce)
            {
                this.attackTimer = this.attackTime;
                this.cooldownTimer = this.coolDownTime;
                this.doOnce = true;
                foreach (SpriteRenderer s in sprites.GetComponentsInChildren<SpriteRenderer>())
                    s.enabled = true;

                foreach (SummoningField s in this.spawnPoints)
                {
                    GameObject e = s.Spawn();
                    if(e != null)
                        this.spawnedEnemies.Add(e);
                }
            }

            if ((this.attackTimer -= Time.deltaTime) < 0) 
            {
                // Doesnt actually run the first time for some reason
                // Ends up in a double attack, doesnt move the player but the
                // cooldown starts
                this.hero.position = this.transform.position + -this.transform.up * 3;
                this.cooldownTimer = this.coolDownTime;
            } else 
                this.hero.position = this.transform.position;
        }

        private void LookAt(Vector3 pos)
        {
            float angle = Vector2.SignedAngle(Vector2.down, (pos - this.transform.position).normalized);
            this.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}