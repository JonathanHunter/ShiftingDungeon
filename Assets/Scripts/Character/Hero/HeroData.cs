namespace ShiftingDungeon.Character.Hero
{
    using UnityEngine;

    public class HeroData : MonoBehaviour
    {
        /// <summary> How much money the hero has. </summary>
        public int money;

        /// <summary> The weapon levels for the hero. </summary>
        public int[] weaponLevels;

        private static HeroData instance;
        /// <summary> The HeroData for this scene. </summary>
        public static HeroData Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<HeroData>();

                return instance;
            }
        }
        
        private void Start()
        {
            if (instance != null && instance != this)
            {
                Debug.LogError("Duplicate GameState detected: removing " + this.gameObject.name);
                Destroy(this.gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(this.gameObject);

            this.money = 0;
        }
    }
}
