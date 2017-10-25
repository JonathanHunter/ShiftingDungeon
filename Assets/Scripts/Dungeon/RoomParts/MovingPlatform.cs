namespace ShiftingDungeon.Dungeon.RoomParts {

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    using Util;

    public class MovingPlatform : MonoBehaviour {

        [SerializeField]
        private Transform destination;
        [SerializeField]
        private float maxSpeed;
        private Vector3 origin;
        private float travelDistance;

        private GameObject switchObject;

        [Header("Collider Order: Right, Left, Bottom, Top")]
        [SerializeField]
        private BoxCollider2D[] collidersToDisableAtOrigin;
        [SerializeField]
        private BoxCollider2D[] collidersToDisableAtDestination;
        [SerializeField]
        private float distanceToDisableColliders;

        private Rigidbody2D rb;
        private float platformTime = 0;

        private List<Transform> riders;

        void Start() {
            rb = GetComponent<Rigidbody2D>();
            origin = transform.position;
            switchObject = transform.GetChild(0).gameObject;

            riders = new List<Transform>();
            travelDistance = (origin - destination.position).magnitude;
        }

        void Update() {


            if ((transform.position - destination.position).magnitude < distanceToDisableColliders)
            {
                foreach (BoxCollider2D collider in collidersToDisableAtDestination)
                    collider.enabled = false;
            }
            else if ((transform.position - origin).magnitude < distanceToDisableColliders)
            {
                foreach (BoxCollider2D collider in collidersToDisableAtOrigin)
                    collider.enabled = false;
            }
            else
            {
                foreach (BoxCollider2D collider in collidersToDisableAtDestination)
                    collider.enabled = true;
                foreach (BoxCollider2D collider in collidersToDisableAtOrigin)
                    collider.enabled = true;
            }
        }

        private void FixedUpdate()
        {
            if (switchObject.gameObject.activeInHierarchy)
                rb.velocity = (destination.position - origin)
                    * Mathf.Sin((platformTime += Time.fixedDeltaTime) * 2 * Mathf.PI * maxSpeed / travelDistance)
                    * Mathf.PI * maxSpeed / travelDistance;
            else
                rb.velocity = Vector2.zero;

            foreach (Transform rider in riders)
                rider.transform.position += (Vector3)rb.velocity * Time.fixedDeltaTime;
            riders.Clear();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == Enums.Tags.Hero.ToString())
                collision.gameObject.layer = (int)Enums.Layers.HeroSuspended;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == Enums.Tags.Hero.ToString())
                collision.gameObject.layer = (int)Enums.Layers.Hero;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            riders.Add(collision.transform);
        }
    }
}