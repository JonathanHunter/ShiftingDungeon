namespace ShiftingDungeon.UI
{
    using UnityEngine;
    using Managers;
    using Util;

    public class Cutscene : MonoBehaviour
    {
        public TextAsset[] pages;
        public Sprite[] backgrounds;
        public int[] whichBackgroundForWhichPage;
        public float textSpeed;
        public string tagToLookFor;
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

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!GameState.Instance.IsPaused)
            {
                if (coll.gameObject.tag == tagToLookFor)
                    start = true;
                else
                    Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), coll);
            }
        }

        void Start()
        {
            sound.SetVolume(GameState.Instance.SFXVol * .75f);
            this.state = State.waiting;
            this.currentText = new System.Text.StringBuilder();
            this.currentPage = 0;
            this.currentLetter = 0;
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
            if (this.state != State.waiting)
            {
                this.image.sprite = this.backgrounds[this.whichBackgroundForWhichPage[currentPage]];
                this.text.text = this.currentText.ToString();
            }
            if (this.state == State.waiting)
            {
                if (start)
                {
                    GameState.Instance.State = Enums.GameState.Cutscene;
                    this.state = State.displaying;
                    this.pageChars = this.pageStrings[this.currentPage].ToCharArray();
                    this.canvas.SetActive(true);
                }
            }
            else if (this.state == State.displaying)
            {
                if (CustomInput.BoolFreshPressDeleteOnRead(CustomInput.UserInput.Pause))
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
                if (CustomInput.BoolFreshPressDeleteOnRead(CustomInput.UserInput.Accept) || 
                    CustomInput.BoolFreshPressDeleteOnRead(CustomInput.UserInput.Attack) || 
                    CustomInput.BoolFreshPressDeleteOnRead(CustomInput.UserInput.Pause))
                {
                    this.currentLetter = 0;
                    this.currentPage++;
                    if (this.currentPage >= this.pages.Length)
                    {
                        this.text.text = "";
                        GameState.Instance.State = Enums.GameState.Playing;
                        this.canvas.SetActive(false);
                        Destroy(this.gameObject);
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