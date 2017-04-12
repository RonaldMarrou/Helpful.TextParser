namespace Helpful.TextParser.Fluent.Interface
{
    public interface IDelimitedPropertyMapToPositionDescriptor<TClass> where TClass : class
    {
        IDelimitedPropertyMapToPropertiesDescriptor<TClass> Position(int position);
    }
}