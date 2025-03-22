namespace ams.bound
{
    public class ConstantBound : Bound
    {
        private readonly float _value;

        public ConstantBound(float value)
        {
            _value = value;
        }


        public override float GetValue()
        {
            return _value;
        }
    }
}