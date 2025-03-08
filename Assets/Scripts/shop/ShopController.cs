using System;
using System.Collections.Generic;
using System.Linq;
using effect;
using resource;
using UnityEngine;
using Zenject;

namespace shop
{
    public class ShopController : MonoBehaviour
    {
        public event Action<List<ShopOfferSO>> OnUpdateShopOffer;
        public event Action<ShopOfferSO> OnShopOfferBought;


        [SerializeField] private List<ShopOfferSO> initialShopOffers;

        [Inject] private ResourceController _resourceController;
        [Inject] private EffectController _effectController;

        private List<ShopOfferSO> _currentShopOffers = new();

        private void Awake()
        {
            _currentShopOffers = initialShopOffers.ToList();
        }

        public void AddShopOffer(ShopOfferSO shopOffer)
        {
            _currentShopOffers.Add(shopOffer);
            UpdateShopOfferEvent();
        }

        public void AddShopOffers(List<ShopOfferSO> shopOffers)
        {
            _currentShopOffers.AddRange(shopOffers);
            UpdateShopOfferEvent();
        }

        public void RemoveShopOffer(ShopOfferSO shopOfferSO)
        {
            _currentShopOffers.Remove(shopOfferSO);
            UpdateShopOfferEvent();
        }

        public void Buy(ShopOfferSO offerData)
        {
            if (!_currentShopOffers.Contains(offerData))
            {
                Debug.LogError($"trying to buy a offer that is not for sale \n {offerData}");
                return;
            }

            if (_resourceController.Pay(offerData.price))
            {
                offerData.ApplyEffect(_effectController);
                RemoveShopOffer(offerData);
                ShopOfferBoughtEvent(offerData);
            }
        }

        private void UpdateShopOfferEvent()
        {
            OnUpdateShopOffer?.Invoke(_currentShopOffers);
        }

        private void ShopOfferBoughtEvent(ShopOfferSO offer)
        {
            OnShopOfferBought?.Invoke(offer);
        }

        public void RequestUpdate()
        {
            UpdateShopOfferEvent();
        }
    }
}