namespace ShiftingDungeon.Character.Enemies
{
	using UnityEngine;
    using Util;
    using Weapons;

	public class GrassShooterEnemy : Enemy {
        [SerializeField]
        private float agroRange = 3;
        [SerializeField]
        private float stunLength = 0.15f;
        [SerializeField]
        private float timeBetweenShots = .8f;
        [SerializeField]
        private SoundPlayer sfx;
		[SerializeField]
		private Vector2 shotLeadError = new Vector2(.8f, 1f);
        [SerializeField]
        private TriShotGun gun;
		[SerializeField]
		private Transform roots;

        private Transform hero;
        private Animator anim;
        private int hitHash;

        private float hitCounter;
        private float stunCounter;
        private float shootCounter;
		private Quaternion rootRot;

        private bool isShooting;

        protected override void LocalInitialize()
        {
            this.hero = Managers.DungeonManager.GetHero().transform;
            this.anim = this.gameObject.GetComponent<Animator>();
            this.hitHash = Animator.StringToHash("Hit");
			this.shootCounter = 0f;
			this.rootRot = this.roots.rotation;
            gun.Init();
            gun.CleanUp();
        }

        protected override void LocalReInitialize()
        {
			this.rootRot = this.roots.rotation;
			hero = Managers.DungeonManager.GetHero().transform;
            isShooting = false;
        }

        protected override void LocalUpdate()
        {
            if (isShooting)
            {
                if (!(isShooting = !gun.WeaponUpdate()))
                    gun.CleanUp();
            }

            if (Vector2.Distance(hero.transform.position, transform.position) > agroRange)
                return;

            
            if (!isShooting)
            {
				this.shootCounter -= Time.deltaTime;
                RotateToPlayer();
				if (CanSeePlayer() && this.shootCounter < 0)
				{
					this.shootCounter = this.timeBetweenShots;
					gun.ReInit();
					isShooting = true;
				} 
            }
			this.roots.rotation = rootRot;
        }

        protected override void LocalDeallocate()
        {
        }

        protected override void LocalDelete()
        {
        }

        protected override void LocalCollision(Collider2D collider)
        {
            if (collider.tag == Enums.Tags.HeroWeapon.ToString())
            {
                this.anim.SetTrigger(hitHash);
                this.stunCounter = stunLength;
                sfx.PlaySongModPitch(0, .1f);
            }
        }

        protected override void TakeDamage(int damage)
        {
            Health -= damage;
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
            Vector2 towards = hero.transform.position - transform.position;
            transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, towards));
        }

		private void RotateToLeadShot()
        {
            Vector2 heroVelocity = this.hero.GetComponent<Rigidbody2D>().velocity;
            float deltaT = (this.transform.position - this.hero.position).magnitude
                / (heroVelocity - ((TriShotGun) this.gun).GetBulletSpeed() * (Vector2)this.transform.right).magnitude;
            Vector2 futurePlayerPos = (Vector2)hero.position + heroVelocity * deltaT
				* (1 - Random.Range(shotLeadError.x, shotLeadError.y));

            Vector2 towards = futurePlayerPos - (Vector2)this.transform.position;
            this.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, towards));
        }
	}
}
