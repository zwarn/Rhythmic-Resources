namespace ams.bound.boundedValue
{
    public class EmptyValueSetter : BoundedValueSetter
    {
        public override float ApplyValue(Bound lowerBound, Bound upperBound)
        {
            return lowerBound.GetValue();
        }
    }
}