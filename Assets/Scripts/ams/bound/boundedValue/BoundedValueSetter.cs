namespace ams.bound.boundedValue
{
    public abstract class BoundedValueSetter
    {
        public abstract float ApplyValue(Bound lowerBound, Bound upperBound);
    }
}