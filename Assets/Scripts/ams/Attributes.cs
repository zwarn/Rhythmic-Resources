using System.Collections.Generic;
using System.Linq;

namespace ams
{
    public class Attributes
    {
        private readonly Dictionary<AttributeType, Attribute> _stats = new();

        public Attributes(Dictionary<AttributeType, float> initialValues)
        {
            foreach (var pair in initialValues)
            {
                _stats[pair.Key] = new Attribute(baseValue: pair.Value);
            }
        }

        public float GetCurrentValue(AttributeType type)
        {
            return _stats[type].CurrentValue;
        }

        public float GetBaseValue(AttributeType type)
        {
            return _stats[type].BaseValue;
        }

        public void SetBaseValue(AttributeType type, float value)
        {
            _stats[type].BaseValue = value;
        }

        public void ResetModifiers()
        {
            _stats.Values.ToList().ForEach(stat => stat.ResetModifiers());
        }

        public void AddModifiers(Dictionary<AttributeType, List<Modifier>> modifiers)
        {
            foreach (var pair in modifiers)
            {
                _stats[pair.Key].AddModifiers(pair.Value);
            }
        }
    }
}