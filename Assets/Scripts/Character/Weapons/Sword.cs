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

        private bool doOnce;
        private float arc;

        private SpriteRenderer spriteRenderer;
        private Rigidbody2D playerBody;

        protected override void LocalInit()
        {
            this.playerBody = GetComponentInParent<Rigidbody2D>();
            this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            this.arc = 0;
            this.maxCombo = 3;
        }

        protected override void LocalReInit()
        {
            this.transform.localRotation = Quaternion.identity;
            this.arc = 0;
            this.doOnce = false;
            this.damageDealer.level = this.Level;

            Vector3 spritePosition;
            switch (comboCounter)
            {
                //Lunge left
                case 1:
                    this.spriteRenderer.flipY = true;
                    spritePosition = this.transform.GetChild(0).transform.localPosition;
                    spritePosition.y = -Mathf.Abs(spritePosition.y);
                    this.transform.GetChild(0).transform.localPosition = spritePosition;
                    this.playerBody.AddForce(playerBody.transform.right * lungeForce, ForceMode2D.Impulse);
                    break;
                //Lunge right
                case 2:
                    this.spriteRenderer.flipY = false;
                    spritePosition = this.transform.GetChild(0).transform.localPosition;
                    spritePosition.y = Mathf.Abs(spritePosition.y);
                    this.transform.GetChild(0).transform.localPosition = spritePosition;
                    this.playerBody.AddForce(playerBody.transform.right * lungeForce, ForceMode2D.Impulse);
                    break;
                //Swing right
                default:
                    this.spriteRenderer.flipY = false;
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


            
            switch (comboCounter)
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
        }
    }
}
