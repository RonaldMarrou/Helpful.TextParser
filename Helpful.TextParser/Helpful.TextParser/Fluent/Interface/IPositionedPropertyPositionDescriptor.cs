namespace Helpful.TextParser.Fluent.Interface
{
    public interface IPositionedPropertyPositionDescriptor<TClass> where TClass : class
    {
        IPositionedPropertiesDescriptor<TClass> Position(int startPosition, int endPosition);
    }
}