namespace ShiftingDungeon.Managers
{
    using UnityEngine;
    using Util;

    public class GameState : MonoBehaviour
    {
        private static GameState instance;
        /// <summary> The GameState for this scene. </summary>
        public static GameState Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<GameState>();

                return instance;
            }
        }

        /// <summary> If true the game is paused. </summary>
        public bool IsPaused { get { return State != Enums.GameState.Playing; } }

        /// <summary> The game's current state. </summary>
        public Enums.GameState State { get; set; }

        private void Start()
        {
            if (instance != null && instance != this)
            {
                Debug.LogError("Duplicate GameState detected: removing " + this.gameObject.name);
                Destroy(this.gameObject);
                return;
            }

            instance = this;
        }

    }
}
