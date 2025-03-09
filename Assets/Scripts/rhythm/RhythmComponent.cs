using System.Collections.Generic;
using recipes;
using resource;
using UnityEngine;
using Zenject;

namespace rhythm
{
    public class RhythmComponent : MonoBehaviour
    {
        [SerializeField] private RhythmCombinator rhythmCombinator;

        private RecipeType _recipeType;
        private List<RecipeSO> _availableRecipe;
        private RecipeSO _currentRecipe;

        [Inject] private RecipeController _recipeController;
        [Inject] private ResourceController _resourceController;

        private void OnEnable()
        {
            _recipeController.OnRecipeChanged += UpdateAvailableRecipes;
            rhythmCombinator.OnBarCompleted += HandleProduction;
            rhythmCombinator.OnRequestPlay += Play;
        }

        private void OnDisable()
        {
            _recipeController.OnRecipeChanged -= UpdateAvailableRecipes;
            rhythmCombinator.OnBarCompleted -= HandleProduction;
            rhythmCombinator.OnRequestPlay -= Play;
        }

        public void Init(RecipeType type)
        {
            _recipeType = type;
        }

        private void Start()
        {
            UpdateAvailableRecipes();
            SelectRecipe(_availableRecipe[0]);
        }

        public void Stop()
        {
            rhythmCombinator.Stop();
        }

        public void Play()
        {
            rhythmCombinator.Play();
        }

        public void SelectRecipe(RecipeSO selectedRecipe)
        {
            if (!_availableRecipe.Contains(selectedRecipe))
            {
                Debug.LogError("Trying to Select a recipe that is not available");
                return;
            }

            _currentRecipe = selectedRecipe;
            rhythmCombinator.SetRecipe(_currentRecipe);
        }

        public void SelectRecipeRight()
        {
            var currentIndex = _availableRecipe.IndexOf(_currentRecipe);
            var nextIndex = currentIndex >= _availableRecipe.Count - 1 ? 0 : currentIndex + 1;
            SelectRecipe(_availableRecipe[nextIndex]);
        }

        public void SelectRecipeLeft()
        {
            var currentIndex = _availableRecipe.IndexOf(_currentRecipe);
            var prevIndex = currentIndex <= 0 ? _availableRecipe.Count - 1 : currentIndex - 1;
            SelectRecipe(_availableRecipe[prevIndex]);
        }

        public RecipeSO GetCurrentRecipe()
        {
            return _currentRecipe;
        }

        private void UpdateAvailableRecipes()
        {
            _availableRecipe = _recipeController.GetAvailableRecipe(_recipeType);
        }

        private void HandleProduction(Dictionary<TimingResult, int> timingResults)
        {
            if (_resourceController.Pay(_currentRecipe.input))
            {
                _resourceController.Gain(_currentRecipe.output);
            }
            else
            {
                //TODO: handle "not enough resource" case
                Debug.LogError("Not enough resources to pay for this recipe");
            }
        }
    }
}