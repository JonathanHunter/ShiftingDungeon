namespace ShiftingDungeon.UI
{
    using UnityEngine;
    using Character.Hero;

    public class WeaponUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] sprites;

        private HeroBehavior hero;
        private int currentIndex;

        private void Start()
        {
            this.hero = Managers.DungeonManager.GetHero().GetComponent<HeroBehavior>();
            this.currentIndex = 0;
            this.sprites[0].SetActive(true);
        }

        private void Update()
        {
            if(this.hero.CurrentWeapon != currentIndex)
            {
                this.sprites[this.currentIndex].SetActive(false);
                this.currentIndex = this.hero.CurrentWeapon;
                this.sprites[this.currentIndex].SetActive(true);
            }
        }
    }
}
