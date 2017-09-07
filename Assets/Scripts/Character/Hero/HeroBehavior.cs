namespace ShiftingDungeon.Character.Hero
{
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
        [SerializeField]
        private Weapon[] weapons = null;
        [SerializeField]
        private SoundPlayer sfx;

        private Animator anim = null;
        private Rigidbody2D rgbdy = null;
        private HeroInput input = null;
        private StateMap stateMap = null;
        private bool doOnce = false;
        private int attackFinishedHash = 0;
        private int hitHash = 0;

        /// <summary> The player's max health. </summary>
        public int MaxHealth { get { return this.maxHealth; } }
        /// <summary> The player's current health. </summary>
        public int Health { get; private set; }
        /// <summary> The player's current weapon. </summary>
        public int CurrentWeapon { get; private set; }
        /// <summary> The player's current state. </summary>
        public Enums.HeroState CurrentState { get; private set; }

        private void Start()
        {
            this.anim = GetComponent<Animator>();
            this.rgbdy = GetComponent<Rigidbody2D>();
            this.input = GetComponent<HeroInput>();
            this.stateMap = new StateMap();
            this.doOnce = false;
            this.attackFinishedHash = Animator.StringToHash("AttackFinished");
            this.hitHash = Animator.StringToHash("Hit");
            this.Health = this.maxHealth;
            this.CurrentWeapon = 0;
            this.CurrentState = Enums.HeroState.Idle;

            foreach (Weapon w in this.weapons)
            {
                w.Init();
                w.CleanUp();
            }
        }

        private void Update()
        {
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
            if (collision.collider.gameObject.tag == Enums.Tags.Enemy.ToString() ||
                collision.collider.gameObject.tag == Enums.Tags.EnemyWeapon.ToString())
            {
                if (this.CurrentState != Enums.HeroState.Hurt &&
                    collision.collider.gameObject.GetComponent<IDamageDealer>() != null)
                {
                    this.Health -= collision.collider.gameObject.GetComponent<IDamageDealer>().GetDamage();
                    Vector2 position = this.transform.position;
                    this.rgbdy.AddForce((position - collision.contacts[0].point).normalized * 5f, ForceMode2D.Impulse);
                    sfx.PlaySong(0);
                }

                anim.SetTrigger(this.hitHash);
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
                anim.SetBool(this.attackFinishedHash, true);
                this.weapons[this.CurrentWeapon].CleanUp();
            }
        }

        private void Hurt()
        {
            if(!this.doOnce)
            {
                this.weapons[this.CurrentWeapon].CleanUp();
                this.doOnce = true;
            }
        }
    }
}
