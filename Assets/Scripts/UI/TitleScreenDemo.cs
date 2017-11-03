namespace ShiftingDungeon.UI
{

    using Managers;
    using UnityEngine;

    /// <summary>
    /// Controls the demo of the game on the title screen.
    /// </summary>
    public class TitleScreenDemo : MonoBehaviour
    {

        /// <summary> The amount of time to display a room before changing rooms. </summary>
        [SerializeField]
        private float roomSwitchTime = 30;
        /// <summary> Timer for switching between rooms in the demo. </summary>
        private float roomSwitchTimer = 0;
        /// <summary> Whether the demo is about to be reset. </summary>
        public bool IsResetting {
            get; private set;
        }

        /// <summary>
        /// Initializes the object.
        /// </summary>
        private void Start() {
            
        }

        /// <summary>
        /// Updates the object.
        /// </summary>
        private void Update() {
            roomSwitchTimer += Time.deltaTime;
            if (roomSwitchTimer > roomSwitchTime)
            {
                Managers.DungeonManager.StartResetTitleScreen();
                ResetTitleScreen();
            }
        }

        /// <summary> Prepares for the demo to change rooms. </summary>
        public void StartReset()
        {
            IsResetting = true;
        }

        /// <summary> Resets the demo when the map changes. </summary>
        public void ResetTitleScreen()
        {
            roomSwitchTimer = 0;
            IsResetting = false;
        }
    }
}