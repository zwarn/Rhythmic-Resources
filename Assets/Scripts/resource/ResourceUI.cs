using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace resource
{
    public class ResourceUI : MonoBehaviour
    {
        public Transform displayParent;
        public ResourceDisplay displayPrefab;

        private readonly Dictionary<Resource, ResourceDisplay> _resourceDisplays = new();

        [Inject] private ResourceController _resourceController;

        private void Update()
        {
            var currentResources = _resourceController.GetResources();

            foreach (var pair in currentResources)
            {
                if (!_resourceDisplays.ContainsKey(pair.Key))
                {
                    var resourceDisplay = Instantiate(displayPrefab, displayParent);
                    resourceDisplay.Initialize(pair.Key);
                    _resourceDisplays.Add(pair.Key, resourceDisplay);
                }

                _resourceDisplays[pair.Key].SetValue(pair.Value);
            }
        }
    }
}