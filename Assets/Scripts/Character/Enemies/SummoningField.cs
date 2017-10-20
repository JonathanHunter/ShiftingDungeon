namespace ShiftingDungeon.Character.Enemies
{
    using UnityEngine;
    using Util;

    public class SummoningField : MonoBehaviour
    {
        [SerializeField]
        private Enums.EnemyTypes typeToSummon = Enums.EnemyTypes.Basic;
        [SerializeField]
        private ParticleSystem smoke = null;

        private SpriteRenderer sprite;
        private bool blocked = false;

        private void Start()
        {
            this.sprite = GetComponent<SpriteRenderer>();
        }

        public GameObject Spawn()
        {
            if (this.blocked)
                return null;

            smoke.Emit(10);
            GameObject e = ObjectPooling.EnemyPool.Instance.GetEnemy(typeToSummon);
            if (e != null)
            {
                e.transform.position = this.transform.position;
                e.transform.rotation = Quaternion.identity;
            }

            return e;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            this.blocked = true;
            this.sprite.enabled = false;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            this.blocked = false;
            this.sprite.enabled = true;
        }
    }
}
