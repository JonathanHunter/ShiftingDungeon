﻿namespace ShiftingDungeon.Character.Enemies
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
        [SerializeField]
        private float stunLength = 0.15f;
        [SerializeField]
        private float maxTimeBetweenHits = .8f;
        [SerializeField]
        private SoundPlayer sfx;

        private Transform hero;
        private Rigidbody2D rgbdy;
        private Animator anim;
        private float walkCounter;
        private int timesWalked;
        private int hitHash;

        private int numHits;
        private float hitCounter;
        private float stunCounter;

        [SerializeField]
        private Gun gun;

        private bool isShooting;

        protected override void LocalInitialize()
        {
            gun.Init();
            gun.CleanUp();
        }

        protected override void LocalReInitialize()
        {
            this.hero = Managers.DungeonManager.GetHero().transform;
            this.rgbdy = this.gameObject.GetComponent<Rigidbody2D>();
            this.anim = this.gameObject.GetComponent<Animator>();
            this.walkCounter = Random.Range(0, this.walkTime + 1);
            this.timesWalked = 0;
            this.hitHash = Animator.StringToHash("Hit");
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

        protected void RotateToPlayer()
        {
            Vector2 towards = this.hero.transform.position - this.transform.position;
            this.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, towards));
        }
    }
}
