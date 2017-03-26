using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Fluent.Interface.Delimited.WithoutChildren;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl.Delimited.WithoutChildren
{
    public class DelimitedWithoutChildrenPropertyPositionDescriptor : IDelimitedWithoutChildrenPropertyPositionDescriptor, IDelimitedWithoutChildrenPropertyRequiredDescriptor
    {
        private readonly DelimitedElement _element;

        public DelimitedWithoutChildrenPropertyPositionDescriptor(DelimitedElement element)
        {
            _element = element;
        }

        public IDelimitedWithoutChildrenPropertyRequiredDescriptor Position(int position)
        {
            _element.Position = position;

            return this;
        }

        public void NotRequired()
        {
            _element.Required = false;
        }

        public void Required()
        {
            _element.Required = true;
        }
    }
}
