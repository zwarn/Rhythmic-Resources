using resource;
using UnityEngine;

namespace shop
{
    public class ShopPriceUI : MonoBehaviour
    {
        public Transform componentTransform;
        public ShopPriceComponentUI componentUIPrefab;

        public void SetData(ResourceList costs)
        {
            costs.resourceAmounts.ForEach(resource =>
            {
                var shopPriceComponent = Instantiate(componentUIPrefab, componentTransform);
                shopPriceComponent.SetData(resource);
            });
        }


    }
}