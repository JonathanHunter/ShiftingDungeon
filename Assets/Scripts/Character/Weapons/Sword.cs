namespace ShiftingDungeon.Character.Weapons
{
    using UnityEngine;

    public class Sword : Weapon
    {
        [SerializeField]
        private float swingSpeed = 400f;
        [SerializeField]
        private float arcLength = 120f;

        private float arc = 0;

        protected override void LocalInit()
        {
            this.arc = 0;
        }

        protected override void LocalReInit()
        {
            this.transform.localRotation = Quaternion.identity;
            this.arc = 0;
        }

        public override bool WeaponUpdate()
        {
            this.arc += Time.deltaTime * this.swingSpeed;
            this.transform.localRotation = Quaternion.Euler(0, 0, -this.arc);
            return this.arc >= this.arcLength;
        }

        protected override void LocalCleanUp()
        {
        }
    }
}
