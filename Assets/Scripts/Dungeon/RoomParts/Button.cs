namespace ShiftingDungeon.Dungeon.RoomParts
{
    using UnityEngine;
    using Util;

    public class Button : MonoBehaviour
    {
        /// <summary> The objects affected by this button. </summary>
        [Tooltip("The objects affected by this button.")]
        public GameObject[] objectsIAffect;
        /// <summary> Whether or not this button can be activated more than once. </summary>
        [Tooltip("Whether or not this button can be activated more than once.")]
        public bool onlyHappenOnce;
        
        private Animator anim;
        private bool wasActivated;
        private bool isOn;
        private int onHash;

        private void Start()
        {
            this.anim = GetComponent<Animator>();
            this.wasActivated = false;
            this.isOn = false;
            this.onHash = Animator.StringToHash("on");
        }

        private void Update()
        {
            if(this.isOn)
                this.anim.SetBool(this.onHash, this.isOn);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == Enums.Tags.Hero.ToString())
            {
                if (!this.wasActivated || !this.onlyHappenOnce)
                {
                    foreach(GameObject g in this.objectsIAffect)
                        g.SetActive(!g.activeSelf);

                    this.wasActivated = true;
                    this.isOn = !isOn;
                    this.anim.SetBool(this.onHash, this.isOn);
                }
            }
            else
                Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), other);
        }
    }
}
