namespace ShiftingDungeon.Managers
{
    using UnityEngine;
    using Dungeon;
    using Dungeon.ProcGen;

    public class DungeonManager : MonoBehaviour
    {
        [SerializeField]
        private DungeonMap map = null;
        [SerializeField]
        private DungeonGenerator dungeonGenerator = null;

        private void Start()
        {
            // Ensure Pools are fully initialized before starting the map
            ObjectPooling.ObjectPool[] pools = FindObjectsOfType<ObjectPooling.ObjectPool>() as ObjectPooling.ObjectPool[];
            foreach(ObjectPooling.ObjectPool pool in pools)
                pool.Init();

            // Generate Map
            if(this.map == null)
            {
                if(this.dungeonGenerator == null)
                {
                    Debug.LogError("Dungeon Generator is null.  Unable to generate map.");
                    return;
                }

                this.map = this.dungeonGenerator.GenerateMap(10);
            }

            this.map.Init();
        }
    }
}
