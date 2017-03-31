using System;
using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl
{
    public class PositionedDescriptor : IPositionedDescriptor
    {
        private readonly Element _element;

        public PositionedDescriptor(Element element)
        {
            _element = element;
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

            return new PositionedPositionDescriptor<TClass>(_element);
        }

        public IPositionedPropertiesOnlyDescriptor<TClass> MapTo<TClass>() where TClass : class
        {
            _element.Type = typeof(TClass);
            _element.ElementType = ElementType.PropertyCollection;

            return null;
        }
    }
}
