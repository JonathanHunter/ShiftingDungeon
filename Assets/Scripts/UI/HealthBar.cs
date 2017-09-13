namespace ShiftingDungeon.UI
{
    using UnityEngine;
    using Character.Hero;

    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private RectTransform bar;

        private HeroBehavior hero;
        private Vector3 originalPosition;
        private Vector3 originalScale;

        private void Start()
        {
            this.hero = Managers.DungeonManager.GetHero().GetComponent<HeroBehavior>();
            this.originalPosition = bar.anchoredPosition;
            this.originalScale = this.transform.localScale;
        }

        private void Update()
        {
            if (this.hero.Health >= 0)
            {
                float percent = ((float)this.hero.Health / (float)this.hero.MaxHealth);
                int difference = this.hero.MaxHealth - this.hero.Health;
                bar.anchoredPosition = new Vector3(originalPosition.x - (5f * difference), originalPosition.y, originalPosition.z);
                bar.localScale = new Vector3(originalScale.x * percent, originalScale.y, originalScale.z);
            }
        }
    }
}
