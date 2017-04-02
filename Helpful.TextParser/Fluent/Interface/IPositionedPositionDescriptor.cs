namespace Helpful.TextParser.Fluent.Interface
{
    public interface IPositionedPositionDescriptor<TClass> where TClass : class
    {
        IPositionedPropertiesDescriptor<TClass> Position(int startPosition, int endPosition);
    }
}