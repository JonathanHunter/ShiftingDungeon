namespace ShiftingDungeon.Character.Weapons
{
    using UnityEngine;
    using ObjectPooling;
    using Util;

    public class TriShotGun : Weapon
    {
        [SerializeField]
        private Enums.BulletTypes type = Enums.BulletTypes.HeroBasic;
        [SerializeField]
        private float bulletSpeed = 0;
        [SerializeField]
        private float lagTime = 0;
        [SerializeField]
        private float bulletSpread = 2;
        [SerializeField]
        private SoundPlayer sfx;

        private bool doOnce = false;
        private float lag = 0;

        protected override void LocalInit()
        {
            doOnce = false;
            lag = 0;
        }

        protected override void LocalReInit()
        {
            doOnce = false;
            lag = lagTime;
        }

        public override bool WeaponUpdate()
        {
            if (!doOnce)
            {
                GameObject b = BulletPool.Instance.GetBullet(type),
                    c = BulletPool.Instance.GetBullet(type),
                    d = BulletPool.Instance.GetBullet(type);
                if (b != null)
                    SpawnDirectedBullet(b, 1);
                if (c != null)
                    SpawnDirectedBullet(c, 0);
                if (d != null)
                    SpawnDirectedBullet(d, -1);
                sfx.PlaySong(0);

                doOnce = true;
            }

            return (lag -= Time.deltaTime * (1f + .5f * Level)) <= 0;            
        }

        protected override void LocalCleanUp()
        {
        }

        private void SpawnDirectedBullet(GameObject b, int direction)
        {
            b.transform.position = transform.position;
            b.transform.rotation = transform.rotation;
            b.transform.localScale = Vector3.one;
            b.GetComponent<Rigidbody2D>().velocity = b.transform.right * (bulletSpeed + this.Level)
                + direction * b.transform.up * bulletSpeed / bulletSpread;
        }

        public float GetBulletSpeed() {
            return bulletSpeed;
        }
    }
}
