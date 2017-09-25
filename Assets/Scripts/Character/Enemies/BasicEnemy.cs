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
        private SoundPlayer sfx;

        private Transform hero;
        private Rigidbody2D rgbdy;
        private Animator anim;
        private float walkCounter;
        private int timesWalked;
        private int hitHash;

        private float stunCounter;
        private float stunLength = 0.15f;

        protected override void LocalInitialize()
        {
            
        }

        protected override void LocalReInitialize()
        {
            this.hero = Managers.DungeonManager.GetHero().transform;
            this.rgbdy = this.gameObject.GetComponent<Rigidbody2D>();
            this.anim = this.gameObject.GetComponent<Animator>();
            this.walkCounter = Random.Range(0, this.walkTime + 1);
            this.timesWalked = 0;
            this.hitHash = Animator.StringToHash("Hit");
        }

        protected override void LocalUpdate()
        {
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
                //If on the 2nd combo attack, apply force
                Sword playerSword = collider.GetComponentInParent<Sword>();
                if (playerSword && playerSword.ComboCounter == 2)
                {
                    Vector2 position = this.transform.position;
                    this.rgbdy.AddForce(collider.transform.parent.transform.parent.transform.right * 9f, ForceMode2D.Impulse);
                }

                this.Health -= collider.gameObject.GetComponent<IDamageDealer>().GetDamage();
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
    }
}
