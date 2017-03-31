namespace Helpful.TextParser.Fluent.Interface
{
    public interface IDelimitedDescriptor
    {
        IDelimitedPositionDescriptor<TClass> MapTo<TClass>(string tag) where TClass : class;

        IDelimitedPropertiesOnlyDescriptor<TClass> MapTo<TClass>() where TClass : class;
    }
}   