namespace ShiftingDungeon.Character.Weapons
{
    using UnityEngine;
    using Util;

    public class Sword : Weapon
    {
        [SerializeField]
        private float swingSpeed = 400f;
        [SerializeField]
        private float arcLength = 120f;
        [SerializeField]
        private SoundPlayer sfx;

        private bool doOnce;
        private float arc;        

        protected override void LocalInit()
        {
            this.arc = 0;
        }

        protected override void LocalReInit()
        {
            this.transform.localRotation = Quaternion.identity;
            this.arc = 0;
            this.doOnce = false;
        }

        public override bool WeaponUpdate()
        {
            if(!this.doOnce)
            {
                this.sfx.PlaySong(0);
                this.doOnce = true;
            }

            this.arc += Time.deltaTime * this.swingSpeed;
            this.transform.localRotation = Quaternion.Euler(0, 0, -this.arc);
            return this.arc >= this.arcLength;
        }

        protected override void LocalCleanUp()
        {
        }
    }
}
