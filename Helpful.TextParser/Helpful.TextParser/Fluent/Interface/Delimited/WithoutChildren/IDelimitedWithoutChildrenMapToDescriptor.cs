namespace Helpful.TextParser.Fluent.Interface.Delimited.WithoutChildren
{
    public interface IDelimitedWithoutChildrenMapToDescriptor
    {
        IDelimitedWithoutChildrenPropertiesDescriptor<TClass> MapTo<TClass>();
    }
}
