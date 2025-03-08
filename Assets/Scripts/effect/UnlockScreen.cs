using System.Collections.Generic;
using navigation;
using UnityEngine;

namespace effect
{
    [CreateAssetMenu(fileName = "Effect", menuName = "Effect/UnlockScreen", order = 0)]
    class UnlockScreen : EffectSO
    {
        public List<NavigationPlaceSO> places;
    }
}