namespace ShiftingDungeon.Character.Traps {
    using Hero;
    using UnityEngine;
    using Util;

    public class SlowGooTrap : Trap {
        [SerializeField]
        private float slowDuration = 5f;
        [SerializeField]
        private float slowedPlayerSpeedPercentage = 0.5f;
        [SerializeField]
        private bool isAlwaysActive = true;
        [SerializeField]
        private float upTime = 3f;
        [SerializeField]
        private float downTime = 6f;
        [SerializeField]
        private float upDownTimeMaxVariation = 1f;

        //TODO Add sfx

        private bool active;
        private float activeTime;
        private float inactiveTime;
        private SpriteRenderer sprite;
        private Collider2D collider2d;

        protected override void LocalInitialize() {
            this.collider2d = this.gameObject.GetComponent<Collider2D>();
            this.sprite = this.gameObject.GetComponent<SpriteRenderer>();
        }

        protected override void LocalReInitialize() {
            active = true;
            activeTime = upTime;
            inactiveTime = downTime;
        }

        protected override void LocalDeallocate() {
        }

        protected override void LocalDelete() {
        }

        protected override void LocalCollision(Collision2D coll) {
        }

        protected void OnTriggerEnter2D(Collider2D coll) {
            if (coll.gameObject.tag == Enums.Tags.Hero.ToString()) {
                coll.gameObject.GetComponent<HeroBehavior>().AlterPlayerMaxSpeed(slowedPlayerSpeedPercentage, slowDuration);
            }
        }

        protected override void LocalUpdate() {
            this.collider2d.enabled = active;
            sprite.sprite = active ? sprites[1] : sprites[0];

            if (!isAlwaysActive) {
                if (active) {
                    activeTime -= Time.deltaTime;
                } else {
                    inactiveTime -= Time.deltaTime;
                }

                if (activeTime <= 0f || inactiveTime <= 0f) {
                    activeTime = upTime + Random.Range(-upDownTimeMaxVariation, upDownTimeMaxVariation);
                    inactiveTime = downTime + Random.Range(-upDownTimeMaxVariation, upDownTimeMaxVariation);
                    active = !active;
                }
            }
        }
    }
}

