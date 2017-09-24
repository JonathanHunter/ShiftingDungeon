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

        void OnTriggerEnter2D(Collider2D coll)
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
            //sound.SetVolume(GameManager.SFXVol % .75f);
            state = State.waiting;
            currentText = new System.Text.StringBuilder();
            currentPage = 0;
            currentLetter = 0;
            pageStrings = new string[pages.Length];
            for (int i = 0; i < pages.Length; i++)
            {
                pageStrings[i] = pages[i].text;
            }
        }

        void Update()
        {
            if (state != State.waiting)
            {
                image.sprite = backgrounds[whichBackgroundForWhichPage[currentPage]];
                text.text = currentText.ToString();
            }
            if (state == State.waiting)
            {
                if (start)
                {
                    GameState.Instance.State = Enums.GameState.Cutscene;
                    state = State.displaying;
                    pageChars = pageStrings[currentPage].ToCharArray();
                    canvas.SetActive(true);
                }
            }
            else if (state == State.displaying)
            {
                if (Util.CustomInput.BoolFreshPress(Util.CustomInput.UserInput.Pause))
                {
                    for (int i = Mathf.RoundToInt(currentLetter); i < pageChars.Length; i++)
                        currentText.Append(pageChars[i]);
                    state = State.paused;
                }
                else
                {
                    if (currentLetter == 0)
                        currentText.Append(pageChars[(int)currentLetter]);
                    int a = (int)(currentLetter);
                    float temp = textSpeed;
                    if (Util.CustomInput.BoolHeld(Util.CustomInput.UserInput.Accept) || Util.CustomInput.BoolHeld(Util.CustomInput.UserInput.Attack))
                        temp *= 2;
                    currentLetter += temp * Time.deltaTime;
                    if (a < (int)currentLetter && currentLetter < pageChars.Length)
                    {
                        currentText.Append(pageChars[(int)currentLetter]);
                        sound.PlaySong(0);
                    }
                    if (currentLetter >= pageChars.Length)
                    {
                        //currentText = new System.Text.StringBuilder(pageStrings[currentPage]);
                        state = State.paused;
                    }
                }
            }
            else
            {
                if (CustomInput.BoolFreshPress(CustomInput.UserInput.Accept) || 
                    CustomInput.BoolFreshPress(CustomInput.UserInput.Attack) || 
                    CustomInput.BoolFreshPress(CustomInput.UserInput.Pause))
                {
                    currentLetter = 0;
                    currentPage++;
                    if (currentPage >= pages.Length)
                    {
                        text.text = "";
                        GameState.Instance.State = Enums.GameState.Playing;
                        canvas.SetActive(false);
                        Destroy(this.gameObject);
                    }
                    else
                    {
                        pageChars = pageStrings[currentPage].ToCharArray();
                        currentText = new System.Text.StringBuilder();
                        state = State.displaying;
                    }
                }
            }
        }
    }
}