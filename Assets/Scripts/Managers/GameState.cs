namespace ShiftingDungeon.Managers
{
    using UnityEngine;

    public class GameState : MonoBehaviour
    {
        [SerializeField]
        private bool isPaused;

        public static GameState Instance { get; private set; }
        
        public bool IsPaused { get { return this.isPaused; } }

        private void Start()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Duplicate GameState detected: removing " + this.gameObject.name);
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
        }

    }
}
