namespace ShiftingDungeon.Character.Weapons
{
    using UnityEngine;
    using ObjectPooling;
    using Util;

    public class Gun : Weapon
    {
        [SerializeField]
        private Enums.BulletTypes type = Enums.BulletTypes.HeroBasic;
        [SerializeField]
        private float bulletSpeed = 0;
        [SerializeField]
        private float lagTime = 0;
        [SerializeField]
        private SoundPlayer sfx;

        public float BulletSpeed { get { return bulletSpeed; } }

        private bool doOnce = false;
        private float lag = 0;

        protected override void LocalInit()
        {
            this.doOnce = false;
            this.lag = 0;
        }

        protected override void LocalReInit()
        {
            this.doOnce = false;
            this.lag = lagTime;
        }

        public override bool WeaponUpdate()
        {
            if (!this.doOnce)
            {
                GameObject b = BulletPool.Instance.GetBullet(type);
                if (b != null)
                {
                    b.transform.position = this.transform.position;
                    b.transform.rotation = this.transform.rotation;
                    b.transform.localScale = Vector3.one;
                    b.GetComponent<Rigidbody2D>().velocity = -b.transform.up * (bulletSpeed + this.Level);
                    this.sfx.PlaySong(0);
                }

                this.doOnce = true;
            }

            return (this.lag -= Time.deltaTime * (1f + .5f * this.Level)) <= 0;            
        }

        protected override void LocalCleanUp()
        {
        }
    }
}
