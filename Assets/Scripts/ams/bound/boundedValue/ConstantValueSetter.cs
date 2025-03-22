using UnityEngine;

namespace ams.bound.boundedValue
{
    public class ConstantValueSetter : BoundedValueSetter
    {
        private readonly float _initialValue;

        public ConstantValueSetter(float initialValue)
        {
            _initialValue = initialValue;
        }

        public override float ApplyValue(Bound lowerBound, Bound upperBound)
        {
            return Mathf.Clamp(_initialValue, lowerBound.GetValue(), upperBound.GetValue());
        }
    }
}