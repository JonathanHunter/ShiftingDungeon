namespace ShiftingDungeon.Character.Traps
{
    using ObjectPooling;
    using UnityEngine;
    using Util;
    using Weapons;

    public abstract class Trap : MonoBehaviour, IPoolable, IDamageDealer
    {
        [SerializeField]
        private int referenceIndex = 0;
        [SerializeField]
        protected int damage = 0;
        [SerializeField]
        protected Sprite[] sprites = null;
        [SerializeField]
        private Enums.Traps type = Enums.Traps.Spike;

        /// <summary> The type of this trap. </summary>
        public Enums.Traps Type { get { return this.type; } }

        private void Update()
        {
            if (Managers.GameState.Instance.IsPaused)
                return;

            LocalUpdate();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            LocalCollision(collision);
        }
        public IPoolable SpawnCopy(int referenceIndex)
        {
            Trap trap = Instantiate<Trap>(this);
            trap.referenceIndex = referenceIndex;
            return trap;
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
            LocalInitialize();
        }

        public void ReInitialize()
        {
            LocalReInitialize();
            this.gameObject.SetActive(true);
        }

        public void Deallocate()
        {
            LocalDeallocate();
            this.gameObject.SetActive(false);
        }

        public void Delete()
        {
            LocalDelete();
            Destroy(this.gameObject);
        }

        public int GetDamage()
        {
            return this.damage;
        }

        protected abstract void LocalUpdate();
        protected abstract void LocalInitialize();
        protected abstract void LocalReInitialize();
        protected abstract void LocalDeallocate();
        protected abstract void LocalDelete();
        protected abstract void LocalCollision(Collision2D coll);
    }

}
