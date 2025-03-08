using System.Collections.Generic;
using navigation;
using recipes;
using UnityEngine;

namespace effect
{
    [CreateAssetMenu(fileName = "Effect", menuName = "Effect/UnlockRecipe", order = 0)]
    class UnlockRecipe : EffectSO
    {
        public List<RecipeSO> recipes;
    }
}