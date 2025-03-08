using resource;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace shop
{
    public class ShopPriceComponentUI : MonoBehaviour
    {
        public TMP_Text amount;
        public Image icon;

        public void SetData(ResourceAmount resourceAmount)
        {
            amount.text = resourceAmount.amount.ToString();
            icon.sprite = resourceAmount.resource.sprite;
        }
    }
}