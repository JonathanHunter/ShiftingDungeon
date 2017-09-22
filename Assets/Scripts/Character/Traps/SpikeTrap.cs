namespace ShiftingDungeon.Character.Traps
{
    using Util;
    using UnityEngine;
    public class SpikeTrap : Trap
    {
        
        [SerializeField]
        private float upTime = 3f;
        [SerializeField]
        private float downTime = 6f;

        
        //TODO Add sfx

        private bool active;
        private float activeTime;
        private float inactiveTime;
        private SpriteRenderer sprite;
        // Update is called once per frame

        protected override void LocalInitialize()
        {
            damage = 1;
        }

        protected override void LocalReInitialize()
        {
            active = false;
            activeTime = upTime;
            inactiveTime = downTime;
        }

        protected override void LocalDeallocate()
        {
        }

        protected override void LocalDelete()
        {
        }

        protected override void LocalCollision(Collision2D coll)
        {
        }

        protected override void LocalUpdate()
        {
            if (sprite == null) sprite = this.gameObject.GetComponent<SpriteRenderer>();
            this.gameObject.GetComponent<Collider2D>().enabled = active;
            sprite.sprite = active ? sprites[1] : sprites[0];
            if (active)
            { 
                activeTime -= Time.deltaTime;
            }
            else
            {
                inactiveTime -= Time.deltaTime;
            }
            if (activeTime <= 0f || inactiveTime <= 0f)
            {
                activeTime = upTime;
                inactiveTime = downTime;
                active = !active;
            }
        }
    }
}

