namespace ShiftingDungeon.Managers
{
    using UnityEngine;

    public class CutsceneManager : MonoBehaviour
    {
        private static CutsceneManager instance;
        /// <summary> The CutsceneManager for this scene. </summary>
        public static CutsceneManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<CutsceneManager>();

                return instance;
            }
        }

        /// <summary> Reference to the global image renderer for cutscenes. </summary>
        public UnityEngine.UI.Image globalImage;
        /// <summary> Reference to the global text box for cutscenes. </summary>
        public UnityEngine.UI.Text globalText;
        /// <summary> Reference to the global canvas for cutscenes. </summary>
        public GameObject globlaCanvas;

        private void Start()
        {
            if (instance != null && instance != this)
            {
                Debug.LogError("Duplicate CutsceneManager detected: removing " + this.gameObject.name);
                Destroy(this.gameObject);
                return;
            }

            instance = this;
        }
    }
}
