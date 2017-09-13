namespace ShiftingDungeon.Managers
{
    using System.Collections;
    using UnityEngine;
    using Dungeon;
    using Dungeon.ProcGen;
    using Dungeon.RoomParts;

    public class DungeonManager : MonoBehaviour
    {
        [SerializeField]
        private DungeonMap map = null;
        [SerializeField]
        private DungeonGenerator dungeonGenerator = null;
        [SerializeField]
        private UI.TransitionOverlay overlay = null;
        [SerializeField]
        private UI.MiniMap miniMap = null;
        [SerializeField]
        private Character.Hero.HeroBehavior heroTemplet = null;

        private static DungeonManager instance;
        private static DungeonManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<DungeonManager>();

                return instance;
            }
        }

        private Character.Hero.HeroBehavior hero;
        private Util.CameraTracker cameraTracker;

        private void Start()
        {
            if (instance != null && instance != this)
            {
                Debug.LogError("Duplicate Dungeon Manager detected: removing " + this.gameObject.name);
                Destroy(this.gameObject);
                return;
            }

            instance = this;

            // Ensure Pools are fully initialized before starting the map
            ObjectPooling.ObjectPool[] pools = FindObjectsOfType<ObjectPooling.ObjectPool>() as ObjectPooling.ObjectPool[];
            foreach(ObjectPooling.ObjectPool pool in pools)
                pool.Init();

            if(this.hero == null)
                this.hero = Instantiate(this.heroTemplet);

            this.cameraTracker = FindObjectOfType<Util.CameraTracker>();
            StartCoroutine(MapSetup());
        }

        /// <summary> Gets the hero. </summary>
        /// <returns> Returns the hero's gameobject. </returns>
        public static GameObject GetHero()
        {
            if(Instance.hero == null)
                Instance.hero = Instantiate(instance.heroTemplet);

            return Instance.hero.gameObject;
        }

        /// <summary> Transitions from the current room to the next. </summary>
        /// <param name="current"> The current room's door. </param>
        public static void TransitionRooms(Door current)
        {
            Instance.StartCoroutine(Instance.SwitchRooms(current));
        }

        private IEnumerator MapSetup()
        {
            this.overlay.Show();
            // Generate Map
            if (this.map == null)
            {
                if (this.dungeonGenerator == null)
                {
                    Debug.LogError("Dungeon Generator is null.  Unable to generate map.");
                    yield break;
                }

                this.map = this.dungeonGenerator.GenerateMap(10);
                yield return 0;
            }

            // Initialize the Map
            this.map.Init();

            // Initialize Mini Map
            if (this.miniMap != null)
                this.miniMap.Init(this.map.Map, new Vector2(this.map.Rooms[0].Row, this.map.Rooms[0].Col));
            
            yield return 0;

            // Initialize room state
            for (int i = 0; i < this.map.Rooms.Length; i++)
            {
                this.map.Rooms[i].Init(i);
                yield return 0;
            }

            this.map.Rooms[0].Activate();
            this.overlay.FadeOut();
            yield break;
        }

        private IEnumerator SwitchRooms(Door current)
        {
            this.overlay.FadeIn();
            Room next;
            Vector2 postion;
            Util.Enums.Direction directionMoved = Util.Enums.Direction.None;
            if (current.Parent.upperDoor == current)
            {
                next = current.Parent.Up;
                postion = new Vector2(0, -10);
                directionMoved = Util.Enums.Direction.Up;
            }
            else if (current.Parent.lowerDoor == current)
            {
                next = current.Parent.Down;
                postion = new Vector2(0, 4);
                directionMoved = Util.Enums.Direction.Down;
            }
            else if (current.Parent.leftDoor == current)
            {
                next = current.Parent.Left;
                postion = new Vector2(7, -3);
                directionMoved = Util.Enums.Direction.Left;
            }
            else
            {
                next = current.Parent.Right;
                postion = new Vector2(-7, -3);
                directionMoved = Util.Enums.Direction.Right;
            }


            if (this.miniMap != null)
            {
                this.miniMap.UpdateMiniMap(directionMoved);
                yield return 0;
            }

            this.hero.gameObject.transform.position = postion;
            this.cameraTracker.ResetPosition();
            current.Parent.Deactivate();
            yield return 0;

            next.Activate();
            this.overlay.FadeOut();
            yield break;
        }
    }
}
