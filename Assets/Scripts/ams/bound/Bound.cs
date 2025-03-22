using System;

namespace ams.bound
{
    public abstract class Bound
    {
        public event Action OnBoundChanged;

        protected void BoundChangedEvent() => OnBoundChanged?.Invoke();
        public abstract float GetValue();
    }
}