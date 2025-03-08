using resource;
using UnityEngine;

namespace rhythm
{
    [CreateAssetMenu(fileName = "Rhythm", menuName = "Rhythm", order = 0)]
    public class RhythmSO : ScriptableObject
    {
        public int barDuration = 4000;
        public int[] notes;
        public AudioClip sound;
        public ResourceList production;
    }
}