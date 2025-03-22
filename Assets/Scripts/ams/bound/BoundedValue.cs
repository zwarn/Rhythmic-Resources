using ams.bound.boundedValue;
using UnityEngine;

namespace ams.bound
{
    public class BoundedValue
    {
        private readonly Bound _lowerBound;
        private readonly Bound _upperBound;

        public BoundedValue(Bound lowerBound, Bound upperBound, BoundedValueSetter valueSetter)
        {
            _lowerBound = lowerBound;
            _upperBound = upperBound;
            CurrentValue = valueSetter.ApplyValue(lowerBound, upperBound);

            _lowerBound.OnBoundChanged += RecomputeBounds;
            _upperBound.OnBoundChanged += RecomputeBounds;
        }

        public float CurrentValue { get; private set; }

        public float MinValue()
        {
            return _lowerBound.GetValue();
        }

        public float MaxValue()
        {
            return _upperBound.GetValue();
        }

        public void SetCurrentValue(BoundedValueSetter valueSetter)
        {
            CurrentValue = valueSetter.ApplyValue(_lowerBound, _upperBound);
        }

        public void ModifyValue(float delta)
        {
            CurrentValue = Mathf.Clamp(CurrentValue + delta, _lowerBound.GetValue(), _upperBound.GetValue());
        }

        public void RecomputeBounds()
        {
            CurrentValue = Mathf.Clamp(CurrentValue, _lowerBound.GetValue(), _upperBound.GetValue());
        }
    }
}