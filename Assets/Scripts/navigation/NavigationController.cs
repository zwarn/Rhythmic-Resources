using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace navigation
{
    public class NavigationController : MonoBehaviour
    {
        public event Action<NavigationPlaceSO> OnPlaceEntered;

        public event Action<NavigationPlaceSO> OnPlaceChangeRequest;

        public event Action<List<NavigationPlaceSO>> OnAvailablePlacesChanged;

        [SerializeField] private List<NavigationPlaceSO> initialPlaces;
        [SerializeField] private Transform placeParent;

        private NavigationPlaceSO _currentSelection;

        private readonly Dictionary<NavigationPlaceSO, NavigationPlace> _places = new();
        private List<NavigationPlaceSO> _availablePlaces = new();

        [Inject] private DiContainer _container;


        public void RequestAvailablePlaceUpdate()
        {
            AvailablePlacesChangedEvent();
        }

        private void Awake()
        {
            _availablePlaces = initialPlaces.ToList();
        }

        private void Start()
        {
            NavigateToPlace(_availablePlaces[0]);
        }

        private void OnEnable()
        {
            OnPlaceChangeRequest += NavigateToPlace;
        }

        private void OnDisable()
        {
            OnPlaceChangeRequest -= NavigateToPlace;
        }

        public void NavigateLeft()
        {
            var currentIndex = _availablePlaces.FindIndex(place => place == _currentSelection);
            var destinationIndex = currentIndex == 0 ? _availablePlaces.Count - 1 : currentIndex - 1;
            var destination = _availablePlaces[destinationIndex];
            NavigateToPlace(destination);
        }

        public void NavigateRight()
        {
            var currentIndex = _availablePlaces.FindIndex(place => place == _currentSelection);
            var destinationIndex = currentIndex == _availablePlaces.Count - 1 ? 0 : currentIndex + 1;
            var destination = _availablePlaces[destinationIndex];
            NavigateToPlace(destination);
        }

        private void NavigateToPlace(NavigationPlaceSO destination)
        {
            if (_currentSelection != null)
            {
                _places[_currentSelection].LeavePlace();
            }

            _currentSelection = destination;

            if (!_places.ContainsKey(_currentSelection))
            {
                InstantiatePlaceScreen(_currentSelection);
            }

            var newPlace = _places[destination];
            newPlace.EnterPlace();

            PlaceEnteredEvent(destination);
        }

        private void InstantiatePlaceScreen(NavigationPlaceSO placeSO)
        {
            var place = _container.InstantiatePrefab(placeSO.screenPrefab, placeParent)
                .GetComponent<NavigationPlace>();
            _places.Add(placeSO, place);
        }

        public void AddPlaces(List<NavigationPlaceSO> places)
        {
            _availablePlaces.AddRange(places);
            _availablePlaces = _availablePlaces.Distinct().ToList();
            AvailablePlacesChangedEvent();
        }

        public void RemovePlace(NavigationPlaceSO place)
        {
            _availablePlaces.Remove(place);
            AvailablePlacesChangedEvent();
        }

        public void RequestPlaceChangeEvent(NavigationPlaceSO placeSO)
        {
            OnPlaceChangeRequest?.Invoke(placeSO);
        }

        public void PlaceEnteredEvent(NavigationPlaceSO placeSO)
        {
            OnPlaceEntered?.Invoke(placeSO);
        }

        public void AvailablePlacesChangedEvent()
        {
            OnAvailablePlacesChanged?.Invoke(_availablePlaces);
        }
    }
}