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
        public RhythmSO rhythm;
        public ResourceList input;
        public ResourceList output;
    }
}