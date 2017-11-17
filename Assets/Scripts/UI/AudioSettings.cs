namespace ShiftingDungeon.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    public class AudioSettings : MonoBehaviour
    {
        [SerializeField]
        private Scrollbar music;
        [SerializeField]
        private Scrollbar sfx;
        [SerializeField]
        private GameObject backButton;

        private GameObject currentSelected;

        void Start()
        {
            this.music.value = Managers.GameState.Instance.MusicVol;
            this.sfx.value = Managers.GameState.Instance.SFXVolbuffer;
            EventSystem.current.SetSelectedGameObject(backButton);
        }

        void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == null)
                EventSystem.current.SetSelectedGameObject(backButton);

            currentSelected = EventSystem.current.currentSelectedGameObject;

            if (currentSelected == this.music.gameObject)
            {
                if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Left))
                {
                    this.music.value -= .2f;
                    Managers.GameState.Instance.MusicVol = this.music.value;
                    Managers.GameState.Instance.bgm.GetComponent<AudioSource>().volume = this.music.value;
                }

                if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Right))
                {
                    this.music.value += .2f;
                    Managers.GameState.Instance.MusicVol = this.music.value;
                    Managers.GameState.Instance.bgm.GetComponent<AudioSource>().volume = this.music.value;
                }
            }

            if (currentSelected == this.sfx.gameObject)
            {
                if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Left))
                {
                    this.sfx.value -= .2f;
                    Managers.GameState.Instance.SFXVolbuffer= this.sfx.value;
                }

                if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Right))
                {
                    this.sfx.value += .2f;
                    Managers.GameState.Instance.SFXVolbuffer = this.sfx.value;
                }
            }

            if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Up))
                Navigator.Navigate(Util.CustomInput.UserInput.Up, currentSelected);
            if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Down))
                Navigator.Navigate(Util.CustomInput.UserInput.Down, currentSelected);
            if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Left))
                Navigator.Navigate(Util.CustomInput.UserInput.Left, currentSelected);
            if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Right))
                Navigator.Navigate(Util.CustomInput.UserInput.Right, currentSelected);
            if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Accept))
                Navigator.CallSubmit();
            if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Cancel))
            {
                EventSystem.current.SetSelectedGameObject(backButton);
                Navigator.CallSubmit();
            }
        }

        public void ChangeVolume(float change)
        {
            Managers.GameState.Instance.SFXVolbuffer = change;
        }

        public void ChangeMusicVolume(float change)
        {
            Managers.GameState.Instance.MusicVol = change;
            Managers.GameState.Instance.bgm.GetComponent<AudioSource>().volume = change;
        }
    }
}