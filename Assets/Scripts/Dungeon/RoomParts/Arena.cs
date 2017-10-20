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
        private Util.SoundPlayer sfx;

        private List<GameObject> spawnedEnemies;
        private bool tripped;

        private void Start()
        {
            this.spawnedEnemies = new List<GameObject>();
            this.tripped = false;
            foreach (GameObject t in this.traps)
                t.SetActive(false);

            foreach (GameObject v in this.victoryObjs)
                v.SetActive(false);
        }

        private void Update()
        {
            if (!tripped || spawnedEnemies.Count == 0)
                return;

            bool allDead = true;
            foreach (GameObject e in this.spawnedEnemies)
                if (e.gameObject.activeInHierarchy)
                    allDead = false;
            
            if(allDead)
            {
                this.spawnedEnemies.Clear();
                foreach (GameObject t in this.traps)
                    t.SetActive(false);

                foreach (GameObject v in this.victoryObjs)
                    v.SetActive(true);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == Util.Enums.Tags.Hero.ToString())
            {
                this.sfx.PlaySong(0);
                foreach (SummoningField s in this.spawners)
                {
                    GameObject e = s.Spawn();
                    if (e != null)
                        this.spawnedEnemies.Add(e);
                }

                foreach (GameObject t in this.traps)
                    t.SetActive(true);

                foreach (GameObject v in this.victoryObjs)
                    v.SetActive(false);

                GetComponent<Collider2D>().enabled = false;
                this.tripped = true;
            }
        }
    }
}
