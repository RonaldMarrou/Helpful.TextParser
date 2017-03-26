using Helpful.TextParser.Fluent.Interface.Delimited.WithChildren;
using Helpful.TextParser.Fluent.Interface.Delimited.WithoutChildren;

namespace Helpful.TextParser.Fluent.Interface.Delimited
{
    public interface IDelimitedDescriptor
    {
        IDelimitedWithChildrenTagDescriptor WithChildren();

        IDelimitedWithoutChildrenMapToDescriptor WithoutChildren();
    }
}
