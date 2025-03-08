using System;
using rhythm;
using UnityEngine;
using UnityEngine.UI;

namespace recipes
{
    public class RecipeSelectorUI : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private RhythmComponent rhythmComponent;

        private void Update()
        {
            image.sprite = rhythmComponent.GetCurrentRecipe().icon;
        }
    }
}