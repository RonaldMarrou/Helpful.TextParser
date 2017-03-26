using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Fluent.Interface.Delimited.WithChildren;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl.Delimited.WithChildren
{
    public class DelimitedWithChildrenTagDescriptor : IDelimitedWithChildrenTagDescriptor, IDelimitedWithChildrenTagPositionDescriptor
    {
        private readonly DelimitedElement _element;

        public DelimitedWithChildrenTagDescriptor(DelimitedElement element)
        {
            _element = element;
        }

        public IDelimitedWithChildrenTagPositionDescriptor Tag(string tag)
        {
            _element.Tag = tag;

            return this;
        }

        public IDelimitedWithChildrenMapToDescriptor Position(int position)
        {
            _element.Position = position;

            return new DelimitedWithChildrenMapToDescriptor(_element);
        }
    }
}
