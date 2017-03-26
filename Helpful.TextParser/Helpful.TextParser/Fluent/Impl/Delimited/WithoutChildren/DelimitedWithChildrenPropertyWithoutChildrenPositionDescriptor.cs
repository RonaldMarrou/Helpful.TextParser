using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Fluent.Interface.Delimited.WithChildren;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl.Delimited.WithoutChildren
{
    public class DelimitedWithChildrenPropertyWithoutChildrenPositionDescriptor : IDelimitedWithChildrenPropertyWithoutChildrenPositionDescriptor, IDelimitedWithChildrenPropertyWithoutChildrenRequiredDescriptor
    {
        private readonly DelimitedElement _element;

        public DelimitedWithChildrenPropertyWithoutChildrenPositionDescriptor(DelimitedElement element)
        {
            _element = element;
        }

        public void Required()
        {
            _element.Required = true;
        }

        public void NotRequired()
        {
            _element.Required = false;
        }

        public IDelimitedWithChildrenPropertyWithoutChildrenRequiredDescriptor Position(int position)
        {
            _element.Position = position;

            return this;
        }
    }
}
