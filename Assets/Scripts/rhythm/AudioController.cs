using UnityEngine;

namespace rhythm
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        public void Play(RhythmSO rhythmSO)
        {
            audioSource.clip = rhythmSO.sound;
            audioSource.Play();
        }

        public void Stop()
        {
            audioSource.Stop();
        }
    }
}