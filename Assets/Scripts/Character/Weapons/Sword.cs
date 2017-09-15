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
        [SerializeField]
        private ContactDamage damageDealer;

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
            this.damageDealer.level = this.Level;
        }

        public override bool WeaponUpdate()
        {
            if(!this.doOnce)
            {
                this.sfx.PlaySong(0);
                this.doOnce = true;
            }

            this.arc += Time.deltaTime * (this.swingSpeed + (50f * this.Level));
            this.transform.localRotation = Quaternion.Euler(0, 0, -this.arc);
            return this.arc >= this.arcLength;
        }

        protected override void LocalCleanUp()
        {
        }
    }
}
