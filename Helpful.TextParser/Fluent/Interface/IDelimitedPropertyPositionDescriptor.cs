namespace Helpful.TextParser.Fluent.Interface
{
    public interface IDelimitedPropertyPositionDescriptor<TClass> where TClass : class
    {
        IDelimitedPropertyPropertiesDescriptor<TClass> Position(int position);
    }
}