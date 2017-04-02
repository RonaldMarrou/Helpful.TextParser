namespace Helpful.TextParser.Fluent.Interface
{
    public interface IDelimitedPropertyMapToDescriptor
    {
        IDelimitedPropertyPositionDescriptor<TClass> MapTo<TClass>(string tag) where TClass : class;

        IDelimitedPropertyRequiredDescriptor Position(int position);
    }
}