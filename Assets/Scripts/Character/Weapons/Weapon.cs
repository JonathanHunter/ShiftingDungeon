namespace ShiftingDungeon.Character.Weapons
{
    using UnityEngine;

    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer sprite = null;
        [SerializeField]
        private bool noSprite = false;

        /// <summary> The sprites for 3 weapon levels. </summary>
        [SerializeField]
        protected Sprite[] weaponLevelArt = null;

        /// <summary> The level of this weapon. </summary>
        public int Level { get; private set; }
        
        /// <summary> Initializes this weapon. </summary>
        public void Init(int level)
        {
            UpdateLevel(level);
            LocalInit();
        }

        public void UpdateLevel(int level)
        {
            this.Level = level;
            if (!this.noSprite)
            {
                if (this.Level < 3)
                    this.sprite.sprite = this.weaponLevelArt[0];
                else if (this.Level < 6)
                    this.sprite.sprite = this.weaponLevelArt[1];
                else if (this.Level < 9)
                    this.sprite.sprite = this.weaponLevelArt[2];
            }
        }

        /// <summary> Readys this weapon to attack. </summary>
        public void ReInit()
        {
            LocalReInit();
            this.gameObject.SetActive(true);
        }

        /// <summary> Cleans up this object </summary>
        public void CleanUp()
        {
            LocalCleanUp();
            this.gameObject.SetActive(false);
        }

        /// <summary> Performs this weapons attack. </summary>
        /// <returns> True if this weapons attack is finished. </returns>
        public abstract bool WeaponUpdate();

        /// <summary> Local Init for subclasses. </summary>
        protected abstract void LocalInit();
        /// <summary> Local ReInit for subclasses. </summary>
        protected abstract void LocalReInit();
        /// <summary> Local CleanUp for subclasses. </summary>
        protected abstract void LocalCleanUp();
    }
}
