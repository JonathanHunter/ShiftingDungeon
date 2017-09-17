namespace ShiftingDungeon.Dungeon
{
    using UnityEngine;
    using UnityEngine.UI;
    using Character.Hero;

    public class Store : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] storeItems;
        [SerializeField]
        private int[] baseCosts;
        [SerializeField]
        private Text storeLabel;

        private int currentItem;

        private void Start()
        {
            this.currentItem = Random.Range(0, this.storeItems.Length);
            this.storeItems[this.currentItem].SetActive(true);
            this.storeLabel.gameObject.SetActive(true);
            this.storeLabel.text = (this.baseCosts[this.currentItem] * (HeroData.Instance.weaponLevels[this.currentItem] + 1)) + "";
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == Util.Enums.Tags.Hero.ToString() && this.storeItems[this.currentItem].activeSelf)
            {
                int cost = (this.baseCosts[this.currentItem] * (HeroData.Instance.weaponLevels[this.currentItem] + 1));
                if (cost <= HeroData.Instance.money)
                {
                    HeroData.Instance.money -= cost;
                    HeroData.Instance.weaponLevels[this.currentItem]++;
                    this.storeItems[this.currentItem].SetActive(false);
                    this.storeLabel.gameObject.SetActive(false);
                }
            }
        }
    }
}
