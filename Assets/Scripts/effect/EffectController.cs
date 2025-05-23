﻿using navigation;
using recipes;
using shop;
using UnityEngine;
using Zenject;

namespace effect
{
    public class EffectController : MonoBehaviour
    {
        [Inject] private ShopController _shopController;
        [Inject] private NavigationController _navigationController;
        [Inject] private RecipeController _recipeController;

        public void ApplyEffect(EffectSO effectSO)
        {
            if (effectSO is UnlockShopOffer unlockShopOffer)
            {
                _shopController.AddShopOffers(unlockShopOffer.offers);
            }
            else if (effectSO is UnlockScreen unlockScreen)
            {
                _navigationController.AddPlaces(unlockScreen.places);
            }
            else if (effectSO is UnlockRecipe unlockRecipe)
            {
                _recipeController.AddRecipes(unlockRecipe.recipes);
            }
            else
            {
                Debug.LogError($"Unsupported Effect {effectSO} in EffectController");
            }
        }
    }
}