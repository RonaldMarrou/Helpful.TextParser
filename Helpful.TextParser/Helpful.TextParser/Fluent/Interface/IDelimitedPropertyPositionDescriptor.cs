namespace Helpful.TextParser.Fluent.Interface
{
    public interface IDelimitedPropertyPositionDescriptor<TClass> where TClass : class
    {
        IDelimitedPropertiesDescriptor<TClass> Position(int position);
    }
}