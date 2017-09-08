namespace ShiftingDungeon.UI
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class PlayButton : MonoBehaviour
    {
        public string Scene;

        public void Load()
        {
            SceneManager.LoadScene(this.Scene);
        }
    }
}
