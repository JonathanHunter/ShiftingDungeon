namespace ShiftingDungeon.Effects
{
    using UnityEngine;

    public class Squish : MonoBehaviour
    {
        [SerializeField]
        private float angularFrequency = 3.5f;
        [SerializeField]
        private float phase = 0f;
        [SerializeField]
        private float amplitude = .55f;
        [SerializeField]
        private float minValue = .5f;
        [SerializeField]
        private float speedIncrease = 2f;
        [SerializeField]
        private bool squishOnStart = false;
        [SerializeField]
        private bool activeInCutscenes = false;

        /// <summary> WHether to run at twice the speed. </summary>
        public bool DoubleSpeed { get; set; }

        private bool squish;
        private float theta;
        private Vector3 originalSize;
        private Vector3 originalPos;

        private void Start()
        {
            this.originalSize = this.transform.localScale;
            this.originalPos = this.transform.localPosition;
            this.squish = this.squishOnStart;
            this.theta = 0f;
        }

        private void Update()
        {
            if (Managers.GameState.Instance.IsPaused && 
                !(this.activeInCutscenes && Managers.GameState.Instance.State == Util.Enums.GameState.Cutscene))
                return;

            if (squish)
            {
                float sizeChange = Mathf.Abs(Mathf.Sin(theta + this.phase) * this.amplitude) + this.minValue;
                if (sizeChange > 1f)
                    sizeChange = 1f;

                this.transform.localScale = new Vector3(
                    this.originalSize.x + ((1 - sizeChange) / 2f),
                    this.originalSize.y - (1 - sizeChange),
                    this.originalSize.z);
                if (this.DoubleSpeed)
                    this.theta += Time.deltaTime * this.angularFrequency * this.speedIncrease;
                else
                    this.theta += Time.deltaTime * this.angularFrequency;

                if (this.theta > 2f * Mathf.PI)
                    this.theta = 0;
            }
        }

        /// <summary> Start the squishing animation. </summary>
        public void StartSquishing()
        {
            this.squish = true;
        }

        /// <summary> Stop the squishing animation. </summary>
        public void StopSquishing()
        {
            this.squish = false;
            this.theta = 0;
            this.transform.localScale = this.originalSize;
            this.transform.localPosition = this.originalPos;
        }
    }
}
