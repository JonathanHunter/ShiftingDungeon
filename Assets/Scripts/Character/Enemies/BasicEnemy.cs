namespace ShiftingDungeon.Character.Enemies
{
    using UnityEngine;
    using Util;
    using Weapons;

    public class BasicEnemy : Enemy
    {
        [SerializeField]
        private float walkTime = 1;
        [SerializeField]
        private int walkCount = 3;
        [SerializeField]
        private float walkSpeed = 2;
        [SerializeField]
        private float agroRange = 3;
        [SerializeField]
        private float stunLength = 0.15f;
        [SerializeField]
        private float maxTimeBetweenHits = .8f;
        [SerializeField]
        private SoundPlayer sfx;
        [SerializeField]
        private Animator anim;

        private Transform hero;
        private Rigidbody2D rgbdy;
        private float walkCounter;
        private int timesWalked;
        private int hitHash;

        private int numHits;
        private float hitCounter;
        private float stunCounter;
        

        protected override void LocalInitialize()
        {
            if(this.anim == null)
                this.anim = this.gameObject.GetComponent<Animator>();

            this.hero = Managers.DungeonManager.GetHero().transform;
            this.rgbdy = this.gameObject.GetComponent<Rigidbody2D>();
            this.hitHash = Animator.StringToHash("Hit");
        }

        protected override void LocalReInitialize()
        {
            this.walkCounter = Random.Range(0, this.walkTime + 1);
            this.timesWalked = 0;
        }

        protected override void LocalUpdate()
        {
            if ((this.hitCounter -= Time.deltaTime) <= 0)
                this.numHits = 0;
            if ((this.stunCounter -= Time.deltaTime) > 0)
                return;
            if (Vector2.Distance(this.hero.transform.position, this.transform.position) > this.agroRange)
                return;

            RotateToPlayer();
            if((this.walkCounter -= Time.deltaTime) <= 0)
            {
                if((this.timesWalked++) >= this.walkCount)
                {
                    this.rgbdy.velocity = this.transform.right * this.walkSpeed * 4f;
                    this.timesWalked = 0;
                }
                else
                {
                    this.rgbdy.velocity = this.transform.right * this.walkSpeed;
                }

                this.walkCounter = this.walkCount;
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
                numHits++;
                if (numHits > 2)
                {
                    //Melee weapons
                    if (collider.transform.parent)
                        this.rgbdy.AddForce(collider.transform.parent.transform.parent.transform.right * 9f, ForceMode2D.Impulse);
                    //Everything else
                    else
                        this.rgbdy.AddForce(collider.transform.right * 9f, ForceMode2D.Impulse);
                    numHits = 0;
                }
                this.anim.SetTrigger(hitHash);
                this.stunCounter = stunLength;
                this.hitCounter = maxTimeBetweenHits;
                sfx.PlaySongModPitch(0, .1f);
            }
        }

        protected override void TakeDamage(int damage)
        {
            if (this.stunCounter > 0)
                return;
            this.Health -= damage;
        }

        private void RotateToPlayer()
        {
            Vector2 towards = this.hero.transform.position - this.transform.position;
            this.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, towards));
        }
    }
}
