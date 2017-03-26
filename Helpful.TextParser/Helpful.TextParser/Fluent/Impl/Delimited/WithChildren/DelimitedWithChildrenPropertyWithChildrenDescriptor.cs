using Helpful.TextParser.Fluent.Impl.Delimited.WithoutChildren;
using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Fluent.Interface.Delimited.WithChildren;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl.Delimited.WithChildren
{
    public class DelimitedWithChildrenPropertyWithChildrenDescriptor<TProperty> : IDelimitedWithChildrenPropertyWithChildrenDescriptor<TProperty>
    {
        private readonly DelimitedElement _element;

        public DelimitedWithChildrenPropertyWithChildrenDescriptor(DelimitedElement element)
        {
            _element = element;
        }

        public IDelimitedWithChildrenPropertyWithoutChildrenPositionDescriptor WithoutChildren()
        {
            _element.ElementType = ElementType.Property;

            return new DelimitedWithChildrenPropertyWithoutChildrenPositionDescriptor(_element);
        }

        public IDelimitedWithChildrenPropertyWithChildrenTagDescriptor WithChildren()
        {
            _element.ElementType = ElementType.Tag;

            return new DelimitedWithChildrenPropertyWithChildrenTagDescriptor(_element);
        }
    }
}
