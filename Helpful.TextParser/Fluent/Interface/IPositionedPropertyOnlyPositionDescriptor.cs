namespace Helpful.TextParser.Fluent.Interface
{
    public interface IPositionedPropertyOnlyPositionDescriptor
    {
        IPositionedPropertyOnlyRequiredDescriptor Position(int startPosition, int endPosition);
    }
}