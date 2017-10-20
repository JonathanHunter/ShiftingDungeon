namespace ShiftingDungeon.Character.Enemies
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Exectioner : Enemy
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

        private enum ExectionerState { Idle, Move, Attack, Shield, Stun }

        private Animator anim;
        private Rigidbody2D rgbdy;
        private Hero.StateMap<ExectionerState> stateMap;
        private ExectionerState currentState = ExectionerState.Idle;
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
        private int movingHash = 0;
        private int shieldingHash = 0;
        private int attackHash = 0;
        private int stunOverHash = 0;

        protected override void LocalInitialize()
        {
            this.anim = GetComponent<Animator>();
            this.rgbdy = GetComponent<Rigidbody2D>();
            this.stateMap = new Hero.StateMap<ExectionerState>();
            this.hero = Managers.DungeonManager.GetHero().transform;
            this.spawnedEnemies = new List<GameObject>();
            this.doOnce = false;
            this.isInvulnerable = false;
            this.isVisible = true;
            this.invunTimer = 0;
            this.flashTimer = 0;
            this.movingHash = Animator.StringToHash("Moving");
            this.shieldingHash = Animator.StringToHash("Shielding");
            this.attackHash = Animator.StringToHash("Attack");
            this.stunOverHash = Animator.StringToHash("StunOver");
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

            ExectionerState temp = this.currentState;
            this.currentState = this.stateMap.GetState(this.anim.GetCurrentAnimatorStateInfo(0).fullPathHash);
            if (temp != currentState)
            {
                this.doOnce = false;
                this.weaponCollider.enabled = true;
            }

            switch (this.currentState)
            {
                case ExectionerState.Idle: Idle(); break;
                case ExectionerState.Move: Move(); break;
                case ExectionerState.Attack: Attack(); break;
                case ExectionerState.Shield: Shield(); break;
                case ExectionerState.Stun: Stun(); break;
            }

            if (Vector2.Distance(this.hero.position, this.transform.position) < this.meleeRange)
            {
                if (this.currentState == ExectionerState.Idle || this.currentState == ExectionerState.Move)
                {
                    this.anim.SetTrigger(this.attackHash);
                    this.anim.SetBool(this.movingHash, false);
                }
            }
        }

        protected override void LocalDeallocate()
        {
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
            if (!this.isInvulnerable && this.currentState != ExectionerState.Shield)
            {
                this.isInvulnerable = true;
                this.Health -= damage;
                this.invunTimer = this.invulnerabilityTime;
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

            if ((this.idleTimer -= Time.deltaTime) < 0)
            {
                float val = Random.Range(0f, 1f);
                if (val < .25f)
                    this.anim.SetBool(this.movingHash, true);
                else if (val < .50f)
                    this.anim.SetBool(this.shieldingHash, true);
                else
                    this.doOnce = false;
            }
        }

        private void Move()
        {
            if(!this.doOnce)
            {
                int val = Random.Range(0, this.summoningPoints.Length + 4);
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
                this.anim.SetBool(this.movingHash, false);
            }
            else
                this.rgbdy.velocity = -this.transform.up * this.moveSpeed;
        }

        private void Attack()
        {
            if (!this.doOnce)
            {
                LookAt(this.hero.position);
                this.doOnce = true;
            }
        }

        private void Shield()
        {
            if(!this.doOnce)
            {
                LookAt(this.hero.position);
                foreach (SummoningField s in this.summoningPoints)
                {
                    GameObject e = s.Spawn();
                    if(e != null)
                        this.spawnedEnemies.Add(e);
                }

                this.shield.SetActive(true);
                this.doOnce = true;
            }

            bool allDead = true;
            foreach (GameObject e in this.spawnedEnemies)
                if (e.gameObject.activeInHierarchy)
                    allDead = false;

            if(allDead)
            {
                this.shield.SetActive(false);
                this.spawnedEnemies.Clear();
                this.anim.SetBool(this.shieldingHash, false);
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
                this.anim.SetTrigger(this.stunOverHash);
            }
        }

        private void LookAt(Vector3 pos)
        {
            float angle = Vector2.SignedAngle(Vector2.down, (pos - this.transform.position).normalized);
            this.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
