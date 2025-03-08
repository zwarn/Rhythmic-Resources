using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace navigation
{
    public class NavigationItem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Sprite circle;
        [SerializeField] private Sprite circleSelected;

        [SerializeField] private Image circleImage;
        [SerializeField] private Image iconImage;

        private NavigationPlaceSO _placeSO;

        [Inject] private NavigationController _navigationController;

        private void OnEnable()
        {
            _navigationController.OnPlaceEntered += UpdateSelectionStatus;
        }

        private void OnDisable()
        {
            _navigationController.OnPlaceEntered -= UpdateSelectionStatus;
        }

        public void SetData(NavigationPlaceSO placeSO)
        {
            _placeSO = placeSO;
            iconImage.sprite = placeSO.icon;
        }

        private void UpdateSelectionStatus(NavigationPlaceSO selectedPlaceSO)
        {
            circleImage.sprite = _placeSO == selectedPlaceSO ? circleSelected : circle;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _navigationController.RequestPlaceChangeEvent(_placeSO);
        }
    }
}