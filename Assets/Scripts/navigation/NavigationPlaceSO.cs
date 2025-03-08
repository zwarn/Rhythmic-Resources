using System;
using UnityEngine;

namespace navigation
{
    [CreateAssetMenu(fileName = "NavigationPlace", menuName = "NavigationPlace", order = 0)]
    public class NavigationPlaceSO : ScriptableObject
    {
        public String name;
        public Sprite icon;
        public NavigationPlace screenPrefab;
    }
}