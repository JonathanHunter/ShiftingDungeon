namespace ShiftingDungeon.Managers
{
    using UnityEngine;

    public class DeathSoundPlayer : MonoBehaviour
    {
        [SerializeField]
        private Util.SoundPlayer sfx;

        private static DeathSoundPlayer instance;
        /// <summary> The GameState for this scene. </summary>
        public static DeathSoundPlayer Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<DeathSoundPlayer>();

                return instance;
            }
        }

        public void PlayDeathSound()
        {
            sfx.PlaySong(0);
        }
    }
}
