namespace Helpful.TextParser.Fluent.Interface
{
    public interface IDelimitedPositionDescriptor<TClass> where TClass : class
    {
        IDelimitedPropertiesDescriptor<TClass> Position(int position);
    }
}