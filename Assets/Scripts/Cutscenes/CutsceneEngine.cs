namespace ShiftingDungeon.Cutscenes
{
    using UnityEngine;
    using Managers;
    using Util;

    public class CutsceneEngine : MonoBehaviour
    {
        public TextAsset[] pages;
        public Sprite[] backgrounds;
        public int[] whichBackgroundForWhichPage;
        public Effects.Translate[] actors;
        public int[] actorEntrancePage;
        public int[] actorExitPage;
        public float textSpeed;
        public SoundPlayer sound;
        public UnityEngine.UI.Image image;
        public UnityEngine.UI.Text text;
        public GameObject canvas;

        enum State { waiting, displaying, paused };

        private string[] pageStrings;
        private char[] pageChars;
        private System.Text.StringBuilder currentText;
        private int currentPage;
        private float currentLetter;
        private State state;
        private bool start;
        private bool waitingToExit;
        private bool doOnce;

        void Start()
        {
            sound.SetVolume(GameState.Instance.SFXVol * .75f);
            this.state = State.waiting;
            this.currentText = new System.Text.StringBuilder();
            this.currentPage = 0;
            this.currentLetter = 0;
            this.start = false;
            this.waitingToExit = false;
            this.doOnce = false;
            this.pageStrings = new string[pages.Length];
            for (int i = 0; i < pages.Length; i++)
                this.pageStrings[i] = this.pages[i].text;

            if (this.image == null)
                this.image = CutsceneManager.Instance.globalImage;

            if (this.text == null)
                this.text = CutsceneManager.Instance.globalText;

            if (this.canvas == null)
                this.canvas = CutsceneManager.Instance.globlaCanvas;
        }

        void Update()
        {
            if (!start)
            {
                if (GameState.Instance.IsPaused && GameState.Instance.State != Enums.GameState.Cutscene)
                    return;
                else if(!this.doOnce)
                {
                    GameState.Instance.State = Enums.GameState.Cutscene;
                    DungeonManager.GetHero().SetActive(false);
                    for (int i = 0; i < this.actors.Length; i++)
                    {
                        if (this.actorEntrancePage[i] == 0)
                            this.actors[i].StartTranslate(true);
                    }

                    this.doOnce = true;
                }
            }

            if(this.waitingToExit)
            {
                bool allDone = true;
                foreach (Effects.Translate actor in this.actors)
                    if (!actor.Finished)
                        allDone = false;

                if (allDone)
                {
                    GameState.Instance.State = Enums.GameState.Playing;
                    DungeonManager.GetHero().SetActive(true);
                    DungeonManager.TransitionMaps();
                }
            }

            if (this.state != State.waiting && !this.waitingToExit)
            {
                this.image.sprite = this.backgrounds[this.whichBackgroundForWhichPage[currentPage]];
                this.text.text = this.currentText.ToString();
            }
            if (this.state == State.waiting)
            {
                if (start)
                {
                    this.state = State.displaying;
                    this.pageChars = this.pageStrings[this.currentPage].ToCharArray();
                    this.canvas.SetActive(true);
                }
                else
                {
                    bool allDone = true;
                    foreach (Effects.Translate actor in this.actors)
                        if (!actor.Finished)
                            allDone = false;

                    if (allDone)
                        start = true;
                }
            }
            else if (this.state == State.displaying)
            {
                if (CustomInput.BoolFreshPress(CustomInput.UserInput.Pause))
                {
                    this.currentText = new System.Text.StringBuilder(this.pageStrings[this.currentPage]);
                    this.state = State.paused;
                }
                else
                {
                    if (this.currentLetter == 0)
                        this.currentText.Append(this.pageChars[(int)currentLetter]);
                    int a = (int)(currentLetter);
                    float temp = this.textSpeed;
                    if (CustomInput.BoolHeld(CustomInput.UserInput.Accept) || CustomInput.BoolHeld(CustomInput.UserInput.Attack))
                        temp *= 2;
                    this.currentLetter += temp * Time.deltaTime;
                    if (a < (int)this.currentLetter && this.currentLetter < this.pageChars.Length)
                    {
                        this.currentText.Append(this.pageChars[(int)this.currentLetter]);
                        this.sound.PlaySong(0);
                    }
                    if (this.currentLetter >= this.pageChars.Length)
                    {
                        //currentText = new System.Text.StringBuilder(pageStrings[currentPage]);
                        this.state = State.paused;
                    }
                }
            }
            else
            {
                if (CustomInput.BoolFreshPress(CustomInput.UserInput.Accept) ||
                    CustomInput.BoolFreshPress(CustomInput.UserInput.Attack) ||
                    CustomInput.BoolFreshPress(CustomInput.UserInput.Pause))
                {
                    this.currentLetter = 0;
                    this.currentPage++;

                    for (int i = 0; i < this.actors.Length; i++)
                    {
                        if (this.actorEntrancePage[i] == this.currentPage)
                            this.actors[i].StartTranslate(true);
                    }

                    for (int i = 0; i < this.actors.Length; i++)
                    {
                        if (this.actorExitPage[i] == this.currentPage)
                        {
                            this.actors[i].transform.localScale = new Vector3(
                                -this.actors[i].transform.localScale.x,
                                this.actors[i].transform.localScale.y,
                                this.actors[i].transform.localScale.z);
                            this.actors[i].StartTranslate(false);
                        }
                    }

                    if (this.currentPage >= this.pages.Length)
                    {
                        this.text.text = "";
                        this.canvas.SetActive(false);
                        this.waitingToExit = true;
                        this.start = false;
                        this.state = State.waiting;
                    }
                    else
                    {
                        this.pageChars = this.pageStrings[this.currentPage].ToCharArray();
                        this.currentText = new System.Text.StringBuilder();
                        this.state = State.displaying;
                    }
                }
            }
        }
    }
}
