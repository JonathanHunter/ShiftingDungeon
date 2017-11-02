namespace ShiftingDungeon.Dungeon
{
    using UnityEngine;
    using UnityEngine.UI;
    using Character.Hero;

    public class HealthStore : MonoBehaviour
    {
        [SerializeField]
        private GameObject storeItem;
        [SerializeField]
        private int baseCost;
        [SerializeField]
        private Text healthLabel;
        [SerializeField]
        private Text shopkeeper;
        
        private void Start()
        {
            this.storeItem.SetActive(true);
            this.healthLabel.gameObject.SetActive(true);
            this.healthLabel.text = this.baseCost + "";
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == Util.Enums.Tags.Hero.ToString() && this.storeItem.activeSelf)
            {
                this.shopkeeper.text = "Press Attack to buy this health refill!";
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.tag == Util.Enums.Tags.Hero.ToString() && this.storeItem.activeSelf)
            {
                if (Util.CustomInput.BoolFreshPress(Util.CustomInput.UserInput.Attack))
                {
                    int cost = this.baseCost;
                    if (cost <= HeroData.Instance.money)
                    {
                        HeroData.Instance.money -= cost;
                        Managers.DungeonManager.GetHero().GetComponent<HeroBehavior>().AddHealth(2);
                        this.storeItem.SetActive(false);
                        this.healthLabel.gameObject.SetActive(false);
                        this.shopkeeper.text = "Thank you!";
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == Util.Enums.Tags.Hero.ToString() && this.storeItem.activeSelf)
            {
                this.shopkeeper.text = "Please buy something!";
            }
        }
    }
}
