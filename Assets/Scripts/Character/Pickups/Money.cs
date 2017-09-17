namespace ShiftingDungeon.Character.Pickups
{
    using UnityEngine;
    using ObjectPooling;

    public class Money : MonoBehaviour, IPoolable
    {
        public int Value {
            get {
                if (this.transform.localScale.x == 1)
                    return 1;
                else if (this.transform.localScale.x == 2)
                    return 10;
                else
                    return 100;
            }
        }

        private Transform hero;
        private int referenceIndex;

        private void Update()
        {
            if (this.hero != null)
            {
                Vector3 dir = this.hero.position - this.transform.position;
                this.transform.Translate(dir.normalized * Time.deltaTime * (1f / dir.magnitude) * 2f);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == Util.Enums.Tags.Hero.ToString())
            {
                this.hero = collision.gameObject.transform;
            }
        }
        
        public IPoolable SpawnCopy(int referenceIndex)
        {
            Money gold = Instantiate<Money>(this);
            gold.referenceIndex = referenceIndex;
            return gold;
        }

        public int GetReferenceIndex()
        {
            return this.referenceIndex;
        }

        public GameObject GetGameObject()
        {
            return this.gameObject;
        }

        public void Initialize()
        {
            this.hero = null;
        }

        public void ReInitialize()
        {
            this.hero = null;
            this.gameObject.SetActive(true);
        }

        public void Deallocate()
        {
            this.hero = null;
            this.gameObject.SetActive(false);
        }

        public void Delete()
        {
            Destroy(this.gameObject);
        }
    }
}
