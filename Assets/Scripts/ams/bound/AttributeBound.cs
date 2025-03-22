namespace ams.bound
{
    public class AttributeBound : Bound
    {
        private readonly Attribute _attribute;

        public AttributeBound(Attribute attribute)
        {
            _attribute = attribute;
            _attribute.OnAttributeChanged += BoundChangedEvent;
        }

        public override float GetValue()
        {
            return _attribute.CurrentValue;
        }
    }
}