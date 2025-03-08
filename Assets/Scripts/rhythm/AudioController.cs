using UnityEngine;

namespace rhythm
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        public void Play(Rhythm rhythm)
        {
            audioSource.clip = rhythm.sound;
            audioSource.Play();
        }

        public void Stop()
        {
            audioSource.Stop();
        }
    }
}