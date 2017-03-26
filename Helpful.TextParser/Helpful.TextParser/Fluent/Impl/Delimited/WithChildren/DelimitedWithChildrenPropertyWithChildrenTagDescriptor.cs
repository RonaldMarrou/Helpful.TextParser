using Helpful.TextParser.Fluent.Interface.Delimited.WithChildren;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl.Delimited.WithChildren
{
    public class DelimitedWithChildrenPropertyWithChildrenTagDescriptor : IDelimitedWithChildrenPropertyWithChildrenTagDescriptor, IDelimitedWithChildrenPropertyWithChildrenTagPositionDescriptor, IDelimitedWithChildrenPropertyMapToDescriptor
    {
        private readonly DelimitedElement _element;

        public DelimitedWithChildrenPropertyWithChildrenTagDescriptor(DelimitedElement element)
        {
            _element = element;
        }

        public IDelimitedWithChildrenPropertyWithChildrenTagPositionDescriptor Tag(string tag)
        {
            _element.Tag = tag;

            return this;
        }

        public IDelimitedWithChildrenPropertyMapToDescriptor Position(int position)
        {
            _element.Position = position;

            return this;
        }

        public IDelimitedWithChildrenPropertiesDescriptor<TClass> MapTo<TClass>()
        {
            return new DelimitedWithChildrenPropertiesDescriptor<TClass>(_element, null);
        }
    }
}
