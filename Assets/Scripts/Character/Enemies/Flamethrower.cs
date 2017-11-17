namespace ShiftingDungeon.Character.Enemies
{
    using UnityEngine;
    using Util;

    public class Flamethrower : Enemy
    {
        [SerializeField]
        private Transform[] flames;
        [SerializeField]
        private Transform[] positions;
        [SerializeField]
        private float extendSpeed;
        [SerializeField]
        private float agroRange;
        [SerializeField]
        private Vector2 rotationTimeRange;
        [SerializeField]
        private float flameTime;
        [SerializeField]
        private float stunLength = 0.15f;
        [SerializeField, Range(0f, 1)]
        private float shotLeadError = 1f;
        [SerializeField]
        private SoundPlayer sfx;
        [SerializeField]
        private Animator anim;

        private enum State { idle, rotating, flaming }

        private State curr;
        private Transform hero;
        private bool doOnce;
        private float desiredAngle;
        private float delta;
        private float stunCounter;
        private float rotateCounter;
        private int hitHash;

        protected override void LocalInitialize()
        {
            this.hero = Managers.DungeonManager.GetHero().transform;
            this.hitHash = Animator.StringToHash("Hit");
        }

        protected override void LocalReInitialize()
        {
            ResetFlames();
            this.curr = State.idle;
            this.doOnce = false;
            this.stunCounter = 0;
            this.rotateCounter = 0;
        }

        protected override void LocalUpdate()
        {
            if (this.stunCounter > 0)
                this.stunCounter -= Time.deltaTime;

            State temp = this.curr;
            switch(this.curr)
            {
                case State.idle: Idle(); break;
                case State.rotating: Rotate(); break;
                case State.flaming: Flame(); break;
            }

            if(temp != this.curr)
            {
                this.doOnce = false;
            }
        }

        protected override void LocalDeallocate()
        {
            ResetFlames();
        }

        protected override void LocalDelete()
        {
        }

        protected override void LocalCollision(Collider2D collision)
        {
            if (this.stunCounter > 0)
                return;
            if (collision.tag == Enums.Tags.HeroWeapon.ToString())
            {
                this.anim.SetTrigger(hitHash);
                this.stunCounter = this.stunLength;
                this.sfx.PlaySongModPitch(0, .1f);
            }
        }

        protected override void TakeDamage(int damage)
        {
            if (this.stunCounter > 0)
                return;
            this.Health -= damage;
        }

        private void ResetFlames()
        { 
            for (int i = 0; i < this.flames.Length; i++)
            {
                this.flames[i].position = Vector3.zero;
                this.flames[i].gameObject.SetActive(false);
            }

            this.delta = 0;
        }

        private void RotateToLeadShot()
        {
            Vector2 heroVelocity = this.hero.GetComponent<Rigidbody2D>().velocity;
            float deltaT = Mathf.Approximately(heroVelocity.magnitude, 0f) 
                ? 0 : (this.transform.position - this.hero.position).magnitude / heroVelocity.magnitude;
            Vector2 futurePlayerPos = (Vector2)hero.position + heroVelocity * deltaT * (1 - this.shotLeadError);
            Vector2 towards = futurePlayerPos - (Vector2)this.transform.position;
            this.desiredAngle = Vector2.SignedAngle(Vector2.right, towards);
        }

        private void Idle()
        {
            if (Vector2.Distance(this.hero.position, this.transform.position) < this.agroRange)
                this.curr = State.rotating;
        }

        private void Rotate()
        {
            if(!doOnce)
            {
                this.rotateCounter = Random.Range(this.rotationTimeRange.x, this.rotationTimeRange.y);
                this.doOnce = true;
            }

            RotateToLeadShot();
            float z = this.transform.rotation.eulerAngles.z;
            float deltaAngle = Mathf.DeltaAngle(z, this.desiredAngle);
            if (Mathf.Abs(deltaAngle) > 0.1f)
            {
                if (deltaAngle > 0)
                    this.transform.rotation = Quaternion.Euler(0, 0, z + Time.deltaTime * 100f);
                else
                    this.transform.rotation = Quaternion.Euler(0, 0, z - Time.deltaTime * 100f);
            }
            if ((this.rotateCounter -= Time.deltaTime) < 0)
                this.curr = State.flaming;
        }

        private void Flame()
        {
            if (!this.doOnce)
            {
                this.rotateCounter = this.flameTime;
                for (int i = 0; i < this.flames.Length; i++)
                {
                    this.flames[i].gameObject.SetActive(true);
                }
                sfx.PlaySongModPitch(1, .1f);
                this.doOnce = true;
            }

            if (this.delta < 1)
            {
                this.delta += Time.deltaTime;
                for (int i = 0; i < this.flames.Length; i++)
                {
                    this.flames[i].localPosition = Vector3.Lerp(Vector3.zero, this.positions[i].localPosition, this.delta);
                }
            }
            else
            {
                if((this.rotateCounter -= Time.deltaTime) < 0)
                {
                    ResetFlames();
                    this.curr = State.idle;
                }
            }
        }
    }
}
