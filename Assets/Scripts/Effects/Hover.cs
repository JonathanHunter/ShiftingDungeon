namespace ShiftingDungeon.Effects
{
    using UnityEngine;

    public class Hover : MonoBehaviour
    {
        [SerializeField]
        private float speed;
        [SerializeField]
        private Vector2 distance;
        [SerializeField]
        private bool hoverOnStart = false;
        [SerializeField]
        private bool activeInCutscenes = false;

        private enum State { back, forward, center }

        private State curr;
        private Vector3 a, b;
        private float t;
        private bool hovering;

        private void Start()
        {
            PerformHover(this.hoverOnStart);
        }

        private void Update()
        {
            if (Managers.GameState.Instance.IsPaused &&
                !(this.activeInCutscenes && Managers.GameState.Instance.State == Util.Enums.GameState.Cutscene))
                return;

            if(hovering)
            {
                if (this.curr == State.back)
                {
                    if (this.t <= 0f)
                    {
                        this.t = 0f;
                        this.curr = State.forward;
                    }
                    else
                        this.t -= Time.deltaTime * this.speed;

                }
                else if (this.curr == State.forward)
                {
                    if (this.t >= 1f)
                    {
                        this.t = 1f;
                        this.curr = State.center;
                    }
                    else
                        this.t += Time.deltaTime * this.speed;
                }
                else
                {
                    if (this.t <= .5f)
                    {
                        this.t = .5f;
                        this.curr = State.back;
                    }
                    else
                        this.t -= Time.deltaTime * this.speed;
                }

                this.transform.localPosition = Vector3.Lerp(this.a, this.b, this.t);
            }
        }

        public void PerformHover(bool hover)
        {
            if (hover)
            {
                Vector3 dist3d = new Vector3(this.distance.x / 2f, this.distance.y / 2f, 0f);
                this.a = this.transform.localPosition - dist3d;
                this.b = this.transform.localPosition + dist3d;
                this.t = .5f;
            }

            this.hovering = hover;
        }
    }
}
