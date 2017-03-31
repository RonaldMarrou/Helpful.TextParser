namespace Helpful.TextParser.Fluent.Interface
{
    public interface IPositionedPropertyMapToDescriptor
    {
        IPositionedPropertyPositionDescriptor<TClass> MapTo<TClass>(string tag) where TClass : class;

        IPositionedPropertyRequiredDescriptor Position(int startPosition, int endPosition);
    }
}