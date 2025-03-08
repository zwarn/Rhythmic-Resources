using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace resource
{
    [Serializable]
    public class ResourceList
    {
        [SerializeField] public List<ResourceAmount> resourceAmounts;

        public ResourceList(List<ResourceAmount> resourceAmounts)
        {
            this.resourceAmounts = resourceAmounts;
        }

        public Dictionary<Resource, int> ToDictionary()
        {
            return resourceAmounts.ToDictionary(entry => entry.resource, entry => entry.amount);
        }
    }


    [Serializable]
    public struct ResourceAmount
    {
        public Resource resource;
        public int amount;
    }
}