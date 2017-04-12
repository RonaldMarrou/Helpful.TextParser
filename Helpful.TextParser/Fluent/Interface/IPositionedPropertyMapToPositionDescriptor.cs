namespace Helpful.TextParser.Fluent.Interface
{
    public interface IPositionedPropertyMapToPositionDescriptor<TClass> where TClass : class
    {
        IPositionedPropertyMapToPropertiesDescriptor<TClass> Position(int startPosition, int endPosition);
    }
}