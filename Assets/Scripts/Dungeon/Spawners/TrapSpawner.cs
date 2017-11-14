namespace ShiftingDungeon.Dungeon.Spawners
{
    using UnityEngine;
    using ObjectPooling;
    using Util;

    public class TrapSpawner : Spawner
    {
        [SerializeField]
        private Enums.Traps type;

        private GameObject trap;

        public override void Spawn()
        {
            if (this.trap == null)
            {
                this.trap = TrapPool.Instance.GetTrap(this.type);
                this.trap.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 9);
                trap.transform.rotation = this.transform.rotation;
            }
        }

        public override void Return()
        {
            if (this.trap != null)
            {
                TrapPool.Instance.ReturnTrap(this.type, this.trap);
                this.trap = null;
            }
        }
    }
}
