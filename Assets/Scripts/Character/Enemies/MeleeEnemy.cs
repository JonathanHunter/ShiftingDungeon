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
        private float timeBetweenSwings = 1f;
        [SerializeField]
        private float stunLength = 0.15f;
        [SerializeField]
        private Weapons.Weapon weapon = null;
        [SerializeField]
        private SoundPlayer sfx = null;
        [SerializeField]
        private ParticleSystem smoke = null;

        private Transform hero;
        private Rigidbody2D rgbdy;
        private Animator anim;
        private float swingTimer;
        private float stunCounter;
        private bool doOnce;
        private bool doneSwinging;
        private int hitHash;

        protected override void LocalInitialize()
        {
            this.hero = Managers.DungeonManager.GetHero().transform;
            this.rgbdy = this.gameObject.GetComponent<Rigidbody2D>();
            this.anim = this.gameObject.GetComponent<Animator>();
            this.hitHash = Animator.StringToHash("Hit");
            this.weapon.Init();
            this.weapon.CleanUp();
        }

        protected override void LocalReInitialize()
        {
            this.swingTimer = 0f;
            this.doneSwinging = true;
            this.doOnce = false;
            if(!this.smoke.isPlaying)
                this.smoke.Play();
        }

        protected override void LocalUpdate()
        {
            if ((this.stunCounter -= Time.deltaTime) > 0)
                return;
            float distance = Vector2.Distance(this.hero.transform.position, this.transform.position);
            if (distance > this.agroRange)
                return;

            if (distance <= meleeDist)
                this.swingTimer -= Time.deltaTime;
            else if (this.doneSwinging)
            {
                RotateToPlayer();
                this.rgbdy.velocity = this.transform.right * this.walkSpeed;
            }

            if (this.swingTimer < 0)
            {
                Attack();
                if (this.doneSwinging)
                {
                    this.swingTimer = this.timeBetweenSwings;
                    this.doOnce = false;
                }
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

        private void RotateToPlayer()
        {
            Vector2 towards = this.hero.transform.position - this.transform.position;
            this.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, towards));
        }

        private void Attack()
        {
            if (!this.doOnce)
            {
                this.weapon.ReInit();
                this.doneSwinging = false;
                this.doOnce = true;
            }

            if (this.weapon.WeaponUpdate())
            {
                this.weapon.CleanUp();
                this.doneSwinging = true;
            }
        }
    }
}
