namespace Helpful.TextParser.Fluent.Interface.Delimited.WithChildren
{
    public interface IDelimitedWithChildrenPropertyMapToDescriptor
    {
        IDelimitedWithChildrenPropertiesDescriptor<TClass> MapTo<TClass>();
    }
}
