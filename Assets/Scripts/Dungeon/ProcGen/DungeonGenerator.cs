namespace ShiftingDungeon.Dungeon.ProcGen
{
    using UnityEngine;
    using MapGen;

    public class DungeonGenerator : MonoBehaviour
    {
        [SerializeField]
        private DungeonMap mapTemplet = null;
        [SerializeField]
        private Room roomTemplet = null;

        /// <summary> Generates dungeon map. </summary>
        /// <param name="mapSize"> The size of map to make. </param>
        /// <returns> The reference to the instantiated map. </returns>
        public DungeonMap GenerateMap(int mapSize)
        {
            DBM dbm = new DBM(mapSize);
            for (int i = 0; i < 10; i++)
                dbm.AddCell();

            Room[] rooms = dbm.GetPatternAsRooms(this.roomTemplet);
            DungeonMap map = Instantiate<DungeonMap>(this.mapTemplet);
            map.transform.position = Vector3.zero;
            map.transform.rotation = Quaternion.identity;
            map.transform.localScale = Vector3.one;
            map.mapSize = new Vector2(mapSize, mapSize);
            map.SetRoomList(rooms);
            return map;
        }
    }
}
