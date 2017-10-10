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
                bar.anchoredPosition = new Vector3(originalPosition.x - 50f * (1 - percent), originalPosition.y, originalPosition.z);
                bar.localScale = new Vector3(originalScale.x * percent, originalScale.y, originalScale.z);
            }
        }
    }
}
