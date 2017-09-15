namespace ShiftingDungeon.UI
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class PlayButton : MonoBehaviour
    {
        public string Scene;

        public void Load()
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            SceneManager.LoadScene(this.Scene);
        }
    }
}
