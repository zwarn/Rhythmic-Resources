using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace navigation
{
    public class NavigationBarUI : MonoBehaviour
    {
        [SerializeField] private Transform itemParent;

        [Inject] private NavigationItemFactory _itemFactory;
        [Inject] private NavigationController _navigationController;

        private readonly Dictionary<NavigationPlaceSO, NavigationItem> _navigationItems = new();

        private void OnEnable()
        {
            _navigationController.OnAvailablePlacesChanged += UpdateAvailablePlaces;
            _navigationController.RequestAvailablePlaceUpdate();
        }

        private void OnDisable()
        {
            _navigationController.OnAvailablePlacesChanged -= UpdateAvailablePlaces;
        }

        private void UpdateAvailablePlaces(List<NavigationPlaceSO> places)
        {
            places.ForEach(place =>
            {
                if (!_navigationItems.ContainsKey(place))
                {
                    var navigationItem = _itemFactory.Create();
                    navigationItem.transform.SetParent(itemParent);
                    navigationItem.SetData(place);
                    _navigationItems.Add(place, navigationItem);
                }
            });

            var keysToRemove = _navigationItems.Keys.Except(places).ToList();

            keysToRemove.ForEach(key =>
            {
                Destroy(_navigationItems[key].gameObject);
                _navigationItems.Remove(key);
            });
        }
    }
}