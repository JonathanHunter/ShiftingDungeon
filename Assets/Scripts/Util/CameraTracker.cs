namespace ShiftingDungeon.Util
{
    using UnityEngine;

    class CameraTracker : MonoBehaviour
    {
        private Transform player;
        public Transform upperBound;
        public Transform lowerBound;
        public Transform leftBound;
        public Transform rightBound;

        void Start()
        {
            this.player = FindObjectOfType<Character.Player.PlayerBehavior>().gameObject.transform;
        }

        void Update()
        {
            float speed = Time.deltaTime * ((Mathf.Ceil(Vector2.Distance(this.transform.position, player.position))) + 1f);
            if (player.position.x > leftBound.transform.position.x && player.position.x < rightBound.position.x)
                this.transform.position = Vector3.MoveTowards(this.transform.position,
                    new Vector3(player.position.x, this.transform.position.y, this.transform.position.z), speed);
            else if (player.position.x < leftBound.transform.position.x)
                this.transform.position = Vector3.MoveTowards(this.transform.position,
                    new Vector3(leftBound.position.x, this.transform.position.y, this.transform.position.z), speed);
            else
                this.transform.position = Vector3.MoveTowards(this.transform.position,
                    new Vector3(rightBound.position.x, this.transform.position.y, this.transform.position.z), speed);

            if (player.position.y > lowerBound.transform.position.y && player.position.y < upperBound.position.y)
                this.transform.position = Vector3.MoveTowards(this.transform.position,
                    new Vector3(this.transform.position.x, player.position.y, this.transform.position.z), speed);
            else if (player.position.y < lowerBound.transform.position.y)
                this.transform.position = Vector3.MoveTowards(this.transform.position,
                    new Vector3(this.transform.position.x, lowerBound.position.y, this.transform.position.z), speed);
            else
                this.transform.position = Vector3.MoveTowards(this.transform.position,
                    new Vector3(this.transform.position.x, upperBound.position.y, this.transform.position.z), speed);
        }

        public void setBounds(Transform leftBound, Transform rightBound, Transform upperBound, Transform lowerBound)
        {
            this.upperBound = upperBound;
            this.lowerBound = lowerBound;
            this.leftBound = leftBound;
            this.rightBound = rightBound;
        }

        /// <summary> Centers the camera on the player immediately. </summary>
        public void ResetPosition()
        {
            this.transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        }
    }
}
