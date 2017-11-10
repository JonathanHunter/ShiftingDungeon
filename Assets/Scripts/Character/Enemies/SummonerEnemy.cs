namespace ShiftingDungeon.Character.Enemies
{
    using System.Collections.Generic;
    using UnityEngine;

    public class SummonerEnemy : Enemy
    {
        [SerializeField]
        private SummoningField[] spawners = null;
        [SerializeField]
        private int summoningRange = 5;
        [SerializeField]
        private float scaredTime = .5f;
        [SerializeField]
        private float stunLength = 0.15f;
        [SerializeField]
        private float idleTime = 1f;
        [SerializeField]
        private float moveSpeed = 3f;
        [SerializeField]
        private Util.SoundPlayer sfx;
        [SerializeField]
        private Animator anim;

        private enum State { idle, summon, scared, wander }

        private List<GameObject> spawnedEnemies;
        private State curr;
        private Transform hero;
        private Rigidbody2D rgbdy;
        private float stunCounter;
        private float scaredCounter;
        private float idleCounter;
        private float animationTime;
        private int summonHash;
        private bool doOnce;
        private bool spawned;
        
        protected override void LocalInitialize()
        {
            foreach (SummoningField f in this.spawners)
                f.gameObject.SetActive(false);

            this.spawnedEnemies = new List<GameObject>();
            this.curr = State.idle;
            this.hero = Managers.DungeonManager.GetHero().transform;
            this.rgbdy = this.GetComponent<Rigidbody2D>();
            this.summonHash = Animator.StringToHash("summon");
        }

        protected override void LocalReInitialize()
        {
            this.spawnedEnemies.Clear();
            this.curr = State.idle;
            this.stunCounter = 0f;
            this.scaredCounter = 0f;
            this.idleCounter = this.idleTime;
            this.doOnce = false;
            this.spawned = false;
        }

        protected override void LocalUpdate()
        {
            if (this.stunCounter > 0)
                this.stunCounter -= Time.deltaTime;

            this.animationTime = this.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
            State temp = this.curr;
            switch (this.curr)
            {
                case State.idle: Idle(); break;
                case State.summon: Summon(); break;
                case State.scared: Scared(); break;
                case State.wander: Wander(); break;
            }

            if(temp != this.curr)
            {
                this.doOnce = false;
            }
        }

        protected override void LocalDeallocate()
        {
            foreach (GameObject g in this.spawnedEnemies)
                ObjectPooling.EnemyPool.Instance.ReturnEnemy(g.GetComponent<Enemy>().Type, g);

            this.spawnedEnemies.Clear();
        }

        protected override void LocalDelete()
        {
        }

        protected override void TakeDamage(int damage)
        {
            if (this.stunCounter > 0)
                return;

            this.Health -= damage;
        }

        protected override void LocalCollision(Collider2D collision)
        {
            if (this.stunCounter > 0)
                return;

            if (collision.tag == Util.Enums.Tags.HeroWeapon.ToString())
            {
                this.stunCounter = stunLength;
                sfx.PlaySongModPitch(0, .1f);
            }
        }

        private void Idle()
        {
            float dist = Vector2.Distance(this.transform.position, this.hero.position);
            if(dist < this.summoningRange)
            {
                bool allDead = true;
                foreach (GameObject e in this.spawnedEnemies)
                    if (e.gameObject.activeInHierarchy)
                        allDead = false;

                if (allDead)
                {
                    this.spawnedEnemies.Clear();
                    this.curr = State.summon;
                    this.anim.SetTrigger(this.summonHash);
                }
            }

            if(this.curr == State.idle)
            {
                if((this.idleCounter -= Time.deltaTime) < 0)
                {
                    this.idleCounter = this.idleTime;
                    this.curr = State.wander;
                }
            }
        }

        private void Summon()
        {
            if(!this.doOnce)
            {
                foreach (SummoningField f in this.spawners)
                    f.gameObject.SetActive(true);
                
                this.spawned = false;
                this.doOnce = true;
            }

            if (this.animationTime >= .5f && !this.spawned)
            {
                this.sfx.PlaySong(0);
                foreach (SummoningField s in this.spawners)
                {
                    GameObject e = s.Spawn();
                    if (e != null)
                        this.spawnedEnemies.Add(e);
                }

                this.spawned = true;
            }

            if(this.animationTime > .9)
            {
                foreach (SummoningField f in this.spawners)
                    f.gameObject.SetActive(false);

                this.spawned = false;
                this.curr = State.scared;
            }
        }

        private void Scared()
        {
            if(!this.doOnce)
            {
                this.scaredCounter = this.scaredTime;
                this.doOnce = true;
            }
            
            if ((this.scaredCounter -= Time.deltaTime) <= 0)
                this.curr = State.idle;

            Vector2 towards = this.hero.transform.position - this.transform.position;
            this.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, -towards));
            this.rgbdy.velocity = this.transform.right * this.moveSpeed;
        }

        private void Wander()
        {
            float dir = Random.Range(0, 360);
            this.transform.rotation = Quaternion.Euler(0, 0, dir);
            this.rgbdy.AddForce(this.transform.right * this.moveSpeed * 5f, ForceMode2D.Impulse);
            this.curr = State.idle;
        }
    }
}
