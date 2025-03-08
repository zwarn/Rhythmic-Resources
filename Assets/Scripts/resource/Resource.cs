using UnityEngine;

namespace resource
{
    [CreateAssetMenu(fileName = "Resource", menuName = "Resource", order = 0)]
    public class Resource : ScriptableObject
    {
        public Sprite sprite;
    }
}