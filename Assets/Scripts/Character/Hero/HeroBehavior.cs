namespace ShiftingDungeon.Character.Hero
{
    using System;
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
        [SerializeField]
        private Weapon[] weapons = null;
        [SerializeField]
        private SoundPlayer sfx;

        private Animator anim = null;
        private Rigidbody2D rgbdy = null;
        private HeroInput input = null;
        private StateMap stateMap = null;
        private bool doOnce = false;
        private int attackHash = 0;
        private int attackFinishedHash = 0;
        private int hitHash = 0;

        /// <summary> The index of the last target the player locked onto. </summary>
        private int targetIndex = 0;
        /// <summary> A crosshair sprite for indicating the enemy target. </summary>
        [SerializeField]
        private Crosshair crosshair;

        /// <summary> The player's max health. </summary>
        public int MaxHealth { get { return this.maxHealth; } }
        /// <summary> The player's current health. </summary>
        public int Health { get; private set; }
        /// <summary> The player's current weapon. </summary>
        public int CurrentWeapon { get; private set; }
        /// <summary> The player's current state. </summary>
        public Enums.HeroState CurrentState { get; private set; }

        public bool AttackQueued { get; internal set; }

        private void Start()
        {
            this.anim = GetComponent<Animator>();
            this.rgbdy = GetComponent<Rigidbody2D>();
            this.input = GetComponent<HeroInput>();
            this.stateMap = new StateMap();
            this.doOnce = false;
            this.attackHash = Animator.StringToHash("Attack");
            this.attackFinishedHash = Animator.StringToHash("AttackFinished");
            this.hitHash = Animator.StringToHash("Hit");
            this.Health = this.maxHealth;
            this.CurrentWeapon = 0;
            this.CurrentState = Enums.HeroState.Idle;
            this.crosshair = Instantiate(crosshair);

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
                case Enums.HeroState.North: Move(); break;
                case Enums.HeroState.NorthEast: Move(); break;
                case Enums.HeroState.East: Move(); break;
                case Enums.HeroState.SouthEast: Move(); break;
                case Enums.HeroState.South: Move(); break;
                case Enums.HeroState.SouthWest: Move(); break;
                case Enums.HeroState.West: Move(); break;
                case Enums.HeroState.NorthWest: Move(); break;
                case Enums.HeroState.Attack: Attack(); break;
                case Enums.HeroState.Hurt: Hurt(); break;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnTriggerEnter2D(collision.collider);
        }
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.tag == Enums.Tags.Enemy.ToString() ||
                collider.gameObject.tag == Enums.Tags.EnemyWeapon.ToString() ||
                collider.gameObject.tag == Enums.Tags.Trap.ToString())
            {
                if (this.CurrentState != Enums.HeroState.Hurt &&
                    collider.gameObject.GetComponent<IDamageDealer>() != null)
                {
                    this.Health -= collider.gameObject.GetComponent<IDamageDealer>().GetDamage();
                    Vector2 position = this.transform.position;
                    this.rgbdy.AddForce(collider.transform.right * 5f, ForceMode2D.Impulse);
                    sfx.PlaySong(0);
                }

                anim.SetTrigger(this.hitHash);
            }
            else if (collider.gameObject.tag == Enums.Tags.Pickup.ToString())
            {
                if (collider.gameObject.GetComponent<Pickups.Money>() != null)
                {
                    Pickups.Money gold = collider.gameObject.GetComponent<Pickups.Money>();
                    HeroData.Instance.money += gold.Value;
                    ObjectPooling.PickupPool.Instance.ReturnGold(gold.gameObject);
                }
            }
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
                Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, 5, 1 << (int) Enums.Layers.Enemy);
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
                    transform.Rotate(Vector3.forward, Vector2.SignedAngle(transform.right, targetPos - transform.position));
                    crosshair.Target(targetPos);
                    sfx.PlaySong(1);
                }
            }
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
            if(dir != Vector2.zero)
                this.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, dir));
            Vector2 right = this.transform.right;
            Vector2 speed = this.rgbdy.velocity + right * this.acceleration;
            if (speed.magnitude < this.maxSpeed)
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
    }
}
