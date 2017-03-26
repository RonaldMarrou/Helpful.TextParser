using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Fluent.Interface.Delimited.WithoutChildren;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl.Delimited.WithoutChildren
{
    public class DelimitedWithoutChildrenMapToDescriptor : IDelimitedWithoutChildrenMapToDescriptor
    {
        private readonly DelimitedElement _element;

        public DelimitedWithoutChildrenMapToDescriptor(DelimitedElement element)
        {
            _element = element;
        }

        public IDelimitedWithoutChildrenPropertiesDescriptor<TClass> MapTo<TClass>()
        {
            return new DelimitedWithoutChildrenPropertiesDescriptor<TClass>(_element, null);
        }
    }
}
