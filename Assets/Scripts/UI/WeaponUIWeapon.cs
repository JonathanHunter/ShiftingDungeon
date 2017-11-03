namespace ShiftingDungeon.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using Character.Hero;

    public class WeaponUIWeapon : MonoBehaviour
    {
        [SerializeField]
        private Image weaponIcon;
        [SerializeField]
        private Text weaponLevel;
        [SerializeField]
        private Sprite[] weaponLevelArt;
        [SerializeField]
        private int weaponIndex;

        private int currentLevel;

        private void Start()
        {
            this.currentLevel = HeroData.Instance.weaponLevels[this.weaponIndex];
            UpdateIcon();
        }

        private void Update()
        {
            if (HeroData.Instance.weaponLevels[this.weaponIndex] != currentLevel)
            {
                this.currentLevel = HeroData.Instance.weaponLevels[this.weaponIndex];
                UpdateIcon();
            }
        }

        private void UpdateIcon()
        {
            if (this.currentLevel < 3)
            {
                this.weaponIcon.sprite = this.weaponLevelArt[0];
                this.weaponLevel.text = "Lv. " + (this.currentLevel + 1);
                this.weaponIcon.gameObject.SetActive(true);
            }
            else if (this.currentLevel < 6)
            {
                this.weaponIcon.sprite = this.weaponLevelArt[1];
                this.weaponLevel.text = "Lv. " + ((this.currentLevel - 3) + 1);
                this.weaponIcon.gameObject.SetActive(true);
            }
            else if (this.currentLevel < 9)
            {
                this.weaponIcon.sprite = this.weaponLevelArt[2];
                this.weaponLevel.text = "Lv. " + ((this.currentLevel - 6) + 1);
                this.weaponIcon.gameObject.SetActive(true);
            }

        }
    }
}
