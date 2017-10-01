namespace ShiftingDungeon.Character.Hero {

    using UnityEngine;

    /// <summary> Displays on the enemy that the hero targets. </summary>
    class Crosshair : MonoBehaviour {

        /// <summary> The crosshair sprite. </summary>
        private SpriteRenderer sprite;
        /// <summary> The amount of time (seconds) that the crosshair is visible for. </summary>
        [SerializeField]
        private float visibleTime = 0.5f;

        /// <summary> Caches the object's components. </summary>
        private void Start()
        {
            sprite = GetComponent<SpriteRenderer>();
            gameObject.SetActive(false);
        }

        /// <summary> Animates the crosshair. </summary>
        private void Update()
        {
            Color color = sprite.color;
            if (color.a > 0)
            {
                color.a -= Time.deltaTime / visibleTime;
                sprite.color = color;
            }
            else
                gameObject.SetActive(false);
        }

        /// <summary>
        /// Sets the crosshair at a certain position.
        /// </summary>
        /// <param name="position">The new crosshair position.</param>
        public void Target(Vector2 position)
        {
            transform.position = position;
            Color color = sprite.color;
            color.a = 1;
            sprite.color = color;
            gameObject.SetActive(true);
        }
    }
}