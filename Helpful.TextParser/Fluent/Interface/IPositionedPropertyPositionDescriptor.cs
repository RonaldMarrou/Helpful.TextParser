namespace Helpful.TextParser.Fluent.Interface
{
    public interface IPositionedPropertyPositionDescriptor
    {
        IPositionedPropertyRequiredDescriptor Position(int startPosition, int endPosition);
    }
}