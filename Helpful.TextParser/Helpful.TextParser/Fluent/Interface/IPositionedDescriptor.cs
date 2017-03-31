namespace Helpful.TextParser.Fluent.Interface
{
    public interface IPositionedDescriptor
    {
        IPositionedPositionDescriptor<TClass> MapTo<TClass>(string tag) where TClass : class;

        IPositionedPropertiesOnlyDescriptor<TClass> MapTo<TClass>() where TClass : class;
    }
}