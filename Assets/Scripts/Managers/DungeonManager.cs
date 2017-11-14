namespace ShiftingDungeon.Managers
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.Tilemaps;
    using Dungeon;
    using Dungeon.ProcGen;
    using Dungeon.RoomParts;
    using Character.Hero;
    using UI;

    public class DungeonManager : MonoBehaviour
    {
        public bool isTitleScreen;

        [SerializeField]
        private DungeonMap[] floors = null;
        [SerializeField]
        private string nextScene = "Title";
        [SerializeField]
        private DungeonGenerator dungeonGenerator = null;
        [SerializeField]
        private UI.TransitionOverlay overlay = null;
        [SerializeField]
        private UI.MiniMap miniMap = null;
        [SerializeField]
        private UI.GameOverScreen gameOverScreen = null;
        [SerializeField]
        private HeroBehavior heroTemplet = null;
        [SerializeField]
        private int currentFloor = 0;
        [SerializeField]
        private Tilemap floorMap;
        
        private static DungeonManager instance;
        public static DungeonManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<DungeonManager>();

                return instance;
            }
        }

        private DungeonMap map;
        private HeroBehavior hero;
        private Util.CameraTracker cameraTracker;
        private TitleScreenDemo titleScreenDemo;

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

            this.hero = GetHero().GetComponent<HeroBehavior>();

            this.cameraTracker = FindObjectOfType<Util.CameraTracker>();
            this.titleScreenDemo = GetComponent<TitleScreenDemo>();
            StartCoroutine(SwitchMaps());
        }

        /// <summary> Gets the hero. </summary>
        /// <returns> Returns the hero's gameobject. </returns>
        public static GameObject GetHero()
        {
            if(Instance.hero == null)
            {
                Instance.hero = Instantiate(instance.heroTemplet);
            }

            return Instance.hero.gameObject;
        }

        /// <summary> Transitions from the current map to the next. </summary>
        public static void TransitionMaps()
        {
            Instance.currentFloor++;
            if (Instance.currentFloor >= Instance.floors.Length)
            {
                GameState.Instance.State = Util.Enums.GameState.Playing;
                SceneManager.LoadScene(Instance.nextScene);
            }
            else
                LoadSelectedFloor();
        }

        /// <summary> Reloads the floor after the hero dies. </summary>
        public static void RestartFloor()
        {
            GetHero().GetComponent<HeroBehavior>().Respawn();
            LoadSelectedFloor();
            Instance.cameraTracker.ResetPosition();
        }

        /// <summary> Begins loading the currently selected floor. </summary>
        private static void LoadSelectedFloor()
        {
            Instance.StartCoroutine(Instance.SwitchMaps());
        }

        /// <summary> Transitions from the current room to the next. </summary>
        /// <param name="current"> The current room's door. </param>
        public static void TransitionRooms(Door current)
        {
            Instance.StartCoroutine(Instance.SwitchRooms(current));
        }

        /// <summary> Displays the game over screen when the player dies. </summary>
        public static void ShowGameOver()
        {
            if(Instance.titleScreenDemo)
            {
                StartResetTitleScreen();
            }
            else
            {
                Instance.gameOverScreen.gameObject.SetActive(true);
            }
        }

        /// <summary> Starts the sequence for resetting the map in the title screen. </summary>
        public static void StartResetTitleScreen()
        {
            Instance.StartCoroutine(Instance.ResetTitleScreen());
        }

        /// <summary> Regenerates the map in the title screen. </summary>
        private IEnumerator ResetTitleScreen()
        {
            if (!titleScreenDemo.IsResetting)
            {
                titleScreenDemo.StartReset();
                yield return new WaitForSeconds(2);
                titleScreenDemo.SwitchTileSet();
                yield return SwitchMaps();
                Instance.hero.Respawn();
                Instance.hero.GetComponent<HeroInputDemo>().ResetDemoHero();
                titleScreenDemo.SwitchDoors(this.map.Rooms[0]);
                titleScreenDemo.ResetTitleScreen();
            }
        }

        private IEnumerator SwitchMaps()
        {
            this.overlay.Show();
            GameState.Instance.State = Util.Enums.GameState.Tranisioning;

            if(this.map != null)
            {
                // Initialize room state
                for (int i = 0; i < this.map.Rooms.Length; i++)
                {
                    this.map.Rooms[i].Deactivate();
                    Destroy(this.map.Rooms[i].gameObject);
                    yield return 0;
                }

                Destroy(this.map.gameObject);
                this.map = null;
                GetHero().transform.position = Vector3.zero;
                yield return 0;
            }

            // Generate Map
            if (this.floors[this.currentFloor] == null)
            {
                if (this.dungeonGenerator == null)
                {
                    Debug.LogError("Dungeon Generator is null.  Unable to generate map.");
                    yield break;
                }

                this.map = this.dungeonGenerator.GenerateMap(10);
            }
            else
                this.map = Instantiate(this.floors[this.currentFloor]);
            
            this.map.transform.position = new Vector3(0f, 3f, -1f);
            yield return 0;

            // Initialize the Map
            this.map.Init();

            // Initialize Mini Map
            if (this.miniMap != null)
                this.miniMap.Init(this.map.Map, new Vector2(this.map.Rooms[0].Row, this.map.Rooms[0].Col));

            RemoveMoney();
            
            yield return 0;

            // Initialize room state
            for (int i = 0; i < this.map.Rooms.Length; i++)
            {
                this.map.Rooms[i].Init(i, i == 2);//(this.map.Rooms.Length - 1));
                yield return 0;
            }

            floorMap.transform.parent.gameObject.SetActive(false);
            this.map.Rooms[0].Activate(this.floorMap);
            this.overlay.FadeOut();
            GameState.Instance.State = Util.Enums.GameState.Playing;

            yield break;
        }

        private IEnumerator SwitchRooms(Door current)
        {
            this.overlay.FadeIn();
            GameState.Instance.State = Util.Enums.GameState.Tranisioning;
            floorMap.transform.parent.gameObject.SetActive(false);
            Room next;
            Vector2 postion;
            Util.Enums.Direction directionMoved = Util.Enums.Direction.None;
            if (current.Parent.upperDoor == current)
            {
                next = current.Parent.Up;
                postion = new Vector2(0, -7);
                directionMoved = Util.Enums.Direction.Up;
            }
            else if (current.Parent.lowerDoor == current)
            {
                next = current.Parent.Down;
                postion = new Vector2(0, 7);
                directionMoved = Util.Enums.Direction.Down;
            }
            else if (current.Parent.leftDoor == current)
            {
                next = current.Parent.Left;
                postion = new Vector2(7, 0);
                directionMoved = Util.Enums.Direction.Left;
            }
            else
            {
                next = current.Parent.Right;
                postion = new Vector2(-7, 0);
                directionMoved = Util.Enums.Direction.Right;
            }

            if (this.miniMap != null)
            {
                this.miniMap.UpdateMiniMap(directionMoved);
                yield return 0;
            }

            RemoveMoney();

            this.hero.gameObject.transform.position = postion;
            next.transform.localPosition = Vector3.zero;
            this.cameraTracker.ResetPosition();
            current.Parent.Deactivate();
            yield return 0;

            next.Activate(this.floorMap);
            this.overlay.FadeOut();
            GameState.Instance.State = Util.Enums.GameState.Playing;
            yield break;
        }

        /// <summary> Removes all money objects from the world. </summary>
        private void RemoveMoney()
        {
            Character.Pickups.Money[] gold = FindObjectsOfType<Character.Pickups.Money>();
            foreach(Character.Pickups.Money g in gold)
            {
                if(g.gameObject.activeInHierarchy)
                    ObjectPooling.PickupPool.Instance.ReturnGold(g.gameObject);
            }

            Character.Weapons.Bullets.Bullet[] bullets = FindObjectsOfType<Character.Weapons.Bullets.Bullet>();
            foreach (Character.Weapons.Bullets.Bullet b in bullets)
            {
                if (b.gameObject.activeInHierarchy)
                    ObjectPooling.BulletPool.Instance.ReturnBullet(b.Type, b.gameObject);
            }
        }
    }
}
