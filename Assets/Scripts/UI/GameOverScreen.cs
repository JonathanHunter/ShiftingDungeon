namespace ShiftingDungeon.UI
{

    using UnityEngine;
    using UnityEngine.UI;
    using Managers;

    /// <summary>
    /// 
    /// </summary>
    class GameOverScreen : MonoBehaviour
    {

        /// <summary> The current amount of time (seconds) to wait before the level restarts. </summary>
        [SerializeField]
        private float restartTime;
        /// <summary> The total amount of time to wait before the level restarts. </summary>
        private float initRestartTime;
        /// <summary> The amount of time (seconds) for the game over text to fade in. </summary>
        [SerializeField]
        private float textAppearTime;
        /// <summary> The amount of time (seconds) before the game over text starts to fade in. </summary>
        [SerializeField]
        private float textStartAppearTime;
        /// <summary> The amount of time (seconds) that the screen has been visible. </summary>
        private float screenDuration = 0;
        /// <summary> The text displaying "Game Over". </summary>
        private Text text;
        /// <summary> The background for the text. </summary>
        private Image background;

        /// <summary>
        /// Initializes the object.
        /// </summary>
        private void Start()
        {
            text = GetComponentInChildren<Text>();
            background = transform.Find("Background").GetComponent<Image>();
            initRestartTime = restartTime;
            Reset();
        }

        /// <summary>
        /// Updates the object.
        /// </summary>
        private void Update()
        {
            restartTime -= Time.deltaTime;
            screenDuration += Time.deltaTime;
            if (text.color.a < 1 && screenDuration > textStartAppearTime)
            {
                Color textColor = text.color;
                float newAlpha = (screenDuration - textStartAppearTime) / textAppearTime;
                textColor.a = newAlpha;
                text.color = textColor;

                Color backgroundColor = background.color;
                backgroundColor.a = newAlpha * 0.7f;
                background.color = backgroundColor;
            }
            if (restartTime <= 0)
            {
                DungeonManager.RestartFloor();

                Reset();
            }
        }

        /// <summary> Resets the game over screen to its initial state before being shown. </summary>
        private void Reset()
        {
            Color textColor = text.color;
            textColor.a = 0;
            text.color = textColor;
            Color backgroundColor = background.color;
            backgroundColor.a = 0;
            background.color = backgroundColor;
            screenDuration = 0;
            restartTime = initRestartTime;
            gameObject.SetActive(false);
        }
    }
}