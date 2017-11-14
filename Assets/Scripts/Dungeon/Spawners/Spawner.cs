namespace ShiftingDungeon.Dungeon.Spawners
{
    using UnityEngine;

    public abstract class Spawner : MonoBehaviour
    {
        public abstract void Spawn();

        public abstract void Return();

        public abstract bool isAlive();
    }
}
