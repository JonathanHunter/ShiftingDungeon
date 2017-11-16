namespace ShiftingDungeon.Scripts.UI
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class End : MonoBehaviour
    {
        public string scene; 

        private void Update()
        {
            if(Util.CustomInput.BoolFreshPress(Util.CustomInput.UserInput.Accept) ||
                Util.CustomInput.BoolFreshPress(Util.CustomInput.UserInput.Attack) ||
                Util.CustomInput.BoolFreshPress(Util.CustomInput.UserInput.Cancel) ||
                Util.CustomInput.BoolFreshPress(Util.CustomInput.UserInput.Pause))
            {
                Managers.GameState.Instance.State = Util.Enums.GameState.Playing;
                Character.Hero.HeroData.Instance.ResetData();
                Random.InitState(System.DateTime.Now.Millisecond);
                SceneManager.LoadScene(this.scene);
            }
        }
    }
}
