namespace ShiftingDungeon.Dungeon
{
    using UnityEngine;
    using UnityEngine.UI;
    using Character.Hero;

    public class WeaponStore : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer storeItem;
        [SerializeField]
        private Text storeItemLevel;
        [SerializeField]
        private int baseCost;
        [SerializeField]
        private Text weaponLabel;
        [SerializeField]
        private Text shopkeeper;
        [SerializeField]
        private int weaponIndex;
        [SerializeField]
        private Sprite[] weaponLevelArt;
        
        private void Start()
        {
            int level = HeroData.Instance.weaponLevels[this.weaponIndex] + 1;
            if(level < 3)
            {
                this.storeItem.sprite = this.weaponLevelArt[0];
                this.storeItemLevel.text = "Lv. " + (level + 1);
                this.storeItem.gameObject.SetActive(true);
            }
            else if(level < 6)
            {
                this.storeItem.sprite = this.weaponLevelArt[1];
                this.storeItemLevel.text = "Lv. " + ((level - 3) + 1);
                this.storeItem.gameObject.SetActive(true);
            }
            else if(level < 9)
            {
                this.storeItem.sprite = this.weaponLevelArt[2];
                this.storeItemLevel.text = "Lv. " + ((level - 6) + 1);
                this.storeItem.gameObject.SetActive(true);
            }
            else 
                this.storeItem.gameObject.SetActive(false);

            this.weaponLabel.gameObject.SetActive(true);
            this.weaponLabel.text = (this.baseCost * (level)) + "";
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == Util.Enums.Tags.Hero.ToString() && this.storeItem.gameObject.activeSelf)
            {
                this.shopkeeper.text = "Press Attack to buy this weapon upgrade!";
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.tag == Util.Enums.Tags.Hero.ToString() && this.storeItem.gameObject.activeSelf)
            {
                if (Util.CustomInput.BoolFreshPress(Util.CustomInput.UserInput.Attack))
                {
                    int cost = (this.baseCost * (HeroData.Instance.weaponLevels[this.weaponIndex] + 1));
                    if (cost <= HeroData.Instance.money)
                    {
                        HeroData.Instance.money -= cost;
                        HeroData.Instance.weaponLevels[this.weaponIndex]++;
                        Managers.DungeonManager.GetHero().GetComponent<HeroBehavior>().UpdateWeaponLevel(
                            this.weaponIndex, HeroData.Instance.weaponLevels[this.weaponIndex]);
                        this.storeItem.gameObject.SetActive(false);
                        this.weaponLabel.gameObject.SetActive(false);
                        this.shopkeeper.text = "Thank you!";
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == Util.Enums.Tags.Hero.ToString() && this.storeItem.gameObject.activeSelf)
            {
                this.shopkeeper.text = "Please buy something!";
            }
        }
    }
}
