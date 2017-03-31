using System;
using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl
{
    public class DelimitedDescriptor : IDelimitedDescriptor
    {
        private readonly Element _element;
        private readonly IParser _parser;

        public DelimitedDescriptor(Element element, IParser parser)
        {
            _element = element;
            _parser = parser;
        }

        public IDelimitedPositionDescriptor<TClass> MapTo<TClass>(string tag) where TClass : class
        {
            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentException($"Tag cannot be empty for {typeof(TClass).FullName}");
            }

            _element.Tag = tag;
            _element.ElementType = ElementType.Tag;
            _element.Type = typeof(TClass);

            return new DelimitedPositionDescriptor<TClass>(_element, _parser);
        }

        public IDelimitedPropertiesOnlyDescriptor<TClass> MapTo<TClass>() where TClass : class
        {
            _element.Type = typeof(TClass);
            _element.ElementType = ElementType.PropertyCollection;

            return new DelimitedPropertiesOnlyDescriptor<TClass>(_element, _parser);
        }
    }
}
