namespace ShiftingDungeon.Character.Weapons
{
    using UnityEngine;
    using Util;

    public class Sword : Weapon
    {
        [SerializeField]
        private float swingSpeed = 400f;
        [SerializeField]
        private float lungeForce = 5f;
        [SerializeField]
        private float arcLength = 120f;
        [SerializeField]
        private SoundPlayer sfx;
        [SerializeField]
        private ContactDamage damageDealer;

        /// <summary> The numerical progression this weapon's combo </summary>
        public int ComboCounter { get; private set; }
        /// <summary> The maximum number of attacks in a weapon's combo </summary>
        private int maxCombo;
        private bool doOnce;
        private float arc;
        /// <summary> The last game time moment an attack was made </summary>
        private float lastAttackTime;
        /// <summary> The maximum time interval between attacks to continue the combo in milliseconds </summary>
        private float comboInterval = 400f;

        private Transform child;
        private Rigidbody2D playerBody;

        protected override void LocalInit()
        {
            this.playerBody = GetComponentInParent<Rigidbody2D>();
            this.child = this.transform.GetChild(0);
            this.arc = 0;
            this.maxCombo = 3;
        }

        protected override void LocalReInit()
        {
            this.ComboCounter = Time.time - lastAttackTime <= comboInterval / 1000f 
                ? (ComboCounter) % (maxCombo - 1) + 1 : 0;
            this.transform.localRotation = Quaternion.identity;
            this.arc = 0;
            this.doOnce = false;
            this.damageDealer.level = this.Level;

            Vector3 spritePosition;
            switch (ComboCounter)
            {
                //Lunge left
                case 1:
                    this.child.localRotation = Quaternion.Euler(0, 0, 180);
                    spritePosition = this.transform.GetChild(0).transform.localPosition;
                    spritePosition.y = -Mathf.Abs(spritePosition.y);
                    this.transform.GetChild(0).transform.localPosition = spritePosition;
                    //this.playerBody.AddForce(playerBody.transform.right * lungeForce, ForceMode2D.Impulse);
                    break;
                //Lunge right
                case 2:
                    this.child.localRotation = Quaternion.Euler(0, 0, 0);
                    spritePosition = this.transform.GetChild(0).transform.localPosition;
                    spritePosition.y = Mathf.Abs(spritePosition.y);
                    this.transform.GetChild(0).transform.localPosition = spritePosition;
                    //this.playerBody.AddForce(playerBody.transform.right * lungeForce, ForceMode2D.Impulse);
                    break;
                //Swing right
                default:
                    this.child.localRotation = Quaternion.Euler(0, 0, 0);
                    spritePosition = this.transform.GetChild(0).transform.localPosition;
                    spritePosition.y = Mathf.Abs(spritePosition.y);
                    this.transform.GetChild(0).transform.localPosition = spritePosition;
                    break;
            }
        }

        public override bool WeaponUpdate()
        {
            if(!this.doOnce)
            {
                this.sfx.PlaySong(0);
                this.doOnce = true;
            }
            
            switch (ComboCounter)
            {
                //Lunge left
                case 1:
                    this.arc += Time.deltaTime * (this.swingSpeed + (50f * this.Level));
                    this.transform.localRotation = Quaternion.Euler(0, 0, this.arc);
                    return this.arc >= this.arcLength;
                //Lunge right
                case 2:
                    this.arc += Time.deltaTime * (this.swingSpeed + (50f * this.Level));
                    this.transform.localRotation = Quaternion.Euler(0, 0, -this.arc);
                    return this.arc >= this.arcLength;
                //Swing right
                default:
                    this.arc += Time.deltaTime * (this.swingSpeed + (50f * this.Level));
                    this.transform.localRotation = Quaternion.Euler(0, 0, -this.arc);
                    return this.arc >= this.arcLength;
            }

            
        }

        protected override void LocalCleanUp()
        {
            lastAttackTime = Time.time;
        }
    }
}
