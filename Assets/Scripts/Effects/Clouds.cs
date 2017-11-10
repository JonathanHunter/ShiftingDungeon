namespace ShiftingDungeon.Effects
{
    using UnityEngine;

    public class Clouds : MonoBehaviour
    {
        public Transform begin = null;
        public Transform end = null;
        public float speed = 0;

        private void Start()
        {

        }

        private void Update()
        {
            this.transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (this.transform.position.x > this.end.position.x)
                this.transform.position = new Vector3(this.begin.position.x, this.transform.position.y, this.transform.position.z);
        }
    }
}
