using Helpful.TextParser.Fluent.Impl.Delimited.WithChildren;
using Helpful.TextParser.Fluent.Impl.Delimited.WithoutChildren;
using Helpful.TextParser.Fluent.Interface.Delimited;
using Helpful.TextParser.Fluent.Interface.Delimited.WithChildren;
using Helpful.TextParser.Fluent.Interface.Delimited.WithoutChildren;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl.Delimited
{
    public class DelimitedDescriptor : IDelimitedDescriptor
    {
        private readonly DelimitedElement _element;

        public DelimitedDescriptor(string delimitationCharacter)
        {
            _element = new DelimitedElement()
            {
                ElementType = ElementType.Tag,
                DelimitationCharacter = delimitationCharacter
            };
        }

        public IDelimitedWithChildrenTagDescriptor WithChildren()
        {
            return new DelimitedWithChildrenTagDescriptor(_element);
        }

        public IDelimitedWithoutChildrenMapToDescriptor WithoutChildren()
        {
            return new DelimitedWithoutChildrenMapToDescriptor(_element);
        }
    }
}
