namespace ShiftingDungeon.Character.Enemies
{
    using UnityEngine;
    using Util;
    using Weapons;

    public class ShooterEnemy : BasicEnemy
    {
        [SerializeField]
        private Gun gun;

        private bool isShooting;

        protected override void LocalInitialize()
        {
            base.LocalInitialize();
            gun.Init();
            gun.CleanUp();
            type = Enums.EnemyTypes.Shooter;
        }

        protected override void LocalReInitialize()
        {
            base.LocalReInitialize();
            isShooting = false;
        }

        protected override void LocalUpdate()
        {
            if ((this.hitCounter -= Time.deltaTime) <= 0)
                this.numHits = 0;
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
            base.LocalCollision(collider);
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
    }
}
