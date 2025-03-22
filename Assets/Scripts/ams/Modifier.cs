namespace ams
{
    public class Modifier
    {
        public AttributeType Attribute { get; }
        public float Value { get; }
        public ModifierType Type { get; }

        public Modifier(AttributeType attribute, float value, ModifierType type)
        {
            Attribute = attribute;
            Value = value;
            Type = type;
        }
    }
}