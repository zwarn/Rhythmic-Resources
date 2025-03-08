using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace recipes
{
    public class RecipeController : MonoBehaviour
    {
        public event Action OnRecipeChanged;

        [SerializeField] private List<RecipeSO> initialRecipe = new();

        private List<RecipeSO> _availableRecipe;

        private void Start()
        {
            _availableRecipe = initialRecipe.ToList();
        }

        public List<RecipeSO> GetAvailableRecipe()
        {
            return _availableRecipe.ToList();
        }

        public List<RecipeSO> GetAvailableRecipe(RecipeType type)
        {
            return _availableRecipe.Where(recipe => recipe.recipeType == type).ToList();
        }

        public void AddRecipe(RecipeSO recipe)
        {
            _availableRecipe.Add(recipe);
            _availableRecipe = _availableRecipe.Distinct().ToList();
            RecipeChangedEvent();
        }

        public void AddRecipes(List<RecipeSO> recipes)
        {
            _availableRecipe.AddRange(recipes);
            _availableRecipe = _availableRecipe.Distinct().ToList();
            RecipeChangedEvent();
        }

        private void RecipeChangedEvent()
        {
            OnRecipeChanged?.Invoke();
        }
    }
}