namespace ShiftingDungeon.Dungeon.RoomParts
{
    using UnityEngine;
    using Util;

    public class OneWayTile : MonoBehaviour 
    {
        public float pushForce = 2f;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.tag == Enums.Tags.Hero.ToString() || other.gameObject.tag == Enums.Tags.Enemy.ToString())
            {
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(this.transform.right * this.pushForce, ForceMode2D.Force);
            }
            else
                Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), other);
        }
    }
}
