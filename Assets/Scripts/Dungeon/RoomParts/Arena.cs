namespace ShiftingDungeon.Dungeon.RoomParts
{
    using System.Collections.Generic;
    using UnityEngine;
    using Character.Enemies;

    public class Arena : MonoBehaviour
    {
        [SerializeField]
        private SummoningField[] spawners = null;
        [SerializeField]
        private GameObject[] traps = null;
        [SerializeField]
        private GameObject[] victoryObjs = null;
        [SerializeField]
        private Effects.Translate[] titleCards;
        [SerializeField]
        private Util.SoundPlayer sfx;
        [SerializeField]
        private float titleCardTime = .5f;

        private List<GameObject> spawnedEnemies;
        private bool tripped;
        private bool titleCardPlaying;
        private float tileCardCounter;

        private void Start()
        {
            this.spawnedEnemies = new List<GameObject>();
            this.tripped = false;
            this.titleCardPlaying = false;
            this.tileCardCounter = 0f;
            foreach (GameObject t in this.traps)
                t.SetActive(false);

            foreach (GameObject v in this.victoryObjs)
                v.SetActive(false);
        }

        private void Update()
        {
            if (!this.tripped)
                return;

            if (this.titleCardPlaying)
            {
                bool allDone = true;
                foreach (Effects.Translate titleCard in this.titleCards)
                    if (!titleCard.Finished)
                        allDone = false;

                if (allDone && (this.tileCardCounter -= Time.deltaTime) < 0)
                {
                    this.titleCardPlaying = false;
                    foreach (Effects.Translate titleCard in this.titleCards)
                        titleCard.StartTranslate(false);

                    this.sfx.PlaySong(0);
                    Managers.GameState.Instance.State = Util.Enums.GameState.Playing;
                    foreach (SummoningField s in this.spawners)
                    {
                        GameObject e = s.Spawn();
                        if (e != null)
                            this.spawnedEnemies.Add(e);

                        s.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                bool allDead = true;
                foreach (GameObject e in this.spawnedEnemies)
                    if (e.gameObject.activeInHierarchy)
                        allDead = false;

                if (allDead)
                {
                    this.tripped = false;
                    Managers.GameState.Instance.bgm.loopSong = 0;
                    Managers.GameState.Instance.bgm.loop = true;
                    Managers.GameState.Instance.bgm.PlaySong(0);
                    this.spawnedEnemies.Clear();
                    foreach (GameObject t in this.traps)
                        t.SetActive(false);

                    foreach (GameObject v in this.victoryObjs)
                        v.SetActive(true);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == Util.Enums.Tags.Hero.ToString())
            {
                foreach (Effects.Translate titleCard in this.titleCards)
                    titleCard.StartTranslate(true);

                foreach (GameObject t in this.traps)
                    t.SetActive(true);

                foreach (GameObject v in this.victoryObjs)
                    v.SetActive(false);

                GetComponent<Collider2D>().enabled = false;
                this.tripped = true;
                this.titleCardPlaying = true;
                this.tileCardCounter = this.titleCardTime;
                Managers.GameState.Instance.bgm.loopSong = 1;
                Managers.GameState.Instance.bgm.loop = true;
                Managers.GameState.Instance.bgm.PlaySong(1);
                Managers.GameState.Instance.State = Util.Enums.GameState.Cutscene;
            }
        }

        public void CleanUpTrap()
        {
            Managers.GameState.Instance.bgm.loopSong = 0;
            Managers.GameState.Instance.bgm.loop = true;
            Managers.GameState.Instance.bgm.PlaySong(0);

            foreach (GameObject e in this.spawnedEnemies)
                if (e.gameObject.activeInHierarchy)
                    ObjectPooling.EnemyPool.Instance.ReturnEnemy(e.GetComponent<Enemy>().Type, e);

            this.spawnedEnemies.Clear();
            foreach (GameObject t in this.traps)
                t.SetActive(false);
        }
    }
}
