namespace ShiftingDungeon.Character.Enemies
{
    using UnityEngine;
    using Util;

    public class MeleeEnemy : Enemy
    {
        [SerializeField]
        private float agroRange = 3f;
        [SerializeField]
        private float meleeDist = .3f;
        [SerializeField]
        private float walkSpeed = 2f;
        [SerializeField]
        private float fleeTime = 1f;
        [SerializeField]
        private float stunLength = 0.15f;
        [SerializeField]
        private Weapons.Weapon weapon = null;
        [SerializeField]
        private SoundPlayer sfx = null;
        [SerializeField]
        private ParticleSystem smoke = null;
        [SerializeField]
        private Animator anim;
        [SerializeField]
        private int weaponLevel;
        [SerializeField]
        [Range(0f, 1f)]
        private float trackingError;

        private enum State { idle, seek, attack, run }

        private Transform hero;
        private Rigidbody2D rgbdy;
        private float fleeCounter;
        private float stunCounter;
        private bool doOnce;
        private int hitHash;
        private State curr;

        protected override void LocalInitialize()
        {
            if (this.anim == null)
                this.anim = this.gameObject.GetComponent<Animator>();

            this.hero = Managers.DungeonManager.GetHero().transform;
            this.rgbdy = this.gameObject.GetComponent<Rigidbody2D>();
            this.hitHash = Animator.StringToHash("Hit");
            this.weapon.Init(this.weaponLevel);
            this.weapon.CleanUp();
        }

        protected override void LocalReInitialize()
        {
            this.fleeCounter = 0f;
            this.doOnce = false;
            this.curr = State.idle;
            if (!this.smoke.isPlaying)
                this.smoke.Play();
        }

        protected override void LocalUpdate()
        {
            if ((this.stunCounter -= Time.deltaTime) > 0)
                return;

            State temp = this.curr;
            switch(this.curr)
            {
                case State.idle: Idle(); break;
                case State.seek: Seek(); break;
                case State.attack: Attack(); break;
                case State.run: Run(); break;
            }

            if(this.curr != temp)
            {
                this.doOnce = false;
            }
        }

        protected override void LocalDeallocate()
        {
            if(this.smoke.isPlaying)
                this.smoke.Stop();

            this.weapon.CleanUp();
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

            if (collision.tag == Enums.Tags.HeroWeapon.ToString())
            {
                this.anim.SetTrigger(hitHash);
                this.stunCounter = stunLength;
                sfx.PlaySongModPitch(0, .1f);
            }
        }

        private void RotateToWherePlayerWas()
        {
            Vector2 heroVelocity = this.hero.GetComponent<Rigidbody2D>().velocity;
            float deltaT = (this.transform.position - this.hero.position).magnitude / (heroVelocity - this.walkSpeed * (Vector2)this.transform.right).magnitude;
            Vector2 futurePlayerPos = (Vector2)hero.position - heroVelocity * deltaT * (1 - this.trackingError);
            Vector2 towards = futurePlayerPos - (Vector2)this.transform.position;
            this.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, towards));
        }

        private void Idle()
        {
            if (Vector2.Distance(this.transform.position, this.hero.position) < this.agroRange)
                this.curr = State.seek;
        }

        private void Seek()
        {
            if (Vector2.Distance(this.transform.position, this.hero.position) < this.meleeDist)
                this.curr = State.attack;
            else
            {
                RotateToWherePlayerWas();
                this.rgbdy.velocity = transform.right * this.walkSpeed;
            }
        }

        private void Attack()
        {
            if (!this.doOnce)
            {
                this.weapon.ReInit();
                this.doOnce = true;
            }

            if (this.weapon.WeaponUpdate())
            {
                this.weapon.CleanUp();
                this.curr = State.run;
            }
        }

        private void Run()
        {
            if(!this.doOnce)
            {
                this.fleeCounter = this.fleeTime;
                this.doOnce = true;
            }

            if ((this.fleeCounter -= Time.deltaTime) <= 0)
            {
                this.curr = State.idle;
            }
            else
            {
                this.rgbdy.velocity = -this.transform.right * this.walkSpeed *2f;
            }
        }
    }
}
