namespace ShiftingDungeon.Character.Weapons.Bullets
{
    using UnityEngine;
    using Util;

    public class HeroBullet : Bullet
    {
        [SerializeField]
        private SoundPlayer sfx;

        protected override void LocalUpdate()
        {
        }

        protected override void LocalInitialize()
        {
        }

        protected override void LocalReInitialize()
        {
        }
        protected override void LocalDeallocate()
        {
        }

        protected override void LocalDelete()
        {
        }

        protected override bool ShouldDestroyBullet(Collider2D collider)
        {
            if (collider.gameObject.GetComponent<Bullet>() != null)
                return false;

            if (collider.gameObject.tag == Enums.Tags.Pickup.ToString())
            {
                if (collider.gameObject.GetComponent<Pickups.Money>() != null)
                {
                    this.sfx.PlaySong(0);
                    Pickups.Money gold = collider.gameObject.GetComponent<Pickups.Money>();
                    Hero.HeroData.Instance.money += gold.Value;
                    ObjectPooling.PickupPool.Instance.ReturnGold(gold.gameObject);
                    return false;
                }
            }

            return true;
        }
    }
}
