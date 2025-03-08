using System;
using System.Collections.Generic;
using effect;
using resource;
using UnityEngine;

namespace shop
{
    [CreateAssetMenu(fileName = "ShopOffer", menuName = "ShopOffer", order = 0)]
    public class ShopOfferSO : ScriptableObject
    {
        public Sprite icon;
        public String description;
        public ResourceList price;
        public List<EffectSO> effects;

        public void ApplyEffect(EffectController effectController)
        {
            effects.ForEach(effectController.ApplyEffect);
        }
    }
}