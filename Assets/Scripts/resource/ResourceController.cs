using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace resource
{
    public class ResourceController : MonoBehaviour
    {
        private readonly Dictionary<Resource, int> _resourceCounts = new();
        [SerializeField] private ResourceList startResources;

        private void Start()
        {
            startResources.resourceAmounts.ForEach(resource => Gain(resource.resource, resource.amount));
        }

        public bool CanPay(Dictionary<Resource, int> cost)
        {
            return cost.All(pair => _resourceCounts.ContainsKey(pair.Key) && _resourceCounts[pair.Key] >= pair.Value);
        }

        public bool Pay(ResourceList cost)
        {
            return Pay(cost.ToDictionary());
        }

        public bool Pay(Dictionary<Resource, int> cost)
        {
            if (!CanPay(cost)) return false;

            foreach (var kvp in cost)
            {
                _resourceCounts[kvp.Key] -= kvp.Value;
            }

            return true;
        }

        public void Gain(Resource type, int amount)
        {
            _resourceCounts.TryAdd(type, 0);
            _resourceCounts[type] += amount;
        }

        public void Gain(ResourceList gains)
        {
            gains.resourceAmounts.ForEach(res => Gain(res.resource, res.amount));
        }

        public void Gain(Dictionary<Resource, int> gains)
        {
            foreach (var pair in gains)
            {
                Gain(pair.Key, pair.Value);
            }
        }

        public Dictionary<Resource, int> GetResources()
        {
            return _resourceCounts;
        }
    }
}