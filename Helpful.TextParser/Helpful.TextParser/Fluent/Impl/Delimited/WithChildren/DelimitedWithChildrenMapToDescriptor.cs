using Helpful.TextParser.Fluent.Interface.Delimited.WithChildren;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl.Delimited.WithChildren
{
    public class DelimitedWithChildrenMapToDescriptor : IDelimitedWithChildrenMapToDescriptor
    {
        private readonly DelimitedElement _element;

        public DelimitedWithChildrenMapToDescriptor(DelimitedElement element)
        {
            _element = element;
        }

        public IDelimitedWithChildrenPropertiesDescriptor<TClass> MapTo<TClass>()
        {
            return new DelimitedWithChildrenPropertiesDescriptor<TClass>(_element, null);
        }
    }
}
