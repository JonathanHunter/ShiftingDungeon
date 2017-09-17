namespace ShiftingDungeon.Character.Enemies
{
    using UnityEngine;
    using Util;

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
        private float walkCounter;
        private int timesWalked;

        protected override void LocalInitialize()
        {
        }

        protected override void LocalReInitialize()
        {
            this.hero = Managers.DungeonManager.GetHero().transform;
            this.rgbdy = this.gameObject.GetComponent<Rigidbody2D>();
            this.walkCounter = Random.Range(0, this.walkTime + 1);
            this.timesWalked = 0;
        }

        protected override void LocalUpdate()
        {
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

        protected override void LocalCollision(Collision2D collision)
        {
            if (collision.collider.tag == Enums.Tags.HeroWeapon.ToString())
            {
                Vector2 position = this.transform.position;
                this.rgbdy.AddForce((position - collision.contacts[0].point).normalized * 5f, ForceMode2D.Impulse);
                sfx.PlaySong(0);
            }
        }

        private void RotateToPlayer()
        {
            Vector2 towards = this.hero.transform.position - this.transform.position;
            this.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, towards));
        }
    }
}
