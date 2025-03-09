using System.Collections.Generic;
using resource;
using rhythm;
using UnityEngine;

namespace recipes
{
    [CreateAssetMenu(fileName = "Recipe", menuName = "Recipe", order = 0)]
    public class RecipeSO : ScriptableObject
    {
        public RecipeType recipeType;
        public Sprite icon;
        public List<RhythmSO> rhythms;
        public ResourceList input;
        public ResourceList output;
    }
}