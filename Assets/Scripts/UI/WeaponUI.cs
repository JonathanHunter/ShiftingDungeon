namespace ShiftingDungeon.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using Character.Hero;

    public class WeaponUI : MonoBehaviour
    {
        [SerializeField]
        private Image image;
        [SerializeField]
        private Sprite[] sprites;

        private HeroBehavior hero;
        private int currentIndex;

        private void Start()
        {
            this.hero = FindObjectOfType<HeroBehavior>();
            this.currentIndex = 0;
            this.image.sprite = sprites[0];
        }

        private void Update()
        {
            if(this.hero.CurrentWeapon != currentIndex)
            {
                this.currentIndex = this.hero.CurrentWeapon;
                this.image.sprite = sprites[this.currentIndex];
            }
        }
    }
}
