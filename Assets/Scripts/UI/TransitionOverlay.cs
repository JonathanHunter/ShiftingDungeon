namespace ShiftingDungeon.UI
{
    using UnityEngine;

    public class TransitionOverlay : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup overlay = null;
        [SerializeField]
        private float speed = 2;

        private float alpha;
        private bool decreasing;

        private void Start()
        {
            this.alpha = 0;
            this.decreasing = true;
        }

        private void Update()
        {
            if (this.alpha > 0 && this.decreasing)
            {
                this.alpha -= Time.deltaTime * this.speed;
                if (this.alpha < 0)
                    this.alpha = 0;

                this.overlay.alpha = this.alpha;
            }

            if (this.alpha < 1 && !this.decreasing)
            {
                this.alpha += Time.deltaTime * this.speed;
                if (this.alpha < 0)
                    this.alpha = 0;

                this.overlay.alpha = this.alpha;
            }
        }

        public void FadeIn()
        {
            this.overlay.alpha = 0f;
            this.alpha = 0f;
            this.decreasing = false;
        }

        public void Show()
        {
            this.overlay.alpha = 1f;
            this.alpha = 1f;
            this.decreasing = false;
        }

        public void FadeOut()
        {
            this.overlay.alpha = 1f;
            this.alpha = 1f;
            this.decreasing = true;
        }
    }
}
