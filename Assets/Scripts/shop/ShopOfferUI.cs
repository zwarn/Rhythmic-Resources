using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace shop
{
    public class ShopOfferUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text description;
        [SerializeField] private Image icon;
        [SerializeField] private ShopPriceUI price;

        [Inject] private ShopController _shopController;

        private ShopOfferSO _offerData;

        public void SetData(ShopOfferSO so)
        {
            _offerData = so;

            description.text = so.description;
            icon.sprite = so.icon;
            price.SetData(so.price);
        }

        public void Buy()
        {
            _shopController.Buy(_offerData);
        }
    }

    public class ShopOfferFactory : PlaceholderFactory<ShopOfferUI>
    {
    }
}