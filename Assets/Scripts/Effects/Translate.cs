namespace ShiftingDungeon.Effects
{
    using UnityEngine;

    public class Translate : MonoBehaviour
    {
        [SerializeField]
        private float speed;
        [SerializeField]
        private Transform endPoint;
        [SerializeField]
        private bool translateOnStart = false;
        [SerializeField]
        private bool activeInCutscenes = false;

        public bool Finished { get { return t >= 1f; } }

        private enum State { back, forward }

        private State curr;
        private Vector3 start, end;
        private float t;

        private void Start()
        {
            this.curr = State.back;
            this.start = this.transform.localPosition;
            this.end = this.endPoint.localPosition;
            this.t = 1f;
            if (this.translateOnStart)
                StartTranslate(true);
        }

        private void Update()
        {
            if (Managers.GameState.Instance.IsPaused &&
                !(this.activeInCutscenes && Managers.GameState.Instance.State == Util.Enums.GameState.Cutscene))
                return;

            if (t < 1f)
            {
                this.t += Time.deltaTime * this.speed;
                if (this.curr == State.back)
                    this.transform.localPosition = Vector3.Lerp(this.end, this.start, this.t);
                else if (this.curr == State.forward)
                    this.transform.localPosition = Vector3.Lerp(this.start, this.end, this.t);
            }
        }

        public void StartTranslate(bool forward)
        {
            this.t = 0f;
            this.curr = forward ? State.forward : State.back;
        }
    }
}
