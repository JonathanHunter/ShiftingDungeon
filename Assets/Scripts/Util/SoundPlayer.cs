
namespace ShiftingDungeon.Util
{
    using UnityEngine;

    public class SoundPlayer : MonoBehaviour
    {
        public AudioClip[] song;
        public new AudioSource audio;
        public bool SFX;
        public bool playOnLoad;
        public bool loop;
        public bool intro;
        public int loopSong;
        public bool dontDestroy;

        void Start()
		{
            if (dontDestroy)
                DontDestroyOnLoad(this.gameObject);
            //if (SFX)
            //    audio.volume = Managers.GameManager.SFXVol;
            //else
            //    audio.volume = Managers.GameManager.MusicVol;
            if (playOnLoad)
                PlaySong(0);
        }

        void Update()
        {
            if (intro && !audio.isPlaying)
            {
                intro = false;
                PlaySong(1);
            }
        }

        public void PlaySong(int index)
        {
            audio.Stop();
            audio.loop = loop && index == loopSong;
            audio.clip = song[index];
            audio.Play();
        }

        public void Pause()
        {
            audio.Pause();
        }

        public void Stop()
        {
            audio.loop = false;
            audio.Stop();
        }

        public void SetVolume(float vol)
        {
            audio.volume = vol;
        }
    }
}

