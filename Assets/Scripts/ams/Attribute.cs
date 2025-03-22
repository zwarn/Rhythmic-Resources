using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ams
{
    public class Attribute
    {
        public event Action OnAttributeChanged;

        private readonly List<Modifier> _modifiers = new();

        private float _baseValue;
        private float _currentValue;

        public Attribute(float baseValue)
        {
            BaseValue = baseValue;
        }

        public float BaseValue
        {
            get => _baseValue;
            set
            {
                _baseValue = value;
                RecalculateValue();
            }
        }

        public float CurrentValue
        {
            get => _currentValue;
        }

        public void AddModifier(Modifier modifier)
        {
            _modifiers.Add(modifier);
            RecalculateValue();
        }

        public void RemoveModifier(Modifier modifier)
        {
            _modifiers.Remove(modifier);
            RecalculateValue();
        }

        public void AddModifiers(List<Modifier> modifiers)
        {
            _modifiers.AddRange(modifiers);
            RecalculateValue();
        }

        public void ResetModifiers()
        {
            _modifiers.Clear();
            RecalculateValue();
        }

        private void RecalculateValue()
        {
            float flatBonus = _modifiers.Where(m => m.Type == ModifierType.Flat).Sum(m => m.Value);
            float additiveBonus = _modifiers.Where(m => m.Type == ModifierType.Additive).Sum(m => m.Value);
            float multiplicativeBonus = _modifiers.Where(m => m.Type == ModifierType.Multiplicative)
                .Aggregate(1f, (acc, mod) => acc * (1 + mod.Value));

            _currentValue = Mathf.CeilToInt((BaseValue + flatBonus) * (1 + additiveBonus) * multiplicativeBonus);
            AttributeChangedEvent();
        }

        protected virtual void AttributeChangedEvent()
        {
            OnAttributeChanged?.Invoke();
        }
    }
}