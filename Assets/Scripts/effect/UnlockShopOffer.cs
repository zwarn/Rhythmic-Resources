using System.Collections.Generic;
using shop;
using UnityEngine;

namespace effect
{
    [CreateAssetMenu(fileName = "Effect", menuName = "Effect/UnlockShopOffer", order = 0)]
    class UnlockShopOffer : EffectSO
    {
        public List<ShopOfferSO> offers;
    }
}