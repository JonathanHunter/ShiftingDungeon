namespace ShiftingDungeon.UI
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.SceneManagement;

    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject mainParent;
        [SerializeField]
        private GameObject mainSelected;
        [SerializeField]
        private GameObject settingsParent;
        [SerializeField]
        private GameObject settingsSelected;
        [SerializeField]
        private GameObject creditsParent;
        [SerializeField]
        private GameObject creditsSelected;
        [SerializeField]
        private string scene;

        private GameObject currentSelected;
        private bool inMain;
        private bool inCredits;

        void Start()
        {
            inMain = true;
			inCredits = false;
            EventSystem.current.SetSelectedGameObject(mainSelected);
            Managers.GameState.Instance.SFXVolbuffer = .5f;
        }

        void Update()
        {
            if (inMain) {
				if (Input.GetKey (KeyCode.Escape))
					Application.Quit ();
				if (inMain && EventSystem.current.currentSelectedGameObject == null) {
					if (inCredits)
						EventSystem.current.SetSelectedGameObject (creditsSelected);
					else
						EventSystem.current.SetSelectedGameObject (mainSelected);
				}

				currentSelected = EventSystem.current.currentSelectedGameObject;

				if (Util.CustomInput.BoolFreshPressDeleteOnRead (Util.CustomInput.UserInput.Up))
					Navigator.Navigate (Util.CustomInput.UserInput.Up, currentSelected);
				if (Util.CustomInput.BoolFreshPressDeleteOnRead (Util.CustomInput.UserInput.Down))
					Navigator.Navigate (Util.CustomInput.UserInput.Down, currentSelected);
				if (Util.CustomInput.BoolFreshPressDeleteOnRead (Util.CustomInput.UserInput.Accept))
					Navigator.CallSubmit ();
				if (inCredits && Util.CustomInput.BoolFreshPressDeleteOnRead (Util.CustomInput.UserInput.Cancel))
					GoToMain ();
			} 
			else if (inCredits) {
				if (Input.GetKey (KeyCode.Escape))
					GoToMain();
				
				currentSelected = EventSystem.current.currentSelectedGameObject;

				if (Util.CustomInput.BoolFreshPressDeleteOnRead (Util.CustomInput.UserInput.Up))
					Navigator.Navigate (Util.CustomInput.UserInput.Up, currentSelected);
				if (Util.CustomInput.BoolFreshPressDeleteOnRead (Util.CustomInput.UserInput.Down))
					Navigator.Navigate (Util.CustomInput.UserInput.Down, currentSelected);
				if (Util.CustomInput.BoolFreshPressDeleteOnRead (Util.CustomInput.UserInput.Accept))
					Navigator.CallSubmit ();
				if (inCredits && Util.CustomInput.BoolFreshPressDeleteOnRead (Util.CustomInput.UserInput.Cancel))
					GoToMain ();
			}
        }
        
        public void GoToMain()
        {
            inMain = true;
			inCredits = false;
            mainParent.SetActive(true);
            settingsParent.SetActive(false);
            creditsParent.SetActive(false);
            EventSystem.current.SetSelectedGameObject(mainSelected);
        }

        public void Play()
        {
            Managers.GameState.Instance.SFXVol = Managers.GameState.Instance.SFXVolbuffer;
            Managers.GameState.Instance.State = Util.Enums.GameState.Playing;
            Character.Hero.HeroData.Instance.ResetData();
            Random.InitState(System.DateTime.Now.Millisecond);
            SceneManager.LoadScene(this.scene);
        }

        public void GoToCredits()
        {
            inMain = false;
            inCredits = true;
            mainParent.SetActive(false);
            settingsParent.SetActive(false);
            creditsParent.SetActive(true);
            EventSystem.current.SetSelectedGameObject(creditsSelected);
        }

        public void GoToSettings()
        {
            inMain = false;
			inCredits = false;
            mainParent.SetActive(false);
            settingsParent.SetActive(true);
            creditsParent.SetActive(false);
            EventSystem.current.SetSelectedGameObject(settingsSelected);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}