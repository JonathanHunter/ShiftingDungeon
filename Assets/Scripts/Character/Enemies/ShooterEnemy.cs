namespace ShiftingDungeon.Character.Enemies
{
    using UnityEngine;
    using Util;
    using Weapons;

    public class ShooterEnemy : Enemy
    {
        [SerializeField]
        private float walkTime = 1;
        [SerializeField]
        private int walkCount = 3;
        [SerializeField]
        private float walkSpeed = 2;
        [SerializeField]
        private float agroRange = 3;
        [SerializeField, Range(0f, 1)]
        private float shotLeadError = 1f;
        [SerializeField]
        private float stunLength = 0.15f;
        [SerializeField]
        private SoundPlayer sfx;
        [SerializeField]
        private Animator anim;
        [SerializeField]
        private int weaponLevel;

        private Transform hero;
        private Rigidbody2D rgbdy;
        private float walkCounter;
        private int hitHash;
        private float desiredAngle;
        
        private float stunCounter;

        [SerializeField]
        private Gun gun;

        private bool isShooting;

        protected override void LocalInitialize()
        {
            if(this.anim == null)
                this.anim = this.gameObject.GetComponent<Animator>();

            this.hero = Managers.DungeonManager.GetHero().transform;
            this.rgbdy = this.gameObject.GetComponent<Rigidbody2D>();
            this.hitHash = Animator.StringToHash("Hit");
            gun.Init(this.weaponLevel);
            gun.CleanUp();
        }

        protected override void LocalReInitialize()
        {
            this.walkCounter = Random.Range(0, this.walkTime + 1);
            isShooting = false;
        }

        protected override void LocalUpdate()
        {
            if ((this.stunCounter -= Time.deltaTime) > 0)
                return;
            
            if (isShooting)
            {
                if (!(isShooting = !gun.WeaponUpdate()))
                    gun.CleanUp();
            }

            if (Vector2.Distance(this.hero.transform.position, this.transform.position) > this.agroRange)
                return;

            
            if (!isShooting)
            {
                RotateToPlayer();
                if ((this.walkCounter -= Time.deltaTime) <= 0)
                {
                    if (CanSeePlayer())
                    {
                        RotateToLeadShot();
                        this.rgbdy.velocity = -this.transform.right * this.walkSpeed;
                        gun.ReInit();
                        isShooting = true;
                    }
                    else
                    {
                        this.rgbdy.velocity = this.transform.right * this.walkSpeed;
                    }
                    this.walkCounter = this.walkCount;
                }

                float z = this.transform.rotation.eulerAngles.z;
                z = z > 180 ? z - 360 : z;
                if (Mathf.Abs(z - this.desiredAngle) > 0.1f)
                {
                    if (z < this.desiredAngle)
                        this.transform.rotation = Quaternion.Euler(0, 0, z + Time.deltaTime * 100f);
                    else
                        this.transform.rotation = Quaternion.Euler(0, 0, z - Time.deltaTime * 100f);
                }
            }
        }

        protected override void LocalDeallocate()
        {
        }

        protected override void LocalDelete()
        {
        }

        protected override void LocalCollision(Collider2D collider)
        {
            if (this.stunCounter > 0)
                return;
            if (collider.tag == Enums.Tags.HeroWeapon.ToString())
            {
                this.anim.SetTrigger(hitHash);
                this.stunCounter = stunLength;
                sfx.PlaySongModPitch(0, .1f);
            }
        }

        protected override void TakeDamage(int damage)
        {
            if (this.stunCounter > 0)
                return;
            this.Health -= damage;
        }

        private bool CanSeePlayer()
        {
            Vector2 toPlayer = hero.transform.position - transform.position;
            int layerMask = ~(1 << (int)Enums.Layers.Enemy | 1 << (int)Enums.Layers.HeroWeapon | 1 << (int)Enums.Layers.Dungeon);
            RaycastHit2D result = Physics2D.Raycast(transform.position, toPlayer.normalized, toPlayer.magnitude, layerMask);
            if (result.collider != null)
            {
                if (result.collider.tag == Enums.Tags.Hero.ToString())
                    return true;
            }
            return false;
        }

        private void RotateToPlayer()
        {
            Vector2 towards = this.hero.position - this.transform.position;
            this.desiredAngle = Vector2.SignedAngle(Vector2.right, towards);
        }

        private void RotateToLeadShot()
        {
            Vector2 heroVelocity = this.hero.GetComponent<Rigidbody2D>().velocity;
            float deltaT = (this.transform.position - this.hero.position).magnitude
                / (heroVelocity - this.gun.BulletSpeed * (Vector2)this.transform.right).magnitude;
            Vector2 futurePlayerPos = (Vector2)hero.position + heroVelocity * deltaT * (1 - shotLeadError);

            Vector2 towards = futurePlayerPos - (Vector2)this.transform.position;
            this.desiredAngle = Vector2.SignedAngle(Vector2.right, towards);
        }
    }
}
