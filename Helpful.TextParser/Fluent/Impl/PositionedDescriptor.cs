using System;
using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl
{
    public class PositionedDescriptor : IPositionedDescriptor
    {
        protected readonly Element _element;
        private readonly IParser _parser;

        public PositionedDescriptor(IParser parser)
        {
            _element = new Element() { LineValueExtractorType = LineValueExtractorType.Positioned };
            _parser = parser;
        }

        public IPositionedPositionDescriptor<TClass> MapTo<TClass>(string tag) where TClass : class
        {
            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentException($"Tag cannot be empty for {typeof(TClass).FullName}");
            }

            _element.Tag = tag;
            _element.ElementType = ElementType.Tag;
            _element.Type = typeof(TClass);

            return new PositionedPositionDescriptor<TClass>(_element, _parser);
        }

        public IPositionedPropertiesOnlyDescriptor<TClass> MapTo<TClass>() where TClass : class
        {
            _element.Type = typeof(TClass);
            _element.ElementType = ElementType.PropertyCollection;

            return new PositionedPropertiesOnlyDescriptor<TClass>(_element, _parser);
        }
    }
}
