namespace Helpful.TextParser.Fluent.Interface.Delimited.WithChildren
{
    public interface IDelimitedWithChildrenPropertyWithChildrenDescriptor<TClass>
    {
        IDelimitedWithChildrenPropertyWithChildrenTagDescriptor WithChildren();

        IDelimitedWithChildrenPropertyWithoutChildrenPositionDescriptor WithoutChildren();
    }
}
