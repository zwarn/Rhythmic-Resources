namespace ams.bound.boundedValue
{
    public class FullValueSetter : BoundedValueSetter
    {
        public override float ApplyValue(Bound lowerBound, Bound upperBound)
        {
            return upperBound.GetValue();
        }
    }
}