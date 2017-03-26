namespace Helpful.TextParser.Fluent.Interface.Delimited.WithChildren
{
    public interface IDelimitedWithChildrenMapToDescriptor
    {
        IDelimitedWithChildrenPropertiesDescriptor<TClass> MapTo<TClass>();
    }
}
