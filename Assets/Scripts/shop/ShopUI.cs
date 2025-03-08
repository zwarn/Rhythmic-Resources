using System;
using System.Collections.Generic;
using System.Linq;
using resource;
using UnityEngine;
using Zenject;

namespace shop
{
    public class ShopUI : MonoBehaviour
    {
        public Transform offerTransform;

        [Inject] private ShopController _shopController;
        [Inject] private ShopOfferFactory _factory;

        private readonly Dictionary<ShopOfferSO, ShopOfferUI> _shopOfferUis = new();

        private void OnEnable()
        {
            _shopController.OnUpdateShopOffer += UpdateShopOffer;
            _shopController.RequestUpdate();
        }

        private void OnDisable()
        {
            _shopController.OnUpdateShopOffer -= UpdateShopOffer;
        }

        private void UpdateShopOffer(List<ShopOfferSO> shopOffers)
        {
            // Add new UIs
            shopOffers.ForEach(offer =>
            {
                if (!_shopOfferUis.ContainsKey(offer))
                {
                    ShopOfferUI offerUI = _factory.Create();
                    offerUI.transform.SetParent(offerTransform);
                    offerUI.SetData(offer);
                    _shopOfferUis.Add(offer, offerUI);
                }
            });

            // Remove UIs no longer needed
            var keysToRemove = _shopOfferUis.Keys.Except(shopOffers).ToList();

            keysToRemove.ForEach(key =>
            {
                Destroy(_shopOfferUis[key].gameObject);
                _shopOfferUis.Remove(key);
            });
        }

        private void Start()
        {
            _shopController.RequestUpdate();
        }
    }
}