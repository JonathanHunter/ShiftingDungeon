namespace ShiftingDungeon.Character.Hero
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Util;
    using Weapons;

    public class HeroBehavior : MonoBehaviour
    {
        [SerializeField]
        private int maxHealth = 10;
        [SerializeField]
        private float acceleration = 1;
        [SerializeField]
        private float maxSpeed = 5;
        /// <summary> The range of the player's enemy-targeting ability. </summary>
        [SerializeField]
        private float targetRange = 7.5f;
        /// <summary> The fraction of money that the player will retain after dying. </summary>
        [SerializeField]
        private float moneyKeptOnDeath;
        /// <summary> A crosshair sprite for indicating the enemy target. </summary>
        [SerializeField]
        private Crosshair crosshair;
        [SerializeField]
        private Weapon[] weapons = null;
        [SerializeField]
        private SoundPlayer sfx;
        [SerializeField]
        private Transform weaponHolder;
        [SerializeField]
        private AnimationClip[] MovementClips;
        [SerializeField]
        private AnimationClip[] AttackClips;
        [SerializeField]
        private AnimationClip[] HurtClips;

        private Animator anim = null;
        private Rigidbody2D rgbdy = null;
        private HeroInput input = null;
        private StateMap<Enums.HeroState> stateMap = null;
        private bool doOnce = false;
        private bool isSpeedAltered = false;
        private int attackHash = 0;
        private int attackFinishedHash = 0;
        private int hitHash = 0;
        private float alteredSpeedPercentage = 0;
        private float alteredSpeedDuration = 0;
        /// <summary> The index of the last target the player locked onto. </summary>
        private int targetIndex = 0;
        private AnimationOverrideHandler animOverride;
        private int currentClipSet;
        /// <summary> The particle system to trigger when the player dies. </summary>
        private ParticleSystem deathParticles = null;
        /// <summary> Whether the player's death animation has started. </summary>
        private bool deathTriggered = false;

        /// <summary> The player's max health. </summary>
        public int MaxHealth { get { return this.maxHealth; } }
        /// <summary> The player's current health. </summary>
        public int Health { get; private set; }
        /// <summary> Whether the player is dead. </summary>
        public bool IsDead { get { return this.Health <= 0; } }
        /// <summary> The player's current weapon. </summary>
        public int CurrentWeapon { get; private set; }
        /// <summary> The player's current state. </summary>
        public Enums.HeroState CurrentState { get; private set; }
        /// <summary> True if the player has an attack queued. </summary>
        public bool AttackQueued { get; internal set; }

        private void Start()
        {
            this.anim = GetComponent<Animator>();
            this.animOverride = new AnimationOverrideHandler(this.anim);
            this.rgbdy = GetComponent<Rigidbody2D>();
            this.input = GetComponent<HeroInput>();
            this.stateMap = new StateMap<Enums.HeroState>();
            this.doOnce = false;
            this.attackHash = Animator.StringToHash("Attack");
            this.attackFinishedHash = Animator.StringToHash("AttackFinished");
            this.hitHash = Animator.StringToHash("Hit");
            this.Health = this.maxHealth;
            this.CurrentWeapon = 0;
            this.CurrentState = Enums.HeroState.Idle;
            this.crosshair = Instantiate(crosshair);
            this.currentClipSet = 0;
            this.deathParticles = GetComponentInChildren<ParticleSystem>();
            // HACK: Addresses Unity bug where ParticleSystem.Emit() initially spawns particles
            // at the origin regardless of the particle system's position, until a particle is emitted.
            this.deathParticles.Emit(1);

            if(HeroData.Instance.weaponLevels == null || HeroData.Instance.weaponLevels.Length == 0)
            {
                HeroData.Instance.weaponLevels = new int[this.weapons.Length];
                for (int i = 0; i < this.weapons.Length; i++)
                    HeroData.Instance.weaponLevels[i] = 0;
            }

            foreach (Weapon w in this.weapons)
            {
                w.Init();
                w.CleanUp();
            }

            this.weapons[this.CurrentWeapon].Level = HeroData.Instance.weaponLevels[this.CurrentWeapon];
        }

        private void Update()
        {
            if (Managers.GameState.Instance.IsPaused)
                return;

            Enums.HeroState temp = this.CurrentState;
            this.CurrentState = this.stateMap.GetState(this.anim.GetCurrentAnimatorStateInfo(0).fullPathHash);
            if (temp != CurrentState)
            {
                this.doOnce = false;
                this.anim.SetBool(this.attackFinishedHash, false);                
            }

            switch(this.CurrentState)
            {
                case Enums.HeroState.Idle: Idle(); break;
                case Enums.HeroState.Move: Move(); break;
                case Enums.HeroState.Attack: Attack(); break;
                case Enums.HeroState.Hurt: Hurt(); break;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == Enums.Tags.Enemy.ToString() ||
                collision.gameObject.tag == Enums.Tags.EnemyWeapon.ToString() || 
                collision.gameObject.tag == Enums.Tags.Trap.ToString())
            {
                Vector2 position = this.transform.position;
                Vector2 forceDirection = (position - collision.contacts[0].point).normalized;
                DamageHero(collision.gameObject, forceDirection);
            }
            else if (collision.gameObject.tag == Enums.Tags.Pickup.ToString())
            {
                if (collision.gameObject.GetComponent<Pickups.Money>() != null)
                {
                    Pickups.Money gold = collision.gameObject.GetComponent<Pickups.Money>();
                    HeroData.Instance.money += gold.Value;
                    ObjectPooling.PickupPool.Instance.ReturnGold(gold.gameObject);
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.tag == Enums.Tags.Enemy.ToString() ||
                collider.gameObject.tag == Enums.Tags.EnemyWeapon.ToString())
            {
                DamageHero(collider.gameObject, collider.transform.right);
            }
        }

        /// <summary>
        /// Damages and knocks back the hero, killing the hero if health drops below 0.
        /// </summary>
        /// <param name="collideObject">The object dealing damage to the hero.</param>
        /// <param name="forceDirection">The direction of knockback.</param>
        private void DamageHero(GameObject collideObject, Vector2 forceDirection)
        {
            if (this.CurrentState != Enums.HeroState.Hurt &&
                collideObject.GetComponent<IDamageDealer>() != null)
            {
                this.Health -= collideObject.GetComponent<IDamageDealer>().GetDamage();
                Vector2 position = this.transform.position;
                this.rgbdy.AddForce(forceDirection * 5f, ForceMode2D.Impulse);
                sfx.PlaySong(0);

                if (IsDead && !deathTriggered)
                {
                    deathTriggered = true;
                    StartCoroutine(Die());
                }
            }

            anim.SetTrigger(this.hitHash);
        }

        /// <summary> Cycles to the next weapon in the players list. </summary>
        public void GoToNextWeapon()
        {
            if (this.CurrentState != Enums.HeroState.Attack)
            {
                this.CurrentWeapon++;
                if (this.CurrentWeapon >= this.weapons.Length)
                    this.CurrentWeapon = 0;
            }

            this.weapons[this.CurrentWeapon].Level = HeroData.Instance.weaponLevels[this.CurrentWeapon];
        }

        /// <summary> Cycles to the previous weapon in the players list. </summary>
        public void GoToPreviousWeapon()
        {
            if (this.CurrentState != Enums.HeroState.Attack)
            {
                this.CurrentWeapon--;
                if (this.CurrentWeapon < 0)
                    this.CurrentWeapon = this.weapons.Length - 1;
            }

            this.weapons[this.CurrentWeapon].Level = HeroData.Instance.weaponLevels[this.CurrentWeapon];
        }

        /// <summary>
        /// Faces the next enemy in the room according to order of distance.
        /// Faces the nearest enemy if the target array needs to be reset.
        /// </summary>
        public void TargetEnemy()
        {
            if (this.CurrentState != Enums.HeroState.Attack)
            {
                Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, this.targetRange, 1 << (int) Enums.Layers.Enemy);
                // Sort targets by ascending order of distance from the player.
                Array.Sort(targets, (t1, t2) => 
                           Comparer<float>.Default.Compare(Vector2.Distance(t1.transform.position, transform.position),
                                                           Vector2.Distance(t2.transform.position, transform.position)));
                if (targetIndex >= targets.Length)
                    targetIndex = 0;
                if (targets.Length == 0)
                {
                    sfx.PlaySong(2);
                }
                else
                {
                    // TODO Account for walls that cannot be shot through once they are added.
                    Vector3 targetPos = targets[targetIndex++].transform.position;
                    float angle = Vector2.SignedAngle(Vector2.right, (targetPos - transform.position).normalized);
                    this.weaponHolder.rotation = Quaternion.Euler(0, 0, angle);
                    CalculateSpriteSet(angle);
                    crosshair.Target(targetPos);
                    sfx.PlaySong(1);
                }
            }
        }

        /// <summary>
        /// Reduces the player's max speed and acceleration to the specified amount, for the sepcified duration
        /// </summary>
        /// <param name="newSpeedPercentage">What percent speed the player should maintain (e.g., 0.8 would result in the player moving at 80% their normal speed)</param>
        /// <param name="duration">How long (in seconds) the slow effect lasts</param>
        public void AlterPlayerMaxSpeed(float newSpeedPercentage, float duration)
        {
            alteredSpeedPercentage = newSpeedPercentage;
            alteredSpeedDuration = duration;

            isSpeedAltered = true;
        }

        private void Idle()
        {
        }

        private void Move()
        {
            int x;
            int y;
            if (this.input.Up)
                y = 1;
            else if (this.input.Down)
                y = -1;
            else
                y = 0;

            if (this.input.Left)
                x = -1;
            else if (this.input.Right)
                x = 1;
            else
                x = 0;

            Vector2 dir = new Vector2(x, y);
            if (dir != Vector2.zero)
            {
                float angle = Vector2.SignedAngle(Vector2.right, dir);
                this.weaponHolder.rotation = Quaternion.Euler(0, 0, angle);
                CalculateSpriteSet(angle);
            }
            Vector2 right = this.weaponHolder.right;

            float currentMaxSpeed = maxSpeed;
            float currentAcceleration = this.acceleration;
            if (isSpeedAltered) {
                currentMaxSpeed *= alteredSpeedPercentage;
                currentAcceleration *= alteredSpeedPercentage;
                alteredSpeedDuration -= Time.deltaTime;
                isSpeedAltered = alteredSpeedDuration > 0;
            }

            Vector2 speed = this.rgbdy.velocity + right * currentAcceleration;
            if (speed.magnitude < currentMaxSpeed && !IsDead)
                this.rgbdy.velocity = speed;

            // Hero moved, so go back to targeting the closest enemy.
            targetIndex = 0;
        }

        private void Attack()
        {
            if(!this.doOnce)
            {
                this.weapons[this.CurrentWeapon].ReInit();
                this.doOnce = true;
            }
            
            if(this.weapons[this.CurrentWeapon].WeaponUpdate())
            {
                this.weapons[this.CurrentWeapon].CleanUp();
                if (this.AttackQueued)
                {
                    this.doOnce = false;
                    this.anim.SetBool(attackHash, true);
                    this.AttackQueued = false;
                }
                else
                {
                    anim.SetBool(this.attackFinishedHash, true);
                }
            }
        }

        private void Hurt()
        {
            if(!this.doOnce)
            {
                this.weapons[this.CurrentWeapon].CleanUp();
                this.doOnce = true;
            }

            targetIndex = 0;
        }

        private void CalculateSpriteSet(float angle)
        {
            if (angle < 0)
                angle += 360f;

            int sprite = 0;
            if (angle < 45)
                sprite = 0;
            else if (angle < 90)
                sprite = 1;
            else if (angle < 135)
                sprite = 2;
            else if (angle < 180)
                sprite = 3;
            else if (angle < 225)
                sprite = 4;
            else if (angle < 270)
                sprite = 5;
            else if (angle < 315)
                sprite = 6;
            else
                sprite = 7;

            if (sprite != this.currentClipSet)
            {
                this.animOverride.OverrideClip(this.MovementClips[0], this.MovementClips[sprite]);
                this.animOverride.OverrideClip(this.AttackClips[0], this.AttackClips[sprite]);
                this.animOverride.OverrideClip(this.HurtClips[0], this.HurtClips[sprite]);
                this.animOverride.ApplyOverrides();
                this.currentClipSet = sprite;
            }
        }

        /// <summary> Causes the player to die and receive a game over. </summary>
        private IEnumerator Die()
        {
            input.enabled = false;
            input.ResetInput();
            weaponHolder.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.25f);

            Managers.DungeonManager.ShowGameOver();
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Renderer>().enabled = false;
            deathParticles.Emit(100);
            sfx.PlaySong(3);
            this.rgbdy.velocity = Vector2.zero;
            this.weapons[this.CurrentWeapon].CleanUp();

            yield return null;
        }

        /// <summary> Reactivates hero components when respawning after death. </summary>
        public void Respawn()
        {
            GetComponent<HeroInput>().enabled = true;
            weaponHolder.gameObject.SetActive(true);
            GetComponent<Collider2D>().enabled = true;
            GetComponent<Renderer>().enabled = true;
            Health = maxHealth;
            HeroData.Instance.money = (int)(moneyKeptOnDeath * HeroData.Instance.money);
            deathTriggered = false;
            transform.position = Vector2.zero;
        }
    }
}
